using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHover : MonoBehaviour
{

    private Outline outline;

    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    void OnMouseEnter()
    {
        outline.enabled = true;
    }

    void OnMouseExit()
    {
        outline.enabled = false;
    }

}
