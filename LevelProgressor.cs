using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelProgressor : MonoBehaviour
{
    public int toLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {

            SaveSystem.SavePlayer(GameObject.Find("InventoryManager").GetComponent<InventoryManager>(), GameObject.Find("Player").GetComponent<PlayerBehaviour>(), GameObject.Find("Cam").GetComponent<CamManager>(), GameObject.Find("RootShoot").GetComponent<Shoot>(), GameObject.Find("InventoryManager").GetComponent<InventoryManager>().saveIndex);
            GameObject.Find("InventoryManager").GetComponent<InventoryManager>().loaded = true;
            GameObject.Find("InventoryManager").GetComponent<InventoryManager>().progressed = true;
            SceneManager.LoadScene(toLevel,LoadSceneMode.Additive);

            SceneManager.UnloadSceneAsync(GameObject.Find("InventoryManager").GetComponent<InventoryManager>().currentLevelSceneIndex);
            GameObject.Find("InventoryManager").GetComponent<InventoryManager>().currentLevelSceneIndex = toLevel;
            GameObject.Find("InventoryManager").GetComponent<InventoryManager>().progressed = true;
            


        }
    }
}
