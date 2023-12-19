using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using No;
using PangGom;

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
            SoundManager.Instance.PlayAudio(SoundManager.Instance.deadBodyEvent, false, this.transform.position);
        }

        public void BloodHandEvent()
        {
            BloodHand.SetActive(false);
        }
    }
}


