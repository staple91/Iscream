using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using No;

namespace LeeJungChul
{
    public class DeadBodyEvent :Puzzle
{
        public GameObject BloodEffect;

        public override void Interact()
        {
            BloodEffect.SetActive(true);
        }
    }
}


