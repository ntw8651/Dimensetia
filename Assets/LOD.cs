using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOD : MonoBehaviour
{
    // Start is called before the first frame update ������ �� �����ϰ� �����ϴ°Ŵϱ� �� �ᵵ ������ ���� �׳ɿ� �̷��� ������... ���ҵ�
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
