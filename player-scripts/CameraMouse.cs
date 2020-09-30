using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouse : MonoBehaviour
{
    public float Damping = 12.0f;
    public Transform Player;
    public float Height = 13.0f;
    public float Offset = 0.0f;
    public float maxDistance = 3.0f;
    public float distance;
 
    private Vector3 Center;
    public float ViewDistance = 3.0f;
 
    void Update()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = ViewDistance;
        Vector3 CursorPosition = Camera.main.ScreenToWorldPoint(mousePos);

        var PlayerPosition = Player.position;

        Center = new Vector3((PlayerPosition.x + CursorPosition.x) / 2, PlayerPosition.y, transform.position.z);

        distance = Vector3.Distance(transform.position, PlayerPosition);

        if (distance > maxDistance)
        {
            Center = new Vector3(maxDistance, Center.y, Center.z);
        }

        transform.position = Vector3.Lerp(transform.position, Center, Time.deltaTime * Damping);
    }
}