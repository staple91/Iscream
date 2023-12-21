using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PangGom;
using KimKyeongHun;

namespace PangGom
{
    public class ToiletsHint : MonoBehaviour, IListenable
    {

        float timer = 0;
        float eventTimer = 0;
        float loudness;

        public float Loudness { get => loudness; set => loudness = value; }
        public Vector3 Pos => transform.position;
        Player loudPlayer;
        public Player LoudPlayer { get => loudPlayer; set => loudPlayer = value; }

        void Start()
        {
            SoundManager.Instance.PlayAudio(SoundManager.Instance.toilelEventFoot, false, transform.position);
            Invoke("soundWaiting", 5f);
            StartCoroutine(GhostMove());//움직임 제어
            //StartCoroutine(Event());//소리를 내면 공격
        }
        void Update()
        {
            //애니메이션
        }
        IEnumerator GhostMove()
        {
            while (timer < 12)
            {
                transform.Translate(Vector3.left * Time.deltaTime/2, Space.World);
                timer += Time.deltaTime;
                Debug.Log("타이머" + timer);
                yield return new WaitForEndOfFrame();
            }
            timer = 0;
            transform.Rotate(0, 90, 0);
            while (timer < 12)
            {
                transform.Translate(Vector3.forward * Time.deltaTime/2, Space.World);
                timer += Time.deltaTime;
                Debug.Log("타이머" + timer);
                yield return new WaitForEndOfFrame();
            }
            timer = 0;
            //모든것이 끝나고 오브젝트 꺼주기
        }
        IEnumerator Event()
        {
            while (eventTimer < 8)//8초 동안 대기
            {
                eventTimer += Time.deltaTime;
                Debug.Log("타이머" + eventTimer);
                yield return new WaitForEndOfFrame();
            }
            eventTimer = 0;
            while (eventTimer < 10)//10초 동안 진행
            {
                eventTimer += Time.deltaTime;
                Debug.Log("타이머" + eventTimer);
                if (Loudness > 20)
                {
                    SoundManager.Instance.PlayAudio(SoundManager.Instance.ghostAttack, false, this.transform.position);
                    Debug.Log("공격!");
                }
                yield return new WaitForEndOfFrame();
            }
        }
        void soundWaiting()
        {
            Debug.Log("사운드 웨이팅");
            SoundManager.Instance.PlayAudio(SoundManager.Instance.toilelEventHumming, false, this.transform.position);
        }
    }
}
