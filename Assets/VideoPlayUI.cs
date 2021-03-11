using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoPlayUI : MonoBehaviour
{
    public UnityEvent OnFnish;
    
    public static VideoPlayUI Instance { get; private set; } 
    
    private VideoPlayer _player;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _player = GetComponent<VideoPlayer>();
        gameObject.SetActive(false);
    }

    public void Playe(VideoClip clip)
    {
        gameObject.SetActive(true);
        _player.clip = clip;
        _player.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.frame >= (long)_player.frameCount-1)
        {
            OnFnish.Invoke();
        }
    }
}
