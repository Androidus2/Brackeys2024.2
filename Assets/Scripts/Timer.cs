using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField]
    private Image timerFill;

    private float timeLeft;

    private void Start()
    {
        timeLeft = GameMaster.TotalTime;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        timerFill.fillAmount = timeLeft / GameMaster.TotalTime;

        if (timeLeft <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("Upgrade");
        }
    }

    public void RemoveTime(float time)
    {
        timeLeft -= time;
    }

}
