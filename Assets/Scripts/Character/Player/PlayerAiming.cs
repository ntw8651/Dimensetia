using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAiming : MonoBehaviour
{
    // Start is called before the first frame update
    //

    public float armSize;
    public GameObject playerCenter;
    public GameObject player;
    public GameObject controlParent;
    

    private PlayerState playerState;

    
    void Start()
    {
        controlParent = transform.parent.gameObject;
        playerState = player.GetComponent<PlayerState>();
    }

    // Update is called once per frame ��... �׷� �� ���� ���� �͸��� �׳� ��~�� ������ �������� ���� ������ �ؼ� ���� �� �غ��� �ȵǸ� ������ ��
    void Update()
    {
        if (playerState.baseState.isAimming)
        {
            AimToMouse();
        }
        else
        {
            AimToWait();
        }
        
        
        //���� ���� ����ٸ�?

    }

    void AimToWait()
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), transform.localScale.z);
    }


    void AimToMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerPosition = playerCenter.transform.position;
        Vector3 muzzlePosition = playerState.nowHand.GetComponent<RangedWeaponBase>().muzzle.transform.position;

        playerState.aimPosition = mousePosition;

        mousePosition.z = 0;
        playerPosition.z = 0;

        Vector3 relativePosition = Vector3.zero;
        if (playerState.nowHand.GetComponent<RangedWeaponBase>().relativePositionObject)
        {
            relativePosition = playerState.nowHand.GetComponent<RangedWeaponBase>().relativePositionObject.transform.position - transform.position;

        }
        

        //������ �ѱ��� ��������
        
        float angle = Mathf.Atan2(mousePosition.y - playerPosition.y, mousePosition.x - playerPosition.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);



        //���⿡�� ��� ���� ��� �κ��� �������ֱ�. �װ��� ���ؼ� ������ arm size ����س���
        //�� ������ ��, ��ü�� �Բ� �������� �����̱�
        //���� �ָ��ִ� ���� �� �ִ�ġ�� �ǵ����ϸ� ���� ��?
        //�̰� �׳� �ʹ� �ָ����°� �����ΰ��ݾ�. �׷� arm MaxSize�� �÷��̾� �ִ� �ȱ��̶� �� ���� ���̸� ����ؼ� �ϸ� �Ǵ� �� �ƴѰ�? 
        //������ �̰� �غ���
        //�켱 �׷�... ���� ���̸� ���� ��������
        //�ϴ� �� ���̴� ���߿� ���¿� �°� �����ϰ� ������ �����ָ� �Ǵ°Ŵϱ� �ϴ� �ΰ� �ű⿡�ٰ� �װ� �ؼ� �ϴ°� ����
        //transform.position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * armMaxSize, Mathf.Sin(angle * Mathf.Deg2Rad) * armMaxSize, transform.position.z) + playerPosition;
        //�̰� ���� relativePosition�� �׳� ���ͼ� ����� �����ϱ�...
        
        
        
        float distance = Vector3.Distance(mousePosition, playerPosition);

        //�̰� �� �˻� �ʿ� NEED ADD

        //�Ѿ �ȱ��� ����

        float positionCollectionValue = Mathf.Max(
            Vector3.Distance(relativePosition, transform.position - playerState.nowHand.GetComponent<ItemBase>().rightHand.transform.position), 
            Vector3.Distance(relativePosition, transform.position - playerState.nowHand.GetComponent<ItemBase>().leftHand.transform.position)
            )
            - armSize;


        //�ƴ� �� �ʿ䵵 ���� positionCollectionValue�� �����ؾ� ���ݾ� �ٵ� ���ϴ� �ͺ��Ͱ� ���� �߸��Ǿ��� �Ҹ��� ����?
        //���� �� relativePosition�� �Ÿ��θ� �̿��Ѵܰ� �����ΰž� ���� �� ���� ��ɱ�
        /*
         1. relative Position�� ����ؼ� armMaxSize�� ���ϰ�, transform.position�� ���� �ְ� �� �ڿ� relativePosition�� ����ֱ�
         ���� ���� ���ϴ� �� �װ��ݾ�? �� relativePosition�̰� �ִ� �ֵ��� ������ relativePosition���� ��� �������� �ϴ� ��. ������ �յڷδ� ���� �Ϸ�� �ٵ� ���Ʒ��δ� ���� �����ΰ���?

        ���� �ѱ��� �Ʒ��� ���� �� �� ������� ������ �ֳ� �� �̰� �� ����?>
         �̰� �ƿ� ���ƾ���
        relative Position�� �ִٸ� relativePosition�� ���� ������ ��°ž� �������� �Ƿ���?
        ���ٸ�, �׳� ���°Ű�.
        �ƴ�... ���� �ٺ����� �߸��Ǿ���. �����̶� ������� �ϳ��� ���� ��� ó���Ϸ��� �� ���Ͱ� �߸��Ȱž�
        �翬�� �ϳ��� ��Ƹ� �������� ������ �ϳ��� relative�� �������� ������ �ϴµ� �̰� ������ ���� ���°��ݾ�.

        1. relative Position�� ���ٸ� �׳� ��Ƹ� �������� ��´�.
         2. �ִٸ� relative Position�� �������� ��´�.
        3. ������ ���� ��, �� ���̸� ����Ͽ� ��´�.

        4. �������� ���� ���� ���� angular���� ����ϰ� �����̼��� �������ش�...? ���, ���� ����� �ѱ� �����̾�� ���� ������? �¾� �ѱ� �����̾����
        5. �� ���̿� �°� ����. 
         */
        if (!playerState.nowHand.GetComponent<RangedWeaponBase>().relativePositionObject)
        {
            if (distance <= armSize)//NEED FIX : armMaxSize ����ؼ� �÷��̾� ������ �ֱ� �� ���� �������� ������ �̰� scale �ݿ��Ǿ �׷��ǰ�? ���� �������� �� �����ؼ� ����Ϸ���...
            {
                transform.position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * (distance - positionCollectionValue), Mathf.Sin(angle * Mathf.Deg2Rad) * (distance - positionCollectionValue), transform.position.z) + playerPosition;
            }
            else
            {
                transform.position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * (armSize - positionCollectionValue), Mathf.Sin(angle * Mathf.Deg2Rad) * (armSize - positionCollectionValue), transform.position.z) + playerPosition - relativePosition;
            }
        }
        else
        {
            transform.position = playerPosition - relativePosition;
        }
        //new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * (armSize - positionCollectionValue), Mathf.Sin(angle * Mathf.Deg2Rad) * (armSize - positionCollectionValue), transform.position.z)


        //Lock At Mouse, ���콺������ ���� ��ȯ.

        playerPosition = player.transform.position;
        float direction = mousePosition.x - playerPosition.x ;
        int directionNormal = (int)Mathf.Sign(direction);
        player.transform.localScale = new Vector3(Mathf.Abs(player.transform.localScale.x) * directionNormal, player.transform.localScale.y, player.transform.localScale.z);

        //Gun Flip
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * directionNormal, Mathf.Abs(transform.localScale.y) * directionNormal, transform.localScale.z);

    }
    
}
