using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwrodController : MonoBehaviour
{
    Rigidbody rigidbody;
    public GameObject hand;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //벡터 무효화 / 플레이어 손에 고정
        rigidbody.velocity = Vector3.zero;
        transform.position = hand.transform.position;
    }
}
