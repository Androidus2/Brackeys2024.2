using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    private static int totalScrap = 0;
    private static int totalTime = 120;

    private static int healthLevel = 4;
    private static int speedLevel = 5;
    private static int radarLevel = 3;
    private static int storageLevel = 4;

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

    public static int HealthLevel
    {
        get
        {
            return healthLevel;
        }
        set
        {
            healthLevel = value;
        }
    }

    public static int SpeedLevel
    {
        get
        {
            return speedLevel;
        }
        set
        {
            speedLevel = value;
        }
    }

    public static int RadarLevel
    {
        get
        {
            return radarLevel;
        }
        set
        {
            radarLevel = value;
        }
    }

    public static int StorageLevel
    {
        get
        {
            return storageLevel;
        }
        set
        {
            storageLevel = value;
        }
    }

}
