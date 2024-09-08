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

    private Rigidbody rb;
    private Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
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
    }

}
