using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatSound : MonoBehaviour
{
    public static BeatSound Instance;
    
    public AudioSource _source;

    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
