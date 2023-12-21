using PangGom;
using System.Collections;
using Photon.Pun;
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

        bool isReverese = false;
        bool isSolved = false;

        [SerializeField]
        GameObject onSolvedClipboard;

        PhotonView ph;
        private void Start()
        {
            OnSolved.AddListener(() => SoundManager.Instance.PlayAudio(SoundManager.Instance.dialSolved, false, transform.position));
            OnSolved.AddListener(() => ph.RPC("OnDialSolved", RpcTarget.All));
        }
        void OnDialSolved()
        {
            isSolved = true;
            onSolvedClipboard.SetActive(true);
        }

        public override void Interact()
        {
            if(!isSolved)
                StartCoroutine(DialCo());
        }

        IEnumerator DialCo()
        {
            float curCheckTime = 0;
            int curAnswerIndex = 0;
            int curAnswer = 0;
            Owner.IsMoveable = false;
            while (!Input.GetKey(KeyCode.E) || !isSolved)
            {
                curAnswer = Mathf.RoundToInt(60 - dialTr.localEulerAngles.y / 6);
                if (curAnswer ==  answerNumArr[curAnswerIndex])
                {
                    curCheckTime += Time.deltaTime;
                    if(curCheckTime > checkTime)
                    {
                        curAnswerIndex++;
                        isReverese = !isReverese;
                        Debug.Log("µþ±ï");
                        if (curAnswerIndex >= answerNumArr.Length)
                        {
                            OnSolved.Invoke();
                            break;
                        }    
                    }    
                }
                else
                    curCheckTime = 0;
                if (Input.GetKey(KeyCode.D) && !isReverese)
                {
                    dialTr.Rotate(Vector3.up);
                }
                if (Input.GetKey(KeyCode.A) && isReverese)
                {
                    dialTr.Rotate(-Vector3.up);
                }
                yield return new WaitForEndOfFrame();
            }
            Owner.IsMoveable = true;

        }
    }

}