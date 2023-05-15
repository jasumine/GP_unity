using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody RB;
    public float speed;
    public float Maxspeed;

    private float speedInput;
    public float forwardAccel = 8f, reverseAccel = 5f;

    public float turnStrength = 180f;
    private float turnInput;

    public bool grounded;

    public Transform groundRay;
    public LayerMask Ground;
    public float rayLength = .7f;


    void Start()
    {
        RB.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        speedInput = 0f;

        // 위쪽 화살표를 누를경우 0~1의 값, 아래쪽을 누를경우 -1까지
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel;
        }
        else if(Input.GetAxisRaw("Vertical")<0)
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccel;
        }

        turnInput = Input.GetAxis("Horizontal");

        if(Input.GetAxis("Vertical")!=0)
        {
            // 현재의 roatation값에서 y축의 값을 넣어주면 회전이 된다.
            // update문에서 컴퓨터의 성능의 차이를 주지않을려면 time.deltaTime을 써준다.
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles
                + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) *
                (RB.velocity.magnitude / 40), 0f));
        }

        transform.position = RB.position;
    }

    // 컴퓨터의 성능마다 차이를 두지 않기위해 fixedUpdate사용
    // edit->project setting->time 에서 변경가능

    private void FixedUpdate()
    {
        grounded = false;

        RaycastHit hit;

        // groundRay의 위치에서, 아래로, 맞춘다, ray의 길이만큼, ground를
        if(Physics.Raycast(groundRay.position, -transform.up, out hit, rayLength, Ground))
        {
            grounded = true;
        }
        
        RB.AddForce(transform.forward * speedInput * 500f);
        
        if(RB.velocity.magnitude>Maxspeed)
        {
            RB.velocity = RB.velocity.normalized * Maxspeed;
        }
        Debug.Log(RB.velocity.magnitude);
    }
}
