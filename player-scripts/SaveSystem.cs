using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    public static void SavePlayer(InventoryManager inventory, PlayerBehaviour player, CamManager cam, Shoot shoot, int saveIndex)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/saveGameName" + saveIndex + ".fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(inventory, player, cam, shoot, saveIndex);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer(int saveIndex)
    {
        string path = Application.persistentDataPath + "/saveGameName" + saveIndex + ".fun";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            
            return data;
            
        }
        else
        {
            Debug.Log("SAVE FILE NOT FOUND IN " + path);
            return null;
        }
    }


}
