using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{

    [SerializeField]
    private float rotationSpeed = 10.0f;

    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
            float rotationY = -Input.GetAxis("Mouse Y") * rotationSpeed * Mathf.Deg2Rad;

            transform.Rotate(Vector3.up, rotationX);
            transform.Rotate(Vector3.right, rotationY);

            // Clamp the rotation on the x axis to prevent the camera from flipping
            Vector3 currentRotation = transform.localEulerAngles;

            if(currentRotation.x > 15f && currentRotation.x < 180f)
            {
                currentRotation.x = 15f;
            }
            else if(currentRotation.x < 340f && currentRotation.x >= 180f)
            {
                currentRotation.x = 340f;
            }

            currentRotation.z = 0.0f;
            transform.localEulerAngles = currentRotation;
        }
    }
}
