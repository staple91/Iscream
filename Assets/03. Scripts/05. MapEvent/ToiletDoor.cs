using No;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


namespace PangGom
{
    public class ToiletDoor : DoorStratagy
    {
        public int tDCCount = 0;//해당 카운트가 0이어야 모든 문이 닫혀있는 것
        public int TDCCount
        {
            get { return tDCCount; }
            set
            {
                tDCCount = value;
            }
        }
        public ToiletDoor(InteractableObject target, bool value) : base(target, value)
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
                    target.StartCoroutine(ToiletOpenDoor(targetTr));
                    SoundManager.Instance.PlayAudio(SoundManager.Instance.toilelDoorOpen, false, target.transform.position);
                }
                else
                {
                    target.StartCoroutine(ToiletCloseDoor(targetTr));
                    SoundManager.Instance.PlayAudio(SoundManager.Instance.toilelDoorClose, false, target.transform.position);
                }
            }

        }
        IEnumerator ToiletOpenDoor(Transform tr)
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
            tDCCount += 1;
            //ToiletDoorClose = false;


        }
        IEnumerator ToiletCloseDoor(Transform tr)
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
            tDCCount -= 1;
        }
    }
}
