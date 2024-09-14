using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

public class Subtitles : MonoBehaviour
{

    [SerializeField]
    private string[] subtitles;

    [SerializeField]
    private TextMeshProUGUI text;

    bool isLoading = false;

    int minusAmount;

    private void Start()
    {
        minusAmount = GameMaster.TutorialStage;
    }

    public void LoadSubtitle(float totalTime, Action<int> callback)
    {
        if (isLoading)
        {
            StopAllCoroutines();
        }
        isLoading = true;
        StartCoroutine(DisplaySubtitle(totalTime, callback));
    }

    IEnumerator DisplaySubtitle(float totalTime, Action<int> callback)
    {
        string currentSubtitle = subtitles[GameMaster.TutorialStage - minusAmount];

        // Break the subtitles by the '|' character into an array of strings
        string[] splitSubtitles = currentSubtitle.Split('|');
        int totalCharacterCount = 0;

        // Loop through the array of strings
        for (int i = 0; i < splitSubtitles.Length; i++)
        {
            // Add the length of the current string to the total character count
            totalCharacterCount += splitSubtitles[i].Length;
        }

        float[] splitSubtitlesTime = new float[splitSubtitles.Length];
        for(int i = 0; i < splitSubtitles.Length; i++)
        {
            splitSubtitlesTime[i] = totalTime / totalCharacterCount * splitSubtitles[i].Length;
        }

        for (int i = 0; i < splitSubtitles.Length; i++)
        {
            text.text = splitSubtitles[i];
            yield return new WaitForSeconds(splitSubtitlesTime[i]);
        }

        text.text = "";
        isLoading = false;

        callback.Invoke(-1);
    }

}
