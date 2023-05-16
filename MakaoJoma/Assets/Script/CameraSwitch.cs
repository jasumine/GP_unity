using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject[] cameras;
    private int curretCam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // A를 누르면
        if(Input.GetKeyDown(KeyCode.R))
        {
            curretCam++; // 배열이 ++ 되고

            if (curretCam >= cameras.Length) // 길이보다 길어지거나 같을경우
                curretCam = 0; // 0으로 초기화한다.

            for(int i=0; i<cameras.Length; i++) // 길이만큼 돌면서
            {
                if (i == curretCam) // i와 curretCam의 번호가 같다면?
                    cameras[i].SetActive(true); // 그 카메라를 true
                else cameras[i].SetActive(false); // 다르면 false로 해주어서 카메라 전환!
            }
        }
    }
}
