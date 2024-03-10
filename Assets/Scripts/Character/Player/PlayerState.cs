using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [Header("Base Status")]
    public string Nickname;
    public BaseState baseState;
    public bool isWatchingRight;//for Player Direction Check, true = right, false = left
    public float searchCircleSize;
    public enum CameraView
    {
        FirstView,
        ThirdFreeView,
        ThirdFixedView,
        SideView,
        QuarterView,
        TopView
    }
    public CameraView cameraView;
    //아 마우스 민감도랑 카메라 민감도를 따로두자 사람마다 다를 수 있자낭
    public float mouseSensitivity;
    public float wheelSensitivity;

    //battle status
    //내가 생각하는게 이게... 아 아하 데미지 받는 스크립트를 두고 그냥 마지막에 if문 하나만 해서 플레이어면 GetComponent PlayerState하고 Enemy면 GetCom~ Enemy하고 하면 되겠구나





    //equipment status
    [Header("Equipment Status")]
    public int nowRangedWeaponNum;
    public GameObject nowRangedWeapon;

    public int nowMeleeWeaponNum;
    public GameObject nowMeleeWeapon;

    public GameObject nowHand;

    [Header("Checker Bools")]
    //자-암깐... 생각해보면 이건 Enemy들도 갖고 있는 상태 아닌가? 그럼 Base로 보내줘야지. 최대한 Base로 보낼 수 있는 걸 보내둬야 나중에 '적이 조준 중이면 무기를 빼앗는다' 같은 걸 만들기도 좋고, '적이 움직일 수 없다' 란 것도 설정하기 좋잖아.
    public bool isOpenInventory;

    [Header("Battle Values")]
    public Vector3 aimPosition;
    public float rangeFirerate;
    public int meleeAnimationCount;//for melee animation
    public int combo;//all attack
    public float meleeComboWaitingTime;
    public float meleeComboDelay; //Delay between Attack and Attack

    //잠깐, 이러면 DialogController를 개인으로 두고... 아니 잠깐, UI는 개인이잖아? 공용이기도 하지만, 
    //음... 아? Dialog에서 발동이나 종료를 아에 UI가 아니라 Player 측에서 처리하고, UI 수정 요청을 보내는거야 오 그럼 되는 거 아닌가?
    //그리고 생각해보니까 WorldOption엑서 받아오면 안되네, 이건 세계설정이 아니라 플레이어 설정에서 받아왔어야 하는 거야
    [Header("Dialog Values")]

    //NEED FIX, 설정 파일에서 값 받아오도록.
    public int defaultDialogDelay;
    public int autoSkipDelay;
    public bool isAutoSkip;
    public bool isStopDialogSkip;
    public bool isOnDialog = false;

    [Header("Linker")]
    public GameObject weaponControl;
    public GameObject dialogController;
    public GameObject playerCamera;

    [Header("DEBUG")]
    public AnimatorState[] State;



    //어음... 장비를... 인벤에 갖고 있다가 몇개만 즉석에서 꺼내서 하잖아? 이 총의 현재 총알 수는 오브젝트에 저장할 텐데 그럼 이게 오브젝트를 복사해오는게 아니라 인벤에 있는걸 불러오는 식으로 해야할거아냐
    //그럼 여기서 GameObject를 따로 선언해서 보여주기보다는 인벤토리에 있는 GameObject한테 링킹 해야하는거...ㅇ아닌가..? 음... 뭔가 좀 어질어질하네 이게 좀 맘에 안드네

    private void Start()
    {
        baseState = Instantiate(baseState);


        dialogController.GetComponent<DialogController>().UpdateLocalValues(this.gameObject);

    }
    private void SetUpOption()//NEED ADD : 옵션 파일에서 기존 설정 불러오기 &&  저장하기
    {

    }
    public void PlayerStateUpdate()//굳이 반환 함수가 필요할까
    {

    }

    public void CameraViewUpdate()
    {
        GetComponent<PlayerMovement>().cameraView = cameraView;
        playerCamera.GetComponent<PlayerCam>().cameraView = cameraView;
        
    }

}

