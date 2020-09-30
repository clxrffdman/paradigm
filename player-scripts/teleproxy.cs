using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleproxy : MonoBehaviour
{
    public Vector3 mousePosition;
    
    
    public Vector3 playerPos;
    public RaycastHit2D ray;
    public float teleDistance;
    public float teleDistanceMax;
    public float distance;

    public float crosshairDistance;

    public LayerMask whatToHit;
    public LayerMask[] hitableLayers;
    public GameObject TeleLocation;
    public GameObject rootShoot;

    public Vector3 hitLocation;
    public float teleAngle;

    public float teleOffset;

    Vector2 realpos;

    public LineRenderer lineRend;



    // Start is called before the first frame update
    void Start()
    {
        TeleLocation = GameObject.Find("TeleLocation");
        rootShoot = GameObject.Find("RootShoot");

        for(int i = 0; i < hitableLayers.Length; i++)
        {
            whatToHit = whatToHit + hitableLayers[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        crosshairDistance = Vector3.Distance(GameObject.Find("Crosshair").transform.position, transform.position);
        if(crosshairDistance < teleDistanceMax)
        {
            teleDistance = crosshairDistance;
        }
        else
        {
            teleDistance = teleDistanceMax;
        }

        teleAngle = rootShoot.GetComponent<Shoot>().currentGun.GetComponent<Aim>().rotation;
        transform.LookAt(GameObject.Find("CrosshairProxy").transform);
        ray = Physics2D.Raycast(transform.position, transform.forward, teleDistance, whatToHit);

        if(ray.collider != null)
        {
            
            TeleLocation.transform.position = new Vector3(ray.point.x + teleOffset*Mathf.Cos(teleAngle*Mathf.Deg2Rad), ray.point.y + teleOffset * Mathf.Sin(teleAngle * Mathf.Deg2Rad), TeleLocation.transform.position.z);
            
            lineRend.SetPosition(0, transform.position);
            lineRend.SetPosition(1, TeleLocation.transform.position);

        }
        if (ray.collider == null)
        {
            
            TeleLocation.transform.position = transform.position + transform.forward.normalized * teleDistance;
            
            lineRend.SetPosition(0, transform.position);
            lineRend.SetPosition(1, TeleLocation.transform.position);
        }
        



        /*
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.position = new Vector3(mousePosition.x, mousePosition.y, 0f);
        */
    }
}
