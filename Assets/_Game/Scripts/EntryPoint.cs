using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    private SceneLoader _sceneLoader;

    private void Start()
    {
        _sceneLoader.LoadScene(Scene.Gameplay);
    }
}
