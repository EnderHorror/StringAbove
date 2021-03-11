using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 每个课程面板管理
/// </summary>
public class CurosePannel : MonoBehaviour
{
    private ProgressRing _progressRing;
    
    
    
    void Start()
    {
        
    }
    /// <summary>
    /// 生成时初始化课程
    /// </summary>
    /// <param name="titile">标题</param>
    /// <param name="subtitle">子标题</param>
    /// <param name="progress">当前进度</param>
    public void Construct(string titile,string subtitle,int progress)
    {
        _progressRing = GetComponentInChildren<ProgressRing>();
        transform.GetChild(0).GetComponent<Text>().text = titile;
        transform.GetChild(1).GetComponent<Text>().text = subtitle;
        _progressRing.SetProgress(progress);
    }
    
    
}
