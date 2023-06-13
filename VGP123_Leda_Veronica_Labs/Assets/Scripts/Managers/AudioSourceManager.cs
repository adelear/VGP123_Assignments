using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    List<AudioSource> currentAudioSources = new List<AudioSource>();
    // Start is called before the first frame update
    void Start()
    {
        currentAudioSources.Add(GetComponent<AudioSource>());    
    }

    public void PlayOneShot(AudioClip clip, bool isMusic)
    {
        foreach (AudioSource source in currentAudioSources)
        {
            if (source.isPlaying)
                continue;

            source.PlayOneShot(clip);
            return; 
        }
        AudioSource temp = gameObject.AddComponent<AudioSource>();  
        currentAudioSources.Add(temp);
        temp.PlayOneShot(clip); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
