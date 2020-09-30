using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InputFieldMainMenu : MonoBehaviour
{
    public int index;
    public TMP_InputField InputField;

    // Start is called before the first frame update
    void Start()
    {
        InputField = GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (InputField.isFocused && InputField.text != "" && Input.GetKey(KeyCode.Return))
        {
            print("uploaded!");
            UploadName(InputField.text);
            
        }
        
    }

    /*
    void OnSubmit()
    {
        if (InputField.isFocused && InputField.text != "")
        {
            UploadName(InputField.text);

        }
    }
    */

    void UploadName(string str)
    {
        GameObject.Find("InventoryManager").GetComponent<InventoryManager>().nameString = str;
        if(index == 1)
        {
            GameObject.Find("UICanvas").GetComponent<MainMenu>().Override1();
        }

        if (index == 2)
        {
            GameObject.Find("UICanvas").GetComponent<MainMenu>().Override2();
        }

        if (index == 3)
        {
            GameObject.Find("UICanvas").GetComponent<MainMenu>().Override3();
        }
    }



}
