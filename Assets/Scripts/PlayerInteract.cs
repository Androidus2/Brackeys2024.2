using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    [SerializeField]
    private Material beam;

    [SerializeField]
    private float beamSpeed = 2.0f;

    [SerializeField]
    private float onBeamTime = 1.0f;

    private int state = 0; // 0 - off, 1 - turning on, 2 - turning off, 3 - on
    private List<Interactable> interactiblesInRange = new List<Interactable>();

    private int nameID;
    private float progress = 0;
    private float onTimer = 0;

    private void Start()
    {
        nameID = Shader.PropertyToID("_Progress");
        beam.SetFloat(nameID, progress);
    }

    private void Update()
    {
        UpdateBeam();
    }

    private void UpdateBeam()
    {
        if (Input.GetKeyDown(KeyCode.E) && state == 0)
        {
            if (interactiblesInRange.Count > 0)
            {
                state = 1;
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
                    if(interactable)
                        interactable.Interact(RemoveInteractedObject);
                }
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
        if(interact && interactiblesInRange.Contains(interact))
            interactiblesInRange.Remove(interact);

        // If there are nulls in the list, remove them
        interactiblesInRange.RemoveAll(item => item == null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            Interactable interactable = other.GetComponent<Interactable>();
            if (interactable)
            {
                if(!interactiblesInRange.Contains(interactable))
                    interactiblesInRange.Add(interactable);
            }
            else
            {
                Debug.LogError("Interactable component not found on object: " + other.name);
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
            }
            else
            {
                Debug.LogError("Interactable component not found on object: " + other.name);
            }
        }
    }

}
