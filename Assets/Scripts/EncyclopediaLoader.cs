using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class EncyclopediaLoader : MonoBehaviour
{
    private AnimalDataHandler animalDataHandler;
    public GameObject listItemPrefab;
    public GameObject ContentList;
    public GameObject MainPanel;
    public GameObject card;
    // Start is called before the first frame update
    void Start()
    {
        animalDataHandler = new AnimalDataHandler();    //probably shouldnt instantiate different data handlers per scene 
        animalDataHandler.LoadAnimalsIntoList();
        // animalDataHandler.PrintAnimalList(); 
        LoadAnimalsToEncyclopedia();
        MainPanel.GetComponent<Button>().onClick.AddListener(()=>{card.active = false;});   //for hiding card 

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadAnimalsToEncyclopedia() {

        foreach(Animal animal in animalDataHandler.animalList.Animals) {
            GameObject listItem = Instantiate (listItemPrefab, ContentList.transform);  //button prefab
            RawImage raw = listItem.transform.Find("ImagePanel").Find("Raw").gameObject.GetComponent<RawImage>();   //finding image gameobject
            StartCoroutine(setButtonImage(animal.ImageURL, raw, listItem, animal));     //using coroutines for loading images. might want to use async ops in the future
            Text ButtonText = listItem.transform.Find("Name").gameObject.GetComponent<Text>();  
            ButtonText.text = animal.CommonName;
        }
    }

    IEnumerator setButtonImage(string url, RawImage raw, GameObject button, Animal animal)
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
                raw.texture = (Texture)((DownloadHandlerTexture) request.downloadHandler).texture;  //sets texture for image in the list
                button.SetActive(true);
                button.GetComponent<Button>().onClick.AddListener(()=>cardPopup(raw, animal.CommonName, animal.Description));   //button has to be active to add listener. WHY!?!?!

            }
        }
    }

    /*
    function to have popup for more info
    basically just hides and unhides the card in front of the list
    */
    public void cardPopup(RawImage raw, string name, string desc) {
        Transform inner = card.transform.Find("Inner");
        RawImage imageSpot = inner.Find("ImagePanel").Find("Raw").gameObject.GetComponent<RawImage>();
        imageSpot.texture = raw.texture;
        
        Text nameSpot = inner.Find("NamePanel").Find("Name").gameObject.GetComponent<Text>();
        nameSpot.text = name;
        
        Text descSpot = inner.Find("DecriptionPanel").Find("Description").gameObject.GetComponent<Text>();
        descSpot.text = desc;

        card.active = !(card.active);
    }
}
