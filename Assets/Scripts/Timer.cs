using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Timer : MonoBehaviour
{

    [SerializeField]
    private Image timerFill;

    [SerializeField]
    private Slider timerFill2;

    [SerializeField]
    private Animator fadeMenu;

    [SerializeField]
    private AudioSource gameOverSound;

    [SerializeField]
    private AudioSource timeLeft30;

    private float timeLeft;

    private float totalTime;

    bool lost = false;
    bool played30Second = false;

    List<AudioSource> audios;

    private void Start()
    {
        timeLeft = GameMaster.GetHealthValue(GameMaster.HealthLevel);
        totalTime = timeLeft;

        audios = new List<AudioSource>();
        AudioSource[] tmpAudios = FindObjectsOfType<AudioSource>();
        foreach(AudioSource audio in tmpAudios)
        {
            audio.volume *= GameMaster.SoundVolume;
            audios.Add(audio);
        }

        Debug.Log("Found " + audios.Count + " audio sources in level.");
    }

    private void Update()
    {
        if(lost)
            return;

        if (!GameMaster.CompletedTutorial)
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                EndLevel();
            }
        }

        timeLeft -= Time.deltaTime;
        timerFill.fillAmount = timeLeft / totalTime;

        timerFill2.value = timeLeft / totalTime;

        if(timeLeft >= 28 && timeLeft <= 31 && !played30Second)
        {
            timeLeft30.Play();
            played30Second = true;
        }


        if (timeLeft <= 0)
        {
            lost = true;
            StartCoroutine(WaitForFadeBeforeLosing());
        }
    }

    public void RemoveTime(float time)
    {
        timeLeft -= time;
    }

    public float GetTimeLeft()
    {
        return timeLeft;
    }

    public void EndLevel()
    {
        if(lost || GameMaster.CompletedTutorial)
            return;
        lost = true;
        StopAllCoroutines();
        StartCoroutine(WaitForLevelEndOnTutorial());
    }

    IEnumerator WaitForLevelEndOnTutorial()
    {
        fadeMenu.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameMaster.CompletedTutorial = true;
        SceneManager.LoadScene("Upgrade");
    }

    IEnumerator WaitForFadeBeforeLosing()
    {
        gameOverSound.Play();
        fadeMenu.SetTrigger("Fade");
        yield return new WaitForSeconds(4f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if(GameMaster.CompletedTutorial)
            SceneManager.LoadScene("Upgrade");
        else
        {
            GameMaster.TutorialStage = 5;
            SceneManager.LoadScene("TestRun");
        }
    }

}
