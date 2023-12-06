using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject option;
    public void OnClick()
    {
        option.SetActive(true);
    }

    public void OnClickExit()
    {
        option.SetActive(false);
    }
}
