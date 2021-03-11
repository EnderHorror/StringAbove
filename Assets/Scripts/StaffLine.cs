using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class StaffLine : MonoBehaviour,IRestart
{
    public Button continueButton;
    public Button backUpButton;
    public static StaffLine Instance { get; private set; }
    public static Stack<Transform> Step = new Stack<Transform>();

    public MeauseLine CurrentMeasure
    {
        get => _meauseLine;
        set
        {
            _meauseLine = value;
            CheackFinsh();
        }
    }
    
    public float speed = 10;
    public  float showTime = 1f;
    public  float deltaShowTime = 0.1f;
    public AnimationCurve Curve;
    public bool moveAble = false;
    public float waitTime = 3;
    public int missCount = 0;


    private Vector3 _startPos;
    private MeauseLine _meauseLine;
    private float startSpeed;
    private int nextPart;
    private int lastPart;
    private bool finished = false;
    private void Awake()
    {
        Instance = this;
        
    }

    private void OnEnable()
    {
        StartCoroutine(Play());
        _startPos = transform.position;

        moveAble = false;
        missCount = 0;
        finished = false;
    }

    private void Start()
    {
        continueButton.onClick.AddListener(Continue);
        backUpButton.onClick.AddListener(BackUp);

        startSpeed = speed;

    }

    private IEnumerator Play()
    {
        Instantiate(Resources.Load<GameObject>("Timer"), GameObject.Find("PlayingAditionCanvas").transform).GetComponent<TimerLable>().CountTime = waitTime;
        yield return new WaitForSeconds(waitTime);
        moveAble = true;
    }



    public void Pause()
    {
        speed = 0;
        BeatSound.Instance._source.Pause();
    }

    public void Resume()
    {
        speed = startSpeed;
        //BeatSound.Instance._source.Play();
        BeatSound.Instance._source.UnPause();
    }
    
    private void FixedUpdate()
    {

    }
    
    private void Update()
    {
        if (moveAble)
        {
            transform.Translate(Vector3.left*(PartStaffLine.MesureLineWidth+PartStaffLine.MesureLineWidth / 16) * speed * Time.deltaTime);
        }
    }

    /// <summary>
    /// 检查是否完成当前的Part
    /// </summary>
    private void CheackFinsh()
    {
        if (CurrentMeasure.barline == "light-light")
        {
            if (missCount > 6)
            {
                moveAble = false;
                continueButton.transform.parent.gameObject.SetActive(true);
            }
        }
        
        if (CurrentMeasure.barline == "light-heavy"&&!finished)
        {
            BeatSound.Instance._source.Pause();
            finished = true;
        }
        
    }

    IEnumerator FinishPlay()
    {
        var go = Instantiate(Resources.Load<GameObject>("UIPrefab\\CircleButton"),new Vector3(Screen.width/2,Screen.height/2),Quaternion.identity,
            GameObject.Find("PlayingAditionCanvas").transform);
            go.GetComponentInChildren<Text>().text = "完成!";
        yield return new WaitForSeconds(2);
        Destroy(go);
        PlayingBackToMenu.Instance.Back();

    }

    private void Continue()
    {
        moveAble = true;
        continueButton.transform.parent.gameObject.SetActive(false);
        //BeatSound.Instance._source.Play();
        BeatSound.Instance._source.UnPause();
        missCount = 0;
    }

    private void BackUp()
    {
        var index = MusicScoreManager.Instance.PartDevideIndex.IndexOf(_meauseLine.MeauseIndex);
        RollBack(MusicScoreManager.Instance.PartDevideIndex[index-1]);
        continueButton.transform.parent.gameObject.SetActive(false);
    }

    public void Restart()
    {
        transform.position = _startPos;
        moveAble = false;
    }

    public void RollBack(int mearueIndex)
    {
        foreach (var line in GetComponentsInChildren<PartStaffLine>())
        {
            line.Restart();
        }
        MusicScoreManager.Instance.RePosition(mearueIndex);
        transform.position  = _startPos;
        OnEnable();

    }
    
    public  void SwitchHelpMode()
    {
        NoteSign.HelpModel = !NoteSign.HelpModel;
    }

    public void SwitchPasuState()
    {
        if (speed <0.1f) speed = startSpeed;
        else
        {
            speed = 0;
        }
    }

    public void SwitchCheatMode()
    {
        NoteSign.CheatMode = !NoteSign.CheatMode;
    }
}
