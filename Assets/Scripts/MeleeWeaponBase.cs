using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class MeleeWeaponBase : MonoBehaviour
{
    [HideInInspector]
    public MeleeWeaponInfo stat;
    [HideInInspector]
    public GameObject user;
    [HideInInspector]
    public bool isAttacking;


    [HideInInspector]
    public BaseState userBaseState;//나중에 막 빙의해서 싸우는 게임 만들어도 재밌겠다 적을 내가 조종하는거지그 슈퍼-핫 처럼
    [HideInInspector]
    public List<GameObject> attackedTargets = new List<GameObject>();
    //잠깐... 이럼 적이 이 무기를 들면 또 에바잖아. 난 적한테도 플레이어랑 똑같이 적용시켜주고 싶은데
    //아 user로 해가지고 들고있는 사람을 지칭하게 할까? 어차피 이건 상속 줘야하니까... 음


    //음 근데 얘는 애니메이션을 플레이어가 재생해야하잖아
    //그냥 이벤트 말고 여기서 아에 처리할까?
    private void Start()
    {

    }
    public virtual void AttackStart()
    {
        ResetAttackedTargets();
        isAttacking = true;

    }
    public virtual void AttackEnd()
    {
        isAttacking = false;//이 is Attacking은 공격 도중에 검에서 파티클 재생이나 그런 걸 담당하기 위한 value, 만약 

    }
    public virtual void CollisionCheckStart()//이걸 따로 만든 이유는, 애니메이션 중간에 데미지가 안들어가야 하는 때를 구분해주기 위해서, NEED FIX COLLIDER TYPE
    {
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Collider2D>().isTrigger = true;
    }
    public virtual void CollisionCheckEnd()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().isTrigger = false;
        ResetAttackedTargets();
    }

    void Bash()
    {
        //여기서 무기 휘두르는 것들을 다 만들고, 여타 효과들을 이걸 인용해간 스크립트에서 짜는걸로   
        //뭐... 총도 나중에 필요하면 그렇게 하고.

    }
    public virtual void InitWeaponUser(GameObject character)
    {
        //need Character GameObject who use this weapon
        user = character;
        if (user.transform.CompareTag("Player"))
        {
            userBaseState = user.GetComponent<PlayerState>().baseState;
        }
        else if (user.transform.CompareTag("Enemy"))
        {
            userBaseState = user.GetComponent<EnemyBase>().baseState;
        }

    }
    public void ResetAttackedTargets()
    {
        attackedTargets = new List<GameObject>();
    }

    public virtual bool CheckAndAddAttackedTargets(GameObject target)
    {

        if (!attackedTargets.Contains(target))
        {
            attackedTargets.Add(target);
            return true;
        }
        return false;
    }

    public virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (!CustomUtility.C_CheckFactionAllie(user.gameObject, collision.gameObject))
        { //적일 경우
            if (CheckAndAddAttackedTargets(collision.transform.gameObject))
            {
                Debug.Log("피격");
                RaycastHit2D hit = default(RaycastHit2D);
                //hit.point = Vector3.positiveInfinity;
                hit.point = collision.transform.position;
                collision.GetComponent<DamagedCharacter>().Hit(10, hit);
            }
        }
    }
    public virtual void PushDamage()
    {

    }


}
