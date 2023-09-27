using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class TickManager : MonoBehaviour
    {
        public static event Action PreUpdate;
        public static event Action FrameUpdate;
        public static event Action PostUpdate;

        [SerializeField] private int framerate = 60;


        private void Awake()
        {
            Application.targetFrameRate = framerate;
            Physics2D.simulationMode = SimulationMode2D.Update;
            DontDestroyOnLoad(gameObject);
        }

        private static void OnPreUpdate()
        {
            PreUpdate?.Invoke();
        }

        private static void OnFrameUpdate()
        {
            FrameUpdate?.Invoke();
        }

        private static void OnPostUpdate()
        {
            PostUpdate?.Invoke();
        }

        


        private void Update()
        {
            // Apparently this works, all pre updates are called BEFORE frame update
            OnPreUpdate();
            OnFrameUpdate();
        }

        private void LateUpdate()
        {
            OnPostUpdate();
        }
    }
}