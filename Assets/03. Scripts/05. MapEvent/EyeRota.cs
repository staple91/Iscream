using KimKyeongHun;
using LeeJungChul;
using No;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KimKyeongHun;
using static UnityEngine.UI.GridLayoutGroup;

namespace PangGom
{
    public class EyeRota : MonoBehaviour
    {
        Player owner;
        public Player Owner
        {
            get => owner;
            set
            {
                owner = value;
            }
        }
        PhotonView pv;
        void Start ()
        {
            pv = GetComponent<PhotonView>();
            owner = GameManager.Instance.curPlayer;
        }
        void Update()
        {
            Vector3 vector = owner.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(vector).normalized;
        }
    }
}