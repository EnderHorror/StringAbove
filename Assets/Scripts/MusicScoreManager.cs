using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MusicXml.Domain;
using MusicXml;
/// <summary>
/// 乐谱管理
/// </summary>
public class MusicScoreManager : MonoBehaviour
{
    public static MusicScoreManager Instance { get; private set; }
    public GameObject[] NoteSigns;
    public List<int> PartDevideIndex = new List<int>();
    
    
    private GameObject _playingCanvas;
    private GameObject _stopSign;
    private int[] attachedPointDuration = new[] {1536,768,384,192,96 };
    private Score score;
    
    
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _playingCanvas = GameObject.Find("PlayingCanvas");
        _stopSign = Resources.Load<GameObject>(@"UIPrefab\Stop");
        _playingCanvas.SetActive(false);
    }

    /// <summary>
    /// 读取XML文件
    /// </summary>
    /// <param name="file"></param>
    private void LoadXMLFile(string file)
    {
        score = MusicXmlParser.GetScore(Resources.Load<TextAsset>(file));
        PartDevideIndex.Clear();
        PartDevideIndex.Add(0);
        foreach (var measure in score.Parts[0].Measures)
        {
            if (measure.Attributes?.BarLine == "light-light")
            {
                PartDevideIndex.Add(measure.Number);
            }
        }
    }

    /// <summary>
    /// 开始进入播放
    /// </summary>
    /// <param name="file"></param>
    public void StartPlay(string file)
    {
        LoadXMLFile(file);
        EnterPlayMode(0);
    }
    
    /// <summary>
    /// 进入演奏模式
    /// </summary>
    /// <param name="file">需要演奏的文件名</param>
    public void EnterPlayMode(int startPos)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        _playingCanvas.SetActive(true);
        
        for (int i = 0; i < score.Parts.Count; i++)//解析分部
        {
            foreach (var measure in score.Parts[i].Measures.Skip(startPos))//解析小节
            {
                if (measure.Attributes.Speed != 0)
                {
                    StaffLine.Instance.speed = (measure.Attributes.Speed / 120f) * 0.5f;
                }
                foreach (var element in measure.MeasureElements)//解析每个按键
                {
                    
                    if (element.Type == MeasureElementType.Note)
                    {
                        Note note = (Note) element.Element;
                        string lineName;
                        Transform parent;
                        if (note.Pitch == null&&note.Type=="whole")//全休止符
                        {

                            int partIndex = i;
                            if (score.Parts.Count != 1)//对单手进行单独处理
                            {
                                if (i == 0) lineName = "4:B";
                                else lineName = "3:D";
                            }
                            else
                            {
                                lineName = "3:D";
                                partIndex = 1;
                                //TODO:有bug,全部休止符生成到了开始位置,因为没有数据
                            }

                            parent = transform.GetChild(0).Find($"Part{partIndex}").Find(lineName);
                            var go = Instantiate(_stopSign, new Vector3(0, 0),Quaternion.identity, parent);
                            go.transform.localPosition = Vector3.zero;
                            parent.GetComponentInParent<PartStaffLine>().AddNote(go.transform,note);
                            
                        }
                        else if(note.Pitch != null)//普通音符
                        {
                            lineName = (note.Pitch.Octave).ToString() + ":" + note.Pitch.Step.ToString();
                            parent = transform.GetChild(0).Find($"Part{i}").Find(lineName);
                            GameObject targetObj = null;
                            switch (note.Duration)//根据时值确定生成的音符物体
                            {
                                case 1024:
                                    targetObj = NoteSigns[3];
                                    break;
                                case 512:
                                    targetObj = NoteSigns[1];
                                    break;
                                case 256:
                                    targetObj = NoteSigns[11];
                                    break;
                                case 128:
                                    targetObj = NoteSigns[5];
                                    break;
                                case 64:
                                    targetObj = NoteSigns[8];
                                    break;
                                case 1536:
                                    targetObj = NoteSigns[3];
                                    break;
                                case 768:
                                    targetObj = NoteSigns[1];
                                    break;
                                case 384:
                                    targetObj = NoteSigns[11];
                                    break;
                                case 192:
                                    targetObj = NoteSigns[5];
                                    break;
                                case 96:
                                    targetObj = NoteSigns[8];
                                    break;
                                default:
                                    targetObj = NoteSigns[8];
                                    break;
                            }

                            var go = Instantiate(targetObj, new Vector3(0, 0),Quaternion.identity, parent);

                            if (attachedPointDuration.Contains(note.Duration)) AddAttachedPoint(go);
                            parent.GetComponentInParent<PartStaffLine>().AddNote(go.transform,note);
                            var sign = go.GetComponent<NoteSign>();
                            sign.pitch = note.Pitch;
                        }
                        else//添加休止符
                        {
                            GameObject targetObj = null;

                            switch (note.Type)
                            {
                                case "quarter":
                                    targetObj = NoteSigns[10];
                                    break;
                                case "eighth":
                                    targetObj = NoteSigns[4];
                                    break;
                                case "16th":
                                    targetObj = NoteSigns[7];
                                    break;
                                case "half"://TODO:添加二分休止符
                                    targetObj = new GameObject();
                                    Destroy(targetObj,1);
                                    break;
                                default:
                                    break;
                            }
                            if (i == 0) lineName = "4:B";
                            else lineName = "2:D";
                            parent = transform.GetChild(0).Find($"Part{i}").Find(lineName);
                            var go = Instantiate(targetObj, new Vector3(0, 0), Quaternion.identity, parent);
                            parent.GetComponentInParent<PartStaffLine>().AddNote(go.transform, note);
                        }
                        
                    }

                }
                transform.GetChild(0).Find($"Part{i}").GetComponent<PartStaffLine>().AddMesureLine(measure.Number,measure.Attributes.BarLine);//添加小节线
                
                if (measure.Attributes != null)
                {
                    //TODO:添加拍号
                }
                
            }

        }
    }

    /// <summary>
    /// 重新定位播放位置
    /// </summary>
    /// <param name="startPos">从哪一个小节开始</param>
    public void RePosition(int startPos)
    {
        EnterPlayMode(startPos);
    }
    
    /// <summary>
    /// 添加附点
    /// </summary>
    /// <param name="obj">目标音符</param>
    void AddAttachedPoint(GameObject obj)
    {
        var go = Instantiate(NoteSigns[14],obj.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localPosition += new Vector3(10, 0);
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
