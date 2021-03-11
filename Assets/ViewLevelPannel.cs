using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewLevelPannel : MonoBehaviour
{
    public CourseObject CouseData;

    public List<Button> CouseButtons = new List<Button>();
    void Start()
    {
        foreach (var course in CouseData.Course)
        {
            var go = Instantiate(Resources.Load<GameObject>(@"UIPrefab\CoursePannel"), transform);
            go.GetComponent<CurosePannel>().Construct(course.name,course.subtitle,0);
            
        }
    }

    
    void Update()
    {
        
    }
}
