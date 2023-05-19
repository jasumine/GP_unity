using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

using Random = UnityEngine.Random;

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

    public int nextCheckpoint;
    public int currentLap;

    public float lapTime, bestLapTime;

    public bool isAI;

    public int currentTarget;
    private Vector3 targetPoint;
    public float aiAccelerSpeed = 1f, aiTurnSpeed = 1, aiReachPoint = 5f, aiPointVariance = 3f;
    private float aiSpeedInput;
    public float aiMaxturn= 15f;



    void Start()
    {
        RB.transform.parent = null;

        dragGround = RB.drag;
        
        if(isAI)
        {
            targetPoint = GameManager.instance.checkPoints[currentTarget].transform.position;
            RandomAITarget();
        }

        UIManager.instance.LapText.text = currentLap + "/" + GameManager.instance.totalLaps;
    }

    // Update is called once per frame
    void Update()
    {

        if (!GameManager.instance.isStarting)
        {

            lapTime += Time.deltaTime;

            if (!isAI) // player일 경우
            {
                var time = System.TimeSpan.FromSeconds(lapTime); // var가 c++의 auto

                UIManager.instance.currentTimeText.text = string.Format($"{time.Minutes}m {time.Seconds}.{time.Milliseconds}");

                speedInput = 0f;

                // 위쪽 화살표를 누를경우 0~1의 값, 아래쪽을 누를경우 -1까지
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    speedInput = Input.GetAxis("Vertical") * forwardAccel;
                }
                else if (Input.GetAxisRaw("Vertical") < 0)
                {
                    speedInput = Input.GetAxis("Vertical") * reverseAccel;
                }

                turnInput = Input.GetAxis("Horizontal");
            }
            else // ai일 경우
            {
                targetPoint.y = transform.position.y;

                if(Vector3.Distance(transform.position,targetPoint)<aiReachPoint)
                {
                    NnextAITarget();
                }


                Vector3 targetDir = targetPoint - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);

                Vector3 localPos = transform.InverseTransformPoint(targetPoint); // world좌표를 지역좌표로 변경
                if (localPos.x < 0f)
                {
                    angle = -angle;
                }

                turnInput = Mathf.Clamp(angle / aiMaxturn, -1, 1); // 최대값 최소값을 설정해둔다.

                if (Mathf.Abs(angle) < aiMaxturn)
                {
                    aiSpeedInput = Mathf.MoveTowards(aiSpeedInput, 1f, aiAccelerSpeed);
                }
                else
                {
                    aiSpeedInput = Mathf.MoveTowards(aiSpeedInput, aiTurnSpeed, aiAccelerSpeed);
                }

                speedInput = aiSpeedInput * forwardAccel;

            }
            // 바퀴꺾기
            leftWheel.localRotation = Quaternion.Euler(leftWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180,
                 leftWheel.localRotation.eulerAngles.z);
            rightWheel.localRotation = Quaternion.Euler(rightWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn),
                rightWheel.localRotation.eulerAngles.z);
        }
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

        if (grounded&& speedInput != 0)
        {
            // 현재의 roatation값에서 y축의 값을 넣어주면 회전이 된다.
            // update문에서 컴퓨터의 성능의 차이를 주지않을려면 time.deltaTime을 써준다.
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles
                + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) * (RB.velocity.magnitude / 40), 0f));
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

        if(isAI)
        {
            if (cpNumber == currentTarget)
            {
                NnextAITarget();
            }

        }
    }

    public void NnextAITarget()
    {
        currentTarget++;
        if (currentTarget >= GameManager.instance.checkPoints.Length)
        {
            currentTarget = 0;
        }

        targetPoint = GameManager.instance.checkPoints[currentTarget].transform.position;
        RandomAITarget();

    }

    public void LapFinish()
    {
        currentLap++;

        if (lapTime < bestLapTime || bestLapTime == 0)
        {
            bestLapTime = lapTime;
        }

        if (currentLap < GameManager.instance.totalLaps)
        {

            lapTime = 0;
            if (!isAI)
            {
                var time = System.TimeSpan.FromSeconds(bestLapTime); // var가 c++의 auto

                UIManager.instance.bestTimeText.text = string.Format($"{time.Minutes}m {time.Seconds}.{time.Milliseconds}");

                UIManager.instance.LapText.text = currentLap + "/" + GameManager.instance.totalLaps;

            }
        }
        else
        {
            if(!isAI)
            {
                isAI = true;
                targetPoint = GameManager.instance.checkPoints[currentTarget].transform.position;
                RandomAITarget();


                var time = System.TimeSpan.FromSeconds(bestLapTime);
                UIManager.instance.bestTimeText.text = string.Format($"{time.Minutes}m {time.Seconds}.{time.Milliseconds}");

                GameManager.instance.FInishRace();
            }
        }
    }

    public void RandomAITarget()
    {
        targetPoint += new Vector3(Random.Range(-aiPointVariance, aiPointVariance), 0f, Random.Range(-aiPointVariance, aiPointVariance));
    }

}
