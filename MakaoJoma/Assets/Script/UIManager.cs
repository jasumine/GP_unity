using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public TMP_Text currentTimeText, bestTimeText, LapText, RankerText, CountText, GoText, raceResultText;
    
    public GameObject resultScreen;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartButton()
    {
        GameManager.instance.Restart();
    }
}
