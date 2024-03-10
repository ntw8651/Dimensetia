using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class PlayerWeaponControl : MonoBehaviour
{

    public GameObject LHand;
    public GameObject RHand;
    public GameObject player;

    private AudioSource audioSource;
    private PlayerState playerState;



    private Animator animator;
    private AnimationClip[] animationClips;


    private bool isPressAttackKeyMelee = false;
    private bool isPlayingAttackAnimation = false;

    private IEnumerator meleeAttackAnimationEndCorutine;
    private IEnumerator turnOffIsAttackingCorutine;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerState = player.GetComponent<PlayerState>();


        //for animation


    }

    // Update is called once per frame
    void Update()
    {
        //need in Update
        if (playerState.nowHand != null)
        {
            //rangedWeapon
            RangedWeaponReload();
            RangedWeaponTrigger();
            RangedWeaponDecreaseFirerate();
            RangedWeaponAimState();


            //meleeWeapon
            MeleeAttack();
        }


        //Set State
        

        
        

        
        

    }



    //Melee Weapon
    public void PlayerMeleeComboAnimationSet()
    {
        
        animator = player.GetComponent<Animator>();

        animationClips = playerState.nowHand.GetComponent<ItemBase>().item.meleeWeaponInfo.comboAnimations;
        AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);
        

        //AnimationClipOverrides clipOverrides = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        //일단, 멕스 콤보 카운트 정해주고. 애니메이션 넣어주기까지. 근데 맥스 콤보 카운트는 그냥 때마다 호출하면 안되나 1번 계산할거 맨날 리스트 길이배만큼 더되는거긴 한데...
        for (int i = 0; i< animationClips.Length; i++) 
        {
            string text = "MeleeCombo_" + (i + 1).ToString();
            aoc[text] = animationClips[i];
        }
        animator.runtimeAnimatorController = aoc;
        //playerState.meleeAnimationCount = 0;
    }
    private void MeleeAttack()
    {
        if(Input.GetMouseButtonDown(0) && playerState.nowHand.GetComponent<ItemBase>().item.type == Item.Type.MeleeWeapon)
        {
            //선입력
            isPressAttackKeyMelee = true;
            
        }


        
        if(isPressAttackKeyMelee && !isPlayingAttackAnimation)
        {
            //나중에는 아에 내부 bool값으로 조정하자 type확인을
            //그게 성능에 더 좋을 듯
            //아니 생각해보니까 이걸 0에서 다시 시작하는게 아니라 아에 끝냈다가 시작하면 되는건가? 뭐 정 이상하면 나중에 delay애니메이션 하나 넣어주면 되는거고?
            if (turnOffIsAttackingCorutine != null)
            {
                StopCoroutine(turnOffIsAttackingCorutine);
            }
            if (meleeAttackAnimationEndCorutine != null)
            {
                StopCoroutine(meleeAttackAnimationEndCorutine);
            }
            


            playerState.meleeAnimationCount += 1;
            playerState.baseState.isAttacking = true;
            if(playerState.meleeAnimationCount > playerState.nowHand.GetComponent<ItemBase>().item.meleeWeaponInfo.comboAnimations.Length)
            {
                //1로 돌아가기
                playerState.meleeAnimationCount = 1;
            }

            //애니메이션 State 설정해주기
            playerState.baseState.isAttacking = true;
            player.GetComponent<Animator>().SetBool("isAttacking", true);
            player.GetComponent<Animator>().SetInteger("MeleeCombo", playerState.meleeAnimationCount);
            
            
            playerState.nowHand.GetComponent<MeleeWeaponBase>().AttackStart();
            playerState.nowHand.GetComponent<MeleeWeaponBase>().CollisionCheckStart();//콜라이더 켜주기


            //애니메이션 끝나기 캔슬

            isPlayingAttackAnimation = true;
            isPressAttackKeyMelee = false;





            
            

            //각종 애니메이션 끝 설정
            float animationLength = animationClips[playerState.meleeAnimationCount-1].length;


            //meleeAttackAnimationEndCorutine = MeleeAttackAnimationEnd(animationLength + playerState.meleeComboDelay);//현재 시작될 애니메이션 길이만큼 후, 애니메이션 끝났다고 말해주기(= 이벤트랑 비슷함)
            //StartCoroutine(meleeAttackAnimationEndCorutine);
            

            //지금 이게 그게 문제네, 3이 실행도 안되고 끝도 안났는데 이미 한걸로 처리되는거야

            //지금은 또 1에서 1이 재생도 안됐는데 2로 되가지고 연달아 재생이 되버리네 아하 저게 생기던게 3애니메이션에서 실제 필요보다 더 많은 시간을 잡아먹어서 그런거였나본데?
        }
        else if (!isPlayingAttackAnimation)
        {
            //끝...인데 일단은 TurnOffIsAttackng함수로 사용중
        }
    }
    public void MeleeAttackEnd()
    {
        isPlayingAttackAnimation = false;
        turnOffIsAttackingCorutine = TurnOffIsAttacking(playerState.meleeComboWaitingTime);
        StartCoroutine(turnOffIsAttackingCorutine);//만약 일정 시간동안 말 없으면 전투자세 풀기

        
        playerState.nowHand.GetComponent<MeleeWeaponBase>().CollisionCheckEnd();//콜라이더 꺼주기
    }
    private IEnumerator MeleeAttackAnimationEnd(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPlayingAttackAnimation = false;
        
    }
    private IEnumerator TurnOffIsAttacking(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerState.baseState.isAttacking = false;//아 이양반은 아에 그거네? 콤보 어택 자체가 하나의 애니메이션으로 묶여있어...
        playerState.meleeAnimationCount = 0;
        player.GetComponent<Animator>().SetInteger("MeleeCombo", playerState.meleeAnimationCount);
        player.GetComponent<Animator>().SetBool("isAttacking", playerState.baseState.isAttacking);

        playerState.nowHand.GetComponent<MeleeWeaponBase>().AttackEnd();//무기 공격모드 종료
    }



    //Ranged Weapon

    private void RangedWeaponAimState()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (playerState.nowHand.GetComponent<ItemBase>().item.type == Item.Type.RangedWeapon)
            {
                playerState.baseState.isAimming = true;
            }

        }
        else if (Input.GetMouseButtonUp(1))
        {
            playerState.baseState.isAimming = false;
        }
    }
    private void RangedWeaponDecreaseFirerate()
    {
        if (playerState.rangeFirerate > 0)
        {
            playerState.rangeFirerate -= Time.deltaTime;
        }
    }
    
    private void RangedWeaponTrigger()
    {
        if (playerState.baseState.isAimming)
        {
            if (Input.GetMouseButton(0) && playerState.nowHand.GetComponent<RangedWeaponBase>().stat.canContinue)
            {
                RangedWeaponFire();
            }
            else if (Input.GetMouseButtonDown(0) && !playerState.nowHand.GetComponent<RangedWeaponBase>().stat.canContinue)
            {
                RangedWeaponFire();//아 생각해보니까 그냥 위에 if문 넣고 return해주면 되는...이 아니구나 이게 맞지
            }
        }
    }

    private void RangedWeaponFire()
    {
        //player hands check
        //if(playerState.nowHand.GetComponent<Item>())

        //Fire Range 나중에 여기에 '원거리 무기를 들고있는 경우' 겜하고싶따
        //NEED FIX : 탄약 감소를 총 내부로 넣기... 그냥 여기 싹다 총 내부로 넣기 스크립트 짤때는 실제처럼. 그러니까 사람은 방아쇠만 당기잖아? 총알은 총에서 나가고. 그래야 나중에 내가 편함
        if (playerState.rangeFirerate <= 0)
            {
                if (playerState.nowHand.GetComponent<RangedWeaponBase>().Shoot()) //fire succes
                {
                    playerState.rangeFirerate = player.GetComponent<PlayerState>().nowHand.GetComponent<RangedWeaponBase>().stat.firerate;
                }
            }

        
    }
    private void RangedWeaponReload()
    {
        //Reload need if player put a gun on hand
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (playerState.nowHand.GetComponent<RangedWeaponBase>().stat.ammo < playerState.nowHand.GetComponent<RangedWeaponBase>().stat.maxAmmo)
            {
                //need Motion
                playerState.nowHand.GetComponent<RangedWeaponBase>().stat.ammo = playerState.nowHand.GetComponent<RangedWeaponBase>().stat.maxAmmo;
            }
        }
    }


    //E T C
    public void PutHandOnWeapon()
    {
        if (playerState.nowHand != null)
        {
            if (playerState.nowHand.GetComponent<ItemBase>().rightHand != null)
            {
                //NEED FIX : trasnform값을 지속적으로 가져오는 거랑 값에 넣어놓고 가져오는거 속도 비교해서 이득인걸로 교체. 안해도 큰 지장은 없긴 할듯
                Vector3 RHandPosition = playerState.nowHand.GetComponent<ItemBase>().rightHand.transform.position;
                if (RHandPosition != Vector3.zero)
                {
                    RHand.SetActive(true);
                    RHand.transform.position = RHandPosition;
                }
            }
            else
            {
                RHand.SetActive(false);
            }

            if (playerState.nowHand.GetComponent<ItemBase>().leftHand != null)
            {
                Vector3 LHandPosition = playerState.nowHand.GetComponent<ItemBase>().leftHand.transform.position;
                if (LHandPosition != Vector3.zero)
                {
                    LHand.SetActive(true);
                    LHand.transform.position = LHandPosition;
                }
            }
            else
            {
                LHand.SetActive(false);
            }
        }
        else
        {
            RHand.SetActive(false);
            LHand.SetActive(false);
        }

        
        //NEED FIX 이거 그냥 null Check로 SetActive(false)시키기


    }
    public void PutHandOutWeapon()
    {
        RHand.SetActive(false);
        LHand.SetActive(false);
    }
}
