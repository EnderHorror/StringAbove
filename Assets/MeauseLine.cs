using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaffLine;

public class MeauseLine : MonoBehaviour
{
    public int MeauseIndex;
    public string barline;
    private float enterX;
    
    void Start()
    {
        enterX = JudgeZone.Instance.GetComponent<BoxCollider2D>().bounds.max.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < enterX)
        {
            if (Instance.CurrentMeasure != this)
            {
                Instance.CurrentMeasure = this;
                enabled = false;
            }
        }
    }
}
