using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngineInternal;

public class PlayerController : MonoBehaviour
{
    // 캐릭터 컨트롤러를 쓸 경우 리지드바디가 없어서 직접 설정해줘야한다.
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
        // 계속 초기화 되는 y의 velocity 값을 저장해두어야함.
        float yStore = moveInput.y;

        Vector3 verMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = verMove + horiMove;
        moveInput.Normalize();

        if (Input.GetKey(KeyCode.LeftShift))
            moveInput = moveInput * runSpeed;

        else
            moveInput = moveInput * moveSpeed;

        moveInput.y = yStore; // 원래 y값을 넣어줌으로써 가속도가 붙게된다.

        moveInput.y += Physics.gravity.y * gravityScale * Time.deltaTime; // 물리적인 힘을 y축에 준다

        if(charCon.isGrounded) // 땅 위에 있을 경우
        {
            moveInput.y = Physics.gravity.y * gravityScale * Time.deltaTime; // 물리적인 힘만 넣어준다.
        }

        // 원을 위치에서 반지름 길이만큼 그린다. layer가 ground인데 그 길이가 0보다 크다(있다?)면  true
        canJump = Physics.OverlapSphere(groundCheckPoint.position, .1f, isGround).Length > 0;

        
        if (canJump)
            canDoubleJump = false; // 불가능 상태

        // 1단 점프로 하고 싶은 경우 아래 코드 2줄만 남기면 된다
        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            moveInput.y = jumpForce; // 점프하는 힘만큼 넣어준다. // 중력을 구현해둬서 알아서 내려옴!

            canDoubleJump = true; // 점프를 한 상태에서만 가능
        }

        else if(canDoubleJump && Input.GetKeyDown(KeyCode.Space))
        {
            moveInput.y = jumpForce;

            canDoubleJump = false; // 점프를 했다면 불가능
        }


        charCon.Move(moveInput*Time.deltaTime); 

        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * Sensivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

        if(Input.GetMouseButtonDown(0))
        {
            // 마우스 위치가 발사되는 위치로 가도록?
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
