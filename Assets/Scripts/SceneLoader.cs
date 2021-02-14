using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenuScene()
    {
        Application.LoadLevel(0);
    }
    
    public void LoadSettingsScene() 
    {
        Application.LoadLevel(1);
    }
}
