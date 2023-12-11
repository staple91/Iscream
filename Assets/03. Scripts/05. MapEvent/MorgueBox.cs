
using No;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;


namespace PangGom
{
    public class MorgueBox : DoorStratagy
    {
        public MorgueBox(InteractableObject target) : base(target)
        {
            this.target = target;
        }

        public override void Act()
        {

            Transform targetTr = target.GetComponent<Transform>();
            Transform morgueBed = targetTr.parent.GetComponent<Transform>();
            morgueBed = morgueBed.transform.GetChild(0); // 자식을 번호로 찾음. 0번째가 첫 번째 자식
            if (!isRunning)
            {
                isRunning = true;
                if (!isOpen)
                    target.StartCoroutine(MorgueOpenDoor(targetTr, morgueBed));
                else
                    target.StartCoroutine(MorgueCloseDoor(targetTr, morgueBed));
            }

        }
        IEnumerator MorgueOpenDoor(Transform tr, Transform bedTr)
        {
            float doorOpenAngle = -120f;
            float smoot = 2f;

            float bedOpen = 0.3f;
            Vector3 bedOpenTarget = new Vector3(bedTr.localPosition.x, bedTr.localPosition.y, 0f);
            float bedsmoot = 1f;
            while (tr.localRotation.eulerAngles.y == 0 || tr.localRotation.eulerAngles.y > 245f)// 문열리고 다음
            {
                Debug.Log(tr.localRotation.eulerAngles.y);
                Quaternion targetRotation = Quaternion.Euler(0, doorOpenAngle, 0);
                tr.localRotation = Quaternion.Slerp(tr.localRotation, targetRotation, smoot * Time.deltaTime);
                yield return new WaitForEndOfFrame();

            }
            while (bedTr.localPosition.z > bedOpen)//선형보간, 배드 빠짐
            {
                bedTr.localPosition = Vector3.Lerp(bedTr.localPosition, bedOpenTarget, bedsmoot * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            isOpen = true;
            isRunning = false;

        }
        IEnumerator MorgueCloseDoor(Transform tr, Transform bedTr)
        {
            float doorCloseAngle = 0;
            float smoot = 2f;

            float bedClose = 1.35f;
            Vector3 bedCloseTarget = new Vector3(bedTr.localPosition.x, bedTr.localPosition.y, 1.4f);
            float bedsmoot = 1f;
            while (bedTr.localPosition.z < bedClose)//베드 닫힌 다음
            {
                bedTr.localPosition = Vector3.Lerp(bedTr.localPosition, bedCloseTarget, bedsmoot * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
            while (tr.localRotation.eulerAngles.y != 0 && tr.localRotation.eulerAngles.y < 356f)//문닫힘
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