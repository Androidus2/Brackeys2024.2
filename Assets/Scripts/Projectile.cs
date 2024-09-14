using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField]
    private float timeDeducedForCollision = 15.0f;

    bool collided = false;

    private void Start()
    {
        GetComponent<AudioSource>().volume *= GameMaster.SoundVolume;

        // After 1.8 seconds, start a tween to scale the projectile to 0f over 0.2 seconds
        transform.DOScale(0f, 0.2f).SetDelay(1.8f).OnComplete(() =>
        {
            // Destroy the projectile after 2 seconds
            Destroy(gameObject);
        });

        // Apply the scale tween to the children
        foreach (Transform child in transform)
        {
            child.DOScale(0f, 0.2f).SetDelay(1.8f);
        }
    }

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

            // Destroy the projectile
            transform.DOScale(0f, 0.1f).OnComplete(() =>
            {
                Destroy(gameObject);
            });

            // Destroy the projectile's children
            foreach (Transform child in transform)
            {
                child.DOScale(0f, 0.1f);
            }
        }

    }

}
