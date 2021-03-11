using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PitchDetector;
using UnityEngine.Video;

/// <summary>
/// 每个课程的小节生成管理
/// </summary>
public class PartButtonGenerate : MonoBehaviour
{
    private GameObject partPrefab;
    
    public static PartButtonGenerate Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        partPrefab = Resources.Load<GameObject>(@"UIPrefab\CircleButton");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 清楚所有课程
    /// </summary>
    public void Clear()
    {
        for (int i = 0; i < transform.childCount; i++)
        {

            Destroy(transform.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// 读取表格内容生成所有小节课程
    /// </summary>
    /// <param name="worksheet"></param>
    public void GeneratePart(List<SubPart> worksheet)
    {
        BackUpButton.Instance.onBackUp.AddListener(Clear);
        foreach (var item in worksheet)
        {
            var go = Instantiate(partPrefab, transform);
            go.GetComponentInChildren<Text>().text = item.name;
            //给按钮绑定事件
            //开始进入弹奏
            if (item.Mode == PlayMode.TimeLine)
            {
                go.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (!ErrorTouchManager.HasMoved)
                    {
                        string file = item.fileName;
                        MusicScoreManager.Instance.StartPlay(file);
                        CourseButtonGenerate.Instance.transform.parent.gameObject.SetActive(false);
                        MicrophonePitchDetector.Instance.ToggleRecord();
                        PlayingBackToMenu.Instance.ToggleActive();
                    }
                });
            }
            else
            {
                go.GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (!ErrorTouchManager.HasMoved)
                    {
                        string file = item.fileName;
                        VideoPlayUI.Instance.Playe(Resources.Load<VideoClip>(file));
                    }
                }); 
            }

        }
    }
    
}
