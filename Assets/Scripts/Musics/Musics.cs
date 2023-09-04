using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musics : MonoBehaviour
{
    public static Musics instance;

    public AudioSource scene1;

    void Awake()
    {
        instance = this;
    }

    public void StopMusic()
    {
        if (scene1 != null && scene1.isPlaying)
        {
            scene1.Stop();
        }
    }    

    // Start is called before the first frame update
    void Start()
    {
        scene1.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}