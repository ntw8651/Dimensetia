using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]
public class EnemyState : ScriptableObject
{
    public string displayName;

    public int meleeDamage;

    public GameObject prefab;
    public Sprite sprite;
}
