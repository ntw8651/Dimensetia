using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BaseState")]

public class BaseState : ScriptableObject
{
    // Start is called before the first frame update
    public int maxHealth;
    public int health;

    //대충 isAttacking은 그걸로 쓸 수 있겠지, 패링...이랄가...
    [Header("Checker Bools")]
    public bool canAttack;
    public bool canMove;
    public bool canJump;
    public bool isAimming;
    public bool isAttacking;
    public bool isMoving;
    public bool isJumping;
    public bool isInBattle;

    public AudioClip hitSFX;
    public GameObject hitVFX;



    [Header("ETC State")]
    public float fallingSpeedMultiply;
    public float moveSpeedMultiply;




    public enum Faction
    {
        players,
        monsters,
        humans,
        hostile, //모두와 적
        allie, //모두와 친구(비전투 npc)
    }
    public Faction faction = Faction.allie;

    //음 수치감소랑 퍼센트감소 둘다 넣을까 일단 resist는 이렇게 판만 짜두고 미뤄두자

}
