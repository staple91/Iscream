using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PangGom
{
    public class ToiletEvent : MonoBehaviour
    {
        [SerializeField]
        int toiletPlayerCount = 0;
        public int ToiletPlayerCount
        {
            get { return toiletPlayerCount; }
            set
            {
                toiletPlayerCount = value;
                if (toiletPlayerCount == PhotonNetwork.CurrentRoom.PlayerCount)//최대인원수에 도달하면 ToiletFull 트루
                    ToiletFull = true;
            }
        }
        [SerializeField]
        bool toiletFull = false;
        public bool ToiletFull
        {
            get { return toiletFull; }
            set { toiletFull = value; }
        }
        bool allClose = true;
        public bool AllClose
        {
            get { return allClose; }
            set
            { allClose = value;
                //if(화장실 문열림 0보다 크면)
                //allClose = false
            }
        }

        [SerializeField]
        bool toiletEventOn = false;
        public bool ToiletEventOn
        {
            get { return toiletEventOn; }
            set
            {
                toiletEventOn = value;
                if (toiletEventOn)
                    EventPlay();
            }
        }
        //Area의 ToiletFull이 True고 tDCCount = 0일때 해당 이벤트 발생
        void Update()
        {
            if (ToiletEventOn)
                return;
            else
            {
                if (ToiletFull && AllClose)
                {
                    ToiletEventOn = true;
                }
            }
        }
        void EventPlay()
        {
            Debug.Log("화장실 이벤트 시작");
        }
    }
}
