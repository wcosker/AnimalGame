using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
private Transform Target;
public float Speed = 1f;
public Vector3 Offset;

void Start()
{
    Target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
}

void LateUpdate()
{
    //if (transform.position.x >= 0 && transform.position.y >= 0)
    //{
    // Compute the position the object will reach
    Vector3 desiredPosition = Target.rotation * (Target.position + Offset);

    // Compute the direction the object will look at
    Vector3 desiredDirection = Vector3.Project( Target.forward, (Target.position - desiredPosition).normalized );

    // Rotate the object
    transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( desiredDirection ), Time.deltaTime * Speed );
    transform.localEulerAngles = new Vector3(50, transform.localEulerAngles.y, 0);

    // Place the object to "compensate" the rotation
    transform.position = Target.position - transform.forward * Offset.magnitude;
    //}
}
}
