using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class UpgradeManager : MonoBehaviour
{

    bool played = false;

    [SerializeField]
    private Animator fadeMenu;

    [SerializeField]
    private AudioSource clickSound;

    private void Start()
    {
        StartCoroutine(WaitToDisableFade());
    }

    public void PlayGame()
    {
        if(played)
            return;
        clickSound.Play();
        played = true;
        StartCoroutine(WaitForFade(false));
    }

    public void PlayTestRun()
    {
        if(played)
            return;
        played = true;
        StartCoroutine(WaitForFade(false));
    }

    public void SkipTutorial()
    {
        if (played)
            return;
        played = true;
        GameMaster.CompletedTutorial = true;
        StopAllCoroutines();
        StartCoroutine(WaitForFade(true));
    }

    IEnumerator WaitForFade(bool skipped)
    {
        fadeMenu.gameObject.SetActive(true);
        fadeMenu.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        if (!skipped)
        {
            if (GameMaster.CompletedTutorial)
                SceneManager.LoadScene("Level");
            else
                SceneManager.LoadScene("TestRun");
        }
        else
        {
            SceneManager.LoadScene("Upgrade");
        }
    }

    IEnumerator WaitToDisableFade()
    {
        yield return new WaitForSeconds(1f);
        fadeMenu.gameObject.SetActive(false);
    }

}
