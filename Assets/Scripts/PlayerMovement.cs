using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;

    [SerializeField]
    private Transform playerModel;

    [SerializeField]
    private float tiltAmount = 2f;

    [SerializeField]
    private AudioSource jetSound;

    [SerializeField]
    private AudioSource jetStartSound;

    [SerializeField]
    private AudioSource jetStopSound;

    private Rigidbody rb;
    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        ApplySounds();

        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        // Move the player using add force, so it has a floaty feel as it is a UFO
        rb.AddForce(new Vector3(movement.x, 0, movement.y) * speed);

        // Tilt the player in the direction it is moving
        //playerModel.localRotation = Quaternion.Euler(movement.y * tiltAmount, 0, -movement.x * tiltAmount);
        // use the velocity of the rigidbody to determine the tilt
        playerModel.localRotation = Quaternion.Euler(rb.velocity.z * tiltAmount, 0, -rb.velocity.x * tiltAmount);

        // If it is spinning on the X or Z axis, add torque to cancel it
        if (Mathf.Abs(rb.angularVelocity.x) > 0.1f)
        {
            rb.AddTorque(Vector3.right * -rb.angularVelocity.x);
        }
        if (Mathf.Abs(rb.angularVelocity.z) > 0.1f)
        {
            rb.AddTorque(Vector3.forward * -rb.angularVelocity.z);
        }
    }

    private void ApplySounds()
    {
        Vector2 currentMovement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if(currentMovement != Vector2.zero && movement == Vector2.zero)
        {
            jetStartSound.Play();
            if(jetStopSound.isPlaying)
                jetStopSound.Stop();
        }
        else if(currentMovement == Vector2.zero && movement != Vector2.zero)
        {
            jetStopSound.Play();
            if(jetStartSound.isPlaying)
                jetStartSound.Stop();
            if(jetSound.isPlaying)
                jetSound.Stop();
        }
        else if(currentMovement != Vector2.zero)
        {
            if(!jetSound.isPlaying && !jetStartSound.isPlaying)
                jetSound.Play();
        }
        else
        {
            if(jetSound.isPlaying)
                jetSound.Stop();
        }
    }

}
