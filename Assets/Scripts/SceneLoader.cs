using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void LoadMainMenuScene()
    {
        Application.LoadLevel(0);
    }
    
    public void LoadMapScene() 
    {
        Application.LoadLevel(1);
    }

    public void LoadSettingsScene() 
    {
        Application.LoadLevel(2);
    }

    public void LoadEncyclopediaScene() 
    {
        Application.LoadLevel(3);
    }
    
    public void LoadCatchScene() 
    {
        Application.LoadLevel(4);
    }

}
