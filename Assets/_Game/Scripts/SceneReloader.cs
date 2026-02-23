using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    private void Awake()
    {
        // DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.UnloadSceneAsync(scene);
            SceneManager.LoadScene(scene.name);
        }
    }
}