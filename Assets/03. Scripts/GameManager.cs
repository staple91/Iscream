using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using KimKyeongHun;

namespace LeeJungChul
{
    public class GameManager : Singleton<GameManager>
    {
        public PhotonView objectPhotonView;
        [SerializeField]
        private Transform[] spawnPoints;

        public List<Player> playerList = new List<Player>();

        private void Start()
        {
            CreatePlayer();
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
                    idx = player.controller.photonView.ViewID / 1000; // player
                    break;
                }
            }
            
            //PhotonNetwork.Instantiate("Player", spawnPoints[idx].position, spawnPoints[idx].rotation, 0);
        }
    }
}


