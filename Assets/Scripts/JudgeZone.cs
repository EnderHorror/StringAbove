using PitchDetector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MusicXml.Domain;
using UnityEngine;

/// <summary>
/// 判定区域
/// </summary>
public class JudgeZone : MonoBehaviour
{
    public static JudgeZone Instance { get; private set; }
    public HashSet<NoteSign> EnterSet;

    private GameObject falseFX;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        falseFX = Resources.Load<GameObject>("FasleTone");
        MicrophonePitchDetector.Instance.judgeZone = this;
    }

    private void OnEnable()
    {
        EnterSet = new HashSet<NoteSign>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 判断键对不对
    /// </summary>
    /// <param name="tone">键名</param>
    public void Judge(Pitch note)
    {
        List<NoteSign> removeSinge = new List<NoteSign>();

        foreach (var item in EnterSet)
        {
            if (note.Step == item.pitch.Step)
            {
                removeSinge.Add(item);
                item.GetComponent<SpriteRenderer>().color = Color.green;
                item.Correct();
            }
        }

        foreach (var item in removeSinge)
        {
            EnterSet.Remove(item);
        }
        removeSinge.Clear();
        
        /*try
        {
            var enmurator = EnterSet.GetEnumerator();
            do
            {
                if (note.Step == enmurator.Current?.pitch.Step)
                {
                    EnterSet.Remove(enmurator.Current);
                    enmurator.Current.GetComponent<SpriteRenderer>().color = Color.green;
                    enmurator.Current.Correct();
                }
            } while (enmurator.MoveNext());
        }
        catch (InvalidOperationException e)
        {
            print(e.StackTrace);
        }*/

        

    }


}
