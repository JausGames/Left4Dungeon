using UnityEditor;
using UnityEngine;
using L4P.Gameplay.Player.Animations;

namespace L4P.Gameplay.Player.TopDown
{
    [RequireComponent(typeof(PlayerAnimatorController))]
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        [Header("Status")]
        private bool sprinting;

        private float rotationSpeed = 20f;
         Vector2 move;

        [Header("Characteristics")]
        [SerializeField] private float acceleration = .5f;
        [SerializeField] private float sprintMultiplier = 1.5f;
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float attackMoveSpeedMultiplier = .1f;
        [SerializeField] private float mobileAttackMoveSpeedMultiplier = .5f;

        [Header("Space")]
        [Header("Components")]
        [SerializeField] protected Rigidbody body;
        [SerializeField] protected Transform cameraContainer;
        [SerializeField] public PlayerAnimatorController animator;
        [SerializeField] public PlayerAnimatorEvent animatorEvent;
        private bool isAttacking;
        private bool isMobileAttacking;

        public Vector2 Move { get => move; set => move = value; }
        public float Acceleration { get => acceleration; set => acceleration = value; }
        public Rigidbody Body { get => body; set => body = value; }
        public float AttackMoveSpeedMultiplier { get => attackMoveSpeedMultiplier; set => attackMoveSpeedMultiplier = value; }
        public float SprintMultiplier { get => sprintMultiplier; set => sprintMultiplier = value; }
        public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
        public bool Sprinting { get => sprinting; set => sprinting = value; }

        private void Awake()
        {
            body = GetComponentInChildren<Rigidbody>();
            animator = GetComponent<PlayerAnimatorController>(); 
            animatorEvent = GetComponentInChildren<PlayerAnimatorEvent>(); 
            animatorEvent.IsAttacking.AddListener(delegate { isAttacking = true; });
            animatorEvent.IsMobileAttacking.AddListener(delegate { isMobileAttacking = true; });
            animatorEvent.IsNotAttacking.AddListener(delegate { isAttacking = false; });
            animatorEvent.IsMobileNotAttacking.AddListener(delegate { isMobileAttacking = false; });
        }
        private void FixedUpdate()
        {
            UpdatePlayerPosition();
            Turning();
            Animate();
        }

        private void Animate()
        {
            //var direction = -cameraContainer.forward * move.x + cameraContainer.right * move.y;
            var direction = body.velocity  / maxSpeed;
            direction.y = 0f;
            var moveY = Vector3.Dot(direction, body.transform.forward);
            var moveX = Vector3.Dot(direction, body.transform.right);
            animator.SetMove(new Vector2(moveX, moveY));
        }

        public void UpdatePlayerPosition()
        {
            var targetSpeed = maxSpeed;
            if (isAttacking)
            {
                targetSpeed *= attackMoveSpeedMultiplier;
            }
            else if (isMobileAttacking)
            {
                targetSpeed *= mobileAttackMoveSpeedMultiplier;
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
            var desiredSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration);

            body.velocity = direction * desiredSpeed;
        }
        void Turning()
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera.
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Create a RaycastHit variable to store information about what was hit by the ray.
            RaycastHit floorHit;

            var targetSpeed = rotationSpeed;
            if (isAttacking || isMobileAttacking)
            {
                targetSpeed *= attackMoveSpeedMultiplier;
            }

            // Perform the raycast and if it hits something on the floor layer...
            if (Physics.Raycast(camRay, out floorHit))
            {
                // Create a vector from the player to the point on the floor the raycast from the mouse hit.
                Vector3 playerToMouse = floorHit.point - body.transform.position;

                // Ensure the vector is entirely along the floor plane.
                playerToMouse.y = 0f;

                // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
                Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

                // Set the player's rotation to this new rotation.
                body.transform.rotation = Quaternion.RotateTowards(body.transform.rotation, newRotatation, targetSpeed);
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