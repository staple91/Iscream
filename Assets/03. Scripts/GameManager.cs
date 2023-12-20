using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using KimKyeongHun;

namespace LeeJungChul
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        private Transform[] spawnPoints;

        public List<Player> playerList = new List<Player>();
        public Player curPlayer;

        public GameObject dollyCart2;

        private void Start()
        {
            CreatePlayer();
            Cursor.visible = false;
        }

        /// <summary>
        /// 게임이 시작 될때 랜덤위치에 플레이어의 수만큼 플레이어를 생성하는 함수
        /// </summary>
        void CreatePlayer()
        {
            int idx = 0;
            foreach(Player player in playerList)
            {
                if(player.controller.photonView.IsMine)
                {

                    break;
                }
            }
            
            curPlayer =PhotonNetwork.Instantiate("Player", spawnPoints[idx].position, spawnPoints[idx].rotation, 0).GetComponent<Player>();
        }
    }
}


