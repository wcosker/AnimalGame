using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SimpleJSON;


public class GameControl : MonoBehaviour
{
    public static GameControl control;

    public AnimalDataHandler animalDataHandler;

    //these are the saved player values
    public float CurrLat;
    public float CurrLong;
    public AudioMixer mixer;

    /**
    * if DDOL object already exists, destroy yourself bro :)
    */
    void Awake()
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

    /**
    * set up music and all player prefs etc etc
    */
    void Start()
    {
        mixer.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol", 0));
        mixer.SetFloat("fxVol", PlayerPrefs.GetFloat("fxVol", 0));

        // populates memory with already collected animals (stored on users device)
        animalDataHandler.LoadAnimalsIntoList();
    }

    /**
    * saves player data to a file (data.swag)
    */
    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.swag";
        FileStream stream = new FileStream(path, FileMode.Create);
        GameData data = new GameData();

        data.CurrLat = CurrLat;
        data.CurrLong = CurrLong;

        formatter.Serialize(stream, data);
        stream.Close();
    }


    /**
    * loads player data from file
    */
    public void Load()
    {
        string path = Application.persistentDataPath + "/data.swag";
        
        //if file is found input data into the local DDOL "control" object
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;

            stream.Close();

            CurrLat = data.CurrLat;
            CurrLong = data.CurrLong;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }
    }

    public void GetLocation()
    {
        StartCoroutine(GetLocationCoroutine());
    }

    public void GetAnimals(Vector3 spawnerPos)
    {
        StartCoroutine(GetAnimalsCoroutine(spawnerPos));
    }

    private IEnumerator GetAnimalsCoroutine(Vector3 spawnerPos)
    {
        string uri = "https://senior-project-backend-server.herokuapp.com/api/get-animals?"
            + "lat=" + CurrLat
            + "&long=" + CurrLong;
        Debug.Log("Link: " + uri);

        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                animalDataHandler.BuildRandomAnimalAndDisplayOnMap(
                    request.downloadHandler.text, 
                    spawnerPos
                );
            }
        }
    }

    /**
    * retrieves user latitude and longitude and prints it, lots of functionality can be easily added here
    */
    private IEnumerator GetLocationCoroutine()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("enable pls");
            yield break;
        }

        // Start service before querying location
        Input.location.Start();

        //you need this wait here or it crashes bc it doesnt start fast enough
        yield return new WaitForSeconds(1);
        
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
        else//make call and get animals
        {
            // Access granted and location value could be retrieved
            Debug.Log("[" + Time.time + "] Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude);
            CurrLat = Input.location.lastData.latitude;
            CurrLong = Input.location.lastData.longitude;
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
class GameData
{
    public float CurrLat;
    public float CurrLong;
}
