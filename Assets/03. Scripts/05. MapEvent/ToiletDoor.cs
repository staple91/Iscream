using No;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


namespace PangGom
{
    public class ToiletDoor : DoorStratagy
    {
        public ToiletDoor(InteractableObject target) : base(target)
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
                    target.StartCoroutine(ToiletOpenDoor(targetTr));
                else
                    target.StartCoroutine(ToiletCloseDoor(targetTr));
            }
        }
        IEnumerator ToiletOpenDoor(Transform tr)
        {
            float doorOpenAngle = 0f;
            float smoot = 2f;
            while (tr.localRotation.eulerAngles.y < 1f)
            {
                Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
                tr.localRotation = Quaternion.Slerp(tr.localRotation, targetRotation, smoot * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            isOpen = true;
            isRunning = false;

        }
        IEnumerator ToiletCloseDoor(Transform tr)
        {
            float doorCloseAngle = -90;
            float smoot = 2f;
            while (tr.localRotation.eulerAngles.y > -89f)
            {
                Quaternion targetRotation = Quaternion.Euler(0, doorCloseAngle, 0);
                tr.localRotation = Quaternion.Slerp(tr.localRotation, targetRotation, smoot * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            isOpen = false;
            isRunning = false;
        }
    }
}
