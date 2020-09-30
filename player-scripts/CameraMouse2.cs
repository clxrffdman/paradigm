using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouse2 : MonoBehaviour
{
    public Transform Parent; // Whatever you want the camera locked to
    public Transform Obj; // The object to place the camera on
    public Transform Enviro; // Environmental camera avg point

    public bool enviroInfluence;

    GameObject levelControl;

    public float Radius = 5f;
    public float Dist;
    public float test;
    public float lerper;
    public Vector3 MousePos1, MousePos2, ScreenMouse, MouseOffset;
    private Vector3 velocity = Vector3.zero;

    public bool bramma;
    public Vector3 limit;


    public bool isLerp;
    /*
    public void OnEnable()
    {

        bramma = false;
        MousePos1 = Input.mousePosition;

        ScreenMouse = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(MousePos1.x, MousePos1.y, Obj.position.z - GetComponent<Camera>().transform.position.z));
        MouseOffset = ScreenMouse - Parent.position;
        MousePos2 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -transform.position.z));
        //IGNORE //Obj.position.y = ((MousePos2.y - Parent.position.y) / 2.0) + Parent.position.y;
        //IGNORE //Obj.position.x = ((MousePos2.x - Parent.position.x) / 2.0) + Parent.position.x;

        //BELOW LINE IS WHAT HAPPENS NORMALLY


        test = (MousePos2.x - Parent.position.x) / 2.0f;
        Obj.position = Vector3.Lerp(transform.position, new Vector3((MousePos2.x - Parent.position.x) / 2.0f + Parent.position.x, (MousePos2.y - Parent.position.y) / 2.0f + Parent.position.y, Obj.position.z),0.5f);


        Dist = Vector2.Distance(new Vector2(Obj.position.x, Obj.position.y), new Vector2(Parent.position.x, Parent.position.y));
        Vector3 d = new Vector2(transform.position.x, transform.position.y);
        //if(Dist <= Radius)
        //{
        // Obj.position = new Vector3((MousePos2.x - Parent.position.x) / 2.0f + Parent.position.x, (MousePos2.y - Parent.position.y) / 2.0f + Parent.position.y, Obj.position.z);
        // }
        var norm = MouseOffset.normalized;

        Vector3 limit = new Vector3(norm.x * Radius + Parent.position.x, norm.y * Radius + Parent.position.y, Obj.position.z);



        //BELOW LINE IS WHAT HAPPENS IF DISTANCE BETWEEN CAMERA AND PLAYER > RADIUS
        if (Vector3.Distance(d, limit) >= 0f)
        {
            //var norm = MouseOffset.normalized;
            //Obj.position.x = norm.x * Radius + Parent.position.x;
            //Obj.position.y = norm.y * Radius + Parent.position.y;
            //Obj.position = new Vector3(norm.x * Radius + Parent.position.x, norm.y * Radius + Parent.position.y, Obj.position.z);
            //Vector3 limit = new Vector3(norm.x * Radius + Parent.position.x, norm.y * Radius + Parent.position.y, Obj.position.z);

            //Obj.position = Vector3.SmoothDamp(transform.position,limit, ref velocity, lerper);
            Obj.position = Vector3.Lerp(transform.position,limit,0.5f);
        }
        

        Invoke("WaitTimer", 0.5f);



    }
    */
    public void Start()
    {
        levelControl = GameObject.Find("LevelController");
        isLerp = true;
    }

    public void WaitTimer()
    {
        bramma = true;
    }

    public void Update()
    {
        if (levelControl.GetComponent<PauseMenu>().paused == false && levelControl.GetComponent<PauseMenu>().inInventory == false)
        {
            MoveLerp();
        }
            
        
        
        



    }

    public void MoveLerp()
    {
        MousePos1 = Input.mousePosition;

        ScreenMouse = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(MousePos1.x, MousePos1.y, Obj.position.z - GetComponent<Camera>().transform.position.z));
        MouseOffset = ScreenMouse - Parent.position;
        MousePos2 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -transform.position.z));
        //IGNORE //Obj.position.y = ((MousePos2.y - Parent.position.y) / 2.0) + Parent.position.y;
        //IGNORE //Obj.position.x = ((MousePos2.x - Parent.position.x) / 2.0) + Parent.position.x;

        //BELOW LINE IS WHAT HAPPENS NORMALLY


        test = (MousePos2.x - Parent.position.x) / 2.0f;
        Obj.position = Vector3.Lerp(transform.position,new Vector3((MousePos2.x - Parent.position.x) / 2.0f + Parent.position.x, (MousePos2.y - Parent.position.y) / 2.0f + Parent.position.y, Obj.position.z),0.01f);


        Dist = Vector2.Distance(new Vector2(Obj.position.x, Obj.position.y), new Vector2(Parent.position.x, Parent.position.y));
        Vector3 d = new Vector2(transform.position.x, transform.position.y);
        //if(Dist <= Radius)
        //{
        // Obj.position = new Vector3((MousePos2.x - Parent.position.x) / 2.0f + Parent.position.x, (MousePos2.y - Parent.position.y) / 2.0f + Parent.position.y, Obj.position.z);
        // }
        var norm = MouseOffset.normalized;

        limit = new Vector3(norm.x * Radius + Parent.position.x, norm.y * Radius + Parent.position.y, Obj.position.z);



        //BELOW LINE IS WHAT HAPPENS IF DISTANCE BETWEEN CAMERA AND PLAYER > RADIUS
        if (Vector3.Distance(d, limit) >= 0f)
        {
            //var norm = MouseOffset.normalized;
            //Obj.position.x = norm.x * Radius + Parent.position.x;
            //Obj.position.y = norm.y * Radius + Parent.position.y;
            //Obj.position = new Vector3(norm.x * Radius + Parent.position.x, norm.y * Radius + Parent.position.y, Obj.position.z);
            //Vector3 limit = new Vector3(norm.x * Radius + Parent.position.x, norm.y * Radius + Parent.position.y, Obj.position.z);

            //Obj.position = Vector3.SmoothDamp(transform.position,limit, ref velocity, lerper);
            Obj.position = Vector3.Lerp(transform.position,limit,0.01f);
        }
    }


    public void MoveCam()
    {
        MousePos1 = Input.mousePosition;

        ScreenMouse = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(MousePos1.x, MousePos1.y, Obj.position.z - GetComponent<Camera>().transform.position.z));
        MouseOffset = ScreenMouse - Parent.position;
        MousePos2 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -transform.position.z));
        //IGNORE //Obj.position.y = ((MousePos2.y - Parent.position.y) / 2.0) + Parent.position.y;
        //IGNORE //Obj.position.x = ((MousePos2.x - Parent.position.x) / 2.0) + Parent.position.x;

        //BELOW LINE IS WHAT HAPPENS NORMALLY


        test = (MousePos2.x - Parent.position.x) / 2.0f;
        Obj.position = new Vector3((MousePos2.x - Parent.position.x) / 2.0f + Parent.position.x, (MousePos2.y - Parent.position.y) / 2.0f + Parent.position.y, Obj.position.z);


        Dist = Vector2.Distance(new Vector2(Obj.position.x, Obj.position.y), new Vector2(Parent.position.x, Parent.position.y));
        Vector3 d = new Vector2(transform.position.x, transform.position.y);
        //if(Dist <= Radius)
        //{
        // Obj.position = new Vector3((MousePos2.x - Parent.position.x) / 2.0f + Parent.position.x, (MousePos2.y - Parent.position.y) / 2.0f + Parent.position.y, Obj.position.z);
        // }
        var norm = MouseOffset.normalized;

        Vector3 limit = new Vector3(norm.x * Radius + Parent.position.x, norm.y * Radius + Parent.position.y, Obj.position.z);



        //BELOW LINE IS WHAT HAPPENS IF DISTANCE BETWEEN CAMERA AND PLAYER > RADIUS
        if (Vector3.Distance(d, limit) >= 0f)
        {
            //var norm = MouseOffset.normalized;
            //Obj.position.x = norm.x * Radius + Parent.position.x;
            //Obj.position.y = norm.y * Radius + Parent.position.y;
            //Obj.position = new Vector3(norm.x * Radius + Parent.position.x, norm.y * Radius + Parent.position.y, Obj.position.z);
            //Vector3 limit = new Vector3(norm.x * Radius + Parent.position.x, norm.y * Radius + Parent.position.y, Obj.position.z);

            //Obj.position = Vector3.SmoothDamp(transform.position,limit, ref velocity, lerper);
            Obj.position = limit;
        }
    }


}
