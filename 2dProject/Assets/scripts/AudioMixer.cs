using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMixer : MonoBehaviour {

    //singleton reference
    public static AudioMixer AudioMixerSingle;

    public AudioClip Slow;
    public AudioClip Medium;
    public AudioClip Fast;
    int song = 0;
    float songLength = 0;
    AudioSource audioSource;

    void Awake()
    {
        if (AudioMixerSingle == null)
        {
            AudioMixerSingle = this;
        }
        else if (AudioMixerSingle != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            //stop any music then pick song
            audioSource.Stop();
            song += 1;
            song = song % 3;

            if (song == 0)
            {
                audioSource.clip = Slow;
                songLength = 67f;
                //audioSource.Play();
            }
            else if (song == 1)
            {
                audioSource.clip = Medium;
                //audioSource.Play();
                songLength = 77f;
            }
            else
            {
                audioSource.clip = Fast;
                //audioSource.Play();
                songLength = 61f;
            }

            //start fade in and fade out for current song
            StartCoroutine(FadeIn(audioSource, 2f));
            //can't have fade longer than song
            StartCoroutine(FadeOut(audioSource, 2f, songLength - 3));
        }
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    StartCoroutine(FadeOut(audioSource, 1f, songLength));
        //}
    }

    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime, float songLength)
    {
        float startVolume = audioSource.volume;
        startVolume = 1f;

        //waits until song is almost over
        while (songLength >= 0)
        {
            songLength -= Time.deltaTime;
            yield return null;
        }

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public static IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = 1f;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 1f;
    }
}
