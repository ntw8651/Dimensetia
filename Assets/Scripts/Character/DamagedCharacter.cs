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
        //�� �Լ��� DamagedCharacter���� �� �ƴϴ� �׳� �̴�� ���� ���� Damage Control Center�����ΰ���
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
         mainPart�� �Ʊ� �� ������ �ſ��� �Ӹ���...�� �������� �ϸ� �Ƿ���
         
        �� Alway������Ʈ�� ����, 

        �ش� ������ Collider, Rigidbody ����...��... ���� �������� �ڵ� ������...�� �غ�? ������ �� ������
        ���� �����Ȱ� ��������... �̰� �׷� ������...�ݴ��... Hinge�� ���ؼ� ��� �� �ƴ϶�, bone�� ���ؼ� ��°���.... �ٵ� �̰� ������ �ȵǴ� �� ������ bone�� sprite�� ��ũ�Ǿ� ������ ������ �̰� sprite�� bone�� ��ũ �Ǿ��־ ��...
         �� ENemy���� �浹 �ȵǰ� �صּ� �ڱ� ���� �浹�� ���ϴ±��� �ٵ� �̷� ������ �����ֱ� �ؾ��ϳ�...������... �׷� �ڵ�ȭ�� ���� �������µ�


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


    public void Hit(int damage, RaycastHit2D hit)//�̰� ���߿� static type...�� �׷��� �����ؼ� ���� string����
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
            state = GetComponent<ObjectBase>().baseState;//�ٵ� �� BaseState�� �ᱹ ScriptableObject�� �� �� �ݾ�. 
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
        //������ ����״ϱ�, NEED FIX, Display �ϴ� ����� �� �ٲٱ�. �����ڸ� ���� Ÿ���� �� ���� �����شٰų�...�׷� �����ΰɷ� �ƴ� �����غ��ϱ� ������ �� ���� �����ݾ�!
        //�������� �� ������... Enemy�� Player�� ��� ��Ʈ���� ����ؾߵǴϱ� �״�� �δ��� Object�� ���� ���� ������ �� ���� �Դ� �� �ƴұ�? 
        //���߿� �� �����غ��� NEED THINK 
        HitEffect(state, hit);
    }
    
    private void HitEffect(BaseState state, RaycastHit2D hit)
    {
        //raycastHit2D��İ� �׳� ��� �ΰ����� ������ �ϴ� �� ���� �� ������.


        //�׳� hit ��ü�� �Ѱ��ָ� �Ǵ°��ݾ� �����غ��ϱ�
        //��... �ٵ� ������ �Ҹ��� ������ �� ���� �븩���ݾ�?...��... �ƴѰ� ��... ������ ����...��... �ƴϴ� �׳� �ϴ� ���� �ϰ� ���߿� ������ �޾Ƽ� ���� �ϳ� ����ϴ� �ɷ� �ٲ���

        //�뷫... "�Ҹ�", "�˹�", "����Ʈ" ������ �ϴ±��� ���� �ϴ� ����Ʈ���� �ຼ��
        
        //�� VFX��... �� ������ ���� �ʿ��� �� ����. �ϴ� ��ƼŬ���� ����� �����, �� �� ��ƼŬ�� ����� ��½�Ű��, �� ����� ����, �� �� ���� �κе� ��Ȯ�� ���Ѿ���. �ش� position�̶� �ű⿡ �ش��ϴ� ������...
        //�׷��� ���߿� �����ı�, �׷� �ſ��� ����, ��...�ƹ�ư �׷�


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


    //������ �Ͼ�� ������ ��, flash white
    //Damage Flash Character
    //DamagedWhiteFlash
    private void InitDamageFlash()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        materials = new Material[spriteRenderers.Length];
        //�ڽ� ������Ʈ ���ο��� ���׸��� �Ҵ�
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

        //Lerp, �Ͼ�� �Ǵ°� ������ ���ϵ���
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
