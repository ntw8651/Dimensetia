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
        Sword,//��
        Dagger,//���(���� �ܰ�)
        Spear,//â

    }
    public int damage;
    public float range;
    public Type type; //�� �̰� ���� ����ߵǴ±���

    public AnimationClip[] comboAnimations;

    public GameObject trajectory;//�� ����? �̰� ���߿� effect������ �ְų� �׳� ��ũ��Ʈ���� �ϵ���...�����ִ� �� ���� �� ������

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
