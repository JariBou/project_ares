using System;
using UnityEngine;

namespace ProjectAres.Core
{
    public class TickManager : MonoBehaviour
    {
        public static event Action PreUpdate;
        public static event Action FrameUpdate;
        public static event Action PostUpdate;

        [SerializeField] private int _framerate = 60;


        private void Awake()
        {
            Application.targetFrameRate = _framerate;
            Physics2D.simulationMode = SimulationMode2D.Update;
            DontDestroyOnLoad(gameObject);
        }

        private static void OnPreUpdate()
        {
            Debug.Log("Calling All PreUpdates");
            PreUpdate?.Invoke();
        }

        private static void OnFrameUpdate()
        {
            Debug.Log("Calling All FrameUpdates");
            FrameUpdate?.Invoke();
        }

        private static void OnPostUpdate()
        {
            Debug.Log("Calling All PostUpdates");
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
    
    public enum SceneBuildIndex
    {
        StartScene = 0,
        SelectionMenu = 1,
        Scene = 2,
    }
}
