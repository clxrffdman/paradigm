using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEnviro : MonoBehaviour
{
    public Transform Parent; // Whatever you want the camera locked to
    public Transform Obj; // The object to place the camera on
    public Transform Enviro; // Environmental camera avg point

    public Transform Filler;

    public float fillerX;
    public float fillerY;
    public float fillerZ;

    GameObject levelControl;

    public bool enviroInfluence;

    public float Radius = 5f;
    public float Dist;
    public float lerper;
    public float enviroVari;
    public Vector3 MousePos1, MousePos2, ScreenMouse, MouseOffset;
    private Vector3 velocity = Vector3.zero;

    public float directionX;
    public float directionY;

    public Vector3 limit;
    public Vector3 d;

    public float startobjZ;

    public void Start()
    {
        levelControl = GameObject.Find("LevelController");
        startobjZ = Obj.position.z;
    }

    public void Update()
    {
        if (levelControl.GetComponent<PauseMenu>().paused == false && levelControl.GetComponent<PauseMenu>().inInventory == false)
        {
            
            MousePos1 = Input.mousePosition;

            ScreenMouse = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(MousePos1.x, MousePos1.y, Obj.position.z - GetComponent<Camera>().transform.position.z));
            MouseOffset = ScreenMouse - Parent.position;
            MousePos2 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -transform.position.z));
            //IGNORE //Obj.position.y = ((MousePos2.y - Parent.position.y) / 2.0) + Parent.position.y;
            //IGNORE //Obj.position.x = ((MousePos2.x - Parent.position.x) / 2.0) + Parent.position.x;

            //BELOW LINE IS WHAT HAPPENS NORMALLY
            if (Parent.position.x < Enviro.position.x)
            {
                directionX = +1f;
            }
            if (Parent.position.x > Enviro.position.x)
            {
                directionX = -1f;
            }

            if (Parent.position.x == Enviro.position.x)
            {
                directionX = 0f;
            }

            ///


            if (Parent.position.y < Enviro.position.y)
            {
                directionY = +1f;
            }
            if (Parent.position.y > Enviro.position.y)
            {
                directionY = -1f;
            }

            if (Parent.position.y == Enviro.position.y)
            {
                directionY = 0f;
            }



            //Obj.position = new Vector3((MousePos2.x - Parent.position.x) / 2.0f + Parent.position.x, (MousePos2.y - Parent.position.y) / 2.0f + Parent.position.y, Obj.position.z);

            //Obj.position = new Vector3(Mathf.Abs((MousePos2.x - Parent.position.x)/2.0f)*directionX + Parent.position.x, Mathf.Abs((MousePos2.y - Parent.position.y) / 2.0f) * directionY + Parent.position.y, Obj.position.z);


            //Obj.position = new Vector3((MousePos2.x - Parent.position.x) / 2.0f + Enviro.position.x, (MousePos2.y - Parent.position.y) / 2.0f + Enviro.position.y, Obj.position.z);
            //Obj.position = new Vector3(Enviro.position.x - Filler.position.x / 2.0f + Parent.position.x, Enviro.position.y - Filler.position.y / 2.0f + Parent.position.y, Filler.position.z);

            Filler.position = new Vector3((MousePos2.x - Parent.position.x) / 2.0f + Parent.position.x, (MousePos2.y - Parent.position.y) / 2.0f + Parent.position.y, Filler.position.z);

            fillerX = Filler.position.x;
            fillerY = Filler.position.y;
            fillerZ = Filler.position.z;

            Obj.position = Vector3.Lerp(transform.position, new Vector3((Enviro.position.x - Filler.position.x) / 2.0f + Filler.position.x, (Enviro.position.y - Filler.position.y) / 2.0f + Filler.position.y, Obj.position.z), 0.05f);


            d = new Vector2(Parent.position.x, Parent.position.y);
            Dist = Vector2.Distance(d, MousePos2);

            //if(Dist <= Radius)
            //{
            // Obj.position = new Vector3((MousePos2.x - Parent.position.x) / 2.0f + Parent.position.x, (MousePos2.y - Parent.position.y) / 2.0f + Parent.position.y, Obj.position.z);
            // }

            var norm = MouseOffset.normalized;

            limit = new Vector3(norm.x * Radius + Parent.position.x, norm.y * Radius + Parent.position.y, Obj.position.z);



            //BELOW LINE IS WHAT HAPPENS IF DISTANCE BETWEEN CAMERA AND PLAYER > RADIUS


            if (Vector3.Distance(Parent.position, limit) >= 0f)
            {
                //var norm = MouseOffset.normalized;
                //Obj.position.x = norm.x * Radius + Parent.position.x;
                //Obj.position.y = norm.y * Radius + Parent.position.y;
                //Obj.position = new Vector3(norm.x * Radius + Parent.position.x, norm.y * Radius + Parent.position.y, Obj.position.z);
                //Vector3 limit = new Vector3(norm.x * Radius + Parent.position.x, norm.y * Radius + Parent.position.y, Obj.position.z);

                //Obj.position = Vector3.SmoothDamp(transform.position,limit, ref velocity, lerper);
                Obj.position = Vector3.Lerp(transform.position, limit, 0.05f);
            }

            Obj.position = new Vector3(Obj.position.x, Obj.position.y, startobjZ);
        }

            
        
        


    }


}
