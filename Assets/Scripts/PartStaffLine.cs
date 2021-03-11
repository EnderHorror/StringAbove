using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MusicXml.Domain;
using UnityEngine;
/// <summary>
/// 每个分别的五线
/// </summary>
public class PartStaffLine : MonoBehaviour,IRestart
{

    /// <summary>
    /// 每小节线宽度
    /// </summary>
    public static float MesureLineWidth = 200;

    public float showNoteDelta = 0.1f;
    
    /// <summary>
    /// 上一根小节线的Transform
    /// </summary>
    private Transform _lastLine;
    /// <summary>
    /// 上一个音符的Transform
    /// </summary>
    private Transform _lastNote;
    private GameObject _mesureLine;
    private GameObject _addLine;
    private float _nextDistance;
    private float _waitTime;

    

    private void OnEnable()
    {
        _mesureLine = Resources.Load<GameObject>("MeauseLine");
        _addLine = Resources.Load<GameObject>("加线");
        Transform parent;
        if (transform.name == "Part0") parent = transform.Find("4:B");
        else parent = transform.Find("3:D");
        var go = Instantiate(_mesureLine,parent);
        _lastLine = go.transform;
        _waitTime = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 添加小节线
    /// </summary>
    public void AddMesureLine(int index,string barline)
    {
        if (_lastLine != null)
        {
            var go = Instantiate(_mesureLine, _lastLine.transform.parent);
            go.transform.localPosition = new Vector3(_lastLine.localPosition.x,0) + new Vector3(MesureLineWidth+ (MesureLineWidth / 16), 0);
            if (_lastNote != null)
            {
                go.transform.localPosition = new Vector3(_lastNote.localPosition.x,0) + new Vector3(MesureLineWidth * _nextDistance, 0);

            }
            _lastLine = go.transform;
            _lastNote = null;
            var mearuser = go.GetComponent<MeauseLine>();
                mearuser.MeauseIndex = index;
                mearuser.barline = barline;
        }
    }
    
    /// <summary>
    /// 定位音符位置,并且添加"加线"
    /// </summary>
    /// <param name="trans">音符的Transform组件</param>
    /// <param name="note">音符</param>
    public void AddNote(Transform trans, Note note)
    {
        if (note.Type == NoteType.whole.ToString() && note.Pitch == null)//如果是全休止符
        {
            
            trans.localPosition += new Vector3(_lastLine.localPosition.x + (MesureLineWidth+(MesureLineWidth / 16)) / 2, 0);
        }
        else
        {
            if (_lastNote != null)//如果是非休止符
            {
                trans.localPosition = new Vector3(_lastNote.localPosition.x + MesureLineWidth * _nextDistance, 0);
            }
            else//如果是休止符
            {
                trans.localPosition = new Vector3(_lastLine.localPosition.x + (MesureLineWidth / 16), 0);
            }

            _nextDistance = note.Duration / 1024f;//计算时值,定位此音符到下一个音符的间距
            _lastNote = trans;
            

            
            

            if (trans.parent.parent.name == "Part0")//上面五线谱
            {
               int offset = GetOutOffStaffWidth(9,17,trans);
               int lineCount = offset / 2;//除以二,因为每隔2行才有一根线
               if (lineCount > 0)//线在上面的时候
               {
                   List<Transform> transList = new List<Transform>();
                   switch (lineCount)
                   {
                           
                       case 1:
                           transList.Add(Instantiate(_addLine, trans.parent.parent.GetChild(7)).transform);
                           break;
                       case 2:
                           var parent1 = trans.parent.parent;
                           transList.Add(Instantiate(_addLine, parent1.GetChild(7)).transform);
                           transList.Add(Instantiate(_addLine, parent1.GetChild(5)).transform);
                           break;
                       case 3:
                           var parent = trans.parent.parent;
                           transList.Add(Instantiate(_addLine, parent.GetChild(7)).transform);
                           transList.Add(Instantiate(_addLine, parent.GetChild(5)).transform);
                           transList.Add(Instantiate(_addLine, parent.GetChild(3)).transform);
                           break;
                           
                   }

                   foreach (var item in transList)//调整线的位置以及父级
                   {
                       item.localPosition = new Vector3(trans.localPosition.x,0);
                       item.parent = trans;
                   }

               }
               else if(lineCount<0)//线在下面的时候,同上
               {
                   List<Transform> transList = new List<Transform>();
                   switch (lineCount)
                   {
                           
                       case -1:
                           transList.Add(Instantiate(_addLine, trans.parent.parent.GetChild(19)).transform);
                           break;
                       case -2:
                           var parent2 = trans.parent;
                           var parent3 = parent2.parent;
                           transList.Add(Instantiate(_addLine, parent3.GetChild(19)).transform);
                           transList.Add(Instantiate(_addLine, parent3.GetChild(21)).transform);
                           break;
                       case -3:
                           var parent = trans.parent;
                           var parent1 = parent.parent;
                           transList.Add(Instantiate(_addLine, parent1.GetChild(19)).transform);
                           transList.Add(Instantiate(_addLine, parent1.GetChild(21)).transform);
                           transList.Add(Instantiate(_addLine, parent1.GetChild(23)).transform);
                           break;
                           
                   }

                   foreach (var item in transList)
                   {
                       item.localPosition = new Vector3(trans.localPosition.x,0);
                       item.parent = trans;
                   }
               }

            }
            if (trans.parent.parent.name == "Part1")//上面五线谱,同上
            {
                int offset = GetOutOffStaffWidth(10,18,trans);
                               int lineCount = offset / 2;
               if (lineCount > 0)
               {
                   List<Transform> transList = new List<Transform>();
                   switch (lineCount)
                   {
                           
                       case 1:
                           transList.Add(Instantiate(_addLine, trans.parent.parent.GetChild(8)).transform);
                           break;
                       case 2:
                           var parent = trans.parent;
                           var parent1 = parent.parent;
                           transList.Add(Instantiate(_addLine, parent1.GetChild(8)).transform);
                           transList.Add(Instantiate(_addLine, parent1.GetChild(6)).transform);
                           break;
                       case 3:
                           var parent2 = trans.parent;
                           var parent3 = parent2.parent;
                           transList.Add(Instantiate(_addLine, parent3.GetChild(8)).transform);
                           transList.Add(Instantiate(_addLine, parent3.GetChild(6)).transform);
                           transList.Add(Instantiate(_addLine, parent3.GetChild(4)).transform);
                           break;
                           
                   }

                   foreach (var item in transList)
                   {
                       item.localPosition = new Vector3(trans.localPosition.x,0);
                       item.parent = trans;
                   }

               }
               else if(lineCount<0)
               {
                   List<Transform> transList = new List<Transform>();
                   switch (lineCount)
                   {
                       case -1:
                           transList.Add(Instantiate(_addLine, trans.parent.parent.GetChild(20)).transform);
                           break;
                       case -2:
                           var parent = trans.parent;
                           var parent1 = parent.parent;
                           transList.Add(Instantiate(_addLine, parent1.GetChild(20)).transform);
                           transList.Add(Instantiate(_addLine, parent1.GetChild(22)).transform);
                           break;
                       case -3:
                           var parent2 = trans.parent;
                           var parent3 = parent2.parent;
                           transList.Add(Instantiate(_addLine, parent3.GetChild(20)).transform);
                           transList.Add(Instantiate(_addLine, parent3.GetChild(22)).transform);
                           transList.Add(Instantiate(_addLine, parent3.GetChild(24)).transform);
                           break;
                   }

                   foreach (var item in transList)
                   {
                       item.localPosition = new Vector3(trans.localPosition.x,0);
                       item.parent = trans;
                   }
               }
            }
            AddFadeInAnimation(trans);
        }
    }

    /// <summary>
    /// 获取音符距离五线谱的距离(上面为正,下面为负数,中间为0)
    /// </summary>
    /// <param name="up">最上面一根线的索引</param>
    /// <param name="down">最下面一根线的索引</param>
    /// <param name="tran">目标</param>
    /// <returns>距离</returns>
    int GetOutOffStaffWidth(int up,int down,Transform tran)
    {
        int pos = FindIndexPos(tran.parent);
        if (up - pos > 0) return up - pos;
        else if (pos - down > 0) return down - pos;
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// 查找当前物体在父级中的索引
    /// </summary>
    /// <param name="trans">要查找的目标</param>
    /// <returns>对应位置的索引</returns>
    /// <exception cref="NullReferenceException"></exception>
    int FindIndexPos(Transform trans)
    {
        for (int i = 0; i < trans.parent.childCount; i++)
        {
            if (trans == trans.parent.GetChild(i)) return i;
        }
        throw new NullReferenceException("所查找的索引不在父级物体内");
    }

    /// <summary>
    /// 添加入场动效
    /// </summary>
    /// <param name="trans">动效对象</param>
    void AddFadeInAnimation(Transform trans)
    {
        var localPosition = trans.localPosition;
        Vector3 pos = localPosition;
        localPosition += new Vector3(0,-1000);
        trans.localPosition = localPosition;
        trans.DOLocalMove(pos,_waitTime).SetEase(StaffLine.Instance.Curve);
        _waitTime += showNoteDelta;
    }

    public void Restart()
    {
        OnEnable();
        for (int i = 0; i < transform.childCount; i++)
        {
            var line = transform.GetChild(i);
            for (int j = 0; j < line.childCount; j++)
            {
                if(line.GetChild(j).name !="StartLine")
                    Destroy(line.GetChild(j).gameObject);
            }
        }
    }
}
