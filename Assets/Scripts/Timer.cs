using System.Collections;
using System.Collections.Generic;
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
            Debug.Log("Game Over");
        }
    }

}
