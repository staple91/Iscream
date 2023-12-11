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
        /// ������ ���� �ɶ� ������ġ�� �÷��̾��� ����ŭ �÷��̾ �����ϴ� �Լ�
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


