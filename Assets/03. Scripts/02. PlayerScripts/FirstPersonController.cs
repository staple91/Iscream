using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace LeeJungChul
{
	
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class FirstPersonController : MonoBehaviour
	{
		[Header("플레이어 스텟")]
		[Tooltip("초당 움직임 속도")]
		public float MoveSpeed = 4.0f;
		[Tooltip("초당 달리기 속도")]
		public float SprintSpeed = 6.0f;
		[Tooltip("플레이어 회전속도")]
		public float RotationSpeed = 1.0f;
		[Tooltip("가속도")]
		public float SpeedChangeRate = 10.0f;
		[Tooltip("캐릭터 중력")]
		public float Gravity = -15.0f;
		[Tooltip("중력이 적용되는 시간")]
		public float FallTimeout = 0.15f;

		[Header("플레이어 바닥체크")]
		[Tooltip("플레이어가 바닥에 있는 상태")]
		public bool Grounded = true;
		[Tooltip("바닥 오프셋")]
		public float GroundedOffset = -0.14f;
		[Tooltip("바닥체크 반경")]
		public float GroundedRadius = 0.5f;
		[Tooltip("바닥 레이어")]
		public LayerMask GroundLayers;

		[Header("시네머신")]
		[Tooltip("카메라가 따라다닐 타겟")]
		public GameObject CinemachineCameraTarget;
		[Tooltip("카메라를 올리는 속도")]
		public float TopClamp = 90.0f;
		[Tooltip("카메라 내리는 속도")]
		public float BottomClamp = -90.0f;
		

		// 시네머신
		private float cinemachineTargetPitch;

		// 플레이어
		private float speed;
		public float rotationVelocity;
		private float verticalVelocity;
		private float terminalVelocity = 53.0f;
		private float fallTimeoutDelta;

#if ENABLE_INPUT_SYSTEM
		// 인풋 시스템
		private PlayerInput playerInput;
#endif
		// 캐릭터 컨트롤러
		private CharacterController controller;

		// 인풋 시스템을 가지고 있다.
		private StarterAssetsInputs input;
		private GameObject mainCamera;

		private const float threshold = 0.01f;

		public bool IsCurrentDeviceMouse
		{
			get
			{
				#if ENABLE_INPUT_SYSTEM
				return playerInput.currentControlScheme == "KeyboardMouse";
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
			controller = GetComponent<CharacterController>();
			input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM
			playerInput = GetComponent<PlayerInput>();

		
#endif
        }

        private void Update()
		{
			
			GroundedCheck();
			Move();
			CharacterGravity();

            
        }

		private void LateUpdate()
		{
			CameraRotation();
		}

		private void GroundedCheck()
		{
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
		}

		/// <summary>
		/// 마우스를 통한 카메라 회전
		/// </summary>
		private void CameraRotation()
		{
			if (input.look.sqrMagnitude >= threshold)
			{
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
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

			if (input.move == Vector2.zero) targetSpeed = 0.0f;

			float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = input.analogMovement ? input.move.magnitude : 1f;

			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

				speed = Mathf.Round(speed * 1000f) / 1000f;
			}
			else
			{
				speed = targetSpeed;
			}

			Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

			if (input.move != Vector2.zero)
			{
				inputDirection = transform.right * input.move.x + transform.forward * input.move.y;
			}

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

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		}
	}
}



