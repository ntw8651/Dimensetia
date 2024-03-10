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
    public Type type; //아 이걸 따로 해줘야되는구나
    
    public GameObject trajectory;
    
    public Sprite weaponSprite; //아 웨폰 스프라이트는 쏠때 들고있는 모습이고 그냥 sprite는 바닥에 떨궈질 떄 모습인걸로 뭐 다르기야 하겠냐만...
    
    public bool canContinue;//연사 가능, 사실 연발 단발 속사 이렇게 기능 바꿔넣고 싶긴 한데 그럼 너무 지저분해질 것 같아서 그냥 가자
    //지금 고민하는게 음... 이 waeponSprite를 어차피 원거리 무기든 근거리 무기든 손에 들거 아냐..? 음... 굳이 따로 둘 필요가 이쓸까..?
    //그치 이걸 아에 WeaponBase에다가 해둬야지 모양도 그렇고 음음 플레이어가 든 총만 쏴야하니까 WeaponControl의 자식을 건드리는 식으로...
    public float reloadTime = 2f;//리로드를 스피드로 하면 정확히 못하니까 Time으로 주고 나중에 애니메이션 시간이랑 나누기 곱하기 짬뽕해서 하면 될듯
    public float firerate = 1f;
    public int bulletStack = 1; //겹쳐서 발사되는 총알 수(기본 1) 
    public float bulletSpreadAngle = 0;//rmsep 장실좀 Sprite가 아니라 Prefab으로 해야대나 object는


    public float armDistance;


    public AudioClip fireSound;
    public AudioClip clickSound;

    //custom bullet
    public GameObject bulletSprite; //총알 날아갈때 모습...을... 음... 총알이 모습이 보인다는 것부터가 이상한거 아닌가?



    public int maxAmmo;
    public int ammo;
}
