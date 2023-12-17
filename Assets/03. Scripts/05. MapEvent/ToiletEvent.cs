using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PangGom
{
    public class ToiletEvent : MonoBehaviour
    {

        bool toiletFull = false;
        public bool ToiletFull
        {
            get { return toiletFull; }
            set { toiletFull = value; }
        }
        bool allClose = false;
        public bool AllClose
        {
            get { return allClose; }
            set
            { allClose = value; }
        }

        bool toiletEventOn = false;
        public bool ToiletEventOn
        { 
            get {  return toiletEventOn; } 
            set 
            {  
                toiletEventOn = value;
                EventPlay();
            }
        }
        //Area의 ToiletFull이 True고 tDCCount = 0일때 해당 이벤트 발생
        void Update()
        {
            if (ToiletEventOn)
                return;
            else
            {
                if (ToiletFull && AllClose)
                {
                    ToiletEventOn = true;
                }
            }
        }
        void EventPlay()
        {
            Debug.Log("화장실 이벤트 시작");
        }
    }
}
