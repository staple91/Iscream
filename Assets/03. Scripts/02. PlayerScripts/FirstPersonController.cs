using UnityEngine;
using Photon.Pun;
using Cinemachine;
using LeeJungChul;
using PangGom;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace LeeJungChul
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class FirstPersonController : MonoBehaviourPun
    {
        [Header("플레이어 속도")]
        [Tooltip("초당 움직임 속도")]
        public float MoveSpeed = 4.0f;
        [Tooltip("초당 달리기 속도")]
        public float SprintSpeed = 6.0f;
        [Tooltip("플레이어 카메라 회전속도")]
        [HideInInspector]
        public float RotationSpeed = 15.0f;
        [Tooltip("가속도")]
        private float SpeedChangeRate = 10.0f;
        [Tooltip("캐릭터 중력")]
        private float Gravity = -15.0f;
        [Tooltip("중력이 적용되는 시간")]
        private float FallTimeout = 0.15f;

        [Header("플레이어 바닥체크")]
        [Tooltip("플레이어가 바닥에 있는 상태")]
        public bool Grounded = true;
        [Tooltip("바닥 오프셋")]
        private float GroundedOffset = -0.14f;
        [Tooltip("바닥체크 반경")]
        private float GroundedRadius = 0.5f;
        [Tooltip("바닥 레이어")]
        public LayerMask GroundLayers;

        [Header("시네머신")]
        [Tooltip("카메라가 따라다닐 타겟")]
        public GameObject CinemachineCameraTarget;
        [Tooltip("카메라를 올리는 상한선")]
        public float TopClamp = 90.0f;
        [Tooltip("카메라 내리는 하한선")]
        public float BottomClamp = -90.0f;

        [Header("플레이어 UI")]
        private Animator playerUI;

        // 시네머신
        private float cinemachineTargetPitch;

        // 플레이어
        private float rotationVelocity;
        private float speed;       
        private float verticalVelocity;
        private float terminalVelocity = 53.0f;
        private float fallTimeoutDelta;

#if ENABLE_INPUT_SYSTEM
        // 인풋 시스템
        private PlayerInput playerInput;
#endif
        // 캐릭터 컨트롤러
        private CharacterController controller;
        private Animator playeranimatior;

        // 인풋 시스템을 가지고 있다.
        private StarterAssetsInputs input;
        private GameObject mainCamera;
        AudioSource walkAudio;

        private const float threshold = 0.01f;

        public GameObject book;
        public bool isBookOpen;
        public GameObject option;
        public bool isoption;

        // 현재 입력 받고 있는 컨트롤러 프로퍼티
        public bool IsCurrentDevice
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return playerInput.currentControlScheme == "any";
#else
				return false;
#endif
            }
        }

        private void Awake()
        {
            if (mainCamera == null)
            {
                mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            playeranimatior = GetComponent<Animator>();
            controller = GetComponent<CharacterController>();
            input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
            playerInput = GetComponent<PlayerInput>();
#endif
            // 로컬 플레이어의 카메라를 따라가 시야를 제공한다.
            if (photonView.IsMine)
            {
                CinemachineVirtualCamera followCam = FindAnyObjectByType<CinemachineVirtualCamera>();

                followCam.Follow = CinemachineCameraTarget.transform;
            }

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // 로컬 플레이어가 아닌 경우 입력을 받지 못함
            if (photonView.IsMine)
            {
                GroundedCheck();
                Move();
                CharacterGravity();
            }

            KeyInput();
        }

        private void LateUpdate()
        {
             
            if (photonView.IsMine)
            {
                CameraRotation();
            }
        }

        private void OnDisable()
        {
            playeranimatior.SetFloat("Speed", 0f);
        }

        private void KeyInput()
        {
            if (input.bookOpen && !isBookOpen)
            {
                book.SetActive(true);
                input.bookOpen = false;
                isBookOpen = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (input.bookOpen && isBookOpen)
            {
                book.SetActive(false); ;
                input.bookOpen = false;
                isBookOpen = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (input.Option && !isoption)
            {
                option.SetActive(true);
                input.Option = false;
                isoption = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (input.Option && isoption)
            {
                option.SetActive(false);
                input.Option = false;
                isoption = false;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        /// <summary>
        /// 땅바닥 체크 함수
        /// </summary>
        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
        }
        
        /// <summary>
        /// 마우스를 통한 카메라 회전 상하는 90도 회전 가능 좌우는 360도까지 회전 가능
        /// </summary>
        private void CameraRotation()
        {
            if (input.look.sqrMagnitude >= threshold)
            {
                float deltaTimeMultiplier = IsCurrentDevice ? 1.0f : Time.deltaTime;
                cinemachineTargetPitch += input.look.y * RotationSpeed * deltaTimeMultiplier;
                rotationVelocity = input.look.x * RotationSpeed * deltaTimeMultiplier;

                cinemachineTargetPitch = ClampAngle(cinemachineTargetPitch, BottomClamp, TopClamp);

                CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(cinemachineTargetPitch, 0.0f, 0.0f);

                transform.Rotate(Vector3.up * rotationVelocity);
            }
        }
        
        /// <summary>
        /// 캐릭터 이동 함수
        /// </summary>
        private void Move()
        {
            float targetSpeed = input.sprint ? SprintSpeed : MoveSpeed;

            if (input.move == Vector2.zero)
            {
                targetSpeed = 0.0f;
            }

            float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;
            
            //SoundManager.Instance.PlayAudio(SoundManager.Instance.playerFootSound, true, transform.position);

            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // 플레이어의 이동 상태에 따라 속도 변경
                speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

                // 속도를 반올림
                speed = Mathf.Round(speed * 1000f) * 0.001f;
            }
            else
            {
                speed = targetSpeed;
            }

            Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

            if (input.move != Vector2.zero)
            {
                if(walkAudio == null)
                    walkAudio = SoundManager.Instance.PlayWaitingAudio(SoundManager.Instance.playerFootSound);
                else if (walkAudio.isPlaying == false)
                    walkAudio = null;
                inputDirection = transform.right * input.move.x + transform.forward * input.move.y;
            }
            // 애니메이션 파라미터의 최대값이 3이기 때문에 최대 speed 인 6에서 2를나눠줌.
            playeranimatior.SetFloat("Speed", speed / 2);
            controller.Move(inputDirection.normalized * (speed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
        }

        /// <summary>
        /// 캐릭터 중력 적용 함수
        /// </summary>
        private void CharacterGravity()
        {
            if (Grounded)
            {
                fallTimeoutDelta = FallTimeout;

                if (verticalVelocity < 0.0f)
                {
                    verticalVelocity = -2f;
                }
            }
            else
            {
                if (fallTimeoutDelta >= 0.0f)
                {
                    fallTimeoutDelta -= Time.deltaTime;
                }

            }

            if (verticalVelocity < terminalVelocity)
            {
                verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        /// <summary>
        /// 360도 회전 가능하게 하는 함수 최소 최대값을 설정해 이 값을 넘지 못하게한다.
        /// </summary> 
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }       
    }
}



