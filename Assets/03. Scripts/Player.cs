using Cinemachine;
using LeeJungChul;
using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using No;
using PangGom;
using YoungJaeKim;

namespace KimKyeongHun
{

    public class Player : MonoBehaviour
    {

        [SerializeField]
        public bool isLocal = false;

        [Header("플레이어 스텟")]
        [Tooltip("플레이어 현재 정신력")]
        [SerializeField] private float currentHp = 100;
        [Tooltip("플레이어 최대 정신력")]
        [SerializeField] private float maxHp = 100;

        [Tooltip("자신의 캐릭터 모델들")]
        [SerializeField]
        Renderer[] tpsRenders;

        public CinemachinePriority cinemachinePriority;
        public Inventory inven;
        
        public GameObject ob1; //시네머신 둘리 실행 전 기존 카메라 1번 
        public GameObject ob2; //시네머신 둘리 실행 후 기존 카메라에서 2번 카메라 


        public CinemachineVirtualCamera vircam;

        

        public Transform fpsHandTr;
        public Transform tpsHandTr;

       


        public Camera playerCam;

        public Transform fpsHandTr;
        public Transform tpsHandTr;
        // 플레이어 정신력 프로퍼티
        public float Hp
        {
            get
            {
                return currentHp;
            }
            set
            {
                currentHp = value;
                if (currentHp > maxHp)
                {
                    currentHp = maxHp;
                }
                if (currentHp <= 0)
                {
                    // 플레이어 사망
                    Debug.Log("플레이어 죽음 ");
                    cinemachinePriority.GetIsPlayerCheck = true;

                }
            }
        }

        MicComponent mic;

        public FirstPersonController controller;
        public StarterAssetsInputs inputsystem;

        private bool isMoveable = true;

        public bool IsMoveable
        {
            get => isMoveable;
            set
            {
                isMoveable = value;
                if (value)
                    controller.enabled = true;
                else
                    controller.enabled = false;

            }
        }

        // Start is called before the first frame update
        void Start()
        {
            inputsystem = GetComponent<StarterAssetsInputs>();
            controller = GetComponent<FirstPersonController>();
            GameManager.Instance.playerList.Add(this);
            mic = GetComponent<MicComponent>();

            cinemachinePriority = GetComponentInChildren<CinemachinePriority>();

            vircam = GetComponentInChildren<CinemachineVirtualCamera>();
            
            
            if (controller.photonView.IsMine)
            {
                foreach (Renderer render in tpsRenders)
                {
                    render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }
            }
            else
            {
                foreach (Behaviour comp in playerCam.GetComponents<Behaviour>())
                {
                    if (comp as PhotonTransformView)
                        continue;
                    comp.enabled = false;
                }

            }

        }


        // Update is called once per frame
        void Update()
        {
            if(controller.photonView.IsMine)
            {
                Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * 10f);
            }



            //플레이어가 죽었을 때 카메라 흔들기 위한 임시테스트 
            if (Input.GetKey(KeyCode.G))
            {
                Hp -= 10;
            }

            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10f, Color.red);

            if (controller.photonView.IsMine && inputsystem.click)
            {
                  
                Debug.Log("a버튼 ");
                Click();
            }

        }

        /// <summary>
        /// 플레이어의 정신력이 줄어들 때 정신력 이미지 애니메이션 활성화
        /// </summary>
        private void HpDown()
        {
            // 정신력이 하락할때

        }

        public void Click()
        {
            if(controller.photonView.IsMine)
                controller.photonView.RPC("Interact", RpcTarget.AllBuffered);
            inputsystem.click = false;
        }
        [PunRPC]
        void DebugDraw()
        {
            Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * 10f);

        }
        [PunRPC]
        public void Interact()
        {
            //문열림, 불 켜기 등등
            RaycastHit hit;
           
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward * 10f, out hit, 10))
            {
                
                if (hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
                {     
                    interactable.Owner = this;
                    interactable.Interact();        

                    Debug.Log("상호작용");
                }
               
            }
            
        }


        public void InteractionDollyCart()
        {
            
            vircam.Follow = ob2.gameObject.transform;
            var pov = vircam.AddCinemachineComponent<CinemachinePOV>();

            pov.m_HorizontalAxis.m_MaxSpeed = 100f;
            pov.m_VerticalAxis.m_MaxSpeed = 80f;

            
         
            this.GetComponent<CharacterController>().enabled = false;


            //둘리카트 실행할 때 플레이어 SkinnedMeshRenderer들을 꺼둔다.
            SkinnedMeshRenderer[] playerSkin = GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach(SkinnedMeshRenderer renderer in playerSkin)
            {
                renderer.enabled = false;
            }
            
            ob2.GetComponent<CinemachineDollyCart>().enabled = true;

        }

        public void CancelDollyCart()
        {
            
            vircam.Follow = ob1.gameObject.transform;
            this.GetComponent<CharacterController>().enabled = true;
            
            //되돌아 올 때는 플레이어 SkinnedMeshRenderer들을 다시 켜준다.
            SkinnedMeshRenderer[] playerSkin = GetComponentsInChildren<SkinnedMeshRenderer>();

            foreach (SkinnedMeshRenderer renderer in playerSkin)
            {
                renderer.enabled = true;
            }

            vircam.DestroyCinemachineComponent<CinemachinePOV>();
            ob2.GetComponent<CinemachineDollyCart>().enabled = false;
            ob2.GetComponent<CinemachineDollyCart>().m_Position = 0;

        }

    }
}
