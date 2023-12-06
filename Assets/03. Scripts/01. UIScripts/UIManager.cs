using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LeeJungChul
{
    public class UIManager : MonoBehaviour
    {
        public GameObject option;
        [Header("로딩 화면")]
        public GameObject panel;

        public void StartLoading()
        {
            panel.SetActive(true);
        }
        public void StartLoadingExit()
        {
            panel.SetActive(false);
        }

        public void OnClick()
        {
            option.SetActive(true);
        }

        public void OnClickExit()
        {
            option.SetActive(false);
        }
    }

}
