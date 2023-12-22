using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextDropEvent : MonoBehaviour
{
    TextMeshProUGUI txt;
    string story;


    void Awake()
    {
        txt = GetComponent<TextMeshProUGUI>();
        story = txt.text;
        txt.text = "";
        
        StartCoroutine(PlayText());
    }
    IEnumerator PlayText()
    {
        foreach (char c in story)
        {
            txt.text += c;
            
            yield return new WaitForSeconds(0.125f);
        }
        
    }

}

