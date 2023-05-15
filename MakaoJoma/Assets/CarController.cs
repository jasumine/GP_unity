using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public Transform groundRay,groundAngle;
    public LayerMask Ground;
    public float rayLength = .7f;

    private float dragGround;
    public float gravity = 10f;

    public Transform leftWheel, rightWheel;
    public float maxWheelTurn;


    void Start()
    {
        RB.transform.parent = null;

        dragGround = RB.drag;
    }

    // Update is called once per frame
    void Update()
    {
        speedInput = 0f;

        // ���� ȭ��ǥ�� ������� 0~1�� ��, �Ʒ����� ������� -1����
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel;
        }
        else if(Input.GetAxisRaw("Vertical")<0)
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccel;
        }

        turnInput = Input.GetAxis("Horizontal");


        // ��������
        leftWheel.localRotation = Quaternion.Euler(leftWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn)-180,
             leftWheel.localRotation.eulerAngles.z);
        rightWheel.localRotation = Quaternion.Euler(rightWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn),
            rightWheel.localRotation.eulerAngles.z);

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
            transform.rotation = Quaternion.FromToRotation(transform.up, normaltarget) *
                transform.rotation;
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

        
        if(RB.velocity.magnitude>Maxspeed)
        {
            RB.velocity = RB.velocity.normalized * Maxspeed;
        }

        transform.position = RB.position;

        if (Input.GetAxis("Vertical") != 0)
        {
            // ������ roatation������ y���� ���� �־��ָ� ȸ���� �ȴ�.
            // update������ ��ǻ���� ������ ���̸� ������������ time.deltaTime�� ���ش�.
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles
                + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) *
                (RB.velocity.magnitude / 40), 0f));
        }

    }
}
