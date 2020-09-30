using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;

    public float smoothSpeed = 0.125f;
    public float offset;

    Camera myCam;

    void Start()
    {
        myCam = GetComponent<Camera>();

    }

    void FixedUpdate()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, smoothSpeed) + new Vector3(0, 0, -10);

        }

             
    }


}
