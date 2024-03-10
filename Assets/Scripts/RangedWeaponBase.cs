using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class RangedWeaponBase : MonoBehaviour
{
    public RangedWeaponInfo statSource;
    public RangedWeaponInfo stat;
    //만약 maxAmmo 0이면 무한발, 예를 들어 뭐... 새총?
    // Start is called before the first frame update
    //이거 그냥 좌표값 하나하나 넣을까
    

    public GameObject muzzle;
    public GameObject fireFlash;
    public GameObject relativePositionObject;//이게 다른 무기에는 필요 없으려나? 그래 근접 무기는 어차피 애니메이션을 적용해줄거고 그러니까 음 일단... 여기만 있으면 될듯?

    private AudioSource audioSource;


    private Vector3 muzzlePosition;
    private Vector3 mousePosition;
    private float bulletAngle;
    //고쳐야...겠지? rightHandPosition 중복 문제 해결할 것 NEED FIX 밥좀 머곡옴 아 그리고 원래 하려던건 Relative Position 추가
    public Vector3 relativePosition;

    private LayerMask itemLayermask = 1 << 8;

    RaycastHit2D hit;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    //이 원거리 샷의 경우는 종류가 그렇게 많은 게 아니니까 그냥 이런 식으로 해도 되지...음음...
    //스으으으으읍 아니 생각만 하다 몇시간 간거야
    //후...완전 싹~다 갈아엎게 생겼구만
    public bool Shoot()
    {
        /*
         * if can't shooting, return false
         * 
         */
        if(stat.ammo <= 0)
        {
            if(stat.clickSound != null)
            {
                audioSource.clip = stat.clickSound;
                audioSource.volume = 1f;
                audioSource.Play();
            }
            return false;


        }
        
        if (stat.bulletSprite == null)//NEED CHANGE -> 샷 형태로
        {
            StartCoroutine(NormalShot());
            stat.ammo -= 1;
        }


        audioSource.clip = stat.fireSound;
        audioSource.volume = 1f; 
        audioSource.PlayOneShot(stat.fireSound);


        return true;
    }
    void GetAngular()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//NEED FIX 쓸모없는 코드
        muzzlePosition = muzzle.transform.position; //두번 가져오는 낭비 없애기 위해
        //bulletAngle = Mathf.Atan2(mousePosition.y - muzzlePosition.y, mousePosition.x - muzzlePosition.x) * Mathf.Rad2Deg;
    }
    IEnumerator NormalShot()
    {

        /*
         일반적으로 쏘는 거
        순서
        1. 궤적 설정
        - 마우스 위치
        2. 궤적 따라 raycast
        3. raycast에 닿은 적에게 데미지
        4. 그 적까지만큼(또는 벽, 또는 최대거리) 선 그리기
         그 어떤 양반이 Vector2보다 3로 해서 그냥 쓰는게 훨 낫다고 하는 걸 들은 바가 잇다...이마립니다.
         */
        GetAngular();
        bulletAngle = transform.rotation.eulerAngles.z;
        bulletAngle = Random.Range(-stat.bulletSpreadAngle, stat.bulletSpreadAngle) + bulletAngle;
        Vector3 direction = new Vector3(Mathf.Cos(bulletAngle * Mathf.Deg2Rad), Mathf.Sin(bulletAngle * Mathf.Deg2Rad), 0);
        hit = Physics2D.Raycast(muzzlePosition, direction, stat.range, ~itemLayermask); 
        //trajectory view
        GameObject trajectory = Instantiate(stat.trajectory, transform);
        LineRenderer trajectLine = trajectory.GetComponent<LineRenderer>();


        if (hit)
        {
            if (stat.bulletSprite == null)
            {
                trajectLine.SetPosition(0, muzzlePosition);
                trajectLine.SetPosition(1, hit.point);
                /*
                if(hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Player") || hit.transform.CompareTag("파괴가능오브젝트")) {
                    hit.transform.gameObject.GetComponent<DamagedCharacter>().Hit(stat.damage, null, hit.point);
                }
                */
            }
        }
        else
        {
            if (stat.bulletSprite == null)
            {
                trajectLine.SetPosition(0, muzzlePosition);
                trajectLine.SetPosition(1, muzzlePosition + direction * stat.range);
            }//이정도면 밝은데에선 별로 눈에 안띄고 어두운데선 잘 띄고 적당한듯?
        }

        //fire flash 근데 소리가 나자마자 맞는게 좀 그렇다
        GameObject fireFlash_ = Instantiate(fireFlash, transform);
        fireFlash.transform.position = muzzle.transform.position;
        fireFlash_.SetActive(true);

        
        yield return new WaitForSeconds(0.02f);
        Destroy(trajectory);

        if (hit)
        {
            if (hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Player"))// || hit.transform.CompareTag("파괴가능오브젝트")
            {
                if (hit.transform.gameObject.GetComponent<DamagedCharacter>())
                {
                    hit.transform.gameObject.GetComponent<DamagedCharacter>().Hit(stat.damage, hit);
                }
                else if(hit.transform.gameObject.GetComponent<DamagedCharacterPart>())
                {
                    Debug.Log("kick");
                    hit.transform.gameObject.GetComponent<DamagedCharacterPart>().Hit(stat.damage, hit);
                }
                
            }
        }

        yield return new WaitForSeconds(0.07f);
        Destroy(fireFlash_);
        fireFlash_ = null;
        trajectory = null;
        //만약 커스텀 텍스쳐를 사용하지 않는다면



    }
   
}
