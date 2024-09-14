using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOutlineManager : MonoBehaviour
{

    SphereCollider sphereCollider;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();

        if (sphereCollider == null)
            Debug.LogError("SphereCollider not found on PlayerOutlineManager");
        else
            sphereCollider.radius = GameMaster.GetRadarValue(GameMaster.RadarLevel);
    }

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
