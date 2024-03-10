using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    PlayerState state;
    void Start()
    {
        animator = GetComponent<Animator>();
        state = GetComponent<PlayerState>();


    }

    // Update is called once per frame 근데 이거 할 때, stickman 자체 scale를 음수로 했으면 그때 총도 같이 flip되야하는거 아닌가? 왜 그때 총만 올ㄴ쪽 보고 있던거지;
    void Update()
    {
        //Mouse Set Direction
        // 나중에 if 무기 조준상태 넣어주기
        //NEED ADD 현재 isWalking False를 Horizontal로만 확인함 나중에는 시점에 따라서 하도록. 일단 그거 넣어두자

        Vector3 playerMoveVector = GetComponent<PlayerMovement>().direction;
        if (GetComponent<PlayerState>().baseState.isAimming)
        {
            animator.SetBool("isAimming", true);
        }
        else
        {
            animator.SetBool("isAimming", false);
        }

        if(playerMoveVector == Vector3.zero)
        {
            animator.SetBool("isWalking", false);
            
        }
        else
        {
            animator.SetBool("isWalking", true);
            //만약 총 안 들고있고 이것저것인 상태라면 
            if (!GetComponent<PlayerState>().baseState.isAimming)
            {
                //좌우 반전, 다만 지금은 3d라 필요x
                //transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(playerHorizontal), transform.localScale.y, transform.localScale.z);
            }
            
        }
    }
}
