using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOD : MonoBehaviour
{
    // Start is called before the first frame update 지금은 뭐 간단하게 구현하는거니까 막 써도 되지만 만약 그냥에 이렇게 넣으면... 망할듯
    public GameObject player;
    public Vector3 FundamentalPostion;
    public Vector3 FundamentalPlayerPostion;
    public float multiply;
    void Start()
    {
        FundamentalPostion = transform.localPosition;
        FundamentalPlayerPostion = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = FundamentalPostion + (FundamentalPlayerPostion - player.transform.position) * 0.1f * multiply;
        transform.position = new Vector3(transform.position.x, transform.position.y, 10);
    }
}
