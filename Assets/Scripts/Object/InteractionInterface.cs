using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionInterface : MonoBehaviour
{
    // Start is called before the first frame update
    public interface IInteraction{
        void ActivateTrigger(GameObject user = null);//플레이어 별 고유 인식자를 넣어줘야 하나? 아니다, 그냥 GameObject 채로 줘서 알아서 식별하게 하자. 어차피 나중에 해당 캐릭터가 가진 캐릭터 변수값도 확인해야 할 것이니까!
    }
}
