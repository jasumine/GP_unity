using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // 싱글톤패턴

    public Check[] checkPoints;

    public int totalLaps;

    public CarController playerCar;
    public List<CarController> aiCars = new List<CarController>(); // c++ 의 vector역할

    public int playerPosition;

    public float timeBetweenCheck = .2f;
    private float checkCounter;

    private void Awake() // start보다 더 빨리 실행
    {
        instance = this; // 싱글톤패턴
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
                if (aiCars.currentLap > playerCar.currentLap) // lap 비교
                    playerPosition++;
                else if (aiCars.currentLap == playerCar.currentLap) // lap이 같다면
                {
                    if (aiCars.nextCheckpoint > playerCar.nextCheckpoint) // 다음 checkPoint 비교
                        playerPosition++;
                    else if (aiCars.nextCheckpoint == playerCar.nextCheckpoint) // checkPoint가 같다면
                    {
                        if (Vector3.Distance(aiCars.transform.position, checkPoints[aiCars.nextCheckpoint].transform.position) // 다음 checkPoint까지의 거리 비교
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
