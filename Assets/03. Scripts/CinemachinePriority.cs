using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


namespace KimKyeongHun
{

    public class CinemachinePriority : MonoBehaviour
    {

        public CinemachineVirtualCamera cim; // 시네머신 우선 순위를 변경하기 위한 것


        // Start is called before the first frame update
        void Start()
        {
            cim = GetComponent<CinemachineVirtualCamera>();
            StartCoroutine(StartCutScene());

        }


        IEnumerator StartCutScene()
        {
            Debug.Log("코루틴 진입 ");
            float startTime = 0f;
            float termTime = 1.5f;

            Debug.Log(startTime);
            while (startTime < termTime)
            {
                startTime += Time.deltaTime;
                if (startTime > termTime)
                {
                    cim.Priority += 1;
                    
                    yield return null;
                }
                
                yield return null;
                
            }
            
        }

    }

   
}
