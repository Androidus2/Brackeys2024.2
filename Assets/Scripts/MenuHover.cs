using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

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

    [SerializeField]
    private UpgradeManager testRunStarter;

    [SerializeField]
    private GameObject instructionTextBox;

    [SerializeField]
    private GameObject skipText;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask shipMask;

    [SerializeField]
    private Slider volumeSlider;

    List<AudioSource> audios;
    List<float> originalVolumes;

    void Start()
    {
        SetUp();
        if (!GameMaster.CompletedTutorial)
            DoTutorial();
        else
        {
            subtitleTextBox.SetActive(false);
            skipText.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !GameMaster.CompletedTutorial)
        {
            testRunStarter.SkipTutorial();
        }
        CheckIfMouseIsOverShip();
        if (Input.GetMouseButtonUp(0) && outline.enabled)
            OnMouseDownCustom();
    }

    void CheckIfMouseIsOverShip()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, shipMask))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!upgradeMenu.activeSelf && (GameMaster.CompletedTutorial || GameMaster.TutorialStage == 4) && !outline.enabled)
                {
                    outline.enabled = true;
                    Cursor.SetCursor(hoverCursorTexture, Vector2.zero, CursorMode.Auto);
                }
            }
            else if(outline.enabled)
            {
                outline.enabled = false;
                Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);
            }
        }
        else if(outline.enabled)
        {
            outline.enabled = false;
            Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);
        }
    }

    private void SetUp()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;

        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);

        audios = new List<AudioSource>();
        originalVolumes = new List<float>();
        AudioSource[] tmpAudios = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in tmpAudios)
        {
            audios.Add(audio);
            originalVolumes.Add(audio.volume);
            audio.volume *= GameMaster.SoundVolume;
        }

        volumeSlider.value = GameMaster.SoundVolume;

        Debug.Log("Found " + audios.Count + " audio sources.");

        upgradeMenu.transform.localScale = Vector3.zero;

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

    public void ChangeVolume()
    {
        float value = volumeSlider.value;
        GameMaster.SoundVolume = value;
        for (int i = 0; i < audios.Count; i++)
        {
            audios[i].volume = originalVolumes[i] * value;
        }
    }

    private void DoTutorial()
    {
        //settingsButton.transform.parent.gameObject.SetActive(false);
        playButton.SetActive(false);
        totalScrapText.transform.parent.gameObject.SetActive(false);
        totalScrapText.transform.parent.localScale = Vector3.zero;
        subtitleTextBox.SetActive(false);
        subtitleTextBox.transform.DOScale(0, 0.2f).SetEase(Ease.OutBack);

        StartCoroutine(WaitBeforeAdvancingSubtitles(2f));
    }

    private void AdvanceTutorial(int dummy)
    {
        GameMaster.TutorialStage++;
        subtitleTextBox.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => subtitleTextBox.SetActive(false));
        if(GameMaster.TutorialStage == 3)
            skipText.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => skipText.SetActive(false));
        if(GameMaster.TutorialStage <= 3)
            StartCoroutine(WaitBeforeAdvancingSubtitles(2f));
        if (GameMaster.TutorialStage == 5)
        {
            upgradeMenu.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => upgradeMenu.SetActive(false));
            totalScrapText.transform.parent.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => totalScrapText.transform.parent.gameObject.SetActive(false));
            testRunStarter.PlayTestRun();
        }
        if(GameMaster.TutorialStage == 4)
        {
            instructionTextBox.transform.localScale = Vector3.zero;
            instructionTextBox.SetActive(true);
            instructionTextBox.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        }
    }

    IEnumerator WaitBeforeAdvancingSubtitles(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        if (!GameMaster.CompletedTutorial)
        {
            subtitleTextBox.SetActive(true);
            subtitleTextBox.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
            subtitleAudios[GameMaster.TutorialStage - 1].Play();
            subtitles.LoadSubtitle(subtitleAudios[GameMaster.TutorialStage - 1].clip.length, AdvanceTutorial);
        }
    }

    private void EnableUpgradePanel()
    {
        if(GameMaster.TutorialStage == 4 && !GameMaster.CompletedTutorial)
        {
            instructionTextBox.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => instructionTextBox.SetActive(false));
            totalScrapText.transform.parent.gameObject.SetActive(true);
            totalScrapText.transform.parent.DOScale(1, 0.2f).SetEase(Ease.OutBack);
            StartCoroutine(WaitBeforeAdvancingSubtitles(2f));
        }
        if (GameMaster.CompletedTutorial)
        {
            playButton.transform.parent.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => playButton.SetActive(false));
        }
        settingsButton.transform.parent.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => settingsButton.transform.parent.gameObject.SetActive(false));
        upgradeMenu.SetActive(true);
        upgradeMenu.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        outline.enabled = false;
        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    public void DisableUpgradePanel()
    {
        if(!upgradeMenu.activeSelf || !GameMaster.CompletedTutorial)
            return;
        upgradeMenu.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => upgradeMenu.SetActive(false));
        playButton.SetActive(true);
        settingsButton.transform.parent.gameObject.SetActive(true);
        playButton.transform.parent.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        settingsButton.transform.parent.DOScale(1, 0.2f).SetEase(Ease.OutBack);
    }

    void OnMouseDownCustom()
    {
        if (!upgradeMenu.activeSelf && (GameMaster.CompletedTutorial || GameMaster.TutorialStage == 4))
        {
            clickSound.Play();
            EnableUpgradePanel();
        }
    }

    /*void OnMouseEnter()
    {
        if(upgradeMenu.activeSelf || !(GameMaster.CompletedTutorial || GameMaster.TutorialStage == 4))
            return;
        outline.enabled = true;
        Cursor.SetCursor(hoverCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        outline.enabled = false;
        Cursor.SetCursor(defaultCursorTexture, Vector2.zero, CursorMode.Auto);
    }*/

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
