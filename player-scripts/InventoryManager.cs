using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{

    public string nameString;

    public int saveIndex;
    public bool[] postcardInventory;

    public bool[] scanInventory;

    public int[] inventory = new int[15];
    public int[] slotInventory = new int[5];
    public int[] hotInventory = new int[1];

    

    public GameObject postcardGallery;
    public int scrap;

    public int currentLevelSceneIndex;

    public bool loaded;
    public bool progressed;
    public bool freshSave;

    public bool canDash;

    

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 120;
        
        


    }

    
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("MenuMusic") == null && GameObject.Find("LevelController") == null)
        {
            SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive);
        }
        scrap = 0;
        postcardInventory = new bool[12];
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown("p"))
        {
            Instantiate(postcardGallery, new Vector3(GameObject.Find("UICanvas").transform.position.x, GameObject.Find("UICanvas").transform.position.y, GameObject.Find("UICanvas").transform.position.z), Quaternion.identity, GameObject.Find("UICanvas").transform);
        }
        */
    }

    public void AcquireCard(int indexCard)
    {
        postcardInventory[indexCard] = true;
    }

    
    /*
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this, GameObject.Find("Player").GetComponent<PlayerBehaviour>());
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        UnloadAllScenesExcept("ManagerScene");
        SceneManager.LoadScene(currentLevelSceneIndex);
        GameObject.Find("Player").GetComponent<PlayerBehaviour>().health = data.health;
        Vector3 pos;
        pos.x = data.playerPosition[0];
        pos.y = data.playerPosition[1];
        pos.z = data.playerPosition[2];
        GameObject.Find("Player").transform.position = pos;

        postcardInventory = data.postcardInventory;
    }

    void UnloadAllScenesExcept(string sceneName)
    {
        int c = SceneManager.sceneCount;
        for (int i = 0; i < c; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            print(scene.name);
            if (scene.name != sceneName)
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
    }
    */
}
