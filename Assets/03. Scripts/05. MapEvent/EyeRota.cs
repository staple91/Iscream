using KimKyeongHun;
using No;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace PangGom
{
    public class EyeRota : MonoBehaviour
    {
        GameObject target;
        Player owner;
        public Player Owner
        {
            get => owner;
            set
            {
                owner = value;
            }
        }
        void Start () 
        {
            //target = Owner.GetComponent<GameObject>();
        }
        void Update()
        {
            //Vector3 vector = target.transform.position - transform.position;
            //transform.rotation = Quaternion.LookRotation(vector).normalized;
        }
    }
}