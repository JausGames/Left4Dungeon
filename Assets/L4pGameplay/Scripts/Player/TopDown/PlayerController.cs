using UnityEditor;
using UnityEngine;

namespace L4P.Gameplay.Player.TopDown
{
    [RequireComponent(typeof(Rigidbody), typeof(Animation.PlayerAnimatorController))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [Header("States")]
        private bool sprinting;

        [Header("Inputs")]
        Vector2 move;

        [Header("Stats")]
        private float acceleration = 2f;
        private float attackMoveSpeedMultiplier = .2f;
        private float sprintMultiplier = 1.5f;
        private float maxSpeed = 10f;

        [Header("Components")]
        [SerializeField] protected Rigidbody body;
        [SerializeField] protected Transform cameraContainer;

        public Vector2 Move { get => move; set => move = value; }
        public float Acceleration { get => acceleration; set => acceleration = value; }
        public Rigidbody Body { get => body; set => body = value; }
        public float AttackMoveSpeedMultiplier { get => attackMoveSpeedMultiplier; set => attackMoveSpeedMultiplier = value; }
        public float SprintMultiplier { get => sprintMultiplier; set => sprintMultiplier = value; }
        public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
        public bool Sprinting { get => sprinting; set => sprinting = value; }

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }
        private void Update()
        {
            UpdatePlayerPosition();
            Turning();
        }

        public void UpdatePlayerPosition()
        {
            //find if attacking
            var attacking = false;

            var targetSpeed = maxSpeed;
            if (attacking)
            {
                targetSpeed *= attackMoveSpeedMultiplier;
            }
            else if (Sprinting)
            {
                targetSpeed *= sprintMultiplier;
            }
            var direction = -cameraContainer.forward * move.x + cameraContainer.right * move.y;
            var directionLength = direction.magnitude;
            if (directionLength > 1)
            {
                direction /= directionLength;
            }

            var currentSpeed = body.velocity.magnitude;
            var desiredSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration);

            body.velocity = direction * desiredSpeed;
        }
        void Turning()
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(camRay, out floorHit))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                transform.rotation = newRotatation;
                //playerRigidbody.MoveRotation (newRotatation);
            }
        }

        public void SetMove(Vector2 move)
        {
            this.move = move;
        }

        public void SetSprint(bool sprinting)
        {
            this.sprinting = sprinting;
        }
    }
}