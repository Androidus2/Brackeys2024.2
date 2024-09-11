using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainFollow : MonoBehaviour
{

    [SerializeField]
    private Transform target;

    private void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
    }

}
