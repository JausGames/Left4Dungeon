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

            InputManager.Controls.Gameplay.UseWeapon.performed += _ => OnUseWeapon(true);
            InputManager.Controls.Gameplay.UseWeapon.canceled += _ => OnUseWeapon(false);
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
        public void OnUseWeapon(bool context)
        {
            //Debug.Log(gameObject.ToString() + ", Network Informations : IsLocalPlayer " + IsLocalPlayer);
            //if (motor == null || !IsOwner) return;
            combat.UseWeapon(context);
        }
    }
}