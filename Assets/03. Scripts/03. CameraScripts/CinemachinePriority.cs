using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

namespace KimKyeongHun
{
    public class PlayerDeadCameraShake : MonoBehaviour
    {

        private CinemachineVirtualCamera playerCam;
        private Vector3 camOriginalPos;

        public PlayerDeadCameraShake(CinemachineVirtualCamera playerCam)
        {
            this.playerCam = playerCam;
        }


        private void Start()
        {
            camOriginalPos = playerCam.transform.position;
            
        }

        public IEnumerator CameraShake()
        {
            float curTime = 0f;

            Cinemachine.NoiseSettings cameraNoise = Resources.Load("6D Wobble") as Cinemachine.NoiseSettings;

            Cinemachine.NoiseSettings cameraNoiseNone = Resources.Load("none") as Cinemachine.NoiseSettings;
            playerCam.AddCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

            while (curTime < 2f)
            {

                curTime += Time.deltaTime;

                playerCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = cameraNoise;
                yield return null;

            }

            playerCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = cameraNoiseNone;
            playerCam.transform.localPosition = camOriginalPos;
        }
    }



    public class CinemachinePriority : MonoBehaviour
    {
        public CinemachineVirtualCamera cim; // 시네머신 우선 순위를 변경하기 위한 것
        private PlayerDeadCameraShake playerDeadCam; // 플레이어가 죽을 때 카메라 흔들리는 연출

        //플레이어의 PhotonView스크립트 가져와서 넣어주기
        [SerializeField]
        public PhotonView PV;

        private void Awake()
        {
            // 이 컴포넌트를 가진 오브젝트는 내 것이 아니면 서버에서 삭제됨. 
            if (!PV.IsMine)
            {
                Destroy(gameObject);
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            cim = GetComponent<CinemachineVirtualCamera>();
            playerDeadCam = new PlayerDeadCameraShake(cim);

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
                    cim.Priority += 5;
                    
                    yield return null;
                }
                
                yield return null;
                
            }
            
        }


        private void Update()
        {

            //플레이어가 죽었을 때 카메라 흔들기 위한 임시테스트 
            if(Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(playerDeadCam.CameraShake());
            }
        }

    }
   
}
