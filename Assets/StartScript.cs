using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{
    private void Start()
    {
        Screen.SetResolution(720, 480, FullScreenMode.Windowed, 60);
        SceneManager.LoadScene(1);
    }
}
