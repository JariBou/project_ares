using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed, 60);
        
        SceneManager.LoadScene((int)SceneBuildIndex.SelectionMenu);
    }
}
