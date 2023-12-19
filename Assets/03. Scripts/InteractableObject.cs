using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using KimKyeongHun;
using LeeJungChul;
using PangGom;

namespace No
{
    public enum InteractType
    {
        Light,
        Door,
        MorgueBox,
        ToiletDoor
    }

    
    public class InteractableObject : MonoBehaviourPunCallbacks, IInteractable
    {
        [SerializeField]
        InteractType type;
        [SerializeField]
        bool toggleValue;
        
        Player owner;
        public InteractStratagy stratagy;

        private void Awake()
        {
            switch (type)
            {
                case InteractType.Light:
                    stratagy = new LightStratagy(this,toggleValue);
                    break;
                case InteractType.Door:
                    stratagy = new DoorStratagy(this, toggleValue);
                    break;
                case InteractType.MorgueBox:
                    stratagy = new MorgueBox(this, toggleValue);
                    break;
                case InteractType.ToiletDoor:
                    stratagy = new ToiletDoor(this, toggleValue);
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
        public InteractStratagy(InteractableObject target, bool value)
        {
            this.target = target;
        }
        public abstract void Act();
    }
    
    public class DoorStratagy : InteractStratagy
    {
        public bool isOpen;
        public bool isRunning = false;
        public DoorStratagy(InteractableObject target, bool value) : base(target, value)
        {
            this.target = target;
            isOpen = value;
        }
        public override void Act()
        {
            Transform targetTr = target.GetComponent<Transform>();
            if (!isRunning)
            {
                isRunning = true;
                if (!isOpen)
                {
                    target.StartCoroutine(OpenDoor(targetTr));
                    SoundManager.Instance.PlayAudio(SoundManager.Instance.doorOpen, false, target.transform.position);
                }
                else
                {
                    target.StartCoroutine(CloseDoor(targetTr));
                    SoundManager.Instance.PlayAudio(SoundManager.Instance.doorClose, false, target.transform.position);
                }
            }

        }
        IEnumerator OpenDoor(Transform tr)
        {
            Debug.Log("��������");
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
        Light light;
        float lightIntensity;
        bool isOn;
        public LightStratagy(InteractableObject target, bool value) : base(target,value)
        {
            this.target = target;
            light = target.GetComponent<Light>();
            lightIntensity = light.intensity;
            isOn = value;
        }
        public override void Act()
        {
            isOn = !isOn;
            if (isOn)
                light.intensity = lightIntensity;
            else
                light.intensity = 0;

        }
    }

}