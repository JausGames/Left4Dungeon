using UnityEngine;
using UnityEngine.InputSystem;
using L4P.Gameplay.Player;
using static UnityEngine.InputSystem.InputAction;
using L4P.Gameplay.Player.TopDown;

namespace L4P.Gameplay.Player.Controls
{

    [RequireComponent(typeof(Controls.InputManager))]
    public class PlayerInputs : MonoBehaviour
    {
        [SerializeField] IPlayerController motor = null;
        [SerializeField] IPlayerCombat combat = null;

        [SerializeField] bool alternativeUse = false;

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

            //InputManager.Controls.Gameplay.UseLeft.performed += _ => OnUseWeapon(true, alternativeUse ? AttackType.Alt : AttackType.Left);
            //InputManager.Controls.Gameplay.UseLeft.canceled += _ => OnUseWeapon(false, alternativeUse ? AttackType.Alt : AttackType.Left);

            InputManager.Controls.Gameplay.UseLeft.performed += _ => OnUseWeapon(true, AttackType.Left);
            InputManager.Controls.Gameplay.UseLeft.canceled += _ => OnUseWeapon(false, AttackType.Left);

            InputManager.Controls.Gameplay.UseRight.performed += _ => OnUseWeapon(true, alternativeUse ? AttackType.Strong : AttackType.Right);
            InputManager.Controls.Gameplay.UseRight.canceled += _ => OnUseWeapon(false, alternativeUse ? AttackType.Strong : AttackType.Right);

            InputManager.Controls.Gameplay.KeyboardAlternative.performed += _ => alternativeUse = true;
            InputManager.Controls.Gameplay.KeyboardAlternative.canceled += _ => alternativeUse = false;
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
        public void OnUseWeapon(bool context, AttackType type)
        {

            Debug.Log("caca : performed = " + context);
            //Debug.Log(gameObject.ToString() + ", Network Informations : IsLocalPlayer " + IsLocalPlayer);
            //if (motor == null || !IsOwner) return;

            combat.UseWeapon(context, type);
        }
    }
}