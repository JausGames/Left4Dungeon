using UnityEngine;
using UnityEngine.InputSystem;
using L4P.Gameplay.Player;
using static UnityEngine.InputSystem.InputAction;

namespace L4P.Gameplay.Player.Controls
{

    [RequireComponent(typeof(Controls.InputManager))]
    public class PlayerInputs : MonoBehaviour
    {
        [SerializeField] IPlayerController motor = null;
        [SerializeField] IPlayerCombat combat = null;

        public void Start()
        {
            /*Debug.Log("Network Informations : IsOwner " + IsOwner);
            if (!IsOwner) return;*/

            motor = GetComponent<IPlayerController>();
            combat = GetComponent<IPlayerCombat>();

            InputManager.Controls.Gameplay.Move.performed += ctx => OnMove(ctx.ReadValue<Vector2>());
            InputManager.Controls.Gameplay.Move.canceled += _ => OnMove(Vector2.zero);

            InputManager.Controls.Gameplay.Sprint.performed += _ => OnSprint(true);
            InputManager.Controls.Gameplay.Sprint.canceled += _ => OnSprint(false);

            InputManager.Controls.Gameplay.UseLeft.performed += _ => OnUseWeapon(true, false);
            InputManager.Controls.Gameplay.UseLeft.canceled += _ => OnUseWeapon(false, false);

            InputManager.Controls.Gameplay.UseRight.performed += _ => OnUseWeapon(true, true);
            InputManager.Controls.Gameplay.UseRight.canceled += _ => OnUseWeapon(false, true);
        }
        public void OnMove(Vector2 context)
        {
            //Debug.Log(gameObject.ToString() + ", Network Informations : IsLocalPlayer " + IsLocalPlayer);
            //if (motor == null || !IsOwner) return;
            motor.SetMove(context);
        }
        public void OnSprint(bool context)
        {
            //Debug.Log(gameObject.ToString() + ", Network Informations : IsLocalPlayer " + IsLocalPlayer);
            //if (motor == null || !IsOwner) return;
            motor.SetSprint(context);
        }
        public void OnUseWeapon(bool context, bool isRightHand)
        {
            Debug.Log("caca : performed = " + context);
            //Debug.Log(gameObject.ToString() + ", Network Informations : IsLocalPlayer " + IsLocalPlayer);
            //if (motor == null || !IsOwner) return;

            combat.UseWeapon(context, isRightHand);
        }
    }
}