using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InteractionInterface;
public class E_Tester : MonoBehaviour, IInteraction
{
    // Start is called before the first frame update
    public GameObject dialogController; //그냥 이럴꺼면 dialogController는 Static으로다가 지정해줘서 어디서든 접근할 수 있게 하는게 편하려나...음 아냐 아니야!
    void Start()
    {
        
    }
    public void ActivateTrigger(GameObject user = null)
    {
        user.GetComponent<PlayerInteraction>().RunDialog("Test_Select");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
