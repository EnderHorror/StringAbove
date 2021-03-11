using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 进度条
/// </summary>
public class ProgressRing : MonoBehaviour
{
    private Image _image;
    private Text _presentTex;
    [SerializeField]private int currentProgress;
    /// <summary>
    /// 当前进度环的进度
    /// </summary>
    public int CurrentProgress
    {
        get => currentProgress;
        private set => currentProgress = Mathf.Clamp(value, 0, 100);
        
    }
    
    

    void Start()
    {
        _image = GetComponent<Image>();
        _presentTex = GetComponentInChildren<Text>();
    }

    private void OnEnable()
    {
        //此操作为了解决重新启用之后层级错误问题
        GetComponent<Canvas>().sortingOrder += 1;
        GetComponent<Canvas>().sortingOrder -= 1;

    }

    /// <summary>
    /// 设置进度环的进度并且启动动画
    /// </summary>
    /// <param name="progress">要设置的值</param>
    public void SetProgress(int progress)
    {
        CurrentProgress = progress;
        //Amount的范围是0-1所以要除以100
        var twnner = _image.DOFillAmount((float)CurrentProgress/100, 1f);
        twnner.onUpdate += () => _presentTex.text = ((int)(_image.fillAmount * 100)).ToString() + '%';
    }
    /// <summary>
    /// 增加进度环的进度
    /// </summary>
    /// <param name="addtion">要增加的值</param>
    public void AddProgress(int addtion)
    {
        CurrentProgress += addtion;
        SetProgress(currentProgress);
    }
}
