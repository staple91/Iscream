using NavKeypad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonNumber : MonoBehaviour
{
    [SerializeField] private PassWord passWord;
    [SerializeField] private string value;
    // Start is called before the first frame update
    public void PressButton()
    {
        Debug.Log("1¹ø´­·¶´Ù.");
           passWord.AddInput(value);                  
    }
}
