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
            var direction = new Vector3(move.x, 0f, move.y);
            var directionLength = direction.magnitude;
            if (directionLength > 1)
            {
                direction /= directionLength;
            }

            var currentSpeed = body.velocity.magnitude;
            var desiredSpeed = Mathf.Lerp(currentSpeed, targetSpeed, acceleration);

            body.velocity = direction * desiredSpeed;
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