using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class DialogController : MonoBehaviour
{
    public string CSVFileName;

    [SerializeField]
    private Dictionary<string, Dictionary<int, Dialog>> dialogs = new Dictionary<string, Dictionary<int, Dialog>>();//이건 어차피 그거니까,
    [SerializeField]
    private Dictionary<int, Dialog> nowDialog;
    

    private int nowDialogId;


    //NEED FIX, 설정 파일에서 값 받아오도록.
    private int defaultDialogDelay;
    private int autoSkipDelay;
    private bool isAutoSkip;
    private float dialogPlaytime;
    private bool isStopDialogSkip;
    private GameObject player;

    public GameObject dialogNameTextbox;
    public GameObject dialogWindow;
    public GameObject dialogTextbox;
    public GameObject worldOptionController;
    public GameObject selectionPrefab;
    

    public void Start()
    {
        LoadDialogs();

    }

    public void Update()
    {
        CheckingDialog();
    }

    public void UpdateLocalValues(GameObject user)
    {
        defaultDialogDelay = user.GetComponent<PlayerState>().defaultDialogDelay;
        isAutoSkip = user.GetComponent<PlayerState>().isAutoSkip;
        autoSkipDelay = user.GetComponent<PlayerState>().autoSkipDelay;
        player = user;
    }

    public void LoadDialogs()
    {
        TextAsset csvData = Resources.Load<TextAsset>(CSVFileName);
        string csvText = csvData.text.Substring(0, csvData.text.Length - 1);
        string[] data = csvText.Split(new char[] { '\n' });
       
        Dictionary<int, Dialog> tempDialogs = new Dictionary<int, Dialog>();
        Dialog tempDialog;

        string eventName = "";


        for (int i=1; i< data.Length; i++)
        {
            string[] row = data[i].Split(new char[] { ',' });//마침 쉼표 기호를 내가 지정해뒀던 게 있으니까 그걸로 대체하면서, 또 쉼표가 나오면 약간 딜레이를 주는 효과도 줄 수 있겠다. 위기를 기회로~
            //아 이걸 딕셔너리로 만들까 딕셔너리로 만들면... 딱 id별로 찾아갈 수 있지 id를 번호따라 분류할 수 있게 한다면 나중에 한 대화가 좀 길어져도 괜찮을 것이고, 그치?
            //그럼 딕셔너리 안의 딕셔너리가 된다 이말이지
            
            if (row[1] == "End")//id end check 왜...지?
            {
                dialogs.Add(eventName, tempDialogs);
                tempDialogs = new Dictionary<int, Dialog>();
            }
            else if (row[1] == "")
            {
                //아무것도 없으면 그냥 넘기기를 해줘야해
            }
            else
            {
                if (row[0] != "")//id save to key 아 무조건 null은 아니게 보는거구나
                {
                    eventName = row[0];
                }
                tempDialog = new Dialog();

                //Get Values
                tempDialog.eventName = row[0];
                tempDialog.id = int.Parse(row[1]);
                tempDialog.speaker = row[2];
                tempDialog.script = TextEditToDialog(row[3]);
                tempDialog.leftImage = row[4];
                tempDialog.rightImage = row[5];
                tempDialog.spotLight = row[6];
                tempDialog.callFunction = row[7];

                int result_i;
                if (row[8] == "")
                {
                    tempDialog.jumpLine = -1;//null = -1.
                }
                else
                {
                    int.TryParse(row[8], out result_i);
                    tempDialog.jumpLine = result_i;//만약 대화가 돌아가는 버그가 난다 == tryParse 실패
                }

                float result_f;

                float.TryParse(row[9], out result_f);
                tempDialog.endDelay = result_f;

                float.TryParse(row[10], out result_f);
                tempDialog.speedMultifly = result_f;
                if(tempDialog.speedMultifly == 0) tempDialog.speedMultifly = 1;

                tempDialogs.Add(tempDialog.id, tempDialog);
            }
        }

    }
    public string TextEditToDialog(string text)
    {
        char[] textChars = text.ToCharArray();
        for(int i=0; i < textChars.Length; i++)//약간 코드 수정에서 앞에서부터 체크하되 i줄이기도 하면서 최대치 줄이기도 하면서 그... 두글자씩도 변경하도록
        {

            if (textChars[i] == 'ⓔ') textChars[i] = ',';
            else if (textChars[i] == 'ⓔ') textChars[i] = ',';

        }
        text = new string(textChars);
        return text;
    }

    public void RunDialog(string eventName)
    {
        
        nowDialog = dialogs[eventName];// List<dialog>
        player.GetComponent<PlayerState>().isOnDialog = true;
        nowDialogId = 0;
        dialogPlaytime = 0;
        dialogTextbox.GetComponent<TextMeshProUGUI>().text = "";
        dialogWindow.SetActive(true);
        if (nowDialog[nowDialogId].callFunction != "")
        {
            string[] events = nowDialog[nowDialogId].callFunction.Split(";");//구분자를 뭐로 할까
            foreach (string i in events)
            {
                CheckFunction(i);
            }
        }

        /*
         1. 디아로그 하나씩 보내기, 오토 메시징이 켜져있는 경우 코루틴 돌리기, 아닐 경우 쭈루룩 출력해주기.
         2. 오토 메시징이 꺼져있는 경우 클릭(또는 특정 키)을 통해서 넘기기
         3. id를 하나씩 올리기로
        4. 잠깐, 그럼 이 dialog를 전체 변수로 지정해줘야지
        5. 그리고 업데이트 문에다가 박아줘야겠는데?
        6. 그래야 순서대로 진행하고 뭐.... 그런 
         */
    }

    public void CheckingDialog()
    {
        if (nowDialog != null)//CheckingDIalog이 함수를 저기 TextScript에 넣어주는게 더 좋을 듯. 그래야 disable되었을 떄 처리 안하니까 이득이득 아니겠어?
        {
            
            if (Input.GetKeyDown("f") && !isStopDialogSkip)
            {
                //재생중이면 재생완료
                //재생 끝이면 ID 추가 및 재생 초기화
                //쭈루루루룩 재생 되는건 저번에 썼던 코드마냥 쓰면 될 듯한데 파이썬에서 썼던거
                //유니티에선 한글 영어 글자 크기가 다른가?
                //생가갷보니까 자동으로 글자 길이에 맞춰주는구나 여긴 와 이게 신기술이지;;;
                if (dialogPlaytime >= nowDialog[nowDialogId].script.Length)//값을 dialog길이로 바꾸기 일단은 오늘응 여기까지~
                {
                    if (nowDialog[nowDialogId].jumpLine != -1)
                    {
                        nowDialogId = nowDialog[nowDialogId].jumpLine;
                    }
                    else if(nowDialog[nowDialogId].jumpLine < -1){
                        EndDialog();
                        return;
                    }
                    else
                    {
                        nowDialogId += 1;
                    }//아 잠깐. 기본값이 0이면 안 된다
                    dialogPlaytime = 0;

                    if (!nowDialog.Keys.Contains(nowDialogId))
                    {
                        EndDialog();
                        return;
                    }
                    //함수 분할 및 실행, 함수 구분자; 변수 구분자:
                    if (nowDialog[nowDialogId].callFunction != "")
                    {
                        string[] events = nowDialog[nowDialogId].callFunction.Split(";");//구분자를 뭐로 할까
                        foreach (string i in events)
                        {
                            CheckFunction(i);
                        }
                       
                    }

                    
                }
                else
                {
                    dialogPlaytime = nowDialog[nowDialogId].script.Length;
                }
            }

            if (!nowDialog.Keys.Contains(nowDialogId) || nowDialogId < -1)
            {
                EndDialog();
                return;
            }
            // Text Length by Time 줄여서 TLT
            if (dialogPlaytime < nowDialog[nowDialogId].script.Length)
            {
                dialogPlaytime += Time.deltaTime * nowDialog[nowDialogId].speedMultifly * worldOptionController.GetComponent<WorldOption>().dialogSpeed;
            }
            else
            {
                dialogPlaytime = nowDialog[nowDialogId].script.Length;
            }
            

            dialogTextbox.GetComponent<TextMeshProUGUI>().text = nowDialog[nowDialogId].script.Substring(0, (int)dialogPlaytime);
        }

    }

    private void EndDialog()
    {
        nowDialog = null;
        nowDialogId = 0;
        dialogWindow.SetActive(false);
        dialogTextbox.GetComponent<TextMeshProUGUI>().text = "";
        StartCoroutine(OnDialogDisableCorutine());
    }
    IEnumerator OnDialogDisableCorutine()
    {
        yield return null;
        player.GetComponent<PlayerState>().isOnDialog = false;
    }

    private void WipeDialogWindow()
    {

    }

    private void CheckFunction(string command)
    {
        string[] commands = command.Split(":");

        string funtionName = commands[0];
        string[] values = new string[commands.Length-1];


        for(int i = 1; i < commands.Length; i++)
        {
            values[i - 1] = commands[i];
        }
        if (funtionName == "ViewSelections")//뭐 이런 식으로 쭉 써내려가면 될 듯
        {
            int[] selectionsID;
            selectionsID = new int[values.Length];
            for (int i=0; i < values.Length; i++)
            {
                selectionsID[i] = int.Parse(values[i]);
            }
            DF_ViewSelections(selectionsID);//이걸 꼭 Dialog적으로만 쓰는 게 아니라 Event적으로 쓰면 재사용성도 좋고 그럴듯... 그러   니까 막 대사출력만이 아니라 우르르쾅쾅하는 상황 같은걸 대사는 가리고서 하는 식으로, 대신 이제 PopupDialog식으로
        }
        else if(funtionName == "TimeSkip")
        {
            dialogPlaytime = nowDialog[nowDialogId].script.Length;
        }
        

        
    }

    //일단뭐...그냥 해둘까 아 이것도 하던 중이었지 이벤트기능 선ㅌ캑지랑 이벤트기능 해야겠다 오늘은ㅇ
    //여기있는 함수들은 링크-개념으로..? 아무튼 뭐 일단 
    public void DF_ShakeCamera()
    {
        //화면 흔드는 기능은 저어어어기 카메라에 넣어줘야겠지
    }

    private List<GameObject> selections = new List<GameObject>();
    private int[] selectionsID;
    public void DF_ViewSelections(int[] _selectionsID)
    {
        isStopDialogSkip = true;
        selectionsID = _selectionsID;
        //selections 복사하기
        for(int i = 0; i<selectionsID.Length;i++)
        {
            GameObject selection = Instantiate(selectionPrefab, transform);
            selections.Add(selection);
            selection.SetActive(true);
            selection.name = "Selection_" + i;
            //script넣어주기
            selection.GetComponent<SelectionUI>().selectionText.GetComponent<TextMeshProUGUI>().text = TextEditToDialog(nowDialog[selectionsID[i]].script);
            selection.GetComponent<SelectionUI>().dialogController = transform.gameObject;

            //위치 배치
            //세로길이를 대략 자른거에다가 id개수로 슥삭싱
            selection.transform.position = new Vector3(selection.transform.position.x, Screen.height/ (selectionsID.Length + 1) * (i + 1), selection.transform.position.z);


        }
        //UI 띄우기



        //입력받기..는 선택지 스크립트에서 하는 걸로 그러니까 여기선 Enable시켜주고 대기하는 걸로... 하면 잠깐 그럼 대사창을 잠시 멈춰야 하나? 
    }
    public void DF_ViewSelectionsReturn(int number)
    {
        dialogPlaytime = 0;
        nowDialogId = nowDialog[selectionsID[number]].jumpLine;
        Debug.Log(nowDialogId);
        Debug.Log(nowDialog[nowDialogId].script);
        isStopDialogSkip = false;
        foreach(GameObject i in selections)
        {
            Destroy(i);
        }
        if(nowDialogId < -1)
        {
            nowDialog = null;
            dialogPlaytime = 0;
        }
    }


    // Update is called once per frame

}
