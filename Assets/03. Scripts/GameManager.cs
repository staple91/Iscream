using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace LeeJungChul
{
    public class GameManager : Singleton<GameManager>
    {

        private void Start()
        {
           // CreatePlayer();
        }

        /// <summary>
        /// 게임이 시작 될때 랜덤위치에 플레이어의 수만큼 플레이어를 생성하는 함수
        /// </summary>
        void CreatePlayer()
        {
            Transform[] spawnPoints = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
            int idx = Random.Range(1, spawnPoints.Length);
            PhotonNetwork.Instantiate("Player", spawnPoints[idx].position, spawnPoints[idx].rotation, 0);
        }
    }
}


