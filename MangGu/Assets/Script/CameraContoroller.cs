using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class CameraContoroller : MonoBehaviour
{
    public Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;  
    }
}
