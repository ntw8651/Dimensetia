using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrap : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null)
        {
            if(collision.CompareTag("Player") || collision.CompareTag("Enemy"))
            {
                RaycastHit2D hit = default(RaycastHit2D);
                hit.point = collision.ClosestPoint(transform.position);
                collision.GetComponent<DamagedCharacter>().Hit(10, hit);
                Debug.Log(collision.transform.name);
            }
        }
    }
}
