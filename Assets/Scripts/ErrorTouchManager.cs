using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorTouchManager : MonoBehaviour
{
    public float maxMoveDistacne = 1f;
    private float movePath = 0;
    
    public static bool HasMoved { get; private set; }
    
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    movePath = 0;
                    break;
                case TouchPhase.Moved:
                    movePath += touch.deltaPosition.magnitude;
                    break;
                case TouchPhase.Ended:
                    movePath = 0;
                    break;
            }
        }

        if (movePath > maxMoveDistacne)
        {
            HasMoved = true;
        }
        else
        {
            HasMoved = false;
        }

    }
}
