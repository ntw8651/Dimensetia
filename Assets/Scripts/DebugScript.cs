using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject weaponControl;
    public bool isWeapon;
    public GameObject weapon;
    public GameObject player;
    public GameObject bulletText;

    public GameObject dialogController;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //카메라 관련, 카메라 조종
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.GetComponent<PlayerState>().cameraView = PlayerState.CameraView.FirstView;
            player.GetComponent<PlayerState>().CameraViewUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.GetComponent<PlayerState>().cameraView = PlayerState.CameraView.ThirdFixedView;
            player.GetComponent<PlayerState>().CameraViewUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.GetComponent<PlayerState>().cameraView = PlayerState.CameraView.ThirdFreeView;
            player.GetComponent<PlayerState>().CameraViewUpdate();
        }




        //아이템 관련, 아이템 손에 들기
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (!isWeapon)
            {
                isWeapon = true;
                Item item = GetComponent<PlayerInventory>().items[0];
                weapon = Instantiate(item.prefab);
                weapon.transform.parent = weaponControl.transform;
                weapon.transform.localPosition = Vector3.zero;
                weapon.transform.localRotation = Quaternion.identity;
                weapon.transform.localScale = new Vector3(Mathf.Abs(weapon.transform.localScale.x), Mathf.Abs(weapon.transform.localScale.y), weapon.transform.localScale.z);
                GetComponent<PlayerState>().nowHand = weapon;
                weapon.GetComponent<RangedWeaponBase>().stat = item.rangedWeaponInfo;
                weaponControl.GetComponent<PlayerWeaponControl>().PutHandOnWeapon();
            }
            else
            {
                Destroy(weapon);
                GetComponent<PlayerState>().baseState.isAimming = false;
                weaponControl.GetComponent<PlayerWeaponControl>().PutHandOutWeapon();
                isWeapon = false;
            }
        }


        //장탄수 확인

        if (player.GetComponent<PlayerState>().nowHand && player.GetComponent<PlayerState>().nowHand.GetComponent<RangedWeaponBase>() != null)
        {
            RangedWeaponInfo rs = player.GetComponent<PlayerState>().nowHand.GetComponent<RangedWeaponBase>().stat;
            bulletText.GetComponent<TextMeshProUGUI>().text = rs.ammo + "/" + rs.maxAmmo;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            PlayerInventory inven = player.GetComponent<PlayerInventory>();
            //Debug.Log(inven.items[0].GetComponent<ItemBase>().item.rangedWeaponInfo + "1");
            //Debug.Log(inven.items[1].GetComponent<ItemBase>().item.rangedWeaponInfo + "2");

        }

        //손 위치 재지정 
        if (Input.GetKeyDown(KeyCode.Z))
        {
            weaponControl.GetComponent<PlayerWeaponControl>().PutHandOnWeapon();
        }
        

    }
}
