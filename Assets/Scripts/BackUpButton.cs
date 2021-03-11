using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/// <summary>
/// 返回上一级菜单
/// </summary>
public class BackUpButton : MonoBehaviour
{
    /// <summary>
    /// 返回上一级执行的事件
    /// </summary>
    public UnityEvent onBackUp;
    public static BackUpButton Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        AudioSource source;
    }

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Back);
        gameObject.SetActive(false);
    }

    void Back()
    {
        onBackUp?.Invoke();
        onBackUp?.RemoveAllListeners();
        gameObject.SetActive(false);
    }
    
    void Update()
    {
        
    }
}
