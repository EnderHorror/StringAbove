using System;
using System.Collections;
using System.Collections.Generic;
using MusicXml.Domain;
using UnityEngine;

/// <summary>
/// 音符实体
/// </summary>
public class NoteSign : MonoBehaviour
{
    public Pitch pitch;
    
    private float enterX;
    private float exitX;

    public static bool HelpModel;
    public static bool CheatMode;

    private bool added = false;
    void Start()
    {
        enterX = JudgeZone.Instance.GetComponent<BoxCollider2D>().bounds.max.x;
        exitX = JudgeZone.Instance.GetComponent<BoxCollider2D>().bounds.min.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void FixedUpdate()
    {
        if(!added)
            if (transform.position.x < enterX&&transform.position.x>exitX)
            {
                if (JudgeZone.Instance.EnterSet.Add(this))
                {
                    KeyBorads.Instance.HightLight(pitch);
                }
                added = true;
            }

        if (CheatMode)
        {
            if (transform.position.x < exitX + 20)
            {
                Correct();
            }
        }
        
        if (HelpModel)
        {
            if (transform.position.x < exitX + 20)
            {
                StaffLine.Instance.Pause();
            }
        }


        if(transform.position.x < exitX)
        {
            Missing();
        }
    }

    /// <summary>
    /// 音符Miss
    /// </summary>
    private void Missing()
    {
        
        JudgeZone.Instance.EnterSet.Remove(this);
        StaffLine.Instance.missCount++;
        GetComponent<SpriteRenderer>().color = Color.red;

        Destroy(gameObject,1);
        this.enabled = false;

    }
    /// <summary>
    /// 音符判断正确
    /// </summary>
    public void Correct()
    {
        GetComponent<SpriteRenderer>().color = Color.green;
        //JudgeZone.Instance.EnterSet.Remove(this);
        Destroy(gameObject,0.5f);
        this.enabled = false;
        StaffLine.Instance.Resume();
    }
    
}
