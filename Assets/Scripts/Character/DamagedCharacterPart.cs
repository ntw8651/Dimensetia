using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedCharacterPart : MonoBehaviour
{
    // Start is called before the first frame update

    /*
     이 스크립트의 역할.
     부모 파츠를 찾아가며, 부분 파츠가 0이 될 시 멸해버리고, 또 공격받은 부위에 대한 표시를 해주는 스크립트
     
    기능:
    부모찾기
    굳이 ChildParts에 대해 접근이 필요한가? 음
    나중에 한번에 확인하거나 그럴 떄에는 CHild Parts를 모두 갖고 있으면 좋긴 하지... 해당 부위가 파괴되었을 떄 하위 파츠도 다 파괴할 수 있기도 하고
    
    그럼 또 여기서 문제 하나가 더 있는데 최상위에서는 모든 Child를 갖고 있을까? 음... 근데 모든 Child를 최상위에서 갖고 있으면 그걸 어떻게 표현하지?
    굳이 모든 Child를 가질 필요 없이, 진짜 신경계처럼 하면 되겠다. 모두 같은 명령 체계를 갖고 맨 위에서 차일드에게 명령을 내리면 차일드는 또 자기의 차일드에게 명령을 전달하는거야 오케이 오케이!
     
     */
    public GameObject mostParentPart;
    public GameObject parentPart;
    public List<GameObject> childParts = new List<GameObject>();

    public int health;

    public enum signalType
    {
        damage,

    }
    void Start()
    {

        //생성되면 가장 먼저 자동으로 부모 등록...잠 깐만...? Joint로 등록된 얘들은 서로 부모관계를 가지면 안되나?
        //생각해보니... 원래 파츠는 이렇게 따로 되어있고 본만 저렇게 짜로 되어있는거니까 괜찮나? 그럼 부모를 검사해서 하는 게 아니라 다르게 해야겠구만...
        //파츠에 id를 붙일...아니야 안돼. 음... 아니지! 아니지!!!! 그 그거다 Connected Rigidbody를 통해 부모를 찾으면 되는거잖아~! 난 천잰가~
        FindParent();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         부모 파츠가 본체 파츠인지 확인.
         그 이후 뭐 알아서 처리
         */
        
    }
    void FindParent()
    {
        
        //부모 오브젝트(Hinge Joint 2d의 Connected Rigidbody) 받아오기
        if (GetComponent<HingeJoint2D>())
        {
            parentPart = null; // 부모 초기화
            GameObject parentObject = GetComponent<HingeJoint2D>().connectedBody.gameObject;

            parentPart = parentObject;
            parentPart.GetComponent<DamagedCharacterPart>().AddChild(transform.gameObject);

        }
    }
    public void AddChild(GameObject child)
    {
        childParts.Add(child);
    }

    public void Hit(int damage, RaycastHit2D hit)
    {
        transform.parent.GetComponent<DamagedCharacter>().Hit(damage, hit);
    }

}
