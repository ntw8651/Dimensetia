using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]

public class Item : ScriptableObject
{
    //아이템 변수의 기준은 바닥에 떨어져 있는 상태를 기준으로.
    public string displayName;

    public Sprite displaySprite; // inventory show simply small image
    public GameObject prefab; // throwable having collision Object
    
    public int count = 1;
    public bool isEquipped = false;
    public bool isDropped = true;

    //ㅋㅋㅋㅋㅋ이거 어떤 아이템이든 들어도 되게 해두고 만약 나중에 드럼통 같은 거 들면 총이라도 맞으면 펑~해버리는 개꿀잼 상황 가능하지 않을까
    public enum Type
    {
        MeleeWeapon, // left click
        RangedWeapon, // right- and left click
        BothWeapon, // left right both click
        Consumable,
        
    }
    public Type type;

    public RangedWeaponInfo rangedWeaponInfo;
    public MeleeWeaponInfo meleeWeaponInfo;

    

}
