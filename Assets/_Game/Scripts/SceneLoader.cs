using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    private readonly Dictionary<Scene, string> _sceneNames = new Dictionary<Scene, string>()
    {
        {Scene.Gameplay, nameof(Scene.Gameplay)} 
    };
    
    public void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(_sceneNames[scene]);
    }
}

public enum Scene
{ 
    Gameplay
}
