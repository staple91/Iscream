using UnityEngine;
using Photon.Pun;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace LeeJungChul
{
	public class StarterAssetsInputs : MonoBehaviourPun
	{
		
		[Header("플레이어 상태")]
		public Vector2 move;
		public Vector2 look;
		public bool click;
		public bool itemUse;
		public bool sprint;

		public bool analogMovement;

		[Header("마우스 상태 ")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}
		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnClick(InputValue value)
        {
			ClickInput(value.isPressed);
        }
		public void OnItemUse(InputValue value)
		{
			ItemUseInput(value.isPressed);
		}
#endif

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void ClickInput(bool newClickState)
        {
			click = newClickState;
        }

		public void ItemUseInput(bool itemUseState)
		{
			itemUse = itemUseState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		// ESC 클릭시 마우스 커서 나옴
		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}