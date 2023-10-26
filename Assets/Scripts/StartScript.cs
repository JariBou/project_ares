using ProjectAres.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectAres
{
    public class StartScript : MonoBehaviour
    {
        private void Start()
        {
            Screen.SetResolution(1920, 1080, FullScreenMode.Windowed, new RefreshRate { numerator = 60, denominator = 1 });
            
        
            SceneManager.LoadScene((int)SceneBuildIndex.SelectionMenu);
        }
    }
}
