using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UIElements;
/*
Player defualt Movement
Jump, Walk, Run, etc.
 
 */


public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public float walkSpeed;
    public GameObject world;
    public GameObject playerCamera;


    //Input State
    public float maxSpeed;
    public float jumpPower;


    //Check State
    public Vector3 velocity; // Y
    public Vector3 direction; //X-Z
    public Vector3 directionVelocity; //X-Z

    public bool isGrounded;
    public bool isJumping;
    public float jumpDegree;
    public float maxJumpDegree;
    public float jumpSpeed;
    public float frictionFactor;
    public Rigidbody rg;

    public GameObject isGroundChecker;

    public float maxJumpCooltime;
    public float jumpCooltime;

    public PlayerState.CameraView cameraView;




    void Start()
    {
        rg = GetComponent<Rigidbody>();
        cameraView = GetComponent<PlayerState>().cameraView;//카메라 업데이트할 때마다 갖다주는걸로 하자 성능향상
    }

    void Update()
    {
        //JumpCheck
        GetState();

        if (Input.GetKey(KeyCode.Space) && isGrounded && !isJumping && jumpCooltime<=0) {//NEED ADD : 점프 대기시간을 넣자
            isJumping = true;
            isGrounded = false;
            //velocity.y = jumpPower;
            rg.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            /*
            if (rg.velocity.y < 0)
            {
                rg.AddForce(Vector3.up * jumpPower + Vector3.up * Mathf.Abs(rg.velocity.y), ForceMode.Impulse);
            }
            else
            {
                
            }*/
            jumpDegree = 0;
            jumpCooltime = maxJumpCooltime;
        }
        if (isGrounded) {
            isJumping = false;
        }
        /*
        if(isJumping && !Input.GetKey(KeyCode.Space)) 
        {
            isJumping = false;
        }
        else if (isJumping)
        {
            jumpDegree += jumpSpeed * Time.deltaTime;
        }
        if (jumpDegree > maxJumpDegree)
        {
            isJumping = false;
        } */
        //아 이걸 했던 이유가 그 점프 속도랑 최대치를 조절하기 위한거였네 원래 물리학을 따르면 높이 점프하려면 무조건 '빠르게' 올라가야 하니까 음...
        //일단 바닥을 뚫어버리는 이게 좀 그런데
        //게다가 이걸 넣었던 이유가 그거였지 참 그 점프를 꾹누르면 더 높게 뛰는거 아하!!!! 내가 이걸 왜했었나 했네
        //음...근데 생각해보니까 어...음... 아니면 그냥 점프는 그대로 주고, 상부에서 체공할 수 있게 하는 그런 능력을 나중에 넣는게 낫겠다.
        //원래 습 에반데 싶은 건 피하랬어!
        //Gravity();


        jumpCooltime -= Time.deltaTime;




        //Get Input 음...이것도 분리해야하나? 아냐 일단 점프는 스페이스바에만 귀속시켜두자
        if (cameraView == PlayerState.CameraView.FirstView || cameraView == PlayerState.CameraView.ThirdFixedView)
        {
            FirstViewInput();
        }
        else if(cameraView == PlayerState.CameraView.ThirdFreeView)
        {
            ThirdFreeViewInput();
        }

        
        
        

    }

    // Update is called once per frame
    void FixedUpdate()
    {


        //Jump
        //Gravity(); //MovePosition을 쓸때만 필요한 녀석
        Move();
        GroundFriction();

        //점프를 점점 올라가게 해서 최대치에 해당하면 바꾸게 해버리는거야





    }
    private void GetState()
    {
        isGrounded = isGroundChecker.GetComponent<IsGroundChecker>().isGrounded;
    }
    

    //각 화면 형태에 맞게 이동 '방향'받기... INPUT
    private void FirstViewInput()
    {
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        direction = transform.forward * vertical + transform.right*horizontal;
    }
    private void ThirdFreeViewInput()
    {
        //카메라 좌표를 받아와서
        float vertical = Input.GetAxisRaw("Vertical");
        float horizontal = Input.GetAxisRaw("Horizontal");
        //이게 y성분만 빼고 normalize해주면 되는거긴 한데 음... 뭔가... 
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;
        forward.y = 0;
        right.y = 0;

        direction = forward.normalized * vertical + right.normalized * horizontal;
        //아하 이미 normalize된거 주는구나 다행다행
        //음... freecam은 이동 방향을 무브에서 줘야하나 아니 잠깐, 그냥 모든 이동을 할 때 방향을 여기서 주면 되네 생각해보니까?

        //그리고 이건 이제  위로 보는 걸 막으면 되거든  그러니까 1도씩 빼는느낌인거지
        direction.y = 0;
    }
    private void SideViewInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector2(horizontal, 0);//잠깐 어차피 그럼 rotate도 여기서 받자
    }

    private void Rotate()
    {

    }


    //MOVE
    private void Move()
    {
        //방향곱해주기
        directionVelocity = direction * Time.deltaTime * walkSpeed;
        

        //방향에 따른 회전 시켜주기, NEED ADD : 플레이어의 의도에 의해 걷는 상황이 아닌 경우 걷기 모션이나 방향 X
        //freeCam과 같이 wasd자유적으로 움직이는 떄에만
        if(cameraView == PlayerState.CameraView.ThirdFreeView)
        {
            if(direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            
        }

        

        //일단, 그럼 x-z축으로 최대치를 제한해야하잖아? 그럼...
        //아 그리고 y축은 어차피 use Gravity로 하면 되니까 일단 지우고


        //왜지? 일단 이런 raycast를 쓰면 땅속으로 아주 약간만 파고들어도 무용지물이 되긴 해 음... movePosition...좀 하자가 많지?
        //velocity로...옮겨봐?
        rg.AddForce(directionVelocity, ForceMode.Impulse);

        Vector3 planeVelocity = rg.velocity;
        planeVelocity.y = 0;
        if (Vector3.Distance(Vector3.zero, planeVelocity) >= maxSpeed)//자...그럼 일단... velocity로 역탄젠트로 길이를 구하고? 그게 일정속도면 이제 제한걸고. 근데 이제한은 또... 아 그래 노멀라이즈 시키고 최대값에 해당하는 사인코사인 주면 되구나
        {
            //음...근데 그냥 그게 낫겠다
            //잠깐... 값이 같아지는 때가 있잖아 생각해보니까
            
            
            planeVelocity = rg.velocity.normalized * maxSpeed;
            rg.velocity = new Vector3(planeVelocity.x, rg.velocity.y, planeVelocity.z);
            
        }
        /*

        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        Debug.DrawRay(transform.position, Vector3.down * Mathf.Abs(velocity.y), Color.red);
        if (Physics.Raycast(ray, out hit, -velocity.y))
        {
            rg.AddForce(directionVelocity + new Vector3(0, hit.point.y - rg.position.y, 0), ForceMode.Impulse);
            //rg.MovePosition(rg.position + directionVelocity + new Vector3(0, hit.point.y - rg.position.y, 0));//아 이미 rg.position에 음수가 있자나 ㅋㅋㅋ;;;  

        }
        else
        {
            rg.AddForce(directionVelocity + velocity, ForceMode.Impulse);
            //rg.MovePosition(rg.position + directionVelocity + velocity);
        }
        */
        //음... 그러니까 movePositoin은 특정 좌표가 중요한 때에 쓰는거다 이말이지 근데 또 velocity면 좀 정교한 무브가 불가능하잖아




    }

    private void GroundFriction()
    {
        if (isGrounded)
        {
            rg.AddForce(-rg.velocity*frictionFactor/10, ForceMode.Impulse);
        }
    }


    //잠시 해야할 일이 생겨따!람쥐
    private void Gravity()  
    {
        /*if (isJumping)
        {
            velocity.y += jumpPower*Time.fixedDeltaTime*jumpSpeed;
        }*/
        if (isGrounded)
        {
            velocity.y = 0;
        }
        else
        {
            float gravity = world.GetComponent<WorldOption>().Gravity;
            velocity.y -= gravity * Time.deltaTime;
            if(velocity.y < -40)
            {
                velocity.y = -40;
            }
        }

        
            
    }
}
