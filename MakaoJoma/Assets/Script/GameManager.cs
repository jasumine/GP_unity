using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // ΩÃ±€≈Ê∆–≈œ

    public Check[] checkPoints;

    public int totalLaps;
    private void Awake() // start∫∏¥Ÿ ¥ı ª°∏Æ Ω««‡
    {
        instance = this; // ΩÃ±€≈Ê∆–≈œ
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
