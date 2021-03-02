using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;


[Serializable]
class Animal
{
    public string CommonName;
    public string Description;
    public string ImageURL;
}

[Serializable]
class AnimalList
{
    public List<Animal> Animals;

    public AnimalList() {
        Animals = new List<Animal>();
    }
}


public class AnimalDataHandler : MonoBehaviour
{
    private AnimalList animalList = new AnimalList();

    public void SaveAnimalList() {
        Debug.Log("Saving!");
        PrintAnimalList();
        
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/AnimalData.dat");

        binaryFormatter.Serialize(file, animalList);

        file.Close();
    }

    public void LoadAnimalsIntoList() {
        Debug.Log("Loading!");
        
        if (File.Exists(Application.persistentDataPath + "/AnimalData.dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + "/AnimalData.dat", FileMode.Open);
            
            animalList = (AnimalList)binaryFormatter.Deserialize(fileStream);
            fileStream.Close();

            PrintAnimalList();
        }
    }

    public void PrintAnimalList() {
        foreach(Animal animal in animalList.Animals) {
            Debug.Log("CommonName: " + animal.CommonName);
            Debug.Log("Description: " + animal.Description);
            Debug.Log("ImageURL: " + animal.ImageURL);
        }
    }

    public void BuildAnimalAndAddToList(string animalData) {
        StartCoroutine(BuildAnimalAndAddToListCoroutine(animalData));
    }

    private IEnumerator BuildAnimalAndAddToListCoroutine(string animalData) {
        var data = JSON.Parse(animalData);
        
        // For now, we are just choosing a random animal
        // in the future, this will be done by seeing which we 
        // already have
        var rng = new System.Random();
        var randomAnimalData = data[rng.Next(data.Count)];
        
        Animal animal = new Animal();
        animal.CommonName = randomAnimalData["Animal"];

        // In order to get the more detailed data, we are going to make a 
        // request to the Wikipedia API.
        string url = randomAnimalData["Wiki_Link"];
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                var wikiData = JSON.Parse(request.downloadHandler.text);

                // We need to extract the pageid from the url, as the JSON
                // is formatted with the object name being the pageid
                //     note: 8 comes from length of 'pageids='
                var idIndex = url.IndexOf("pageids=");
                string pageId = url.Substring(idIndex+8);

                // get necessary data from json
                animal.Description = wikiData["query"]["pages"][pageId]["extract"];
                animal.ImageURL = wikiData["query"]["pages"][pageId]["thumbnail"]["source"];

                // Now that we have all of the data, we are going to add to list and save
                // to the device.
                this.animalList.Animals.Add(animal);
                SaveAnimalList();
            }
        }
    }

}
