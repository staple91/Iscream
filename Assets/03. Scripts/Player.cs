using Cinemachine;
using LeeJungChul;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using No;
using UnityEngine.UI;
using PangGom;
using YoungJaeKim;

namespace KimKyeongHun
{

    public class Player : MonoBehaviourPun , IPunObservable
    {
        bool isRaycasting;
        public bool IsRaycasting
        {
            get => isRaycasting;
            set
            {
                isRaycasting = value;
                if(isRaycasting)
                {
                    Interact();                   
                }
            }
        }


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
        public Item item;
        
        public GameObject ob1; //시네머신 둘리 실행 전 기존 카메라 1번 
        public GameObject ob2; //시네머신 둘리 실행 후 기존 카메라에서 2번 카메라 

        [Header("플레이어가 가지고있는 UI")]
        [Tooltip("상호작용 할 수 있는 상태 일시 이미지가 빨간색으로 변한다.")]
        [SerializeField] private Image InteractImage;
        [Tooltip("플레이어가 피격시 흔들리는 애니메이션 실행")]
        [SerializeField] private Image mentalityImage;
        [SerializeField] private TextMeshProUGUI playerNickname;

        public CinemachineVirtualCamera vircam;

        public bool isHidden = true;

        public Transform fpsHandTr;
        public Transform tpsHandTr;

      

        public Camera playerCam;
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

//            ob2 = GameObject.FindGameObjectWithTag("yeah");

            playerNickname.text = PhotonManager.nick;

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
            if (photonView.IsMine)
            {
                if (isHidden)
                {
                    playerCam.cullingMask = ~(1 << 10);

                }
                else { playerCam.cullingMask = -1; }             
            }
            mic.SetListener();
            if(controller.photonView.IsMine)
            {
                controller.photonView.RPC("DebugDraw", RpcTarget.AllBuffered);
            }
            

            //플레이어가 죽었을 때 카메라 흔들기 위한 임시테스트 
            if (Input.GetKey(KeyCode.G))
            {
                Hp -= 10;
            }


            if (controller.photonView.IsMine && inputsystem.click)
            {
                Debug.Log("a버튼 ");
                Click();
            }

            IsInteract();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ItemActive();
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

            IsRaycasting = true;

            inputsystem.click = false;
        }
        [PunRPC]
        void DebugDraw()
        {
            Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * 10f);

        }
        public void Interact()
        {
                    Debug.Log("sendnesxt");
            //문열림, 불 켜기 등등
            RaycastHit hit;
           
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward * 10f, out hit, 10))
            {
                if (hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
                {     
                    interactable.Owner = this;
                    interactable.Interact();

                    Debug.Log("상호작용");

                }

            }
        }

        public void ItemActive()
        {
            inven.curItem.item.Active();
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

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                stream.SendNext(IsRaycasting);
            }
            else
            {
                IsRaycasting = (bool)stream.ReceiveNext();
            }
            IsRaycasting = false;
        }

        public void IsInteract()
        {
            RaycastHit hit;

            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward * 10f, out hit, 10))
            {
                if (hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
                    InteractImage.color = Color.red;
                else
                    InteractImage.color = Color.white;
            }
        }
    }
}
