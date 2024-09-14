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
        StartCoroutine(WaitForFade());
    }

    IEnumerator WaitForFade()
    {
        fadeMenu.gameObject.SetActive(true);
        fadeMenu.SetTrigger("Fade");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Level");
    }

    IEnumerator WaitToDisableFade()
    {
        yield return new WaitForSeconds(1f);
        fadeMenu.gameObject.SetActive(false);
    }

}
