using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L4P.Gameplay.Player
{
    public interface IPlayerController
    {
        bool Sprinting { get; set; }
        Vector2 Move { get; set; }

        float Acceleration { get; set; }
        float AttackMoveSpeedMultiplier { get; set; }
        float SprintMultiplier { get; set; }
        float MaxSpeed { get; set; }

        Rigidbody Body { get; set; }




        public void UpdatePlayerPosition();

        public void SetMove(Vector2 move);
        public void SetSprint(bool sprinting);
    }
}
