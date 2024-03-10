using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public static class CustomUtility
{

    public static float C_GetRandomNormalDistribution()
    {

        return 0;
    }


    /// <summary>
    /// Checking Faction Aliement
    /// </summary>
    /// <returns>if target is allie, return true</returns>
    public static bool C_CheckFactionAllie(GameObject user, GameObject target)
    {
        BaseState userState = C_GetBaseState(user);
        BaseState targetState = C_GetBaseState(target);

        if(userState == null && targetState != null)
        {
            //이건 주인을 떠난 무기, 말하자면 그냥 굴러다니는 게 부딪힌 것
            //아니 잠깐 이건 동료 체크잖아 난 날린 무기 추돌 체크인줄 일단 이케만 해둬도 오류는 안뜰ㄷ즛.. NEED FIX
        }

        if(userState == null || targetState == null) 
        {
            return true; //아에 등록 안된 게 콜라이딩 되면 무시해주기... NEED FIX : 이렇게 되면 나중에 동료한테 휘둘렀을 때 힐되거나 그런 템에서 버그가 생길 듯, 바꾸기
        }
        if (userState.faction == targetState.faction)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static BaseState C_GetBaseState(GameObject target)
    {
        BaseState state = null;
        if(target == null)
        {

            return null;
        }
        if (target.transform.tag == "Player")
        {
            state = target.GetComponent<PlayerState>().baseState;
        }
        else if (target.transform.tag == "Enemy")
        {
            state = target.GetComponent<EnemyBase>().baseState;
        }
        else if (target.transform.tag == "Object")
        {
            state = target.GetComponent<ObjectBase>().baseState;
        }

        //이것도 사실 인터페이스로 만들면 됐던 것이 아닐까...
        return state;
    }





    
    //지금 내가 생각하는 건 여기, Static함수에서 저 DialogContoller를 미리 로드해두고, 이 링크를 건내주는거야...근데 이게 되나? 안되는데 음...
}
