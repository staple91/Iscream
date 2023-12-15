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
                    target.StartCoroutine(MorgueOpenDoor(targetTr));
                else
                    target.StartCoroutine(MorgueCloseDoor(targetTr));
            }

        }
        IEnumerator MorgueOpenDoor(Transform tr)
        {
            float doorOpenAngle = -120f;
            float smoot = 2f;

            while (tr.localRotation.eulerAngles.y == 0 || tr.localRotation.eulerAngles.y > 245f)// ¹®¿­¸®°í ´ÙÀ½
            {
                Debug.Log(tr.localRotation.eulerAngles.y);
                Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
                tr.localRotation = Quaternion.Slerp(tr.localRotation, targetRotation, smoot * Time.deltaTime);
                yield return new WaitForEndOfFrame();

            }
            isOpen = true;
            isRunning = false;

        }
        IEnumerator MorgueCloseDoor(Transform tr)
        {
            float doorCloseAngle = 0;
            float smoot = 2f;

            while (tr.localRotation.eulerAngles.y != 0 && tr.localRotation.eulerAngles.y < 356f)//¹®´ÝÈû
            {
                Debug.Log(tr.localRotation.eulerAngles.y);
                Quaternion targetRotation = Quaternion.Euler(0, doorCloseAngle, 0);
                tr.localRotation = Quaternion.Slerp(tr.localRotation, targetRotation, smoot * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            isOpen = false;
            isRunning = false;
        }
    }
}
