using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

    private void Start()
    {
        timeLeft = GameMaster.GetHealthValue(GameMaster.HealthLevel);
        totalTime = timeLeft;
    }

    private void Update()
    {
        if(lost)
            return;

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

    IEnumerator WaitForFadeBeforeLosing()
    {
        gameOverSound.Play();
        fadeMenu.SetTrigger("Fade");
        yield return new WaitForSeconds(4f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("Upgrade");
    }

}
