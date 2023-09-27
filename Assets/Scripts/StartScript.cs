using Core;
using ProjectAres.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectAres
{
    public class StartScript : MonoBehaviour
    {
        private void Start()
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.Windowed, 60);
        
            SceneManager.LoadScene((int)SceneBuildIndex.SelectionMenu);
        }
    }
}
