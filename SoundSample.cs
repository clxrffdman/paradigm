using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSample : MonoBehaviour
{
    public AudioSource aud;
    public float lifetime;
    public float blend;
    public AudioClip clip;

    public bool playing;

    // Start is called before the first frame update
    void Awake()
    {
        aud = GetComponent<AudioSource>();
        playing = false;
        lifetime = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            lifetime = lifetime - Time.deltaTime;
        }

        if(lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SpawnSound(AudioClip a, float b, float vol)
    {
        lifetime = a.length;
        aud.volume = vol;
        aud.clip = a;
        aud.panStereo = b;
        playing = true;
        PlaySound();
    }

    void PlaySound()
    {
        aud.Play();
    }
}
