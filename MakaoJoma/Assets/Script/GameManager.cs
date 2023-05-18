using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // �̱�������

    public Check[] checkPoints;

    public int totalLaps;

    public CarController playerCar;
    public List<CarController> aiCars = new List<CarController>(); // c++ �� vector����

    public int playerPosition;

    public float timeBetweenCheck = .2f;
    private float checkCounter;

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
        checkCounter -= Time.deltaTime;
        if(checkCounter <0) 
        {
            playerPosition = 1;
            foreach (CarController aiCars in aiCars)
            {
                if (aiCars.currentLap > playerCar.currentLap) // lap ��
                    playerPosition++;
                else if (aiCars.currentLap == playerCar.currentLap) // lap�� ���ٸ�
                {
                    if (aiCars.nextCheckpoint > playerCar.nextCheckpoint) // ���� checkPoint ��
                        playerPosition++;
                    else if (aiCars.nextCheckpoint == playerCar.nextCheckpoint) // checkPoint�� ���ٸ�
                    {
                        if (Vector3.Distance(aiCars.transform.position, checkPoints[aiCars.nextCheckpoint].transform.position) // ���� checkPoint������ �Ÿ� ��
                            > Vector3.Distance(playerCar.transform.position, checkPoints[playerCar.nextCheckpoint].transform.position))
                        {
                            playerPosition++;
                        }
                    }
                }
            }
        }
        //UIManager.instance.LankerText.text = string.Format($"{playerPosition})
        checkCounter = timeBetweenCheck;
        
    }
}
