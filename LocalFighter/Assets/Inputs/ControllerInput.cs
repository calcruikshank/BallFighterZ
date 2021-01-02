// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/ControllerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ControllerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ControllerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ControllerInput"",
    ""maps"": [
        {
            ""name"": ""MouseInputs"",
            ""id"": ""664105de-9e32-4ff2-99b8-df3fb6d33a42"",
            ""actions"": [
                {
                    ""name"": ""RightStickDash"",
                    ""type"": ""Value"",
                    ""id"": ""7d8856a2-71c1-4987-8637-38058d2001d7"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": ""StickDeadzone(min=0.2)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyboardMove"",
                    ""type"": ""Value"",
                    ""id"": ""de7db1a5-6067-474c-8566-eafc1e03529e"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""KeyboardMove1"",
                    ""type"": ""Value"",
                    ""id"": ""1fd15ee8-ec03-4b2f-902f-9f8d87b6aa93"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PunchLeft"",
                    ""type"": ""Button"",
                    ""id"": ""c56b8dd7-6ea5-4b5a-aeff-d3b8b5b5eca4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""PunchRight"",
                    ""type"": ""Button"",
                    ""id"": ""a4a1ecae-23b5-4ada-8a68-60d05304afc6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ReleasePunchRight"",
                    ""type"": ""Button"",
                    ""id"": ""ca6c73c3-3924-42f8-be92-47fe51705fc7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""ReleasePunchLeft"",
                    ""type"": ""Button"",
                    ""id"": ""d982424f-01bc-447f-af6b-8aacbd2f603c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""ShieldRight"",
                    ""type"": ""Button"",
                    ""id"": ""7d806d99-d494-49d6-b92c-2137df7b87ad"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ShieldLeft"",
                    ""type"": ""Button"",
                    ""id"": ""08cbb95d-239d-491f-b98b-308ee593e2d6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ReleaseShieldLeft"",
                    ""type"": ""Button"",
                    ""id"": ""70ae3f0f-7220-4428-b2c9-0d7e6a53b38e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""ReleaseShieldRight"",
                    ""type"": ""Button"",
                    ""id"": ""203fe03b-047a-485e-a2ec-21e5d09b8390"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Restart"",
                    ""type"": ""Button"",
                    ""id"": ""4b600468-f111-4a29-9842-f34e57b8497d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""MouseMove"",
                    ""type"": ""Value"",
                    ""id"": ""93cc1103-2e43-4ab3-89e2-90325a033b56"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseDash"",
                    ""type"": ""Button"",
                    ""id"": ""4824931f-aad6-4f98-a446-4abda282ff48"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ShieldBoth"",
                    ""type"": ""Button"",
                    ""id"": ""8338f122-8cdb-4519-bd3d-1683b30df819"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""ReleaseShieldBoth"",
                    ""type"": ""Button"",
                    ""id"": ""8a2b77b2-8dd1-4b8c-b17c-41f2a674f106"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""ReleaseDash"",
                    ""type"": ""Button"",
                    ""id"": ""6ad97e47-6417-4026-8511-b1f747fa0092"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""ReleaseDashController"",
                    ""type"": ""Value"",
                    ""id"": ""c17df5be-fa50-4002-928e-edd92425606b"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""17275dd6-1886-4086-91ee-2b4e9b5b80ec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=1)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b77b0ed4-2877-406b-b467-1be981d1dba1"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone(min=0.2)"",
                    ""groups"": ""Controller"",
                    ""action"": ""RightStickDash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d15a2648-1c0d-4d26-adca-0b8ccfd12f2a"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Controller"",
                    ""action"": ""KeyboardMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""57adf1c4-d228-4e99-a415-642fb276ce42"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KeyboardMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""84761c0d-ad5f-43e2-a6d2-f7f1a16b30c9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""KeyboardMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a61a6c9e-90bd-43ff-bc56-5a9c689c7994"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""KeyboardMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1a09221a-e997-4154-a3f3-1be4ac796da0"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""KeyboardMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d3f59524-7077-4bcc-8ea1-1803c339f869"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""KeyboardMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ff6d80cf-4ed7-4c33-8b50-0eee7ebba7a3"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PunchLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e1c75d1b-6ded-41fc-a09e-0ba3916f9f46"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PunchLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dbc7f0ef-7019-40fc-9424-59b105b5a3d5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""PunchLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6deb2b39-d693-4de5-b10b-1bd28ca0cc74"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PunchRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ac13d4d-417b-40da-a875-e5e08801b2f6"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""PunchRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c95349f-8d19-4cc8-b5d9-3f3e79722687"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""PunchRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b54eb45c-ec61-45cf-92ec-d968dd888d69"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ReleasePunchRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6b04bc83-11c8-49aa-98c5-1cf228119263"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ReleasePunchRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9990425d-20bd-4f39-83b1-cbbeb6ad073e"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""ReleasePunchRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a2d7bb90-cb2c-4a20-8590-4378756db43f"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ReleasePunchLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""333ec3eb-f5b9-462a-bb1b-80bca4dd9a67"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ReleasePunchLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""190b078f-4e81-4df5-a97e-21cf9efe717f"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""ReleasePunchLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""473a8f39-7d54-40da-acdc-f4d1b55adc9e"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ShieldRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""185a59b7-7ab4-459e-a1fd-0ddb30d79cbc"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ShieldLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""416db132-a20d-4a07-a00e-e72c6d3059d9"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ReleaseShieldLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bcd69a90-a8ed-44e5-ab17-67db5b863bc5"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ReleaseShieldRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""abfdb2d0-af37-41e7-9c29-958837c28ec3"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a397536d-8377-455d-bd1c-dc36e9d21d5f"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""MouseMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""59a93f3c-0582-4ff9-bf6d-f68ab58e4839"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Controller"",
                    ""action"": ""KeyboardMove1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dcafbcd0-3277-4b6f-90ba-641bb632641e"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""MouseDash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""463d5113-8f8b-4fcb-99d8-a16ef33ffcb6"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""ShieldBoth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a701501-ee3a-404a-9f4d-c564653a6458"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""ReleaseShieldBoth"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc0013be-cd73-4d7a-92e9-fa0988a5196f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""ReleaseDash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0374481-b72e-4b77-ba60-23afcf775ea0"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""ReleaseDashController"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""00dc67d9-1270-40cd-9d6a-6c3e753d080a"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Controller"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4e7586b-acc6-422f-b566-3c817a3af1c5"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard And Mouse"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard And Mouse"",
            ""bindingGroup"": ""Keyboard And Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Controller"",
            ""bindingGroup"": ""Controller"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // MouseInputs
        m_MouseInputs = asset.FindActionMap("MouseInputs", throwIfNotFound: true);
        m_MouseInputs_RightStickDash = m_MouseInputs.FindAction("RightStickDash", throwIfNotFound: true);
        m_MouseInputs_KeyboardMove = m_MouseInputs.FindAction("KeyboardMove", throwIfNotFound: true);
        m_MouseInputs_KeyboardMove1 = m_MouseInputs.FindAction("KeyboardMove1", throwIfNotFound: true);
        m_MouseInputs_PunchLeft = m_MouseInputs.FindAction("PunchLeft", throwIfNotFound: true);
        m_MouseInputs_PunchRight = m_MouseInputs.FindAction("PunchRight", throwIfNotFound: true);
        m_MouseInputs_ReleasePunchRight = m_MouseInputs.FindAction("ReleasePunchRight", throwIfNotFound: true);
        m_MouseInputs_ReleasePunchLeft = m_MouseInputs.FindAction("ReleasePunchLeft", throwIfNotFound: true);
        m_MouseInputs_ShieldRight = m_MouseInputs.FindAction("ShieldRight", throwIfNotFound: true);
        m_MouseInputs_ShieldLeft = m_MouseInputs.FindAction("ShieldLeft", throwIfNotFound: true);
        m_MouseInputs_ReleaseShieldLeft = m_MouseInputs.FindAction("ReleaseShieldLeft", throwIfNotFound: true);
        m_MouseInputs_ReleaseShieldRight = m_MouseInputs.FindAction("ReleaseShieldRight", throwIfNotFound: true);
        m_MouseInputs_Restart = m_MouseInputs.FindAction("Restart", throwIfNotFound: true);
        m_MouseInputs_MouseMove = m_MouseInputs.FindAction("MouseMove", throwIfNotFound: true);
        m_MouseInputs_MouseDash = m_MouseInputs.FindAction("MouseDash", throwIfNotFound: true);
        m_MouseInputs_ShieldBoth = m_MouseInputs.FindAction("ShieldBoth", throwIfNotFound: true);
        m_MouseInputs_ReleaseShieldBoth = m_MouseInputs.FindAction("ReleaseShieldBoth", throwIfNotFound: true);
        m_MouseInputs_ReleaseDash = m_MouseInputs.FindAction("ReleaseDash", throwIfNotFound: true);
        m_MouseInputs_ReleaseDashController = m_MouseInputs.FindAction("ReleaseDashController", throwIfNotFound: true);
        m_MouseInputs_Select = m_MouseInputs.FindAction("Select", throwIfNotFound: true);
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

    // MouseInputs
    private readonly InputActionMap m_MouseInputs;
    private IMouseInputsActions m_MouseInputsActionsCallbackInterface;
    private readonly InputAction m_MouseInputs_RightStickDash;
    private readonly InputAction m_MouseInputs_KeyboardMove;
    private readonly InputAction m_MouseInputs_KeyboardMove1;
    private readonly InputAction m_MouseInputs_PunchLeft;
    private readonly InputAction m_MouseInputs_PunchRight;
    private readonly InputAction m_MouseInputs_ReleasePunchRight;
    private readonly InputAction m_MouseInputs_ReleasePunchLeft;
    private readonly InputAction m_MouseInputs_ShieldRight;
    private readonly InputAction m_MouseInputs_ShieldLeft;
    private readonly InputAction m_MouseInputs_ReleaseShieldLeft;
    private readonly InputAction m_MouseInputs_ReleaseShieldRight;
    private readonly InputAction m_MouseInputs_Restart;
    private readonly InputAction m_MouseInputs_MouseMove;
    private readonly InputAction m_MouseInputs_MouseDash;
    private readonly InputAction m_MouseInputs_ShieldBoth;
    private readonly InputAction m_MouseInputs_ReleaseShieldBoth;
    private readonly InputAction m_MouseInputs_ReleaseDash;
    private readonly InputAction m_MouseInputs_ReleaseDashController;
    private readonly InputAction m_MouseInputs_Select;
    public struct MouseInputsActions
    {
        private @ControllerInput m_Wrapper;
        public MouseInputsActions(@ControllerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @RightStickDash => m_Wrapper.m_MouseInputs_RightStickDash;
        public InputAction @KeyboardMove => m_Wrapper.m_MouseInputs_KeyboardMove;
        public InputAction @KeyboardMove1 => m_Wrapper.m_MouseInputs_KeyboardMove1;
        public InputAction @PunchLeft => m_Wrapper.m_MouseInputs_PunchLeft;
        public InputAction @PunchRight => m_Wrapper.m_MouseInputs_PunchRight;
        public InputAction @ReleasePunchRight => m_Wrapper.m_MouseInputs_ReleasePunchRight;
        public InputAction @ReleasePunchLeft => m_Wrapper.m_MouseInputs_ReleasePunchLeft;
        public InputAction @ShieldRight => m_Wrapper.m_MouseInputs_ShieldRight;
        public InputAction @ShieldLeft => m_Wrapper.m_MouseInputs_ShieldLeft;
        public InputAction @ReleaseShieldLeft => m_Wrapper.m_MouseInputs_ReleaseShieldLeft;
        public InputAction @ReleaseShieldRight => m_Wrapper.m_MouseInputs_ReleaseShieldRight;
        public InputAction @Restart => m_Wrapper.m_MouseInputs_Restart;
        public InputAction @MouseMove => m_Wrapper.m_MouseInputs_MouseMove;
        public InputAction @MouseDash => m_Wrapper.m_MouseInputs_MouseDash;
        public InputAction @ShieldBoth => m_Wrapper.m_MouseInputs_ShieldBoth;
        public InputAction @ReleaseShieldBoth => m_Wrapper.m_MouseInputs_ReleaseShieldBoth;
        public InputAction @ReleaseDash => m_Wrapper.m_MouseInputs_ReleaseDash;
        public InputAction @ReleaseDashController => m_Wrapper.m_MouseInputs_ReleaseDashController;
        public InputAction @Select => m_Wrapper.m_MouseInputs_Select;
        public InputActionMap Get() { return m_Wrapper.m_MouseInputs; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseInputsActions set) { return set.Get(); }
        public void SetCallbacks(IMouseInputsActions instance)
        {
            if (m_Wrapper.m_MouseInputsActionsCallbackInterface != null)
            {
                @RightStickDash.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnRightStickDash;
                @RightStickDash.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnRightStickDash;
                @RightStickDash.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnRightStickDash;
                @KeyboardMove.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnKeyboardMove;
                @KeyboardMove.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnKeyboardMove;
                @KeyboardMove.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnKeyboardMove;
                @KeyboardMove1.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnKeyboardMove1;
                @KeyboardMove1.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnKeyboardMove1;
                @KeyboardMove1.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnKeyboardMove1;
                @PunchLeft.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnPunchLeft;
                @PunchLeft.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnPunchLeft;
                @PunchLeft.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnPunchLeft;
                @PunchRight.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnPunchRight;
                @PunchRight.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnPunchRight;
                @PunchRight.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnPunchRight;
                @ReleasePunchRight.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleasePunchRight;
                @ReleasePunchRight.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleasePunchRight;
                @ReleasePunchRight.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleasePunchRight;
                @ReleasePunchLeft.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleasePunchLeft;
                @ReleasePunchLeft.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleasePunchLeft;
                @ReleasePunchLeft.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleasePunchLeft;
                @ShieldRight.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnShieldRight;
                @ShieldRight.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnShieldRight;
                @ShieldRight.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnShieldRight;
                @ShieldLeft.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnShieldLeft;
                @ShieldLeft.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnShieldLeft;
                @ShieldLeft.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnShieldLeft;
                @ReleaseShieldLeft.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseShieldLeft;
                @ReleaseShieldLeft.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseShieldLeft;
                @ReleaseShieldLeft.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseShieldLeft;
                @ReleaseShieldRight.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseShieldRight;
                @ReleaseShieldRight.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseShieldRight;
                @ReleaseShieldRight.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseShieldRight;
                @Restart.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnRestart;
                @Restart.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnRestart;
                @Restart.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnRestart;
                @MouseMove.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnMouseMove;
                @MouseMove.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnMouseMove;
                @MouseMove.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnMouseMove;
                @MouseDash.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnMouseDash;
                @MouseDash.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnMouseDash;
                @MouseDash.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnMouseDash;
                @ShieldBoth.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnShieldBoth;
                @ShieldBoth.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnShieldBoth;
                @ShieldBoth.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnShieldBoth;
                @ReleaseShieldBoth.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseShieldBoth;
                @ReleaseShieldBoth.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseShieldBoth;
                @ReleaseShieldBoth.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseShieldBoth;
                @ReleaseDash.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseDash;
                @ReleaseDash.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseDash;
                @ReleaseDash.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseDash;
                @ReleaseDashController.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseDashController;
                @ReleaseDashController.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseDashController;
                @ReleaseDashController.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnReleaseDashController;
                @Select.started -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_MouseInputsActionsCallbackInterface.OnSelect;
            }
            m_Wrapper.m_MouseInputsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @RightStickDash.started += instance.OnRightStickDash;
                @RightStickDash.performed += instance.OnRightStickDash;
                @RightStickDash.canceled += instance.OnRightStickDash;
                @KeyboardMove.started += instance.OnKeyboardMove;
                @KeyboardMove.performed += instance.OnKeyboardMove;
                @KeyboardMove.canceled += instance.OnKeyboardMove;
                @KeyboardMove1.started += instance.OnKeyboardMove1;
                @KeyboardMove1.performed += instance.OnKeyboardMove1;
                @KeyboardMove1.canceled += instance.OnKeyboardMove1;
                @PunchLeft.started += instance.OnPunchLeft;
                @PunchLeft.performed += instance.OnPunchLeft;
                @PunchLeft.canceled += instance.OnPunchLeft;
                @PunchRight.started += instance.OnPunchRight;
                @PunchRight.performed += instance.OnPunchRight;
                @PunchRight.canceled += instance.OnPunchRight;
                @ReleasePunchRight.started += instance.OnReleasePunchRight;
                @ReleasePunchRight.performed += instance.OnReleasePunchRight;
                @ReleasePunchRight.canceled += instance.OnReleasePunchRight;
                @ReleasePunchLeft.started += instance.OnReleasePunchLeft;
                @ReleasePunchLeft.performed += instance.OnReleasePunchLeft;
                @ReleasePunchLeft.canceled += instance.OnReleasePunchLeft;
                @ShieldRight.started += instance.OnShieldRight;
                @ShieldRight.performed += instance.OnShieldRight;
                @ShieldRight.canceled += instance.OnShieldRight;
                @ShieldLeft.started += instance.OnShieldLeft;
                @ShieldLeft.performed += instance.OnShieldLeft;
                @ShieldLeft.canceled += instance.OnShieldLeft;
                @ReleaseShieldLeft.started += instance.OnReleaseShieldLeft;
                @ReleaseShieldLeft.performed += instance.OnReleaseShieldLeft;
                @ReleaseShieldLeft.canceled += instance.OnReleaseShieldLeft;
                @ReleaseShieldRight.started += instance.OnReleaseShieldRight;
                @ReleaseShieldRight.performed += instance.OnReleaseShieldRight;
                @ReleaseShieldRight.canceled += instance.OnReleaseShieldRight;
                @Restart.started += instance.OnRestart;
                @Restart.performed += instance.OnRestart;
                @Restart.canceled += instance.OnRestart;
                @MouseMove.started += instance.OnMouseMove;
                @MouseMove.performed += instance.OnMouseMove;
                @MouseMove.canceled += instance.OnMouseMove;
                @MouseDash.started += instance.OnMouseDash;
                @MouseDash.performed += instance.OnMouseDash;
                @MouseDash.canceled += instance.OnMouseDash;
                @ShieldBoth.started += instance.OnShieldBoth;
                @ShieldBoth.performed += instance.OnShieldBoth;
                @ShieldBoth.canceled += instance.OnShieldBoth;
                @ReleaseShieldBoth.started += instance.OnReleaseShieldBoth;
                @ReleaseShieldBoth.performed += instance.OnReleaseShieldBoth;
                @ReleaseShieldBoth.canceled += instance.OnReleaseShieldBoth;
                @ReleaseDash.started += instance.OnReleaseDash;
                @ReleaseDash.performed += instance.OnReleaseDash;
                @ReleaseDash.canceled += instance.OnReleaseDash;
                @ReleaseDashController.started += instance.OnReleaseDashController;
                @ReleaseDashController.performed += instance.OnReleaseDashController;
                @ReleaseDashController.canceled += instance.OnReleaseDashController;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
            }
        }
    }
    public MouseInputsActions @MouseInputs => new MouseInputsActions(this);
    private int m_KeyboardAndMouseSchemeIndex = -1;
    public InputControlScheme KeyboardAndMouseScheme
    {
        get
        {
            if (m_KeyboardAndMouseSchemeIndex == -1) m_KeyboardAndMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard And Mouse");
            return asset.controlSchemes[m_KeyboardAndMouseSchemeIndex];
        }
    }
    private int m_ControllerSchemeIndex = -1;
    public InputControlScheme ControllerScheme
    {
        get
        {
            if (m_ControllerSchemeIndex == -1) m_ControllerSchemeIndex = asset.FindControlSchemeIndex("Controller");
            return asset.controlSchemes[m_ControllerSchemeIndex];
        }
    }
    public interface IMouseInputsActions
    {
        void OnRightStickDash(InputAction.CallbackContext context);
        void OnKeyboardMove(InputAction.CallbackContext context);
        void OnKeyboardMove1(InputAction.CallbackContext context);
        void OnPunchLeft(InputAction.CallbackContext context);
        void OnPunchRight(InputAction.CallbackContext context);
        void OnReleasePunchRight(InputAction.CallbackContext context);
        void OnReleasePunchLeft(InputAction.CallbackContext context);
        void OnShieldRight(InputAction.CallbackContext context);
        void OnShieldLeft(InputAction.CallbackContext context);
        void OnReleaseShieldLeft(InputAction.CallbackContext context);
        void OnReleaseShieldRight(InputAction.CallbackContext context);
        void OnRestart(InputAction.CallbackContext context);
        void OnMouseMove(InputAction.CallbackContext context);
        void OnMouseDash(InputAction.CallbackContext context);
        void OnShieldBoth(InputAction.CallbackContext context);
        void OnReleaseShieldBoth(InputAction.CallbackContext context);
        void OnReleaseDash(InputAction.CallbackContext context);
        void OnReleaseDashController(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
    }
}
