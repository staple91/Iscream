using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using No;

namespace LeeJungChul
{
    public class DeadBodyEvent :Puzzle
{
        public GameObject BloodEffect;
        public GameObject BloodHand;

        public override void Interact()
        {
            BloodEffect.SetActive(true);
            BloodHand.SetActive(true);
        }
    }
}


