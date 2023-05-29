using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cast : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Input.mousePosition); ȭ�� ��ǥ
       // Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); viewport

        if(Input.GetMouseButtonDown(0))
        {
            //// ���콺�� ���� x,�� y ��ġ���� camera�� near���� ���ش�.
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            //Vector3 dir = mousePos - Camera.main.transform.position; // ���콺�� ��ġ���� ī�޶��� ��ġ���� ���ָ� ���� transfrom.position�� �����Եȴ�?!
            //dir = dir.normalized;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // screenPoint to ray�� Ǯ�� ���� 3���� �ȴ�!

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

            RaycastHit hit;
            if(Physics.Raycast( ray, out hit,100.0f))
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }
}
