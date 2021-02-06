using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AnimalDisplay : MonoBehaviour 
{
    public Text CommonName;
    public Text Description;
    public RawImage RawImage;

    public IEnumerator SetImage(string url)
    {
        using(UnityWebRequest request = UnityWebRequestTexture.GetTexture(url)) 
        {
            yield return request.SendWebRequest();

            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else 
            {
                this.RawImage.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            }
        }
    }
}