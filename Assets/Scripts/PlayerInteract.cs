using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private GameObject instructionObject;

    [SerializeField]
    private Material beam;

    [SerializeField]
    private float beamSpeed = 2.0f;

    [SerializeField]
    private float onBeamTime = 1.0f;

    private int maxWeight;
    private int currentWeight = 0;

    [SerializeField]
    private TextMeshProUGUI weightText;

    [SerializeField]
    private SphereCollider objectHolder;

    [SerializeField]
    private AudioSource beamDown;

    [SerializeField]
    private AudioSource beamUp;

    [SerializeField]
    private AudioSource noStorage;

    [SerializeField]
    private TextMeshProUGUI collectedScrapText;

    [SerializeField]
    private Subtitles subtitles;

    [SerializeField]
    private GameObject subtitleTextBox;

    [SerializeField]
    private AudioSource[] subtitleAudios;

    [SerializeField]
    private Timer timer;

    private int collectedScrap = 0;

    private int state = 0; // 0 - off, 1 - turning on, 2 - turning off, 3 - on
    private List<Interactable> interactiblesInRange = new List<Interactable>();

    private bool bunkerInRange = false;
    private List<Dummy> dummies = new List<Dummy>();

    private int nameID;
    private float progress = 0;
    private float onTimer = 0;

    private List<Interactable> allInteractiblesInRange = new List<Interactable>();

    private void Start()
    {
        nameID = Shader.PropertyToID("_Progress");
        beam.SetFloat(nameID, progress);

        maxWeight = GameMaster.GetStorageValue(GameMaster.StorageLevel);
        weightText.text = currentWeight + " / " + maxWeight;

        instructionObject.transform.localScale = Vector3.zero;

        if (!GameMaster.CompletedTutorial)
        {
            subtitleTextBox.transform.localScale = Vector3.zero;
            subtitleTextBox.SetActive(false);
            StartCoroutine(WaitBeforeAdvancingSubtitles(2f));
        }
    }


    private void AdvanceTutorial(int dummy)
    {
        GameMaster.TutorialStage++;
        subtitleTextBox.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => subtitleTextBox.SetActive(false));
        if (GameMaster.TutorialStage != 7 && GameMaster.TutorialStage != 9 && GameMaster.TutorialStage != 12)
            StartCoroutine(WaitBeforeAdvancingSubtitles(2f));
        else if (GameMaster.TutorialStage == 12)
            timer.EndLevel();
        else if (GameMaster.TutorialStage == 7 && interactiblesInRange.Count > 0)
        {
            instructionObject.SetActive(true);
            instructionObject.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        }
        else if (GameMaster.TutorialStage == 9 && bunkerInRange && currentWeight > 0)
        {
            instructionObject.SetActive(true);
            instructionObject.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        }
    }

    IEnumerator WaitBeforeAdvancingSubtitles(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        subtitleTextBox.SetActive(true);
        subtitleTextBox.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
        subtitleAudios[GameMaster.TutorialStage - 5].Play();
        subtitles.LoadSubtitle(subtitleAudios[GameMaster.TutorialStage - 5].clip.length, AdvanceTutorial);
    }


    private void Update()
    {
        UpdateBeam();
    }

    private void UpdateBeam()
    {
        if (Input.GetKeyDown(KeyCode.E) && state == 0)
        {
            if (interactiblesInRange.Count > 0 || (bunkerInRange && currentWeight > 0))
            {
                if(!GameMaster.CompletedTutorial && (GameMaster.TutorialStage != 7 && GameMaster.TutorialStage != 9))
                    return;

                if(!GameMaster.CompletedTutorial && GameMaster.TutorialStage == 7 && interactiblesInRange.Count == 0)
                    return;

                Debug.Log("Stage: " + GameMaster.TutorialStage);

                state = 1;

                if(instructionObject)
                    instructionObject.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => instructionObject.SetActive(false));

                beamDown.Play();
            }
            else if(GameMaster.CompletedTutorial && interactiblesInRange.Count == 0 && allInteractiblesInRange.Count > 0)
            {
                noStorage.Play();
            }
        }
        else if (state == 1)
        {
            progress += Time.deltaTime * beamSpeed;
            if (progress >= 1)
            {
                progress = 1;
                onTimer = 0;
                state = 3;
                foreach (Interactable interactable in interactiblesInRange)
                {
                    if (interactable)
                    {
                        if(currentWeight + interactable.GetWeight() <= maxWeight)
                        {
                            currentWeight += interactable.GetWeight();
                            interactable.Interact(RemoveInteractedObject);
                        }
                    }
                }

                if(!GameMaster.CompletedTutorial && GameMaster.TutorialStage == 7 && currentWeight > 0)
                {
                    StartCoroutine(WaitBeforeAdvancingSubtitles(2f));
                }

                if (bunkerInRange)
                {
                    if(!GameMaster.CompletedTutorial && GameMaster.TutorialStage == 9 && currentWeight > 0)
                    {
                        StartCoroutine(WaitBeforeAdvancingSubtitles(2f));
                    }

                    GameMaster.TotalScrap += currentWeight;
                    collectedScrap += currentWeight;
                    currentWeight = 0;
                    weightText.text = currentWeight + " / " + maxWeight;
                    collectedScrapText.text = "" + collectedScrap;
                    ReleaseDummies();
                }

                beamUp.Play();
            }
            beam.SetFloat(nameID, progress);
        }
        else if (state == 2)
        {
            progress -= Time.deltaTime * beamSpeed;
            if (progress <= 0)
            {
                progress = 0;
                state = 0;

                bool ok = false;

                foreach (Interactable interactable in interactiblesInRange)
                {
                    if (interactable)
                    {
                        if (currentWeight + interactable.GetWeight() <= maxWeight)
                        {
                            ok = true;
                            break;
                        }
                    }
                }

                if (instructionObject && ((interactiblesInRange.Count > 0 && ok) || (bunkerInRange && currentWeight > 0)))
                {
                    instructionObject.SetActive(true);
                    instructionObject.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
                }

                weightText.text = currentWeight + " / " + maxWeight;
            }
            beam.SetFloat(nameID, progress);
        }
        else if (state == 3)
        {
            onTimer += Time.deltaTime;
            if (onTimer >= onBeamTime)
            {
                state = 2;
                onTimer = 0;
            }
        }
    }

    private void RemoveInteractedObject(Interactable interact)
    {
        if(interact)
            MakeDummyObject(interact.GetDummyPrefab(), interact.transform.localScale);
        if (interact && interactiblesInRange.Contains(interact))
        {
            interactiblesInRange.Remove(interact);
        }
        if(interact && allInteractiblesInRange.Contains(interact))
        {
            allInteractiblesInRange.Remove(interact);
        }

        // If there are nulls in the list, remove them
        interactiblesInRange.RemoveAll(item => item == null);
        allInteractiblesInRange.RemoveAll(item => item == null);
    }

    private void MakeDummyObject(Transform obj, Vector3 originalSize)
    {
        if (obj)
        {
            Transform dummy = Instantiate(obj, objectHolder.transform.position, objectHolder.transform.rotation);
            dummy.SetParent(objectHolder.transform);

            // Give a random rotation
            dummy.localRotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

            // Give a random position inside the objectHolder sphere
            // Make it so the object is at most radius away from the center
            dummy.localPosition = new Vector3(Random.Range(-objectHolder.radius, objectHolder.radius), Random.Range(0, objectHolder.radius), Random.Range(-objectHolder.radius, objectHolder.radius));
            if(dummy.localPosition.magnitude > objectHolder.radius)
                dummy.localPosition = dummy.localPosition.normalized * Random.Range(0, objectHolder.radius);

            Dummy dummyScript = dummy.GetComponent<Dummy>();
            dummyScript.SetOriginalSize(originalSize);

            if (dummyScript)
            {
                dummies.Add(dummyScript);
            }
            else
            {
                Debug.LogError("Dummy component not found on object: " + obj.name);
            }
        }
    }

    private void ReleaseDummies()
    {
        foreach (Dummy dummy in dummies)
        {
            if (dummy)
            {
                dummy.Release();
            }
        }
        dummies.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (interactable)
            {
                if (!interactiblesInRange.Contains(interactable))
                {
                    if (currentWeight + interactable.GetWeight() <= maxWeight)
                    {
                        interactiblesInRange.Add(interactable);
                    }
                    allInteractiblesInRange.Add(interactable);
                }
            }
            else
            {
                Debug.LogError("Interactable component not found on object: " + other.name);
            }

            if (instructionObject && (interactiblesInRange.Count > 0 || (bunkerInRange && currentWeight > 0)) && state == 0)
            {
                if (GameMaster.CompletedTutorial || GameMaster.TutorialStage == 7)
                {
                    instructionObject.SetActive(true);
                    instructionObject.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
                }
            }
        }
        else if (other.CompareTag("Bunker"))
        {
            bunkerInRange = true;
            if (instructionObject && (interactiblesInRange.Count > 0 || (bunkerInRange && currentWeight > 0)) && state == 0)
            {
                if (GameMaster.CompletedTutorial || GameMaster.TutorialStage == 9)
                {
                    instructionObject.SetActive(true);
                    instructionObject.transform.DOScale(1, 0.2f).SetEase(Ease.OutBack);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (interactable)
            {
                if(interactiblesInRange.Contains(interactable))
                    interactiblesInRange.Remove(interactable);
                if (allInteractiblesInRange.Contains(interactable))
                    allInteractiblesInRange.Remove(interactable);
            }
            else
            {
                Debug.LogError("Interactable component not found on object: " + other.name);
            }

            if (instructionObject && interactiblesInRange.Count == 0 && !bunkerInRange)
            {
                instructionObject.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => instructionObject.SetActive(false));
            }
        }
        else if (other.CompareTag("Bunker"))
        {
            bunkerInRange = false;
            if (instructionObject && interactiblesInRange.Count == 0 && !bunkerInRange)
            {
                instructionObject.transform.DOScale(0, 0.2f).SetEase(Ease.InBack).OnComplete(() => instructionObject.SetActive(false));
            }
        }
    }

}
