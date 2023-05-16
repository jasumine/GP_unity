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
        // A�� ������
        if(Input.GetKeyDown(KeyCode.R))
        {
            curretCam++; // �迭�� ++ �ǰ�

            if (curretCam >= cameras.Length) // ���̺��� ������ų� �������
                curretCam = 0; // 0���� �ʱ�ȭ�Ѵ�.

            for(int i=0; i<cameras.Length; i++) // ���̸�ŭ ���鼭
            {
                if (i == curretCam) // i�� curretCam�� ��ȣ�� ���ٸ�?
                    cameras[i].SetActive(true); // �� ī�޶� true
                else cameras[i].SetActive(false); // �ٸ��� false�� ���־ ī�޶� ��ȯ!
            }
        }
    }
}
