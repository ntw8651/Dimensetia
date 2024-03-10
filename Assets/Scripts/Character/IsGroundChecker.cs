using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGroundChecker : MonoBehaviour
{
    public bool isGrounded;
    private void OnTriggerStay(Collider collision)
    {
        if (collision != null && collision.isTrigger == false)
        {
            isGrounded = true;
        }


    }
    private void OnTriggerExit(Collider collision)
    {
        isGrounded = false;
    }


}
