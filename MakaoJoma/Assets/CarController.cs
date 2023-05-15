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

        if(Input.GetAxis("Vertical")!=0)
        {
            // ������ roatation������ y���� ���� �־��ָ� ȸ���� �ȴ�.
            // update������ ��ǻ���� ������ ���̸� ������������ time.deltaTime�� ���ش�.
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles
                + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Mathf.Sign(speedInput) *
                (RB.velocity.magnitude / 40), 0f));
        }

        transform.position = RB.position;
    }

    // ��ǻ���� ���ɸ��� ���̸� ���� �ʱ����� fixedUpdate���
    // edit->project setting->time ���� ���氡��

    private void FixedUpdate()
    {
        grounded = false;

        RaycastHit hit;

        // groundRay�� ��ġ����, �Ʒ���, �����, ray�� ���̸�ŭ, ground��
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
