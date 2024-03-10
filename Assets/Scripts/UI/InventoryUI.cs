using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // Start is called before the first frame update
    /*
     1. Ű ������ ������
    2. ������ �� �÷��̾� �κ��丮 ���� ��������
    3. �װ� �ѷ��ֱ�

     
     
     
     */

    private List<Item> itemsOrigin = new List<Item>();
    public GameObject player;
    public GameObject itemCellPrefab;
    
    private List<Item> items;
    [SerializeField]
    private Canvas canvas;
    private GraphicRaycaster gRaycaster;

    [SerializeField]
    private GameObject weaponControl;

    public GameObject background;
    private PlayerState playerState;
    public GameObject itemCells;



    private enum InventoryState
    {
        WeaponWindow,
        etc,
    }

    void Start()
    {
        playerState = player.GetComponent<PlayerState>();
        gRaycaster = canvas.GetComponent<GraphicRaycaster>();
    }

    // Update is called once per frame
    void Update()
    {

        //Inventory OpenCheck
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (playerState.isOpenInventory)
            {
                playerState.isOpenInventory = false;
                CloseInventory();
            }
            else
            {
                playerState.isOpenInventory = true;
                OpenInventory();
            }
        }

        //Opened Inventory... ���߿� ���콺 raycast UI�θ� ��ũ��Ʈ�� �� ���ս�Ű��
        if (playerState.isOpenInventory)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ped = new PointerEventData(null);
                ped.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                gRaycaster.Raycast(ped, results);
                if (results.Count <= 0)
                {
                    return;
                }
                if (results[0].gameObject.transform.tag == "ItemCell")
                {
                    int itemId = results[0].gameObject.transform.GetComponent<ItemCell>().id;
                    SelectItem(items[itemId]);
                }

            }
        }
    }

    void SelectItem(Item item)
    {
        if (playerState.nowHand != null)
        {
            Destroy(playerState.nowHand);
            playerState.baseState.isAimming = false;
        }

        GameObject weapon = Instantiate(item.prefab);
        weapon.GetComponent<ItemBase>().item = item;
        weapon.transform.parent = weaponControl.transform;
        weapon.GetComponent<Rigidbody2D>().simulated = false;
        if (weapon.GetComponent<PolygonCollider2D>())
        {
            weapon.GetComponent<PolygonCollider2D>().enabled = false;
        }
        else if (weapon.GetComponent<BoxCollider2D>()) //NEED DELETE, ������ �ݶ��̴��� ����
        {
            weapon.GetComponent<BoxCollider2D>().enabled = false;
        }

        
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.localScale = new Vector3(Mathf.Abs(weapon.transform.localScale.x), Mathf.Abs(weapon.transform.localScale.y), weapon.transform.localScale.z);
        playerState.nowHand = weapon;

        if (weapon.GetComponent<RangedWeaponBase>() != null)
        {
            weapon.GetComponent<RangedWeaponBase>().stat = item.rangedWeaponInfo;
        }
        if(weapon.GetComponent<MeleeWeaponBase>() != null)
        {
            weapon.GetComponent<MeleeWeaponBase>().stat = item.meleeWeaponInfo;
            weaponControl.GetComponent<PlayerWeaponControl>().PlayerMeleeComboAnimationSet();
            weapon.GetComponent<Rigidbody2D>().isKinematic = true;
            weapon.GetComponent<Rigidbody2D>().simulated = true;

            weapon.GetComponent<MeleeWeaponBase>().InitWeaponUser(player);
        }

        
        weaponControl.GetComponent<PlayerWeaponControl>().PutHandOnWeapon();



    }
    

    void GetInventoryData()
    {
        itemsOrigin = player.GetComponent<PlayerInventory>().items;
        
    }
    void CreateItemCell(Item item, int count)
    {
        GameObject itemCell = Instantiate(itemCellPrefab, itemCells.transform);
        itemCell.GetComponent<ItemCell>().text.GetComponent<TextMeshProUGUI>().SetText(item.displayName);
        itemCell.GetComponent<ItemCell>().image.GetComponent<Image>().sprite = item.displaySprite;
        itemCell.GetComponent<RectTransform>().localPosition = new Vector3(0, 380 - count * 150, 0);
        itemCell.GetComponent<ItemCell>().id = count;
    }
    void SortItems()
    {
        items = itemsOrigin; 
    }
    public void OpenInventory()
    {
        //�ð� ������Ű�鼭 �� �׷��� ����� �κ��丮�� ������ �ƴϸ� Ű������Ʈ�ѷ� �׷��� ���� �ű⿡�� ó���ұ� ���� ��εǳ� �ϴ��� ������ �ھƵΰ� ���߿� �ű���
        //��� �����ߴµ� �����غ��ϱ� Enable�ȵȻ��·� �����Ұ��ݾ�
        background.SetActive(true);
        GetInventoryData();
        SortItems();
        for(int i = 0; i < items.Count; i++)
        {
            CreateItemCell(items[i], i);
        }
    }
    public void CloseInventory()
    {
        foreach(Transform child in itemCells.transform)
        {
            Destroy(child.gameObject);
        }
        background.SetActive(false);
    }
    

}
