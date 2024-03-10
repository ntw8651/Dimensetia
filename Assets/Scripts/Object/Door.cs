using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InteractionInterface;

public class Door : MonoBehaviour, IInteraction
{
    // Start is called before the first frame update
    private bool isOpen;
    void Start()
    {
        
    }

    public void ActivateTrigger(GameObject user = null)//누가 상호작용했는지를 받아야 하려나?
    {
        
    }


    public void ControlDoor()
    {
        //Open... NEED ADD : 열쇠 존재 확인
        /*
         현재 열림닫힘 상태 변경
        이미지 변경
        상호작용 글자 변경 (열기 -> 닫기)
        콜라이더 변경
         */
        if (!isOpen)
        {
            isOpen = true;
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }    
    
}
