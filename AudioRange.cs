using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRange : MonoBehaviour
{

    public GameObject player;
    public AudioSource aud;
    public float range;
    public float maxSoundPan;
    public float offset;

    public bool act;

    // Start is called before the first frame update
    void Start()
    {
        act = true;
        player = GameObject.Find("Player");
        aud = GetComponent<AudioSource>();
        StartCoroutine(ChangePan());
    }


    void OnDisable()
    {
        StopCoroutine(ChangePan());
    }

    void OnEnable()
    {
        StartCoroutine(ChangePan());
    }

    

    private IEnumerator ChangePan()
    {
        while (act){
            offset = (transform.position.x - player.transform.position.x);
            maxSoundPan = 0.8f;


            /*if(offset <= range && offset >= -range)
            {
                aud.panStereo = offset / range;
            }*/


            aud.panStereo = ((Mathf.Atan(range * offset)) / (Mathf.PI / 2f)) * maxSoundPan;

            yield return new WaitForSeconds(0.2f);
        }
        
    }
}
