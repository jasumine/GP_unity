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
        // Debug.Log(Input.mousePosition); 화면 좌표
       // Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); viewport

        if(Input.GetMouseButtonDown(0))
        {
            //// 마우스가 찍은 x,와 y 위치에서 camera의 near값을 빼준다.
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            //Vector3 dir = mousePos - Camera.main.transform.position; // 마우스의 위치에서 카메라의 위치값을 빼주면 실제 transfrom.position이 나오게된다?!
            //dir = dir.normalized;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // screenPoint to ray를 풀면 위의 3줄이 된다!

            Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

            RaycastHit hit;
            if(Physics.Raycast( ray, out hit,100.0f))
            {
                Debug.Log(hit.collider.gameObject.name);
            }
        }
    }
}
