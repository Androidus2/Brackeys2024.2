using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField]
    private float timeDeducedForCollision = 15.0f;

    bool collided = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collided)
        {
            return;
        }

        if (other.CompareTag("PlayerMain"))
        {
            collided = true;

            PlayerCollision playerCollision = other.GetComponent<PlayerCollision>();

            if (playerCollision)
            {
                playerCollision.RemoveTime(timeDeducedForCollision);
            }
        }

    }

}
