using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static InteractionInterface;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject inventory;
    public GameObject player;
    public GameObject playerCenter;
    private float searchCircleSize;
    private LayerMask droppedItem = 1 << 8;//NEED FIX, 나중에 layerMaskCenter만들어서 거기서 값 따와서 지정하도록 하기
    /*
     1. 마우스 아래에 있는 아이템이 현재 Collider 내에 있는지 확인하기
     없다면
         1. Collider 내에 있는 아이템 가져오기
         2. tag == item 으로 거르기
         3. 가장 가까이 있는 아이템 갖고 오기
        아에 레이어 마스크 없이 할까? 상호작용키는 결국 줍기 키도 될꺼고...
     
     
     */
    [SerializeField]
    private GameObject selectedObject;//스읍... 찾지를 못하는데
    public GameObject interactionText;//middle Low Text

    public GameObject interactPopUp;//display
    public GameObject interactPopUpPrefab;//prefab

    private enum TargetType
    {
        DroppedItem,
        Interactionable
    }
    private TargetType targetType;




    // Update is called once per frame
    void Update()
    {
        InteractionCheck();
        PickupItemCheck();//NEED FIX : 함수명 바꾸기
    }
    //여길 아에 뜯어 고치자.

    
    // 플레이어 상호작용

    private void PickupItemCheck()
    {
        if (selectedObject != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (selectedObject.GetComponent<ItemBase>())
                {
                    PickupItem(selectedObject);
                }
                else if(selectedObject.GetComponent<IInteraction>() != null && !player.GetComponent<PlayerState>().isOnDialog)
                {
                    selectedObject.GetComponent<IInteraction>().ActivateTrigger(player);
                }
                
            }
        }
    }
    private void PickupItem(GameObject item)
    {
        //아이템 정보 가져오기
        Item item_info = item.GetComponent<ItemBase>().item;

        //인벤토리에 아이템 정보 등록 및 실세계 아이템 제거 그래 이건 생각해보면 이벤트 실행 주체가 플레이어가 줍는거지 아이템이 주워지는게 아니잖아? 이게 직관적이니까 이렇게 그냥 두자. 모든 스크립트는 능동적 대상을 베이스로 둔다는
        //대략 그런 느낌을 둬야지 나중에 어디서 실행하는지를 확인하기 좋을거야. 오키오키 이대로 가자
        player.GetComponent<PlayerInventory>().AddItem(item_info);
        Destroy(item);

    }
    private void InteractionCheck()
    {
        //NEED CHANGE item한정이 아닌 모든 상호작용 가능 오브젝트로 확대...아무래도 이거 Update보단 Enter로 하는게 훠어어어얼씬 이득일 것 같긴 한데... 아니지 어차피 사람이 움직이니까 Update로 두긴 해야겠네; 장실점
        //함수로 개별화
        searchCircleSize = player.GetComponent<PlayerState>().searchCircleSize;
        
        //이전 아이템 지우기
        if (selectedObject != null)
        {
            if (targetType == TargetType.DroppedItem)
            {
                selectedObject.GetComponent<SpriteRenderer>().material.SetInt("_Enable", 0);//근데 무기같은 경우들은 자식으로 나뉘어져있을텐데 이걸 자식마다 다 적용해줘야하려나 습...
            }
            else if(targetType == TargetType.Interactionable)
            {
                
            }
            
        }


        if (!CheckMouseHover())
        {
            if (!CheckCircleOver())
            {
                
                selectedObject = null;
            }
        }

        if (selectedObject != null)
        {
            interactionText.SetActive(true);
            if(targetType == TargetType.DroppedItem)
            {
                selectedObject.GetComponent<SpriteRenderer>().material.SetInt("_Enable", 1);
                //상호작용인지 오브젝트인지에 따라. 각 함수에서도 type 변인 추가하기
                interactionText.GetComponent<TextMeshProUGUI>().text = selectedObject.name;
            }
            else
            {
                interactionText.GetComponent<TextMeshProUGUI>().text = selectedObject.name;
            }
            
        }
        else
        {
            Destroy(interactPopUp);//생각해보니까 Destroy하는 것보다 비활성화-활성화 시키면서 이동시키는게 훨씬 좋을 듯? NEED FIX
            interactionText.SetActive(false);
        }
    }
    private bool CheckCircleOver()
    {
        //Collider2D[] targets = Physics2D.OverlapCircleAll(playerCenter.transform.position, searchCircleSize);
        Collider[] targets = Physics.OverlapSphere(playerCenter.transform.position, searchCircleSize);

        if (targets.Length > 0)
        {
            float minDistance = Mathf.Infinity;
            Collider closeTarget = null;
            foreach (Collider target in targets)
            {
                if (target.transform.GetComponent<ItemBase>() || target.GetComponent<IInteraction>() != null)
                {
                    if (target.transform.tag == "Item")//NEED CHANGE 이거 그 상호 작용한 그런 것들 다 &&로 엮기     아이템일 경우
                    {
                        if (Vector3.Distance(playerCenter.transform.position, target.transform.position) < minDistance)
                        {
                            minDistance = Vector2.Distance(playerCenter.transform.position, target.transform.position);
                            closeTarget = target;
                            targetType = TargetType.DroppedItem;
                        }
                    }
                    else if(target.GetComponent<IInteraction>() != null)//상호작용가능대상
                    {
                        if (Vector3.Distance(playerCenter.transform.position, target.transform.position) < minDistance)
                        {
                            minDistance = Vector2.Distance(playerCenter.transform.position, target.transform.position);
                            closeTarget = target;
                            targetType = TargetType.Interactionable;
                        }
                    }
                }
                
                //아니면 가까이 있는 아이템들을 목록으로 띄워줄까? 약간... 폴아웃이나 스카이림에서 Root인가? 그 모드 쓰는 감성으로다가, 물론 그건 상자지만 어? 나쁘지 않은데 또, 생각해보니까
                //매회 Update마다 이거 검사하는거 겁나 비효율적이잖아, 그러니까 Enter이랑 Exit를 이용해서 그냥 리스트를 아에 두고서 하면 어? 나쁘지 않은데?
                //목록으로 띄워주기 아니면 가까이가면 무기에 이름 뜨게해서(+하이라이트) 클릭으로 주울 수 있게, 이것도 나쁘지 않은데?
                //근데 이렇게 한다면 우려되는게, 일단 전투 중에 아이템이 떨어질 경우 이것 때문에 미스 클릭이 생길 수 있어. 그리고 주변에 아이템 있을 때마다 이게 뜨면 비슷하게 화면 가리는 방해요소가 될 수 있겠네
                //플레이어가 구별해서 아이템을 얻을 이유가 거의 없잖아? 그러니까 그냥 일반 무한 중첩템의 경우 자동 획득되게 하고, 아닌 템중에서는 선택적 획득...이게 제일 나을 듯? 어쩃든 이 주변 상호작용 서클은 필요하니까 만들자
                //슬슬 적 만들어보고싶은 타이밍이 왔다제~
                //자자 드가보자꾹나
            }
            if(closeTarget != null)
            {
                if(selectedObject != closeTarget.transform.gameObject)
                {
                    selectedObject = closeTarget.transform.gameObject;
                    //프리팹 생성
                    Destroy(interactPopUp);
                    interactPopUp = Instantiate(interactPopUpPrefab);
                    interactPopUp.transform.localScale = Vector3.one;
                    interactPopUp.transform.parent = selectedObject.transform;
                    
                    //이건 임시고 일단 높이를 따와야겠지
                    //Get Height
                    //이건 임시라서 바꿔야할 듯//좀따 고치기
                    //interactPopUp.transform.localPosition = new Vector3(0, selectedObject.GetComponent<Collider>().bounds.size.y / selectedObject.transform.localScale.y + selectedObject.GetComponent<Collider2D>().offset.y/2, 0);//Collider은 Collider의 모든 것을 포함한 아 2D로 해줘야 하나?
                }
                
                return true;
            }
            else
            {
                return false;
            }
            
        }
        else
        {
            return false;
        }
    }

    private bool CheckMouseHover()
    {
        //잠만, 아이템끼리 겹쳐있는 상황...아이템끼리 겹칠 수 있게 할까? 아니면 안겹치게? 일단 겹치게 하는 게 나을 듯 한 느낌인 느낌인데 그럼 뭐 알아서 앞부터 되겠지
        /*
        Vector2 cursorPosition = Camera.main.WorldToScreenPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(cursorPosition, Vector2.zero);
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.tag == "DroppedItem")//NEED CHANGE 이거 그 상호 작용한 그런 것들 다 &&로 엮기
                {
                    if (Vector2.Distance(hit.transform.position, player.transform.position) <= searchCircleSize)
                    {
                        //처리
                        selectedObject = hit.transform.gameObject;
                        return true;
                    }
                }
            }

        }
        return false;
        */
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.transform.tag == "DroppedItem")
            {
                if(Vector3.Distance(hit.transform.position, player.transform.position) <= searchCircleSize)
                {
                    selectedObject = hit.transform.gameObject;
                    return true;
                }
            }
        }
        return false;
    }


    //플레이어가 받기
    public void RunDialog(string _eventName)
    {
        GetComponent<PlayerState>().dialogController.GetComponent<DialogController>().RunDialog(_eventName);
    }
}
