using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace L4P.Gameplay.Player.Controls
{
    public class InputManager : MonoBehaviour
    {
        private static readonly IDictionary<string, int> mapStates = new Dictionary<string, int>();

        private static PlayerControls controls;

        public static PlayerControls Controls
        {
            get
            {
                if (controls != null) { return controls; }
                return controls = new PlayerControls();
            }
        }

        private void Awake()
        {
            if (controls != null) { return; }
            controls = new PlayerControls();
        }

        private void OnEnable() => Controls.Enable();
        private void OnDisable() => Controls.Disable();
        private void OnDestroy() => controls = null;

        public static void Add(string mapName)
        {
            mapStates.TryGetValue(mapName, out int value);
            mapStates[mapName] = value + 1;
            UpdateMapState(mapName);
        }

        public static void Remove(string mapName)
        {
            mapStates.TryGetValue(mapName, out int value);
            mapStates[mapName] = Mathf.Max(value - 1, 0);
            UpdateMapState(mapName);
        }

        private static void UpdateMapState(string mapName)
        {
            int value = mapStates[mapName];

            if (value > 0)
            {
                Controls.asset.FindActionMap(mapName).Enable();

                return;
            }

            Controls.asset.FindActionMap(mapName).Enable();
        }
    }
}