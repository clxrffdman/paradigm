using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextBoxChoice : MonoBehaviour
{
    public GameObject textBox;

    public TextMeshProUGUI theText;

    public string[] textLines;
    public float[] typeSpeedArray;

    public int currentLine;
    public int endAtLine;

    
    public bool on;

    public bool isTyping = false;
    public bool cancelTyping = false;

    public float typeSpeed;

    public int optionCount;
    public int[] optionLines;
    public string[] options;
    public int[] choiceIndex;

    public bool choosing;

    public GameObject op1;
    public GameObject op2;

    public Sprite textimg;

    public GameObject clickPrompt;


    public GameObject tiedInteractable;

    

    void Awake()
    {
        op1 = transform.GetChild(transform.childCount-2).gameObject;
        op2 = transform.GetChild(transform.childCount-1).gameObject;

    }

    // Start is called before the first frame update
    void Start()
    {
        clickPrompt = transform.Find("ClickPromptTextBox").gameObject;
        clickPrompt.SetActive(false);


        choosing = false;
        theText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        /*
        typeSpeed = tiedInteractable.GetComponent<Interactable>().typeSpeed;
        //textLines = tiedInteractable.GetComponent<Interactable>().textLines;
        endAtLine = tiedInteractable.GetComponent<Interactable>().endLine;
        optionLines = tiedInteractable.GetComponent<Interactable>().optionLines;
        options = tiedInteractable.GetComponent<Interactable>().options;

        if (tiedInteractable.GetComponent<Interactable>().textImg != null)
        {
            textimg = tiedInteractable.GetComponent<Interactable>().textImg;
            transform.GetChild(1).GetComponent<Image>().sprite = textimg;
        }


        if (tiedInteractable.GetComponent<Interactable>().typeSpeedArray.Length != 0)
        {
            typeSpeedArray = tiedInteractable.GetComponent<Interactable>().typeSpeedArray;
        }
        else
        {
            typeSpeedArray = new float[endAtLine + 1];
            for (int i = 0; i < typeSpeedArray.Length; i++)
            {
                typeSpeedArray[i] = typeSpeed;
            }
        }
        */
        GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -350, 0);

        on = false;
        OnOpen();

        textBox = gameObject;
        currentLine = 0;

        //UpdateLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && on && !choosing)
        {

            if (!isTyping)
            {
                currentLine++;
                //UpdateLine();

                if (currentLine > endAtLine)
                {
                    on = false;
                    OnClose();
                }
                else
                {
                    StartCoroutine(TextScoll(textLines[currentLine]));
                }
            }
            else if (isTyping && !cancelTyping)
            {

                cancelTyping = true;
                clickPrompt.SetActive(true);
            }


        }





    }

    public void ChoiceA()
    {
        print("A");
        choosing = false;
        currentLine++;
        //textLines[currentLine] = "";
        StartCoroutine(TextScoll(textLines[currentLine]));
    }

    public void ChoiceB()
    {

        print("B");
        choosing = false;
        currentLine++;
        //textLines[currentLine] = "";
        StartCoroutine(TextScoll(textLines[currentLine]));
    }

    private IEnumerator TextScoll(string lineOfText)
    {
        clickPrompt.SetActive(false);
        choosing = false;
        op1.SetActive(false);
        op2.SetActive(false);
        int letter = 0;
        theText.text = "";
        isTyping = true;
        cancelTyping = false;
        while (isTyping && !cancelTyping && (letter < textLines[currentLine].Length))
        {
            typeSpeed = typeSpeedArray[currentLine];
            theText.text = textLines[currentLine].Substring(0, letter);
            letter++;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }
        theText.text = textLines[currentLine];
        clickPrompt.SetActive(true);
        for (int i = 0; i < optionLines.Length; i++)
        {
            if(currentLine == optionLines[i])
            {
                
                choosing = true;
                clickPrompt.SetActive(false);
                op1.SetActive(true);
                op2.SetActive(true);
                op1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = options[optionCount];
                op2.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = options[optionCount+1];
                optionCount = optionCount + 2;
            }
        }

        isTyping = false;
        cancelTyping = false;

    }

    void UpdateLine()
    {
        theText.text = textLines[currentLine];
    }

    void OnOpen()
    {
        LeanTween.scale(gameObject, new Vector3(6.675f, 2.175f, 6f), 0.25f).setOnComplete(SetActive).setIgnoreTimeScale(true);
        LeanTween.value(gameObject, -350, -150, 0.25f).setIgnoreTimeScale(true).setOnUpdate((float val) => {
            GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, val, 0);
        });
        GameObject.Find("LevelController").GetComponent<PauseMenu>().canMenu = false;

        GameObject.Find("Player").GetComponent<PlayerController>().midTele = true;


        GetComponent<SoundSampleAdd>().PlaySound(0, 1);


        GameObject.Find("LevelController").GetComponent<PauseMenu>().HideUI(true);

        GameObject.Find("Crosshair").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("RootShoot").GetComponent<Shoot>().shootEnable = false;
        GameObject.Find("Player").GetComponent<PlayerController>().interactEnable = false;
        GameObject.Find("LevelController").GetComponent<PauseMenu>().inInventory = true;
        Time.timeScale = 0;



        //tiedInteractable.GetComponent<Interactable>().canInteract = false;
    }


    public void OnClose()
    {


        GameObject.Find("LevelController").GetComponent<PauseMenu>().canMenu = true;

        GameObject.Find("LevelController").GetComponent<PauseMenu>().HideUI(false);


        LeanTween.scale(gameObject, new Vector3(6.675f, 0f, 0f), 0.25f).setOnComplete(DestroyMe).setIgnoreTimeScale(true);
        LeanTween.value(gameObject, -150, -350, 0.25f).setIgnoreTimeScale(true).setOnUpdate((float val) => {
            GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, val, 0);
        });
        GameObject.Find("Crosshair").GetComponent<SpriteRenderer>().enabled = true;
        GameObject.Find("RootShoot").GetComponent<Shoot>().shootEnable = true;
        GameObject.Find("Player").GetComponent<PlayerController>().interactEnable = true;
        GameObject.Find("LevelController").GetComponent<PauseMenu>().inInventory = false;
        Time.timeScale = 1;





        GameObject.Find("Player").GetComponent<PlayerController>().midTele = false;
        GameObject.Find("RootShoot").GetComponent<Shoot>().shootEnable = true;
        // tiedInteractable.GetComponent<Interactable>().clicked = true;
        //tiedInteractable.GetComponent<Interactable>().canInteract = true;
    }

    void DestroyMe()
    {

        //tiedInteractable.GetComponent<Interactable>().canInteract = true;

        Destroy(gameObject);
    }



    void SetActive()
    {
        StartCoroutine(TextScoll(textLines[currentLine]));
        on = true;
    }
}
