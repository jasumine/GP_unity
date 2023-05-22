using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private CharacterController charCon;

    private Vector3 moveInput;

    public Transform camTrans;

    public float Sensivity;

    public bool invertX;
    public bool invertY;
    void Start()
    {
        charCon = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 verMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");

        moveInput = verMove + horiMove;
        moveInput.Normalize();
        moveInput = moveInput * moveSpeed;

        charCon.Move(moveInput*Time.deltaTime); // 리지드바디 없이 일일이 중력을 구현가능

        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * Sensivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));
    }
}
