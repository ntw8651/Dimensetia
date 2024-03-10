using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    /*
     This Script for all enemy.
    START SETTING INFO
     */

    /*
     음... 적은 적마다 다양한 공격 방식을 갖고 있겠지...

    아 대충 하는 말이 Scriptable로 개체 속성을 지정해두면 나중에 EnemyBase같은 걸 붙였을때 개체마다 다른 스크립트를 넣을 필요 없이(기본 행동에 한해서)
    개체 속성만 뚜까뚜까 해가지고 할 수 있다 머 그런 
     
     나중에 Info는 Stat로 바꾸고 EnemyBase는 EnemyBaseBehavior정도로 바꾸면 알아먹기 더 편할듯

    이 EnemyBase도 상속으로 주는 게 맞겠지
    
     */
    [SerializeField]
    private EnemyState enemyStateOriginal;
    [SerializeField]
    private BaseState baseStateOriginal;
    [HideInInspector]
    public EnemyState enemyState;
    [HideInInspector]
    public BaseState baseState;//아 근데 이럼 또 좀... 복잡해지는데 체력은 enemy냐 general이냐?라는 의문에서부터 시작해서 damage는? ??이런 식으로 결국엔 그냥 하나로 귀결되잖아... 아
    //그냥 generalState에 최대한 때려박을까...

    void Start()
    {
        //initialize
        enemyState = Instantiate(enemyStateOriginal);
        baseState = Instantiate(baseStateOriginal);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
