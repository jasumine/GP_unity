using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    //private Rigidbody Rb;
    public float speed = 5f;

    bool _move = false;
    Vector3 destPos;
    void Start()
    {
        // Rb= GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        //float inputX = Input.GetAxis("Horizontal");
        //float inputZ = Input.GetAxis("Vertical");
        //Vector3 velocity = new Vector3(inputX,0,inputZ);
        //velocity *= speed;
        //Rb.velocity = velocity;

        // quaternion.slerp 일정한 비율로 회전시키겠다는 뜻
        if(Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.1f);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.1f);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.1f);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.1f);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }


        Vector3 look = transform.TransformDirection(Vector3.forward); // 앞방향을 항상 바라보게끔
        Debug.DrawRay(transform.position, look * 10, Color.red);

        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position + Vector3.up, look, 10);

        //foreach(RaycastHit hit in hits)
        //{
        //    Debug.Log(hit.collider.gameObject.name);
        //}


        /*
        if(Physics.Raycast(transform.position+ Vector3.up, look,out hit, 10))
        {
            Debug.Log(hit.collider.gameObject.name);
        }*/

        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.green, 1.0f);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                destPos = hit.point;
                _move = true;
                Debug.Log(hit.collider.gameObject.name);
            }
        }

        if(_move) 
        {
            Vector3 dir = destPos - transform.position; // 내가 가야하는 방향 = 목표위치 - 현재 위치
            if(dir.magnitude<0.001)
            {
                _move = false; // 도착했다면 false
            }
            else
            {
                float moveDist = Mathf.Clamp(speed * Time.deltaTime, 0, dir.magnitude); // 속도 *deltatime * 방향vector?
                transform.position += dir.normalized * moveDist;
                transform.LookAt(destPos); // 목적지 위치로 바라보도록
            }
        }


    }
}
