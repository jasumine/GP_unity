using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMove : MonoBehaviour
{

    public bool move, rotate;
    public float moveSpeed, rotateSpeed;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
            transform.position += new Vector3(moveSpeed, 0f, 0f) * Time.deltaTime;

        if (rotate)
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, rotateSpeed * Time.deltaTime, 0f));
    }
}
