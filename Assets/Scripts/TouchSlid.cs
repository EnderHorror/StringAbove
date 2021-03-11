using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用来控制滑动
/// </summary>
public class TouchSlid : MonoBehaviour
{
    public float sliderScal = 10;
    
    private float startX;
    private float rightBound;
    private GridLayoutGroup _group;
    void Start()
    {
        startX = transform.localPosition.x;
        _group = GetComponent<GridLayoutGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        rightBound = startX + (_group.spacing.x+_group.cellSize.x)*transform.childCount;



#if UNITY_EDITOR
        transform.position += new Vector3(Input.GetAxis("Mouse ScrollWheel") * sliderScal,0);
        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x,- rightBound,startX),0);
#else
        if(Input.touchCount > 0)
        {
            if (Input.GetTouch(0).deltaPosition.magnitude > 0.2f)
            {
                transform.position += new Vector3(Input.GetTouch(0).deltaPosition.x,0);        
                transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x,- rightBound,startX),0);
            }
        }

#endif

        
    }
}
