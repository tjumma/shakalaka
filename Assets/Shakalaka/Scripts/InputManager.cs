using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Shakalaka
{
    public class InputManager : MonoBehaviour
    {
        public Action<Vector2, float> OnFingerDown;
        
        public Action<Vector2, float> OnStartTouch;
        public Action<Vector2, float> OnEndTouch;

        public Vector2 TouchPosition;
        
        private TouchControls _touchControls;

        private void Awake()
        {
            _touchControls = new TouchControls();
        }

        private void OnEnable()
        {
            _touchControls.Enable();
            EnhancedTouchSupport.Enable();
            
            _touchControls.Touch.TouchPress.started += StartTouch;
            _touchControls.Touch.TouchPress.canceled += EndTouch;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
        }

        private void OnDisable()
        {
            _touchControls.Disable();
            EnhancedTouchSupport.Disable();
            
            _touchControls.Touch.TouchPress.started -= StartTouch;
            _touchControls.Touch.TouchPress.canceled -= EndTouch;
            UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
        }

        private void Update()
        {
            TouchPosition = _touchControls.Touch.TouchPosition.ReadValue<Vector2>();
        }

        private void FingerDown(Finger finger)
        {
            Debug.Log($"FingerDown at {finger.screenPosition}");
            OnFingerDown?.Invoke(finger.screenPosition, Time.time);
        }

        private void StartTouch(InputAction.CallbackContext ctx)
        {
            var touchPosition = _touchControls.Touch.TouchPosition.ReadValue<Vector2>();
            Debug.Log($"Touch started at {touchPosition}");
            OnStartTouch?.Invoke(touchPosition, (float)ctx.startTime);
        }

        private void EndTouch(InputAction.CallbackContext ctx)
        {
            var touchPosition = _touchControls.Touch.TouchPosition.ReadValue<Vector2>();
            Debug.Log($"Touch ended at {touchPosition}");
            OnEndTouch?.Invoke(touchPosition, (float)ctx.time);
        }
    }
}