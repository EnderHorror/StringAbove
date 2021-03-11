using System;
using System.Collections;
using System.Collections.Generic;
using MusicXml.Domain;
using UnityEngine;
using UnityEngine.UI;

public class KeyBorads : MonoBehaviour
{
    
    public static KeyBorads Instance { get; private set; }

    private void Awake()
    {
        if(gameObject.name =="KeyBoardWhite")
            Instance = this;
    }

    void Start()
    {
        var buttons = GetComponentsInChildren<Button>();
        foreach (var button in buttons)
        {
            button.onClick.AddListener(()=>JudgeZone.Instance.Judge(new Pitch(int.Parse(button.name[0].ToString()),button.name[2].ToString())));
        }

        gameObject.SetActive(false);
    }

    public void HightLight(Pitch note)
    {
        var target = transform.Find($"{note.Octave}:{note.Step}");
        if(target == null) return;
        var sprite    = target.GetComponent<Image>();
        sprite.color = Color.yellow;
        StartCoroutine(RestoreColor(sprite));

        
    }

    IEnumerator RestoreColor(Image image)
    {
        yield return new WaitForSeconds(2);
        image.color = Color.white;
    }
    
}
