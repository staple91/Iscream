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
            Invoke("soundWaiting", 4f);
            StartCoroutine(GhostMove());//움직임 제어
            StartCoroutine(Event());//소리를 내면 공격
        }
        void Update()
        {

            //애니메이션
        }
        IEnumerator GhostMove()
        {
            while (timer < 6)//8초 동안 대기
            {
                transform.Translate(Vector3.left * Time.deltaTime, Space.World);
                timer += Time.deltaTime;
                Debug.Log("타이머" + timer);
                yield return new WaitForEndOfFrame();
            }
            timer = 0;
            transform.Rotate(0, 90, 0);
            while (timer < 6)//8초 동안 대기
            {
                transform.Translate(Vector3.forward * Time.deltaTime, Space.World);
                timer += Time.deltaTime;
                Debug.Log("타이머" + timer);
                yield return new WaitForEndOfFrame();
            }
            timer = 0;
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
                    SoundManager.Instance.PlayAudio(SoundManager.Instance.ghostAttack, false, transform.position);
                    Debug.Log("공격!");
                }
                yield return new WaitForEndOfFrame();
            }
        }
        void soundWaiting()
        {
            Debug.Log("사운드 웨이팅");
            SoundManager.Instance.PlayAudio(SoundManager.Instance.toilelEventHumming, false, transform.position);
        }
    }
}
