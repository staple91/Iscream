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
        float loudness;

        public float Loudness { get => loudness; set => loudness = value; }
        public Vector3 Pos => transform.position;
        Player loudPlayer;
        public Player LoudPlayer { get => loudPlayer; set => loudPlayer = value; }

        void Start()
        {
            SoundManager.Instance.PlayAudio(SoundManager.Instance.toilelEventFoot, false, transform.position);
            Invoke("soundWaiting", 5f);
        }

        void Update()
        {
            //발소리
            //노래소리
            //애니메이션
            //소리를 내면 공격
        }
        IEnumerator Event()
        {
            while (timer < 10)
            {
                timer += Time.deltaTime;
                Debug.Log("타이머" + timer);
                if (Loudness > 20)
                    //공격
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
