using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOutlineManager : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Interactable"))
            return;
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
            interactable.SetOutline(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Interactable"))
            return;
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
            interactable.SetOutline(false);
    }

}
