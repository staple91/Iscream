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

        private void Start()
        {
            parentsObj = transform.parent.parent.gameObject;
            toiletEvent = parentsObj.GetComponent<ToiletEvent>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Player(Clone)")
            {
                toiletEvent.ToiletPlayerCount += 1;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.name == "Player")
            {
                toiletEvent.ToiletPlayerCount -= 1;
            }
        }
    }
}
