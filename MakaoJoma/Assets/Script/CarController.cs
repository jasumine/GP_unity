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

            if (!isAI) // player�� ���
            {
                var time = System.TimeSpan.FromSeconds(lapTime); // var�� c++�� auto

                UIManager.instance.currentTimeText.text = string.Format($"{time.Minutes}m {time.Seconds}.{time.Milliseconds}");

                speedInput = 0f;

                // ���� ȭ��ǥ�� ������� 0~1�� ��, �Ʒ����� ������� -1����
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
            else // ai�� ���
            {
                targetPoint.y = transform.position.y;

                if(Vector3.Distance(transform.position,targetPoint)<aiReachPoint)
                {
                    NnextAITarget();
                }


                Vector3 targetDir = targetPoint - transform.position;
                float angle = Vector3.Angle(targetDir, transform.forward);

                Vector3 localPos = transform.InverseTransformPoint(targetPoint); // world��ǥ�� ������ǥ�� ����
                if (localPos.x < 0f)
                {
                    angle = -angle;
                }

                turnInput = Mathf.Clamp(angle / aiMaxturn, -1, 1); // �ִ밪 �ּҰ��� �����صд�.

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
            // ��������
            leftWheel.localRotation = Quaternion.Euler(leftWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180,
                 leftWheel.localRotation.eulerAngles.z);
            rightWheel.localRotation = Quaternion.Euler(rightWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn),
                rightWheel.localRotation.eulerAngles.z);
        }
    }

    // ��ǻ���� ���ɸ��� ���̸� ���� �ʱ����� fixedUpdate���
    // edit->project setting->time ���� ���氡��

    private void FixedUpdate()
    {
        grounded = false;

        RaycastHit hit;
        Vector3 normaltarget = Vector3.zero;

        // groundRay�� ��ġ����, �Ʒ���, �����, ray�� ���̸�ŭ, ground layer��
        if(Physics.Raycast(groundRay.position, -transform.up, out hit, rayLength, Ground))
        {
            grounded = true;

            normaltarget = hit.normal;
        }

        // groundAngle�� ��ġ����, �Ʒ���, �����, ray�� ���̸�ŭ, ground layer��
        if (Physics.Raycast(groundAngle.position, -transform.up, out hit, rayLength, Ground))
        {
            grounded = true;

            normaltarget = (normaltarget + hit.normal) / 2f;
        }
        

            // ������  normal ȸ��
            if (grounded)
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, normaltarget) * transform.rotation;
        }

        // ������ ����
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
            // ������ roatation������ y���� ���� �־��ָ� ȸ���� �ȴ�.
            // update������ ��ǻ���� ������ ���̸� ������������ time.deltaTime�� ���ش�.
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
                var time = System.TimeSpan.FromSeconds(bestLapTime); // var�� c++�� auto

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
