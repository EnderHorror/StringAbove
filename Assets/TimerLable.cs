using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class TimerLable : MonoBehaviour
{

    public float CountTime { get; set; } = 3;
    
    private Text _text;

    private void Start()
    {
        _text = GetComponentInChildren<Text>();
        StartCoroutine(CountTimeCoroutine(CountTime));
    }


    private IEnumerator CountTimeCoroutine(float time)
    {
        var targetTime = Time.time + time;
        while (Time.time < targetTime)
        {
            yield return new WaitForSeconds(0.1f);
            _text.text = ((int)((targetTime - Time.time)*2)).ToString();
            if ((targetTime - Time.time) < 1)
            {
                _text.text = "开始";
            }
        }
        Destroy(gameObject);
    }
}
