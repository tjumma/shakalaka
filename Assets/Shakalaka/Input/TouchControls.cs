//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.1
//     from Assets/Shakalaka/Input/TouchControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Shakalaka
{
    public partial class @TouchControls: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @TouchControls()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""TouchControls"",
    ""maps"": [
        {
            ""name"": ""Touch"",
            ""id"": ""91ff0f7c-ea6e-41e2-bc8a-3bd4a1557d09"",
            ""actions"": [
                {
                    ""name"": ""TouchInput"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ff7e3612-3224-47db-b29b-97e421b73440"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TouchPress"",
                    ""type"": ""Button"",
                    ""id"": ""9e8f7ec1-ee27-462b-9da5-c90b3b5b24ba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TouchPosition"",
                    ""type"": ""PassThrough"",
                    ""id"": ""33d347b1-8d5d-49ff-840f-9f309ac8e646"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""22f926a7-fbf7-4333-a7e9-8c46abb3a760"",
                    ""path"": ""<Touchscreen>/primaryTouch"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""513a8672-57ae-4a3a-9a48-360cab9e29eb"",
                    ""path"": ""<Touchscreen>/primaryTouch/press"",
                    ""interactions"": ""Press(behavior=2)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPress"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40c921c6-a270-4c9e-b96f-036b62f2260d"",
                    ""path"": ""<Touchscreen>/primaryTouch/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TouchPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Touch
            m_Touch = asset.FindActionMap("Touch", throwIfNotFound: true);
            m_Touch_TouchInput = m_Touch.FindAction("TouchInput", throwIfNotFound: true);
            m_Touch_TouchPress = m_Touch.FindAction("TouchPress", throwIfNotFound: true);
            m_Touch_TouchPosition = m_Touch.FindAction("TouchPosition", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Touch
        private readonly InputActionMap m_Touch;
        private List<ITouchActions> m_TouchActionsCallbackInterfaces = new List<ITouchActions>();
        private readonly InputAction m_Touch_TouchInput;
        private readonly InputAction m_Touch_TouchPress;
        private readonly InputAction m_Touch_TouchPosition;
        public struct TouchActions
        {
            private @TouchControls m_Wrapper;
            public TouchActions(@TouchControls wrapper) { m_Wrapper = wrapper; }
            public InputAction @TouchInput => m_Wrapper.m_Touch_TouchInput;
            public InputAction @TouchPress => m_Wrapper.m_Touch_TouchPress;
            public InputAction @TouchPosition => m_Wrapper.m_Touch_TouchPosition;
            public InputActionMap Get() { return m_Wrapper.m_Touch; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(TouchActions set) { return set.Get(); }
            public void AddCallbacks(ITouchActions instance)
            {
                if (instance == null || m_Wrapper.m_TouchActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_TouchActionsCallbackInterfaces.Add(instance);
                @TouchInput.started += instance.OnTouchInput;
                @TouchInput.performed += instance.OnTouchInput;
                @TouchInput.canceled += instance.OnTouchInput;
                @TouchPress.started += instance.OnTouchPress;
                @TouchPress.performed += instance.OnTouchPress;
                @TouchPress.canceled += instance.OnTouchPress;
                @TouchPosition.started += instance.OnTouchPosition;
                @TouchPosition.performed += instance.OnTouchPosition;
                @TouchPosition.canceled += instance.OnTouchPosition;
            }

            private void UnregisterCallbacks(ITouchActions instance)
            {
                @TouchInput.started -= instance.OnTouchInput;
                @TouchInput.performed -= instance.OnTouchInput;
                @TouchInput.canceled -= instance.OnTouchInput;
                @TouchPress.started -= instance.OnTouchPress;
                @TouchPress.performed -= instance.OnTouchPress;
                @TouchPress.canceled -= instance.OnTouchPress;
                @TouchPosition.started -= instance.OnTouchPosition;
                @TouchPosition.performed -= instance.OnTouchPosition;
                @TouchPosition.canceled -= instance.OnTouchPosition;
            }

            public void RemoveCallbacks(ITouchActions instance)
            {
                if (m_Wrapper.m_TouchActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(ITouchActions instance)
            {
                foreach (var item in m_Wrapper.m_TouchActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_TouchActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public TouchActions @Touch => new TouchActions(this);
        public interface ITouchActions
        {
            void OnTouchInput(InputAction.CallbackContext context);
            void OnTouchPress(InputAction.CallbackContext context);
            void OnTouchPosition(InputAction.CallbackContext context);
        }
    }
}
