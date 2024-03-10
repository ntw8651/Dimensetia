using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerCam : MonoBehaviour
{
    public Camera cam;
    public GameObject player;
    public GameObject playerCenter;
    public float camMoveSpeed;
    public float camRotateSpeed;

    public PlayerState.CameraView cameraView;//NEED CHANGE TO UPDATE

    //Camera Position
    public GameObject FirstViewCamera;
    public float CamViewRotateX, CamViewRotateY;
    public float cameraSensitive;//NEED OPTION (OPTION은 나중에 설정...Setting에서 넣어주기)


    public GameObject thirdFreeCamera;//굳이 평소에도 따라다닐 필요는 없긴 한데... 음
    public float thirdFreeCameraDistance;

    public GameObject thirdFixedCamera;
    public float wheelSensitivity;



    public float minThirdFreeCameraDistance;

    public Vector3 reboundPosition;
    public float reboundFactor;


    // Start is called before the first frame update
    void Start()
    {
        cameraView = player.GetComponent<PlayerState>().cameraView;
        UpdateControlSetting();
    }

    private void Update()
    {
        InputZoomInOut();
        if (cameraView == PlayerState.CameraView.FirstView)
        {
            FirstViewCam();
        }
        else if (cameraView == PlayerState.CameraView.ThirdFixedView)
        {
            ThirdViewFixedCam();
        }
        else if (cameraView == PlayerState.CameraView.ThirdFreeView)
        {
            ThirdViewFreeCam();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {


        /*
        if (player.GetComponent<PlayerState>().baseState.isAimming)
        {
            transform.position = Vector2.Lerp(transform.position, (playerCenter.transform.position * 5 + player.GetComponent<PlayerState>().aimPosition * 2) / 7, camSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
        else
        {
            transform.position = Vector2.Lerp(transform.position, playerCenter.transform.position, camSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        }
        */
    }

    void UpdateControlSetting()
    {
        wheelSensitivity = player.GetComponent<PlayerState>().wheelSensitivity;
    }

    void InputZoomInOut()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if(cameraView == PlayerState.CameraView.FirstView)
        {
            if(wheelInput < 0)
            {
                //3인칭으로 바꾸고 최소크기
                //이거 좀있다 스카이림 해보면서 3인칭 어떻게 전환되는지 좀 봐야겠다.
                
                thirdFreeCameraDistance = minThirdFreeCameraDistance;
                player.GetComponent<PlayerState>().cameraView = PlayerState.CameraView.ThirdFreeView;
                player.GetComponent<PlayerState>().CameraViewUpdate();
            }
        }
        else if(cameraView == PlayerState.CameraView.ThirdFreeView)
        {
            //WheelInput에 따라 변경
            thirdFreeCameraDistance -= wheelInput * wheelSensitivity;//일단은 그냥 두고
            if(thirdFreeCameraDistance < minThirdFreeCameraDistance)
            {
               
                player.GetComponent<PlayerState>().cameraView = PlayerState.CameraView.FirstView;
                player.GetComponent<PlayerState>().CameraViewUpdate();
            }
        }
    }

    void FirstViewCam()
    {
        CamViewRotateX += -Input.GetAxis("Mouse Y") * Time.deltaTime * cameraSensitive;
        CamViewRotateY += Input.GetAxis("Mouse X") * Time.deltaTime * cameraSensitive;

        CamViewRotateX = Mathf.Clamp(CamViewRotateX, -90, 90);

        transform.position = FirstViewCamera.transform.position;//아에 그냥...카메라를 여러개...달까...?

        //음...1인칭...카메라는... 이걸 transformPosition으로 따라가면... 버벅거릴 것 같은데
        

        Quaternion quatCam = Quaternion.Euler(new Vector3(CamViewRotateX, CamViewRotateY, 0));
        Quaternion quatPlayer = Quaternion.Euler(new Vector3(0, CamViewRotateY, 0));
        //생각해보니까 lerp도 그냥 일단은 플레이어 position에 귀속시켜두고 속도의 반대로 약간 무브시키면 되는거잖아..? 난 천잰가?..라곤 하지만 음음 일단 뭐... ㅇ러 뭐야 이런 방ㅂ벙이 있었네? 진짜 천잰가
        //그럼 이걸 플레이어 이동방...어... 아 그럼 플레이어 좌표계를 카메라 좌표계로 바꾸면 되는건가>

        //transform.rotation = Quaternion.Slerp(transform.rotation, quat, Time.deltaTime); //보간 움직임
        
        
        transform.rotation = quatCam;//어...잠깐,  음 firstView면...이걸... 몸을 움직이고 몸을 따라 카메라가 가는 식이 맞긴 하지
        player.transform.rotation = quatPlayer;
    }

    void ThirdViewFixedCam()
    {
        //This is for Aim Camera
        CamViewRotateX += -Input.GetAxis("Mouse Y") * Time.deltaTime * cameraSensitive;//NEED CHANGE
        CamViewRotateY += Input.GetAxis("Mouse X") * Time.deltaTime * cameraSensitive;

        CamViewRotateX = Mathf.Clamp(CamViewRotateX, -90, 90);

        transform.position = FirstViewCamera.transform.position;//아에 그냥...카메라를 여러개...달까...?

        //음...1인칭...카메라는... 이걸 transformPosition으로 따라가면... 버벅거릴 것 같은데
        //롤...하고싶으니...하는걸로...ㅎㅎ 내일 이어서!
        Debug.Log(player.transform.localEulerAngles.y);
        Quaternion quatCam = Quaternion.Euler(new Vector3(CamViewRotateX, CamViewRotateY, 0));
        Quaternion quatPlayer = Quaternion.Euler(new Vector3(0, CamViewRotateY, 0));
       


        //transform.rotation = Quaternion.Slerp(transform.rotation, quat, Time.deltaTime); //보간 움직임
       // transform.rotation = quatCam;//어...잠깐,  음 firstView면...이걸... 몸을 움직이고 몸을 따라 카메라가 가는 식이 맞긴 하지
        player.transform.rotation = quatPlayer;

        //

        //머야 이건 왜 fixedUpdate에 넣었지? 프레임이면 Update에 넣어야할텐데..?
        /*
        transform.position = Vector3.Lerp(transform.position, thirdFixedCamera.transform.position, camMoveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, quatCam, camRotateSpeed * Time.deltaTime);
         */
        transform.position = thirdFixedCamera.transform.position;
        transform.rotation = quatCam;
        //이게 약간 어음 아 일단 freeCam으로.

    }
    void ThirdViewFreeCam()
    {
        //NEED
        /*
         휠을 통해 thirdFreeCameraDistance 조절..? 일단 뭐 조절은 필요 없다고 두고
         
         */
        CamViewRotateX += -Input.GetAxis("Mouse Y") * Time.deltaTime * cameraSensitive;//NEED CHANGE
        CamViewRotateY += Input.GetAxis("Mouse X") * Time.deltaTime * cameraSensitive;

        CamViewRotateX = Mathf.Clamp(CamViewRotateX, -89, 89);

        Quaternion quatCam = Quaternion.Euler(new Vector3(CamViewRotateX, CamViewRotateY, 0));

        transform.rotation = quatCam;//아하 우 앞 상만 해뒀구나 아 어차피 rotate에서 기본 안가져오니까 괜찮네 ㅎㅎ!
        //transform.position = Vector3.Lerp(transform.position, thirdFreeCamera.transform.position - transform.forward * thirdFreeCameraDistance, camMoveSpeed * Time.deltaTime);

        //아니 근데 rebound에 lerp주면서 하면 똑같은 거 아닌가 결국?..음..
        //아니지, 3인칭에는 rebound가 필요없지 않나? 생각해보면...  일단 보류.
        //velocity 가져오기
        
        Vector3 cameraRebound = -player.GetComponent<PlayerMovement>().rg.velocity * reboundFactor;
        reboundPosition = Vector3.Lerp(reboundPosition, cameraRebound, camMoveSpeed * Time.deltaTime);
        Debug.Log(cameraRebound);
        transform.position = thirdFreeCamera.transform.position - transform.forward * thirdFreeCameraDistance + reboundPosition;
        //아에 플레이어 밑에 넣어버리면 편한데.... 난 카메라 하나로 다해먹기로 했으니까 뭐...
    }




}
