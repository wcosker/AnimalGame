using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class QueryForAnimalData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAnimalData() {
        int lat = 50;
        int lng = 50;

        string uri = "https://senior-project-backend-server.herokuapp.com/api/get-animals?"
            + "lat=" + lat  
            + "&long=" + lng;

        StartCoroutine(HandleAnimalRequest(uri));
    }

    private IEnumerator HandleAnimalRequest(string uri) {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            } 
            else {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
}
