using System;
using UnityEngine.InputSystem;

namespace ProjectAres.Core
{
    [Serializable]
    public class PlayerConfiguration
    {
        public PlayerConfiguration(PlayerInput pi)
        {
            PlayerIndex = pi.playerIndex;
            Input = pi;
        }
    
        public PlayerConfiguration(PlayerInput pi, int selectionIndex)
        {
            PlayerIndex = pi.playerIndex;
            Input = pi;
            SelectionIndex = selectionIndex;
        }

        public void ChangeInput(PlayerInput pi)
        {
            Input = pi;
        }

        public PlayerInput Input { get; private set; }
        public int PlayerIndex { get; private set; }
        public bool IsReady { get; set; }
        public int SelectionIndex { get; set; }
        public InputDevice[] InputDevices { get; set; }
    
        // Maybe get a reference to the SO of the character here when pressing confirm idk, or just have another static class with all SOs

    }
}