using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LeeJungChul
{
    public class UIManager : MonoBehaviour
    {
        [Header("옵션 화면")]
        [Tooltip("해상도와 사운드를 조절할 수 있는 옵션 화면")]
        [SerializeField] private GameObject option;

        [Header("로딩 화면")]
        [Tooltip("멀티게임에서 플레이어 매칭을 기다리는 로딩 화면")]
        [SerializeField] private GameObject panel;

        #region 로딩화면 활성화 및 비활성화 기능
        public void StartLoading()
        {
            panel.SetActive(true);
        }
        public void StartLoadingExit()
        {
            panel.SetActive(false);
        }
        #endregion

        #region 옵션화면 활성화 및 비활성화 기능
        public void OnClick()
        {
            option.SetActive(true);
        }

        public void OnClickExit()
        {
            option.SetActive(false);
        }
        #endregion

        public void SoloPlay()
        {
            SceneManager.LoadScene("PracticeScene");
        }

        #region 게임 종료
        public void GameExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        #endregion
    }


}
