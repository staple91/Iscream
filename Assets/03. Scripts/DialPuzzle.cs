using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace No
{
    public class DialPuzzle : Puzzle
    {
        [SerializeField]
        Transform dialTr;

        [SerializeField]
        int[] answerNumArr;
        [SerializeField]
        float checkTime;


        public override void Interact()
        {
            StartCoroutine(DialCo());
        }

        IEnumerator DialCo()
        {
            float curCheckTime = 0;
            int curAnswerIndex = 0;
            int curAnswer = 0;
            Owner.IsMoveable = false;
            while (!Input.GetKey(KeyCode.E))
            {
                curAnswer = Mathf.RoundToInt(60 - dialTr.localEulerAngles.y / 6);
                if (curAnswer ==  answerNumArr[curAnswerIndex])
                {
                    curCheckTime += Time.deltaTime;
                    if(curCheckTime > checkTime)
                    {
                        curAnswerIndex++;
                        Debug.Log("µþ±ï");
                        if (curAnswerIndex >= answerNumArr.Length)
                            break;
                    }    
                }
                else
                    curCheckTime = 0;
                if (Input.GetKey(KeyCode.A))
                {
                    dialTr.Rotate(-Vector3.up);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    dialTr.Rotate(Vector3.up);
                }
                yield return new WaitForEndOfFrame();
            }
            Owner.IsMoveable = true;

        }
    }

}