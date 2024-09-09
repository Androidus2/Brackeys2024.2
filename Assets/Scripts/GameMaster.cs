using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    private static int totalScrap = 0;
    private static int totalTime = 30;

    private static GameMaster instance;

    public static int TotalScrap
    {
        get
        {
            return totalScrap;
        }
        set
        {
            totalScrap = value;
            Debug.Log("Total scrap: " + totalScrap);
        }
    }

    public static int TotalTime
    {
        get
        {
            return totalTime;
        }
        set
        {
            totalTime = value;
        }
    }

    public static GameMaster Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            totalScrap = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
