using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="CheckPoint")
        {
            Debug.Log("num:" + other.GetComponent<Check>().checkNum);
        }
    }
}
