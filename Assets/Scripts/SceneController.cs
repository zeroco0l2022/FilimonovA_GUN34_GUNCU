using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneController : MonoBehaviour
{
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            OpenGameScene();
        }
    }

    public void OpenMainScene()
    {
        SceneManager.LoadScene(0);
    }

    private void OpenGameScene()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }
} 