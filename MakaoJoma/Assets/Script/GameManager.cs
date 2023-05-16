using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Check[] checkPoints;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<checkPoints.Length; i++)
        {
            checkPoints[i].checkNum = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
