using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour
{
    // ĳ���� ��Ʈ�ѷ��� �� ��� ������ٵ� ��� ���� ����������Ѵ�.
    public float moveSpeed, runSpeed, gravityScale, jumpForce;
    private CharacterController charCon;

    private Vector3 moveInput;

    public Transform camTrans;

    public float Sensivity;

    public bool invertX;
    public bool invertY;

    private bool canJump;
    private bool canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask isGround;

    public Animator anim;

    public GameObject bullet;
    public Transform firePoint;

    void Start()
    {
        charCon = GetComponent<CharacterController>();
    }

    void Update()
    {
        // ��� �ʱ�ȭ �Ǵ� y�� velocity ���� �����صξ����.
        float yStore = moveInput.y;

        Vector3 verMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = verMove + horiMove;
        moveInput.Normalize();

        if (Input.GetKey(KeyCode.LeftShift))
            moveInput = moveInput * runSpeed;

        else
            moveInput = moveInput * moveSpeed;

        moveInput.y = yStore; // ���� y���� �־������ν� ���ӵ��� �ٰԵȴ�.

        moveInput.y += Physics.gravity.y * gravityScale * Time.deltaTime; // �������� ���� y�࿡ �ش�

        if(charCon.isGrounded) // �� ���� ���� ���
        {
            moveInput.y = Physics.gravity.y * gravityScale * Time.deltaTime; // �������� ���� �־��ش�.
        }

        // ���� ��ġ���� ������ ���̸�ŭ �׸���. layer�� ground�ε� �� ���̰� 0���� ũ��(�ִ�?)��  true
        canJump = Physics.OverlapSphere(groundCheckPoint.position, .1f, isGround).Length > 0;

        
        if (canJump)
            canDoubleJump = false; // �Ұ��� ����

        // 1�� ������ �ϰ� ���� ��� �Ʒ� �ڵ� 2�ٸ� ����� �ȴ�
        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            moveInput.y = jumpForce; // �����ϴ� ����ŭ �־��ش�. // �߷��� �����صּ� �˾Ƽ� ������!

            canDoubleJump = true; // ������ �� ���¿����� ����
        }

        else if(canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            moveInput.y = jumpForce;

            canDoubleJump = false; // ������ �ߴٸ� �Ұ���
        }


        charCon.Move(moveInput*Time.deltaTime); 

        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * Sensivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

        if(Input.GetMouseButtonDown(0))
        {
            // ���콺 ��ġ�� �߻�Ǵ� ��ġ�� ������?
            RaycastHit hit;
            if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 50f))
            {
                if(Vector3.Distance(camTrans.position , hit.point) > 2)
                {
                    firePoint.LookAt(hit.point);
                }
            }
                
            else
                firePoint.LookAt(camTrans.position + (camTrans.forward * 30f));

            Instantiate(bullet, firePoint.position, firePoint.rotation);

        }


        anim.SetFloat("moveSpeed", moveInput.magnitude);
        anim.SetBool("onGround", canJump);


    }
}
