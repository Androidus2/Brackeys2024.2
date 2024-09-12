using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    [SerializeField]
    private float graceTimeAfterCollision = 1.0f;

    [SerializeField]
    private float timeDeducedForCollision = 5.0f;

    private float timeSinceLastCollision = 0.0f;

    Timer timer;

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
    }

    private void Update()
    {
        timeSinceLastCollision += Time.deltaTime;
    }

    public void RemoveTime(float time)
    {
        if (timer != null)
        {
            timer.RemoveTime(time);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (timeSinceLastCollision < graceTimeAfterCollision)
        {
            return;
        }

        Debug.Log("Player collided with " + collision.gameObject.name);

        if (timer != null)
        {
            timer.RemoveTime(timeDeducedForCollision);
        }

        timeSinceLastCollision = 0.0f;
    }

}
