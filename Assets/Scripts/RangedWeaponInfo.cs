using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName ="RangedWeapon")]
public class RangedWeaponInfo : ScriptableObject
{
    public enum Type
    {
        handgun,
        shotgun,
        rifle,
    }
    public int damage;
    public float range;
    public Type type; //�� �̰� ���� ����ߵǴ±���
    
    public GameObject trajectory;
    
    public Sprite weaponSprite; //�� ���� ��������Ʈ�� �� ����ִ� ����̰� �׳� sprite�� �ٴڿ� ������ �� ����ΰɷ� �� �ٸ���� �ϰڳĸ�...
    
    public bool canContinue;//���� ����, ��� ���� �ܹ� �ӻ� �̷��� ��� �ٲ�ְ� �ͱ� �ѵ� �׷� �ʹ� ���������� �� ���Ƽ� �׳� ����
    //���� ����ϴ°� ��... �� waeponSprite�� ������ ���Ÿ� ����� �ٰŸ� ����� �տ� ��� �Ƴ�..? ��... ���� ���� �� �ʿ䰡 �̾���..?
    //��ġ �̰� �ƿ� WeaponBase���ٰ� �ص־��� ��絵 �׷��� ���� �÷��̾ �� �Ѹ� �����ϴϱ� WeaponControl�� �ڽ��� �ǵ帮�� ������...
    public float reloadTime = 2f;//���ε带 ���ǵ�� �ϸ� ��Ȯ�� ���ϴϱ� Time���� �ְ� ���߿� �ִϸ��̼� �ð��̶� ������ ���ϱ� «���ؼ� �ϸ� �ɵ�
    public float firerate = 1f;
    public int bulletStack = 1; //���ļ� �߻�Ǵ� �Ѿ� ��(�⺻ 1) 
    public float bulletSpreadAngle = 0;//rmsep ����� Sprite�� �ƴ϶� Prefab���� �ؾߴ볪 object��


    public float armDistance;


    public AudioClip fireSound;
    public AudioClip clickSound;

    //custom bullet
    public GameObject bulletSprite; //�Ѿ� ���ư��� ���...��... ��... �Ѿ��� ����� ���δٴ� �ͺ��Ͱ� �̻��Ѱ� �ƴѰ�?



    public int maxAmmo;
    public int ammo;
}
