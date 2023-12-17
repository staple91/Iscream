using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PangGom
{
    public class ToiletArea : MonoBehaviour
    {
        ToiletEvent toiletEvent;
        GameObject parentsObj;

        int toiletPlayerCount = 0;
        public int ToiletPlayerCount
        {
            get { return toiletPlayerCount; }
            set
            {
                toiletPlayerCount = value;
                if (toiletPlayerCount == PhotonNetwork.CountOfPlayers)//최대인원수에 도달하면 ToiletFull 트루
                    toiletEvent.ToiletFull = true;
            }
        }



        private void Start()
        {
            parentsObj = transform.parent.parent.gameObject;
            toiletEvent = parentsObj.GetComponent<ToiletEvent>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Player")
            {
                ToiletPlayerCount += 1;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "Player")
            {
                ToiletPlayerCount -= 1;
            }
        }
    }
}
