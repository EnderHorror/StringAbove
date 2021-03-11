using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 用来生成和管理课程
/// </summary>
public class CourseButtonGenerate : MonoBehaviour
{
    public CourseObject dataBase;
    
    public static CourseButtonGenerate Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
        foreach (var mian in dataBase.Course)
        {
            var go = Instantiate(Resources.Load<GameObject>(@"UIPrefab\CoursePannel"), transform);
            go.GetComponent<CurosePannel>().Construct(mian.name,mian.subtitle,0);
            go.GetComponent<Button>().onClick.AddListener(()=>
            {
                SelectCourse(mian.parts);
                ManualAudioSource.Instance.Clip = mian.manualAudio;
            });
        }
        
    }
    

    /// <summary>
    /// 按下按钮选择课程
    /// </summary>
    /// <param name="sheet">课程的表格页</param>
    void SelectCourse(List<SubPart> parts)
    {
        if (!ErrorTouchManager.HasMoved)
        {
            PartButtonGenerate.Instance.GeneratePart(parts);
            BackUpButton.Instance.gameObject.SetActive(true);
            HideAll();
            BackUpButton.Instance.onBackUp.AddListener(ShowAll);
        }

        
    }
    /// <summary>
    /// 隐藏所有课程
    /// </summary>
    public void HideAll()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        GetComponent<TouchSlid>().enabled = false;//先警用滑动,否则会出现返回之后位置不在原来的地方
    }
    /// <summary>
    /// 显示所有课程
    /// </summary>
    public void ShowAll()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        GetComponent<TouchSlid>().enabled = true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
