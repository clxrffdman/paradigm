using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{

    public GameObject teleProxy;
    public GameObject teleLocation;
    public bool checkingAttackAngle;

    public BoxCollider2D boxCollider;

    public float attackLength;
    public float attackWidth;

    public GameObject bulletNumber;


    public List<Collider2D> colliders = new List<Collider2D>();
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!colliders.Contains(other))
        { colliders.Add(other); }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        colliders.Remove(other);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        boxCollider.size = new Vector2(boxCollider.size.x, 0.8f);

        checkingAttackAngle = true;
        //StartCoroutine(UpdateTeleAttackDirection());
    }

    // Update is called once per frame
    void OnEnable()
    {
        StopAllCoroutines();

        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        checkingAttackAngle = true;
        //StartCoroutine(UpdateTeleAttackDirection());
    }

    void FixedUpdate()
    {
        foreach(Collider2D col in colliders)
        {
            if(col == null)
            {
                colliders.Remove(col);
            }
        }

        if(checkingAttackAngle == true)
        {

            boxCollider.size = new Vector2(Vector2.Distance(teleProxy.transform.position, teleLocation.transform.position) + 0.5f, boxCollider.size.y);
            boxCollider.offset = new Vector2(boxCollider.size.x / 2, 0);


            transform.right = teleLocation.transform.position - transform.position;

        }

    }


    /*
    public IEnumerator UpdateTeleAttackDirection()
    {


        while (checkingAttackAngle == true)
        {

            boxCollider.size = new Vector2(Vector2.Distance(teleProxy.transform.position, teleLocation.transform.position) + 0.4f, boxCollider.size.y);
            boxCollider.offset = new Vector2(boxCollider.size.x / 2, 0);


            transform.right = teleLocation.transform.position - transform.position;


            yield return new WaitForSeconds(0.01f);
        }

    }
    */

    
}
