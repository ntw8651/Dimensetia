using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName ="MeleeWeapon")]
public class MeleeWeaponInfo : ScriptableObject
{
    public enum Type
    {
        Sword,//검
        Dagger,//대거(전투 단검)
        Spear,//창

    }
    public int damage;
    public float range;
    public Type type; //아 이걸 따로 해줘야되는구나

    public AnimationClip[] comboAnimations;

    public GameObject trajectory;//검 궤적? 이건 나중에 effect쪽으로 넣거나 그냥 스크립트마다 하드코...딩해주는 게 나을 것 같은데

    public Sprite weaponSprite;


    public bool canContinue;
    public float reloadTime = 2f;
    public float firerate = 1f;

    public float armDistance;
    public Vector2 LHandPosition;
    public Vector2 RHandPosition;

    public AudioClip fireSound;
    //custom bullet



    public int maxAmmo;
    public int ammo;
}
