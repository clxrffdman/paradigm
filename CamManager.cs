using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{

    public Vector3 camPos;
    

    //Sets camPos to the position of this object every frame.
    void Update()
    {
        camPos = transform.position;
    }
}
