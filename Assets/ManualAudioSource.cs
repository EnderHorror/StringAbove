using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManualAudioSource : MonoBehaviour
{
    public static ManualAudioSource Instance { get; private set; }
    public AudioClip Clip;
    
    private AudioSource Source;
    private Button _button;

    private void Awake()
    {
        Instance = this;

    }

    void Start()
    {
        Source = GetComponent<AudioSource>();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() =>
        {
            Source.clip = Clip;
            Source.Play();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
