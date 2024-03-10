using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class DamagedCharacter : MonoBehaviour
{
    /*
     This Function control the Object damage system. hit, dot damage, and die..?
     */
    // Start is called before the first frame update
    AudioSource audioSource;


    //for Shader
    private SpriteRenderer[] spriteRenderers;
    private Material[] materials;

    [SerializeField]
    private Color damageFlashColor = Color.white;

    private float damageFlashTime = 0.2f;
    private Coroutine damageFlashCorutine;

    public GameObject mainPart;

    void Start()
    {
        //이 함수를 DamagedCharacter말고 아 아니다 그냥 이대로 두자 대충 Damage Control Center느낌인거임
        audioSource = GetComponent<AudioSource>();

        InitDamageFlash();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CallDamageFlash();
        }
        DeathCheck();

    }

    //NEED FIX : Move another script Death Check Script
    private void DeathCheck()
    {
        BaseState state = null;
        if (transform.tag == "Player")
        {
            state = GetComponent<PlayerState>().baseState;
        }
        else if (transform.tag == "Enemy")
        {
            state = GetComponent<EnemyBase>().baseState;
        }
        else
        {
            state = GetComponent<ObjectBase>().baseState;
        }

        if (state.health <= 0)
        {
            //Change to Ragdoll
            ChangeToRagdoll();
        }
    }
    private void ChangeToRagdoll()
    {
        /*
         mainPart가 아까 그 지정한 거에서 머리부...인 느낌으로 하면 되려나
         
        저 Alway업데이트를 끄고, 

        해당 부위에 Collider, Rigidbody 적용...을... 본을 기준으로 자동 정렬을...함 해봐? 가능할 것 같은데
        본이 설정된걸 기준으로... 이걸 그럼 오히려...반대로... Hinge를 통해서 잡는 게 아니라, bone을 통해서 잡는거지.... 근데 이게 역산이 안되는 게 문젠데 bone에 sprite가 링크되어 있으면 좋은데 이건 sprite에 bone이 링크 되어있어서 음...
         아 ENemy끼리 충돌 안되게 해둬서 자기 몸에 충돌을 안하는구나 근데 이럼 각도를 정해주긴 해야하나...스으읍... 그럼 자동화의 꿈이 망가지는데


         */
        
        for(int i=0; i<transform.childCount;i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.GetComponent<PolygonCollider2D>())
            {
                transform.GetComponent<Collider2D>().enabled = false;
                child.GetComponent<SpriteSkin>().enabled = false;
                child.GetComponent<PolygonCollider2D>().enabled = true;
                child.GetComponent<Rigidbody2D>().isKinematic = false;
                child.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                child.GetComponent<Rigidbody2D>().simulated = true;
            }
        }
    } 


    public void Hit(int damage, RaycastHit2D hit)//이거 나중에 static type...뭐 그런거 선언해서 쓰기 string말고
    {
        int damageResult = 0;

        BaseState state = null;

        damageResult += damage;


        if (transform.tag == "Player")
        {
            state = GetComponent<PlayerState>().baseState;
        }
        else if (transform.tag == "Enemy")
        {
            state = GetComponent<EnemyBase>().baseState; 
        }
        else
        {
            state = GetComponent<ObjectBase>().baseState;//근데 이 BaseState는 결국 ScriptableObject가 될 거 잖아. 
        }

        CallDamageFlash();
        state.health -= damageResult;
        if(hit.transform != null)
        {
            Debug.Log(hit.transform.name + ", " + state.health);
        }
        else
        {
            Debug.Log(state.health);
        }
        //어차피 디버그니까, NEED FIX, Display 하는 방법을 좀 바꾸기. 말하자면 현재 타겟팅 된 적을 보여준다거나...그런 느낌인걸로 아니 생각해보니까 어차피 몹 위에 띄울거잖아!
        //데미지를 준 다음에... Enemy나 Player의 경우 도트딜을 계산해야되니까 그대로 두더라도 Object의 경우는 괜히 연산을 더 많이 먹는 게 아닐까? 
        //나중에 잘 생각해보기 NEED THINK 
        HitEffect(state, hit);
    }
    
    private void HitEffect(BaseState state, RaycastHit2D hit)
    {
        //raycastHit2D방식과 그냥 방식 두가지로 나눠서 하는 게 맞을 것 같은데.


        //그냥 hit 자체를 넘겨주면 되는거잖아 생각해보니까
        //음... 근데 몹마다 소리를 지정할 순 없는 노릇이잖아?...음... 아닌가 음... 재질에 따라서...음... 아니다 그냥 일단 이케 하고 나중에 여러개 받아서 그중 하나 출력하는 걸로 바꾸자

        //대략... "소리", "넉백", "이펙트" 정도로 하는구나 스읍 일단 이펙트부터 줘볼까
        
        //힛 VFX는... 좀 수정이 많이 필요할 것 같다. 일단 파티클부터 제대로 만들고, 또 이 파티클을 제대로 출력시키고, 또 제대로 끄고, 또 힛 당한 부분도 명확히 시켜야해. 해당 position이랑 거기에 해당하는 부위도...
        //그래야 나중에 부위파괴, 그런 거에도 쓰고, 워...아무튼 그럼


        //hit Position initalize
        if(hit.point == null || hit.point == Vector2.positiveInfinity || hit.point == Vector2.negativeInfinity)
        {
            hit.point = transform.position;
        }
        

        if(state.hitSFX != null)
        {
            audioSource.clip = state.hitSFX;
            audioSource.Play();
        }
        if(state.hitVFX != null && hit.point != null)
        {
            GameObject hitVFX = Instantiate(state.hitVFX, hit.point, Quaternion.identity);
            
            hitVFX.GetComponent<ParticleSystem>().Play();
            //Destroy(hitVFX.gameObject, hitVFX.GetComponent<ParticleSystem>().main.duration);
            StartCoroutine(StopHitVFX(hitVFX));
            //hitVFX.GetComponent<ParticleSystem>().Emit(25);
        }


    }
    IEnumerator StopHitVFX(GameObject hitVFX)
    {
        yield return new WaitForSeconds(hitVFX.GetComponent<ParticleSystem>().main.duration);
        //hitVFX.GetComponent<ParticleSystem>().Stop();
        Destroy(hitVFX.gameObject);
    }


    //맞으면 하얗게 빛나는 거, flash white
    //Damage Flash Character
    //DamagedWhiteFlash
    private void InitDamageFlash()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        materials = new Material[spriteRenderers.Length];
        //자식 오브젝트 전부에게 메테리얼 할당
        for(int i= 0; i < spriteRenderers.Length; i++)
        {
            materials[i] = spriteRenderers[i].material;
        }
    }

    private void CallDamageFlash()
    {
        damageFlashCorutine = StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash()
    {
        SetDamageFlashColor();

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;

        //Lerp, 하얗게 되는게 서서히 변하도록
        while(elapsedTime < damageFlashTime)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / damageFlashTime));
            SetDamageFlashAmount(currentFlashAmount);
            yield return null;
        }
    }
    private void SetDamageFlashColor()
    {
        for (int i =0; i< materials.Length; i++)
        {
            materials[i].SetColor("_FlashColor", damageFlashColor);
        }
    }
    private void SetDamageFlashAmount(float amount)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_FlashLightAmount", amount);
        }
    }

   

}
