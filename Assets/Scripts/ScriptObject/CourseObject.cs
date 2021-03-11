using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 用来储存课程
/// </summary>
[CreateAssetMenu]
public class CourseObject : ScriptableObject
{



    public List<CoursePart> Course;


}
/// <summary>
/// 大章节
/// </summary>
[Serializable]
public struct CoursePart
{
    public string name;
    [Multiline]
    public string subtitle;
    public AudioClip manualAudio;
    public List<SubPart> parts;
}
/// <summary>
/// 子章节
/// </summary>
[Serializable]
public struct SubPart
{
    [Multiline]
    public string name;
    public string fileName;
    public PlayMode Mode;

}
/// <summary>
/// 练习模式
/// </summary>
[Serializable]
public enum PlayMode
{
    TimeLine,
    Video
}
