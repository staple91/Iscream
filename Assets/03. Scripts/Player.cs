using Cinemachine;
using LeeJungChul;
using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using No;

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



        // 플레이어 정신력 프로퍼티
        public float Hp
        {
            get
            {
                return currentHp;
            }
            set
            {
                if (currentHp > maxHp)
                {
                    currentHp = maxHp;
                }
                if (currentHp <= 0)
                {
                    // 플레이어 사망
                }
            }
        }

        public GameObject playerCam;
        MicComponent mic;

        public FirstPersonController controller;

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
            controller = GetComponent<FirstPersonController>();
            playerCam = GameObject.FindGameObjectWithTag("MainCamera");
            mic = GetComponent<MicComponent>();

        }


        // Update is called once per frame
        void Update()
        {

            Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward * 10f, Color.red);



            if (controller.photonView.IsMine && (Input.GetKey(KeyCode.JoystickButton0) || Input.GetMouseButtonDown(0)))
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
            Interact();
        }

        public void Interact()
        {
            //문열림, 불 켜기 등등

            RaycastHit hit;

            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward * 10f, out hit, 10))
            {
                
                if(hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    interactable.Owner = this;
                    interactable.Interact();
                    Debug.Log("상호작용");
                }
                
                

            }
        }


    }
}
