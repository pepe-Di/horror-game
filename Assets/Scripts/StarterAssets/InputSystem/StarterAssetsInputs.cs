using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public Vector2 scroll;
		public bool jump;
		public bool sprint;
		public bool crouch;
		public bool interact;
		public bool click;
		public bool middleMouse;
		public bool middleMouseUp;
		public float mouseX;
		public float mouseY;
		public bool esc;
		public bool zoom;
		public bool f;

		[Header("Movement Settings")]
		public bool analogMovement;

#if !UNITY_IOS || !UNITY_ANDROID
		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
		public bool locked_input = false;
#endif
        private void Awake()
        {
			//Controls_ controls = new Controls_();
			

		}
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        public void OnMove(InputValue value)
		{
			if (!locked_input) MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}
		public void OnScroll(InputValue value)
		{
			if (!locked_input) ScrollInput(value.Get<Vector2>());
		}
		public void OnMouseX(InputValue value)
		{
			if (cursorInputForLook)
			{
				MouseXInput(value.Get<float>());
			}
		}

		public void OnMouseY(InputValue value)
		{
			if (cursorInputForLook)
			{
				MouseYInput(value.Get<float>());
			}
		}

		public void OnJump(InputValue value)
		{
			if (!locked_input) JumpInput(value.isPressed);
		}
		public void OnMiddleMouse(InputValue value)
		{
			if (!locked_input) MiddleMouseInput(value.isPressed);
		}
		public void OnMiddleMouseUp(InputValue value)
		{
			if (!locked_input) MiddleMouseUpInput(value.isPressed);
		}
		public void OnZoom(InputValue value)
		{
			if (!locked_input) ZoomInput(value.isPressed);
		}

		public void OnF(InputValue value)
		{
			if (!locked_input) FInput(value.isPressed);
		}

		public void OnEsc(InputValue value)
		{
			EscInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			if (!locked_input) SprintInput(value.isPressed);
		}

		public void OnCrouch(InputValue value)
		{
			if (!locked_input) CrouchInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
		{
			if(!locked_input)InteractInput(value.isPressed);
		}

		public void OnClick(InputValue value)
		{
			if (!locked_input) ClickInput(value.isPressed);
		}
#else
	// old input sys if we do decide to have it (most likely wont)...
#endif
		public void MouseXInput(float newMouseXDirection)
		{
			mouseX = newMouseXDirection;
		}

		public void MouseYInput(float newMouseYDirection)
		{
			mouseY = newMouseYDirection;
		}

		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}
		public void ScrollInput(Vector2 newScrollDirection)
		{
			scroll = newScrollDirection;
		}
		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}
		public void MiddleMouseInput(bool newMiddleMouseState)
		{
			middleMouse = newMiddleMouseState;
		}
		public void MiddleMouseUpInput(bool newMiddleMouseUpState)
		{
			middleMouseUp = newMiddleMouseUpState;
		}
		public void ZoomInput(bool newZoomState)
		{
			zoom = newZoomState;
		}

		public void EscInput(bool newEscState)
		{
			esc = newEscState;
		}

		public void FInput(bool newFState)
		{
			f = newFState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void CrouchInput(bool newCrouchState)
		{
			crouch = newCrouchState;
		}

		public void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}
		public void ClickInput(bool newClickState)
		{
			click = newClickState;
		}

#if !UNITY_IOS || !UNITY_ANDROID

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#endif

	}
	
}