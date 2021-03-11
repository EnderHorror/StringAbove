using PitchDetector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayingBackToMenu : MonoBehaviour
{
    public GameObject PlaySatate;
    private Button _button;

    public static PlayingBackToMenu Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Back);
        gameObject.SetActive(false);
        PlaySatate.SetActive(false);
    }

    public void Back()
    {
        var lines = StaffLine.Instance.GetComponentsInChildren<IRestart>();
        foreach (var item in lines)
        {
            item.Restart();
        }
        StaffLine.Instance.Restart();

        for (int i = 0; i < MusicScoreManager.Instance.transform.childCount; i++)
        {
            MusicScoreManager.Instance.transform.GetChild(i).gameObject.SetActive(false);
        }
        CourseButtonGenerate.Instance.transform.parent.gameObject.SetActive(true);
        MicrophonePitchDetector.Instance.ToggleRecord();
        ToggleActive();
        PlaySatate.SetActive(false);
    }
    /// <summary>
    /// 改变启动状态
    /// </summary>
    public void ToggleActive()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).gameObject.SetActive(!transform.parent.GetChild(i).gameObject.activeSelf);
        }
    }
    
}
