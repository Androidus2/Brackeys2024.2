using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    [SerializeField]
    private Material material;

    [SerializeField]
    private float dissolveSpeed = 1.0f;

    [SerializeField]
    private int weight = 1;

    [SerializeField]
    private Transform dummyPrefab;

    private int state = 0; // 0 - on, 1 - turning off, 2 - off
    private int nameID;

    private float progress = 0;

    // Remember the callback
    private Action<Interactable> callback;

    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        nameID = Shader.PropertyToID("_Progress");
        material.SetFloat(nameID, progress);
    }

    private void Update()
    {
        UpdateDissolve();
    }

    private void UpdateDissolve()
    {
        if (state == 1)
        {
            progress += Time.deltaTime * dissolveSpeed;
            if (progress >= 1)
            {
                progress = 1;
                state = 2;
            }
            material.SetFloat(nameID, progress);
        }
        else if(state == 2)
        {
            if (callback != null)
            {
                callback(this);
                callback = null;
            }
            Destroy(gameObject);
        }
    }

    public void Interact(Action<Interactable> Callback)
    {
        if (state == 0)
        {
            state = 1;
            callback = Callback;
            // Set the material
            material.SetFloat(nameID, progress);
            GetComponent<Renderer>().material = material;
            outline.enabled = false;
        }
    }

    public int GetWeight()
    {
        return weight;
    }

    public Transform GetDummyPrefab()
    {
        return dummyPrefab;
    }

    public void SetOutline(bool value)
    {
        outline.enabled = value;
    }

}
