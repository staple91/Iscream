using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using KimKyeongHun;
using LeeJungChul;

namespace No
{
    public enum InteractType
    {
        Light,
        Door,
        MorgueBox

    }
    public class InteractableObject : MonoBehaviourPunCallbacks, IInteractable
    {
        [SerializeField]
        InteractType type;

        PhotonView PV;
        
        Player owner;
        InteractStratagy stratagy;

        private void Awake()
        {
            switch (type)
            {
                case InteractType.Light:
                    stratagy = new LightStratagy(this);
                    break;
                case InteractType.Door:
                    stratagy = new DoorStratagy(this);
                    break;
                case InteractType.MorgueBox:
                    stratagy = new DoorStratagy(this);
                    break;
            }
        }

        public Player Owner
        {
            get => owner;
            set
            {
                owner = value;
            }
        }

        public void Interact()
        {
            stratagy.Act();
        }

    }

    public abstract class InteractStratagy
    {
        protected InteractableObject target;
        public InteractStratagy(InteractableObject target)
        {
            this.target = target;
        }
        public abstract void Act();
    }

    public class DoorStratagy : InteractStratagy
    {
        public bool isOpen = false;
        public bool isRunning = false;
        public DoorStratagy(InteractableObject target) : base(target)
        {
            this.target = target;
        }
        public override void Act()
        {
            Transform targetTr = target.GetComponent<Transform>();
            if (!isRunning)
            {
                isRunning = true;
                if (!isOpen)
                    target.StartCoroutine(OpenDoor(targetTr));
                else
                    target.StartCoroutine(CloseDoor(targetTr));
            }

        }
        IEnumerator OpenDoor(Transform tr)
        {
            Debug.Log("문열린당");
            float doorOpenAngle = 90f;
            float smoot = 2f;
            while (tr.localRotation.eulerAngles.y < 89f)
            {
                Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
                tr.localRotation = Quaternion.Slerp(tr.localRotation, targetRotation, smoot * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            isOpen = true;
            isRunning = false;


        }
        IEnumerator CloseDoor(Transform tr)
        {
            float doorCloseAngle = 0;
            float smoot = 2f;
            while (tr.localRotation.eulerAngles.y > 1f)
            {
                Quaternion targetRotation = Quaternion.Euler(0, doorCloseAngle, 0);
                tr.localRotation = Quaternion.Slerp(tr.localRotation, targetRotation, smoot * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            isOpen = false;
            isRunning = false;
        }
    }
    public class LightStratagy : InteractStratagy
    {
        public LightStratagy(InteractableObject target) : base(target)
        {
            this.target = target;
        }
        public override void Act()
        {
        }
    }

}