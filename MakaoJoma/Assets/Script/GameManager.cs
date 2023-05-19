using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    public bool isStarting = true;
    public float timeBetweenStart = 1f;
    private float startCounter;
    public int countdown = 3;

    public bool raceComplete = false;

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

        startCounter = timeBetweenStart;

        UIManager.instance.CountText.text = countdown + "!";
    }

    // Update is called once per frame
    void Update()
    {

        if (isStarting)
        {
            startCounter -= Time.deltaTime;
            if(startCounter<0)
            {
                countdown--;
                startCounter = timeBetweenStart;

                UIManager.instance.CountText.text = countdown + "!";

                if (countdown == 0)
                {
                    isStarting = false;

                    UIManager.instance.CountText.gameObject.SetActive(false);
                    UIManager.instance.GoText.gameObject.SetActive(true);
                }
                 
            }

        }

        else
        {
            checkCounter -= Time.deltaTime;
            if (checkCounter < 0)
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
                                < Vector3.Distance(playerCar.transform.position, checkPoints[aiCars.nextCheckpoint].transform.position))
                            {
                                playerPosition++;
                            }
                        }
                    }
                }
                checkCounter = timeBetweenCheck;
            }
            UIManager.instance.RankerText.text = playerPosition + "/" + (aiCars.Count + 1);
        }
    }

    public void FInishRace()
    {
        raceComplete = true;
        switch(playerPosition)
        {
            case 1:
                UIManager.instance.raceResultText.text = "You Are First";
                break;
            case 2:
                UIManager.instance.raceResultText.text = "You Are Second";
                break;
            case 3:
                UIManager.instance.raceResultText.text = "You Are Third";
                break;
            default:
                UIManager.instance.raceResultText.text = "You Are " + playerPosition + "th";
                break;
        }
        UIManager.instance.resultScreen.SetActive(true);
    }

    public void Restart()
    {
      SceneManager.LoadScene(0);
    }
}
