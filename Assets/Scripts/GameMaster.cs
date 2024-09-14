using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{

    private static int totalScrap = 0;

    private static int healthLevel = 1;
    private static int speedLevel = 1;
    private static int radarLevel = 1;
    private static int storageLevel = 1;

    private static int maxHealthLevel = 4;
    private static int maxSpeedLevel = 5;
    private static int maxRadarLevel = 3;
    private static int maxStorageLevel = 4;

    [SerializeField]
    private float[] speedValues;
    [SerializeField]
    private float[] radarValues;
    [SerializeField]
    private int[] storageValues;
    [SerializeField]
    private float[] healthValues;

    [SerializeField]
    private int[] speedCosts;
    [SerializeField]
    private int[] radarCosts;
    [SerializeField]
    private int[] storageCosts;
    [SerializeField]
    private int[] healthCosts;

    private static bool completedTutorial = false;
    private static int tutorialStage = 1;

    private static GameMaster instance;
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

            totalScrap = 1000;

            healthLevel = 1;
            speedLevel = 1;
            radarLevel = 1;
            storageLevel = 1;

            SceneManager.LoadScene("Upgrade");
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public static int MaxHealthLevel
    {
        get
        {
            return maxHealthLevel;
        }
    }
    public static int MaxSpeedLevel
    {
        get
        {
            return maxSpeedLevel;
        }
    }
    public static int MaxRadarLevel
    {
        get
        {
            return maxRadarLevel;
        }
    }
    public static int MaxStorageLevel
    {
        get
        {
            return maxStorageLevel;
        }
    }

    public static float GetSpeedValue(int level)
    {
        return instance.speedValues[level - 1];
    }
    public static float GetRadarValue(int level)
    {
        return instance.radarValues[level - 1];
    }
    public static int GetStorageValue(int level)
    {
        return instance.storageValues[level - 1];
    }
    public static float GetHealthValue(int level)
    {
        return instance.healthValues[level - 1];
    }

    public static int GetSpeedCost(int level)
    {
        return instance.speedCosts[level - 1];
    }
    public static int GetRadarCost(int level)
    {
        return instance.radarCosts[level - 1];
    }
    public static int GetStorageCost(int level)
    {
        return instance.storageCosts[level - 1];
    }
    public static int GetHealthCost(int level)
    {
        return instance.healthCosts[level - 1];
    }

    public static bool CompletedTutorial
    {
        get
        {
            return completedTutorial;
        }
        set
        {
            completedTutorial = value;
        }
    }
    public static int TutorialStage
    {
        get
        {
            return tutorialStage;
        }
        set
        {
            tutorialStage = value;
        }
    }
}
