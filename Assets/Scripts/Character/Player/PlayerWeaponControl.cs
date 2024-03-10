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
        //�ϴ�, �߽� �޺� ī��Ʈ �����ְ�. �ִϸ��̼� �־��ֱ����. �ٵ� �ƽ� �޺� ī��Ʈ�� �׳� ������ ȣ���ϸ� �ȵǳ� 1�� ����Ұ� �ǳ� ����Ʈ ���̹踸ŭ ���Ǵ°ű� �ѵ�...
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
            //���Է�
            isPressAttackKeyMelee = true;
            
        }


        
        if(isPressAttackKeyMelee && !isPlayingAttackAnimation)
        {
            //���߿��� �ƿ� ���� bool������ �������� typeȮ����
            //�װ� ���ɿ� �� ���� ��
            //�ƴ� �����غ��ϱ� �̰� 0���� �ٽ� �����ϴ°� �ƴ϶� �ƿ� ���´ٰ� �����ϸ� �Ǵ°ǰ�? �� �� �̻��ϸ� ���߿� delay�ִϸ��̼� �ϳ� �־��ָ� �Ǵ°Ű�?
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
                //1�� ���ư���
                playerState.meleeAnimationCount = 1;
            }

            //�ִϸ��̼� State �������ֱ�
            playerState.baseState.isAttacking = true;
            player.GetComponent<Animator>().SetBool("isAttacking", true);
            player.GetComponent<Animator>().SetInteger("MeleeCombo", playerState.meleeAnimationCount);
            
            
            playerState.nowHand.GetComponent<MeleeWeaponBase>().AttackStart();
            playerState.nowHand.GetComponent<MeleeWeaponBase>().CollisionCheckStart();//�ݶ��̴� ���ֱ�


            //�ִϸ��̼� ������ ĵ��

            isPlayingAttackAnimation = true;
            isPressAttackKeyMelee = false;





            
            

            //���� �ִϸ��̼� �� ����
            float animationLength = animationClips[playerState.meleeAnimationCount-1].length;


            //meleeAttackAnimationEndCorutine = MeleeAttackAnimationEnd(animationLength + playerState.meleeComboDelay);//���� ���۵� �ִϸ��̼� ���̸�ŭ ��, �ִϸ��̼� �����ٰ� �����ֱ�(= �̺�Ʈ�� �����)
            //StartCoroutine(meleeAttackAnimationEndCorutine);
            

            //���� �̰� �װ� ������, 3�� ���൵ �ȵǰ� ���� �ȳ��µ� �̹� �Ѱɷ� ó���Ǵ°ž�

            //������ �� 1���� 1�� ����� �ȵƴµ� 2�� �ǰ����� ���޾� ����� �ǹ����� ���� ���� ������� 3�ִϸ��̼ǿ��� ���� �ʿ亸�� �� ���� �ð��� ��ƸԾ �׷��ſ�������?
        }
        else if (!isPlayingAttackAnimation)
        {
            //��...�ε� �ϴ��� TurnOffIsAttackng�Լ��� �����
        }
    }
    public void MeleeAttackEnd()
    {
        isPlayingAttackAnimation = false;
        turnOffIsAttackingCorutine = TurnOffIsAttacking(playerState.meleeComboWaitingTime);
        StartCoroutine(turnOffIsAttackingCorutine);//���� ���� �ð����� �� ������ �����ڼ� Ǯ��

        
        playerState.nowHand.GetComponent<MeleeWeaponBase>().CollisionCheckEnd();//�ݶ��̴� ���ֱ�
    }
    private IEnumerator MeleeAttackAnimationEnd(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPlayingAttackAnimation = false;
        
    }
    private IEnumerator TurnOffIsAttacking(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerState.baseState.isAttacking = false;//�� �̾���� �ƿ� �װų�? �޺� ���� ��ü�� �ϳ��� �ִϸ��̼����� �����־�...
        playerState.meleeAnimationCount = 0;
        player.GetComponent<Animator>().SetInteger("MeleeCombo", playerState.meleeAnimationCount);
        player.GetComponent<Animator>().SetBool("isAttacking", playerState.baseState.isAttacking);

        playerState.nowHand.GetComponent<MeleeWeaponBase>().AttackEnd();//���� ���ݸ�� ����
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
                RangedWeaponFire();//�� �����غ��ϱ� �׳� ���� if�� �ְ� return���ָ� �Ǵ�...�� �ƴϱ��� �̰� ����
            }
        }
    }

    private void RangedWeaponFire()
    {
        //player hands check
        //if(playerState.nowHand.GetComponent<Item>())

        //Fire Range ���߿� ���⿡ '���Ÿ� ���⸦ ����ִ� ���' ���ϰ�͵�
        //NEED FIX : ź�� ���Ҹ� �� ���η� �ֱ�... �׳� ���� �ϴ� �� ���η� �ֱ� ��ũ��Ʈ ©���� ����ó��. �׷��ϱ� ����� ��Ƽ踸 ����ݾ�? �Ѿ��� �ѿ��� ������. �׷��� ���߿� ���� ����
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
                //NEED FIX : trasnform���� ���������� �������� �Ŷ� ���� �־���� �������°� �ӵ� ���ؼ� �̵��ΰɷ� ��ü. ���ص� ū ������ ���� �ҵ�
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

        
        //NEED FIX �̰� �׳� null Check�� SetActive(false)��Ű��


    }
    public void PutHandOutWeapon()
    {
        RHand.SetActive(false);
        LHand.SetActive(false);
    }
}
