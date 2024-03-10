using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventListener : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject weaponContol;
    void Start()
    {
        weaponContol = GetComponent<PlayerState>().weaponControl;
    }

    // Update is called once per frame
    public void MeleeAttackEnd()
    {
        weaponContol.transform.GetComponent<PlayerWeaponControl>().MeleeAttackEnd();
    }
}
