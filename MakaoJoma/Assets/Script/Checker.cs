using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checker : MonoBehaviour
{
    public CarController car;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="CheckPoint")
        {
            car.CheckpointHit(other.GetComponent<Check>().checkNum);
        }
    }
}
