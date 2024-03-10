using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedCharacterPart : MonoBehaviour
{
    // Start is called before the first frame update

    /*
     �� ��ũ��Ʈ�� ����.
     �θ� ������ ã�ư���, �κ� ������ 0�� �� �� ���ع�����, �� ���ݹ��� ������ ���� ǥ�ø� ���ִ� ��ũ��Ʈ
     
    ���:
    �θ�ã��
    ���� ChildParts�� ���� ������ �ʿ��Ѱ�? ��
    ���߿� �ѹ��� Ȯ���ϰų� �׷� ������ CHild Parts�� ��� ���� ������ ���� ����... �ش� ������ �ı��Ǿ��� �� ���� ������ �� �ı��� �� �ֱ⵵ �ϰ�
    
    �׷� �� ���⼭ ���� �ϳ��� �� �ִµ� �ֻ��������� ��� Child�� ���� ������? ��... �ٵ� ��� Child�� �ֻ������� ���� ������ �װ� ��� ǥ������?
    ���� ��� Child�� ���� �ʿ� ����, ��¥ �Ű��ó�� �ϸ� �ǰڴ�. ��� ���� ��� ü�踦 ���� �� ������ ���ϵ忡�� ����� ������ ���ϵ�� �� �ڱ��� ���ϵ忡�� ����� �����ϴ°ž� ������ ������!
     
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

        //�����Ǹ� ���� ���� �ڵ����� �θ� ���...�� ��...? Joint�� ��ϵ� ����� ���� �θ���踦 ������ �ȵǳ�?
        //�����غ���... ���� ������ �̷��� ���� �Ǿ��ְ� ���� ������ ¥�� �Ǿ��ִ°Ŵϱ� ������? �׷� �θ� �˻��ؼ� �ϴ� �� �ƴ϶� �ٸ��� �ؾ߰ڱ���...
        //������ id�� ����...�ƴϾ� �ȵ�. ��... �ƴ���! �ƴ���!!!! �� �װŴ� Connected Rigidbody�� ���� �θ� ã���� �Ǵ°��ݾ�~! �� õ�鰡~
        FindParent();
    }

    // Update is called once per frame
    void Update()
    {
        /*
         �θ� ������ ��ü �������� Ȯ��.
         �� ���� �� �˾Ƽ� ó��
         */
        
    }
    void FindParent()
    {
        
        //�θ� ������Ʈ(Hinge Joint 2d�� Connected Rigidbody) �޾ƿ���
        if (GetComponent<HingeJoint2D>())
        {
            parentPart = null; // �θ� �ʱ�ȭ
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
