using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using UnityEngine.Networking;
using SimpleJSON;
using System.Runtime;

public class AnimalFactory : MonoBehaviour
{
    private AbstractMap map;
    [SerializeField]
    private GameObject animal;
    void Awake()
    {
        map = FindObjectOfType<AbstractMap>();
        StartCoroutine("getLocationOfPin");
    }
     IEnumerator getLocationOfPin()
    {
        yield return new WaitForSeconds(8);
        string uri = "https://senior-project-backend-server.herokuapp.com/api/get-animals?"
    + "lat=" + map.WorldToGeoPosition(transform.position).x
    + "&long=" + map.WorldToGeoPosition(transform.position).y;
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
                var data = JSON.Parse(request.downloadHandler.text);

                var rng = new System.Random();
                var randomAnimal = data[rng.Next(data.Count)];
                Debug.Log(request.downloadHandler.text);
                Debug.Log(randomAnimal["Animal"]);
                SpawnAnimal(randomAnimal["Animal"].ToString().Replace("\"",""));
            }
        }
    }

    private void SpawnAnimal(string name)
    {
        animal.GetComponent<Animal>().name = name;
        Debug.Log("Instantiating...");
        float x = transform.position.x + Random.Range(-3f, 3f);
        float y = transform.position.y;
        float z = transform.position.z + Random.Range(-3f, 3f);
        Instantiate(animal,new Vector3(x,y,z), Quaternion.identity);
    }
}
