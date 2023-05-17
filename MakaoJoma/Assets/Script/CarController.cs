using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody RB;
    
    public float Maxspeed;
    private float speedInput;

    public float forwardAccel = 8f, reverseAccel = 5f;

    public float turnStrength = 180f;
    private float turnInput;

    public bool grounded;

    public Transform groundRay,groundAngle;
    public LayerMask Ground;
    public float rayLength = .7f;

    private float dragGround;
    public float gravity = 10f;

    public Transform leftWheel, rightWheel;
    public float maxWheelTurn;

    private int nextCheckpoint;
    public int currentLap;

    public float lapTime, bestLapTime;
    void Start()
    {
        RB.transform.parent = null;

        dragGround = RB.drag;
        UIManager.instance.LapText.text = currentLap + "/" + GameManager.instance.totalLaps;
    }

    // Update is called once per frame
    void Update()
    {
        lapTime += Time.deltaTime;

        var time = System.TimeSpan.FromSeconds(lapTime); // var가 c++의 auto

        UIManager.instance.currentTimeText.text = string.Format($"{time.Minutes}m {time.Seconds}.{time.Milliseconds}");

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


        // 바퀴꺾기
        leftWheel.localRotation = Quaternion.Euler(leftWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn)-180,
             leftWheel.localRotation.eulerAngles.z);
        rightWheel.localRotation = Quaternion.Euler(rightWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn),
            rightWheel.localRotation.eulerAngles.z);

    }

    // 컴퓨터의 성능마다 차이를 두지 않기위해 fixedUpdate사용
    // edit->project setting->time 에서 변경가능

    private void FixedUpdate()
    {
        grounded = false;

        RaycastHit hit;
        Vector3 normaltarget = Vector3.zero;

        // groundRay의 위치에서, 아래로, 맞춘다, ray의 길이만큼, ground layer를
        if(Physics.Raycast(groundRay.position, -transform.up, out hit, rayLength, Ground))
        {
            grounded = true;

            normaltarget = hit.normal;
        }

        // groundAngle의 위치에서, 아래로, 맞춘다, ray의 길이만큼, ground layer를
        if (Physics.Raycast(groundAngle.position, -transform.up, out hit, rayLength, Ground))
        {
            grounded = true;

            normaltarget = (normaltarget + hit.normal) / 2f;
        }
        

            // 땅에서  normal 회전
            if (grounded)
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, normaltarget) * transform.rotation;
        }

        // 땅에서 감속
        if(grounded)
        {
            RB.drag = dragGround;
            RB.AddForce(transform.forward * speedInput * 500f);
        }
        else
        {
            RB.drag = .1f;
            RB.AddForce(-Vector3.up * gravity * 50f);
        }

        
        if(RB.velocity.magnitude > Maxspeed)
        {
            RB.velocity = RB.velocity.normalized * Maxspeed;
        }

        transform.position = RB.position;

        if (grounded&& Input.GetAxis("Vertical") != 0)
        {
            // 현재의 roatation값에서 y축의 값을 넣어주면 회전이 된다.
            // update문에서 컴퓨터의 성능의 차이를 주지않을려면 time.deltaTime을 써준다.
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles
                + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) *
                (RB.velocity.magnitude / 40), 0f));
        }

    }

    public void CheckpointHit(int cpNumber)
    {
        if(cpNumber==nextCheckpoint)
        {
            nextCheckpoint++;

            if (nextCheckpoint == GameManager.instance.checkPoints.Length)
            {
                nextCheckpoint = 0;
                LapFinish();
            }
         
        }
    }

    public void LapFinish()
    {
        currentLap++;

        if(lapTime<bestLapTime || bestLapTime==0)
        {
            bestLapTime = lapTime;
        }

        lapTime = 0;

        var time = System.TimeSpan.FromSeconds(bestLapTime); // var가 c++의 auto

        UIManager.instance.bestTimeText.text = string.Format($"{time.Minutes}m {time.Seconds}.{time.Milliseconds}");

        UIManager.instance.LapText.text = currentLap + "/" + GameManager.instance.totalLaps;

        if (currentLap == GameManager.instance.totalLaps)
            Application.Quit();
    }

}
