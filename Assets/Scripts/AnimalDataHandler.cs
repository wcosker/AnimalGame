using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using Mapbox.Unity.Map;
using Mapbox.Utils;


/*class AnimalDisplayObject : MonoBehaviour
{
    public string CommonName;
    public Animal animal;
    
    public AnimalDataHandler animalDataHandler;
    public AnimalList animalList;

    void Start()
    {
        animalDataHandler = GameObject.Find("GameControl").GetComponent<AnimalDataHandler>();
        animalDataHandler.PrintAnimalList();
        animalList = animalDataHandler.animalList;
    }

    void OnMouseDown()
    {
        Debug.Log(CommonName + " has been caught and added to Encyclopedia!");
        animalList.Animals.Add(animal);
        animalDataHandler.SaveAnimalList();
        Destroy(this.gameObject);
    }
}*/

[Serializable]
public class Animal
{
    public string CommonName;
    public string Description;
    public string ImageURL;
}

[Serializable]
public class AnimalList
{
    public List<Animal> Animals;

    public AnimalList() {
        Animals = new List<Animal>();
    }
}

public class AnimalDataHandler : MonoBehaviour
{
    public GameObject zebra;
    public AnimalList animalList = new AnimalList();
    private AbstractMap map;

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

            // PrintAnimalList();
        }
    }

    public void PrintAnimalList() {
        Debug.Log("Printing animal list...");
        Debug.Log("Animal List Size: " + animalList.Animals.Count);

        foreach(Animal animal in animalList.Animals) {
            Debug.Log("CommonName: " + animal.CommonName);
            Debug.Log("Description: " + animal.Description);
            Debug.Log("ImageURL: " + animal.ImageURL);
        }
    }

    public void SpawnAnimalAtPosition(string animalData) {
        StartCoroutine(SpawnAnimalAtPositionCoroutine(animalData));
    }

    private IEnumerator SpawnAnimalAtPositionCoroutine(string animalData) {
        var data = JSON.Parse(animalData)[0];

        // pick a random animal from the spawner to spawn
        var rng = new System.Random();
        var randomAnimalData = data["Animals"][rng.Next(data["Animals"].Count)];
        
        Animal animal = new Animal();
        animal.CommonName = randomAnimalData["Common_Name"];

        Debug.Log("Animal Selected: " + animal.CommonName);

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

                // spawn display animal
                map = FindObjectOfType<AbstractMap>();

                // need to flip because unity expects LatLong
                Vector2d spawnerLatLong = new Vector2d(data["coordinates"][1], data["coordinates"][0]);

                GameObject newGuy = zebra;
                
                newGuy.GetComponent<AnimalDisplayObject>().CommonName = randomAnimalData["Common_Name"];
                newGuy.GetComponent<AnimalDisplayObject>().animal = animal;
                
                Vector3 spawnerLocalPosition = map.GeoToWorldPosition(spawnerLatLong);
                spawnerLocalPosition = new Vector3(spawnerLocalPosition.x - 6, spawnerLocalPosition.y, spawnerLocalPosition.z - 2);
                Debug.Log("SPAWNER LOCAL " + spawnerLocalPosition);
                newGuy.transform.position = new Vector3(spawnerLocalPosition.x,spawnerLocalPosition.y+1,spawnerLocalPosition.z);
                Instantiate(newGuy);

                // NOTE: we used to add to the list here, but Adam is going to make it so that when you press the sphere
                // it will be added the list
                // Now that we have all of the data, we are going to add to list and save
                // to the device.
                // this.animalList.Animals.Add(animal);
                // SaveAnimalList();
            }
        }
    }

    public void SpawnDemoAnimal()
    {
        Animal animal = new Animal();
        animal.CommonName = "Mottled duck";
        animal.Description = "The mottled duck (Anas fulvigula) or mottled mallard is a medium-sized dabbling duck. It is intermediate in appearance between the female mallard and the American black duck. It is closely related to those species, and is sometimes considered a subspecies of the former, but this is inappropriate (see Systematics below).\nThere are two distinct subspecies of the mottled duck. One subspecies, the Gulf Coast mottled duck (A. f. maculosa), lives on the Gulf of Mexico coast between Alabama and Tamaulipas (Mexico); outside the breeding season, individual birds may venture as far south as Veracruz. The other, the Florida mottled duck (A. f. fulvigula), is resident in central and southern Florida and occasionally strays north to Georgia. The same disjunct distribution pattern was also historically found in the local sandhill cranes. Individuals of both subspecies were introduced into South Carolina in the 1970s and 1980s, where the birds of mixed ancestry have greatly expanded in range, extending through the Atlantic coastal plain of Georgia into northeastern Florida.Along the Gulf of Mexico coast, the mottled duck is one of the most frequently banded waterfowl. This is due in part to the fact that it is mostly non-migratory. Approximately one out of every 20 mottled ducks is banded, making it an extremely prized and sought after bird among hunters.";
        animal.ImageURL = "https://upload.wikimedia.org/wikipedia/commons/1/1e/Florida_Mottled_Ducks.jpg";

        GameObject newGuy = zebra;
        newGuy.GetComponent<AnimalDisplayObject>().CommonName =  "Mottled duck";
        newGuy.GetComponent<AnimalDisplayObject>().animal = animal;

        newGuy.transform.position = new Vector3(UnityEngine.Random.Range(-25.0f, 20.0f), 5 ,UnityEngine.Random.Range(20, 80));
        Instantiate(newGuy);
    }
}
