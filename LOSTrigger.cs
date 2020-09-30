using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOSTrigger : MonoBehaviour
{
    public GameObject camTrigger;
    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator Activated()
    {
        GetComponent<EdgeCollider2D>().enabled = false;
        camTrigger.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);

        GetComponent<BoxCollider2D>().enabled = false;
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<EnemyBehaviour>() != null)
            {
                if (enemy.GetComponent<EnemyBehaviour>().detected == false)
                {
                    enemy.GetComponent<EnemyBehaviour>().changeState = true;
                }
                enemy.GetComponent<EnemyBehaviour>().detected = true;
            }



            if (enemy.GetComponent<MeleeEnemyBehaviour>() != null)
            {
                if (enemy.GetComponent<MeleeEnemyBehaviour>().detected == false)
                {
                    enemy.GetComponent<MeleeEnemyBehaviour>().changeState = true;
                }
                enemy.GetComponent<MeleeEnemyBehaviour>().detected = true;
            }


        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            StartCoroutine(Activated());
        }
        
    }

    
}
