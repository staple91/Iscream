using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using KimKyeongHun;

namespace No
{
    public enum InteractType
    {
        Light,
        Door,

    }
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField]
        InteractType type;

        Player owner;
        PhotonView PV;
        InteractStratagy stratagy;

        private void Awake()
        {
            PV = GetComponent<PhotonView>();
            switch (type)
            {
                case InteractType.Light:
                    stratagy = new LightStratagy(this);
                    break;
                case InteractType.Door:
                    stratagy = new DoorStratagy(this);
                    break;
            }
        }

        public Player Owner
        {
            get => owner;
            set => owner = value;
        }

        public void Interact()
        {
            if (owner.isLocal)
            {
                RpcInteract();
                return;
            }
            PV.RPC("RpcInteract", RpcTarget.AllBuffered);
        }
        [PunRPC]
        void RpcInteract()
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
        bool isOpen = false;
        bool isRunning = false;
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