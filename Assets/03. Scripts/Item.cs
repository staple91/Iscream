using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KimKyeongHun;


namespace No
{
    public abstract class Item : MonoBehaviour, IInteractable
    {
        Player owner;
        public Player Owner
        {
            get => owner;
            set => owner = value;
        }

        public abstract void Interact();
        public abstract void Use();

    }
}
