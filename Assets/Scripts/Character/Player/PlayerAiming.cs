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

    // Update is called once per frame 습... 그래 머 저기 나온 것마냥 그냥 겁~나 빠르게 피하지도 못할 정도로 해서 하자 머 해보고 안되면 뒤집지 뭐
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
        
        
        //만약 총을 들었다면?

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
        

        //각도는 총구를 기준으로
        
        float angle = Mathf.Atan2(mousePosition.y - playerPosition.y, mousePosition.x - playerPosition.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);



        //무기에다 어깨 끝이 닿는 부분을 지정해주기. 그것을 통해서 적당한 arm size 계산해내기
        //총 조준할 때, 상체도 함께 기울어지기 움직이기
        //가장 멀리있는 손이 딱 최대치가 되도록하면 좋을 듯?
        //이게 그냥 너무 멀리가는게 문제인거잖아. 그럼 arm MaxSize를 플레이어 최대 팔길이랑 저 무기 길이를 고려해서 하면 되는 거 아닌가? 
        //오케이 이거 해보자
        //우선 그럼... 팔의 길이를 어케 가져오지
        //일단 팔 길이는 나중에 형태에 맞게 측정하고 스케일 곱해주면 되는거니까 일단 두고 거기에다가 그거 해서 하는걸 하자
        //transform.position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * armMaxSize, Mathf.Sin(angle * Mathf.Deg2Rad) * armMaxSize, transform.position.z) + playerPosition;
        //이게 지금 relativePosition을 그냥 따와서 생기는 문제니까...
        
        
        
        float distance = Vector3.Distance(mousePosition, playerPosition);

        //이거 손 검사 필요 NEED ADD

        //넘어간 팔길이 보정

        float positionCollectionValue = Mathf.Max(
            Vector3.Distance(relativePosition, transform.position - playerState.nowHand.GetComponent<ItemBase>().rightHand.transform.position), 
            Vector3.Distance(relativePosition, transform.position - playerState.nowHand.GetComponent<ItemBase>().leftHand.transform.position)
            )
            - armSize;


        //아니 볼 필요도 없이 positionCollectionValue는 일정해야 하잖아 근데 변하는 것부터가 뭔가 잘못되었단 소리지 뭐지?
        //지금 이 relativePosition을 거리로만 이용한단게 문제인거야 팔은 또 왤케 긴걸까
        /*
         1. relative Position을 고려해서 armMaxSize를 정하고, transform.position을 갖다 넣고 그 뒤에 relativePosition을 집어넣기
         내가 지금 원하는 건 그거잖아? 저 relativePosition이게 있는 애들은 기준을 relativePosition으로 잡고 돌리도록 하는 것. 오케이 앞뒤로는 완전 완료다 근데 위아래로는 뭐가 문제인거지?

        지금 총구가 아래를 향할 때 좀 길어지는 문제가 있네 음 이건 또 왜지?>
         이거 아에 갈아엎자
        relative Position이 있다면 relativePosition을 기준 축으로 삼는거야 이정도면 되려나?
        없다면, 그냥 가는거고.
        아니... 뭔가 근본부터 잘못되었어. 권총이랑 기관총을 하나로 같이 묶어서 처리하려는 것 부터가 잘못된거야
        당연히 하나는 어꺠를 기준으로 돌리고 하나는 relative를 기준으로 돌리려 하는데 이게 성립할 리가 없는거잖아.

        1. relative Position이 없다면 그냥 어꺠를 기준으로 잡는다.
         2. 있다면 relative Position을 기준으로 잡는다.
        3. 기준을 잡을 때, 팔 길이를 고려하여 잡는다.

        4. 기준으로 잡은 것을 통해 angular값을 계산하고 로테이션을 지정해준다...? 잠깐, 각도 계산은 총구 기준이어야 하지 않을까? 맞아 총구 기준이어야해
        5. 팔 길이에 맞게 띄운다. 
         */
        if (!playerState.nowHand.GetComponent<RangedWeaponBase>().relativePositionObject)
        {
            if (distance <= armSize)//NEED FIX : armMaxSize 계산해서 플레이어 변수로 넣기 아 로컬 포지션을 잡으면 이게 scale 반영되어서 그런건가? 로컬 포지션은 좀 조심해서 써야하려나...
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


        //Lock At Mouse, 마우스쪽으로 몸통 전환.

        playerPosition = player.transform.position;
        float direction = mousePosition.x - playerPosition.x ;
        int directionNormal = (int)Mathf.Sign(direction);
        player.transform.localScale = new Vector3(Mathf.Abs(player.transform.localScale.x) * directionNormal, player.transform.localScale.y, player.transform.localScale.z);

        //Gun Flip
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * directionNormal, Mathf.Abs(transform.localScale.y) * directionNormal, transform.localScale.z);

    }
    
}
