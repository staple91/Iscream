using KimKyeongHun;
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
        GameObject tr;
        Player curPlayer;

        private void Start()
        {
            parentsObj = transform.parent.parent.parent.gameObject;
            toiletEvent = parentsObj.GetComponent<ToiletEvent>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Player(Clone)")
            {
                curPlayer = other.GetComponent<Player>();
                toiletEvent.ToiletPlayerCount += 1;
                tr = transform.parent.gameObject;
                toiletEvent.myDoorVec = tr.transform.position + new Vector3(0.76f, 2, 0);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "Player(Clone)")
            {
                curPlayer = null;
                toiletEvent.ToiletPlayerCount -= 1;
            }
        }
    }
}
