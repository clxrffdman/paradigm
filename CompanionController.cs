using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionController : MonoBehaviour
{
    //Variables to be preset within Unity.
    public Vector3 desiredLocation;
    public GameObject player;
    public PlayerController playerController;
    public PlayerBehaviour playerBehaviour;
    public DashAbility dashAbility;

    public LineRenderer line;

    public float moveSpeed;

    public List<Vector3> positionBuffer;

    public bool updatingLocation;

    public bool moveToggle;


    // Start is called before the first frame update
    void Start()
    {


        if(player == null)
        {
            player = GameObject.Find("Player");
        }
        if(playerController == null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        if(playerBehaviour == null)
        {
            playerBehaviour = player.GetComponent<PlayerBehaviour>();
        }
        if(dashAbility == null)
        {
            dashAbility = player.GetComponent<DashAbility>();
        }

        if(line == null)
        {
            line = GetComponent<LineRenderer>();
        }

        StartCoroutine(UpdateDesiredLocation());
        RegenDroneToggle(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 relativePos = desiredLocation - transform.position;
        //GetComponent<Rigidbody2D>().AddForce(relativePos * moveSpeed);
        
        //Sets the ends of the line to the gameObject's position and player position.
        line.SetPosition(0, transform.position);
        line.SetPosition(1, player.transform.position);

    }

    //Updates the position buffer with the player's position
    public void TeleLocate()
    {
        
        for(int i = 0; i < positionBuffer.Count; i++)
        {
            positionBuffer[i] = player.transform.position;
        }

        desiredLocation = player.transform.position;

        transform.position = desiredLocation;
    }

    //Updates the desired location of the companion.
    public IEnumerator UpdateDesiredLocation()
    {
        while (updatingLocation)
        {
            if (!playerBehaviour.isMaxHealth)
            {
                line.enabled = true;


            }
            else
            {
                line.enabled = false;
            }


            if (playerController.isMoving)
            {
                positionBuffer.Add(player.transform.position);

                if (positionBuffer.Count > 15)
                {
                    positionBuffer.RemoveAt(0);
                }
            }
            else
            {
                if (positionBuffer.Count < 1)
                {
                    positionBuffer.Add(player.transform.position);
                }
            }
            

            if (playerController.isMoving)
            {


                desiredLocation = positionBuffer[0];
            }
            else
            {
                /*
                if(positionBuffer.Count > 5)
                {
                    positionBuffer.RemoveRange(5, positionBuffer.Count-5);
                }

                if (positionBuffer.Count > 0)
                {

                    desiredLocation = positionBuffer[0];
                    if(positionBuffer.Count > 1)
                    {
                        positionBuffer.RemoveAt(0);
                    }
                    

                }

                */




            }

            

            transform.LeanMove(desiredLocation, 0.01f);

            yield return new WaitForSeconds(0.01f);
        }
        
    }

    public void RegenDroneToggle(bool state)
    {

        playerBehaviour.canRegen = state;
        dashAbility.companion = gameObject;
        
        
    }





}
