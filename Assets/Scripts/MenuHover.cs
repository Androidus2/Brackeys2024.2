using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuHover : MonoBehaviour
{

    private Outline outline;

    [SerializeField]
    private Texture2D defaultCursorTexture;
    [SerializeField]
    private Texture2D hoverCursorTexture;

    [SerializeField]
    private GameObject upgradeMenu;

    [SerializeField]
    private MenuShipLoader menuShipLoader;

    [SerializeField]
    private TextMeshProUGUI currentHealthText;
    [SerializeField]
    private TextMeshProUGUI currentRadarText;
    [SerializeField]
    private TextMeshProUGUI currentStorageText;
    [SerializeField]
    private TextMeshProUGUI currentSpeedText;

    [SerializeField]
    private TextMeshProUGUI nextHealthText;
    [SerializeField]
    private TextMeshProUGUI nextRadarText;
    [SerializeField]
    private TextMeshProUGUI nextStorageText;
    [SerializeField]
    private TextMeshProUGUI nextSpeedText;

    [SerializeField]
    private TextMeshProUGUI healthCostText;
    [SerializeField]
    private TextMeshProUGUI radarCostText;
    [SerializeField]
    private TextMeshProUGUI storageCostText;
    [SerializeField]
    private TextMeshProUGUI speedCostText;

    [SerializeField]
    private TextMeshProUGUI totalScrapText;

    [SerializeField]
    private AudioSource upgradeSound;

    [SerializeField]
    private AudioSource clickSound;

    [SerializeField]
    private GameObject playButton;

    [SerializeField]
    private GameObject settingsButton;

    [SerializeField]
    private AudioSource[] subtitleAudios;

    [SerializeField]
    private Subtitles subtitles;

    [SerializeField]
    private GameObject subtitleTextBox;

    void Start()
    {
        SetUp();
        if (!GameMaster.CompletedTutorial)
            DoTutorial();
    }

    private void SetUp()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);

        currentHealthText.text = "" + GameMaster.GetHealthValue(GameMaster.HealthLevel);
        currentRadarText.text = "" + GameMaster.GetRadarValue(GameMaster.RadarLevel);
        currentStorageText.text = "" + GameMaster.GetStorageValue(GameMaster.StorageLevel);
        currentSpeedText.text = "" + GameMaster.GetSpeedValue(GameMaster.SpeedLevel);

        if (GameMaster.HealthLevel < GameMaster.MaxHealthLevel)
            nextHealthText.text = "" + GameMaster.GetHealthValue(GameMaster.HealthLevel + 1);
        else
            nextHealthText.text = "MAX";

        if (GameMaster.RadarLevel < GameMaster.MaxRadarLevel)
            nextRadarText.text = "" + GameMaster.GetRadarValue(GameMaster.RadarLevel + 1);
        else
            nextRadarText.text = "MAX";

        if (GameMaster.StorageLevel < GameMaster.MaxStorageLevel)
            nextStorageText.text = "" + GameMaster.GetStorageValue(GameMaster.StorageLevel + 1);
        else
            nextStorageText.text = "MAX";

        if (GameMaster.SpeedLevel < GameMaster.MaxSpeedLevel)
            nextSpeedText.text = "" + GameMaster.GetSpeedValue(GameMaster.SpeedLevel + 1);
        else
            nextSpeedText.text = "MAX";

        if (GameMaster.HealthLevel < GameMaster.MaxHealthLevel)
            healthCostText.text = "" + GameMaster.GetHealthCost(GameMaster.HealthLevel);
        else
        {
            healthCostText.text = "";
            healthCostText.transform.parent.gameObject.SetActive(false);
        }

        if (GameMaster.RadarLevel < GameMaster.MaxRadarLevel)
            radarCostText.text = "" + GameMaster.GetRadarCost(GameMaster.RadarLevel);
        else
        {
            radarCostText.text = "";
            radarCostText.transform.parent.gameObject.SetActive(false);
        }

        if (GameMaster.StorageLevel < GameMaster.MaxStorageLevel)
            storageCostText.text = "" + GameMaster.GetStorageCost(GameMaster.StorageLevel);
        else
        {
            storageCostText.text = "";
            storageCostText.transform.parent.gameObject.SetActive(false);
        }

        if (GameMaster.SpeedLevel < GameMaster.MaxSpeedLevel)
            speedCostText.text = "" + GameMaster.GetSpeedCost(GameMaster.SpeedLevel);
        else
        {
            speedCostText.text = "";
            speedCostText.transform.parent.gameObject.SetActive(false);
        }

        totalScrapText.text = "" + GameMaster.TotalScrap;
    }

    private void DoTutorial()
    {
        settingsButton.SetActive(false);
        playButton.SetActive(false);
        totalScrapText.transform.parent.gameObject.SetActive(false);
        subtitleTextBox.SetActive(false);

        StartCoroutine(WaitBeforeAdvancingSubtitles(2f));
    }

    private void AdvanceTutorial(int dummy)
    {
        GameMaster.TutorialStage++;
        subtitleTextBox.SetActive(false);
        StartCoroutine(WaitBeforeAdvancingSubtitles(2f));
    }

    IEnumerator WaitBeforeAdvancingSubtitles(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        subtitleTextBox.SetActive(true);
        subtitleAudios[GameMaster.TutorialStage - 1].Play();
        subtitles.LoadSubtitle(subtitleAudios[GameMaster.TutorialStage - 1].clip.length, AdvanceTutorial);
    }

    private void EnableUpgradePanel()
    {
        playButton.SetActive(false);
        settingsButton.SetActive(false);
        upgradeMenu.SetActive(true);
        outline.enabled = false;
        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void DisableUpgradePanel()
    {
        if(!upgradeMenu.activeSelf)
            return;
        upgradeMenu.SetActive(false);
        playButton.SetActive(true);
        settingsButton.SetActive(true);
    }

    void OnMouseDown()
    {
        if (!upgradeMenu.activeSelf)
        {
            clickSound.Play();
            EnableUpgradePanel();
        }
    }

    void OnMouseEnter()
    {
        if(upgradeMenu.activeSelf)
            return;
        outline.enabled = true;
        Cursor.SetCursor(hoverCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        outline.enabled = false;
        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void UpgradeHealth()
    {
        clickSound.Play();
        if (GameMaster.HealthLevel < GameMaster.MaxHealthLevel && GameMaster.TotalScrap >= GameMaster.GetHealthCost(GameMaster.HealthLevel))
        {
            GameMaster.TotalScrap -= GameMaster.GetHealthCost(GameMaster.HealthLevel);
            GameMaster.HealthLevel++;
            menuShipLoader.LoadShip();
            currentHealthText.text = "" + GameMaster.GetHealthValue(GameMaster.HealthLevel);
            if (GameMaster.HealthLevel < GameMaster.MaxHealthLevel)
            {
                nextHealthText.text = "" + GameMaster.GetHealthValue(GameMaster.HealthLevel + 1);
                healthCostText.text = "" + GameMaster.GetHealthCost(GameMaster.HealthLevel);
            }
            else
            {
                nextHealthText.text = "MAX";
                healthCostText.text = "";
                healthCostText.transform.parent.gameObject.SetActive(false);
            }

            totalScrapText.text = "" + GameMaster.TotalScrap;
            upgradeSound.Play();
            DisableUpgradePanel();
        }
    }

    public void UpgradeRadar()
    {
        clickSound.Play();
        if (GameMaster.RadarLevel < GameMaster.MaxRadarLevel && GameMaster.TotalScrap >= GameMaster.GetRadarCost(GameMaster.RadarLevel))
        {
            GameMaster.TotalScrap -= GameMaster.GetRadarCost(GameMaster.RadarLevel);
            GameMaster.RadarLevel++;
            menuShipLoader.LoadShip();
            currentRadarText.text = "" + GameMaster.GetRadarValue(GameMaster.RadarLevel);
            if(GameMaster.RadarLevel < GameMaster.MaxRadarLevel)
            {
                nextRadarText.text = "" + GameMaster.GetRadarValue(GameMaster.RadarLevel + 1);
                radarCostText.text = "" + GameMaster.GetRadarCost(GameMaster.RadarLevel);
            }
            else
            {
                nextRadarText.text = "MAX";
                radarCostText.text = "";
                radarCostText.transform.parent.gameObject.SetActive(false);
            }

            totalScrapText.text = "" + GameMaster.TotalScrap;
            upgradeSound.Play();
            DisableUpgradePanel();
        }
    }

    public void UpgradeStorage()
    {
        clickSound.Play();
        if (GameMaster.StorageLevel < GameMaster.MaxStorageLevel && GameMaster.TotalScrap >= GameMaster.GetStorageCost(GameMaster.StorageLevel))
        {
            GameMaster.TotalScrap -= GameMaster.GetStorageCost(GameMaster.StorageLevel);
            GameMaster.StorageLevel++;
            menuShipLoader.LoadShip();
            currentStorageText.text = "" + GameMaster.GetStorageValue(GameMaster.StorageLevel);
            if(GameMaster.StorageLevel < GameMaster.MaxStorageLevel)
            {
                nextStorageText.text = "" + GameMaster.GetStorageValue(GameMaster.StorageLevel + 1);
                storageCostText.text = "" + GameMaster.GetStorageCost(GameMaster.StorageLevel);
            }
            else
            {
                nextStorageText.text = "MAX";
                storageCostText.text = "";
                storageCostText.transform.parent.gameObject.SetActive(false);
            }

            totalScrapText.text = "" + GameMaster.TotalScrap;
            upgradeSound.Play();
            DisableUpgradePanel();
        }
    }

    public void UpgradeSpeed()
    {
        clickSound.Play();
        if (GameMaster.SpeedLevel < GameMaster.MaxSpeedLevel && GameMaster.TotalScrap >= GameMaster.GetSpeedCost(GameMaster.SpeedLevel))
        {
            GameMaster.TotalScrap -= GameMaster.GetSpeedCost(GameMaster.SpeedLevel);
            GameMaster.SpeedLevel++;
            menuShipLoader.LoadShip();
            currentSpeedText.text = "" + GameMaster.GetSpeedValue(GameMaster.SpeedLevel);
            if(GameMaster.SpeedLevel < GameMaster.MaxSpeedLevel)
            {
                nextSpeedText.text = "" + GameMaster.GetSpeedValue(GameMaster.SpeedLevel + 1);
                speedCostText.text = "" + GameMaster.GetSpeedCost(GameMaster.SpeedLevel);
            }
            else
            {
                nextSpeedText.text = "MAX";
                speedCostText.text = "";
                speedCostText.transform.parent.gameObject.SetActive(false);
            }

            totalScrapText.text = "" + GameMaster.TotalScrap;
            upgradeSound.Play();
            DisableUpgradePanel();
        }
    }

}
