using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InteractionInterface;

public class BadGuy : MonoBehaviour, IInteraction
{
    // Start is called before the first frame update

    //NEED FIX : 이걸 개인 스크립트가 아니라 Base로 이식하기
    Rigidbody rg;

    [SerializeField]
    private Vector3 velocity;
    
    [SerializeField]
    private GameObject isGroundChecker;

    public float moveSpeedMultiply = 3;

    public void ActivateTrigger(GameObject user = null) 
    {
        //대사 띄우기
        user.GetComponent<PlayerInteraction>().RunDialog("Test_AnyComment");

    }
    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //근데 따로해도 문제될 건 없잖아 그치? 그럼 따로 두는게 맞다고 본다?근데 또 다른 스크립트에 접근하려면 GetComponent해가면서 자원 낭비잖아. 일단 그럼 한군데다가 구현해놓고 
        //너무 갔다 싶으면 분할하면 되겠지 뭐 ㅇㅋ ㄱ
        if (Input.GetKey(KeyCode.H))
        {
            MoveHorizontal(-1);
        }
        else if (Input.GetKey(KeyCode.K))
        {
            MoveHorizontal(1);
        }
        else if (Input.GetKey(KeyCode.U))
        {
            Jump();
        }

    }

    void MoveHorizontal(int direction)//X-axis Move
    {
        velocity.x = direction*moveSpeedMultiply;
    }
    void Jump()
    {
        if (isGroundChecker.GetComponent<IsGroundChecker>().isGrounded)
        {
            velocity.y = 40;//NEED CHANGE 점프 파워 하드코딩 해제
        }
        
    }
    void AddGravity()
    {
        velocity.y -= 9.8f;
        if (isGroundChecker.GetComponent<IsGroundChecker>().isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
    }
    private void FixedUpdate()
    {
        AddGravity();
        rg.MovePosition(rg.position + (Vector3) velocity * Time.deltaTime);
    }//하고 여기다가 애니메이션이랑 그런 것 좀 첨가해주면 완-벽하겠구만
    //그럼...이제 공격도 만들어볼까? 일단 애니메이션 먼저 줘보자...라고 생각했지만? 밥좀 먹어야징
}
