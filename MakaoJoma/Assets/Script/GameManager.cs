using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱�������

    public Check[] checkPoints;

    public int totalLaps;
    private void Awake() // start���� �� ���� ����
    {
        instance = this; // �̱�������
    }

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
