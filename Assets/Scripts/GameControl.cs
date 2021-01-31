using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
  //*************************************
    public static GameControl control;//*
  //*************************************
    // ^^^^ This is the object that stores ALL of the players dynamic data
    //used for upgrades etc

    public int maxUses;//these are the saved player values
    public float moveSpeed;
    public float wateringTime;
    public int waterAllUses;
    public AudioMixer mixer;

    void Awake()//if DDOL object already exists, destroy yourself bro :)
    {
        if(control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()//set up music and all player prefs etc etc
    {
        mixer.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol", 0));
        mixer.SetFloat("fxVol", PlayerPrefs.GetFloat("fxVol", 0));
    }

    public void Save()//saves player data to a file (data.swag)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.swag";
        FileStream stream = new FileStream(path, FileMode.Create);
        GameData data = new GameData();

        data.maxUses = maxUses;
        data.moveSpeed = moveSpeed;
        data.wateringTime = wateringTime;
        data.waterAllUses = waterAllUses;

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public void Load()//loads player data from file
    {
        string path = Application.persistentDataPath + "/data.swag";
        if (File.Exists(path))//if file is found input data into the local DDOL "control" object
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;

            stream.Close();

            maxUses = data.maxUses;
            moveSpeed = data.moveSpeed;
            wateringTime = data.wateringTime;
            waterAllUses = data.waterAllUses;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }
    }

    public void getLocation()//this starts the enumerator that allows the retrieval of location
    {
        StartCoroutine(EnumLocation());
    }

    private IEnumerator EnumLocation()//retrieves user latitude and longitude and prints it, lots of functionality can be easily added here
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("enable pls");
            yield break;
        }

        // Start service before querying location
        Input.location.Start();

        yield return new WaitForSeconds(1);//you need this wait here or it crashes bc it doesnt start fast enough
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            Debug.Log("[" + Time.time + "] Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }
    public void musicVolume(float volume)
    {
        mixer.SetFloat("musicVol", volume);
        PlayerPrefs.SetFloat("musicVol", volume);
    }

    public void effectsVolume(float volume)
    {
        mixer.SetFloat("fxVol", volume);
        PlayerPrefs.SetFloat("fxVol", volume);
    }

    public void goToNewScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}

[Serializable]
class GameData //THIS HOLDS RAW PLAYER DATA
{
    public float moveSpeed;
    public float wateringTime;
    public int maxUses;
    public int waterAllUses;
}
