using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EncyclopediaLoader : MonoBehaviour
{
    private AnimalDataHandler animalDataHandler;
    public GameObject listItemPrefab;
    public GameObject cardPrefab;
    public GameObject ContentList;
    public GameObject MainPanel;
    private GameObject card;
    // Start is called before the first frame update
    void Start()
    {
        animalDataHandler = new AnimalDataHandler();
        animalDataHandler.LoadAnimalsIntoList();
        // animalDataHandler.PrintAnimalList();
        LoadAnimalsToEncyclopedia();
        MainPanel.GetComponent<Button>().onClick.AddListener(()=>Destroy(card));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadAnimalsToEncyclopedia() {

        foreach(Animal animal in animalDataHandler.animalList.Animals) {
            GameObject listItem = Instantiate (listItemPrefab, ContentList.transform);
            Image imageSpot = listItem.transform.Find("ImagePanel").gameObject.GetComponent<Image>();
            StartCoroutine(setImage(animal.ImageURL, imageSpot)); //balanced parens CAS
            Text ButtonText = GameObject.Find("Name").GetComponent<Text>();
            ButtonText.text = "test";
            listItem.GetComponent<Button>().onClick.AddListener(()=>cardPopup(imageSpot.sprite, animal.CommonName, animal.Description));
        }
    }

    IEnumerator setImage(string url, Image card) {
        WWW www = new WWW(url);
        yield return www;

        // calling this function with StartCoroutine solves the problem
        Debug.Log("Why on earh is this never called?");
        card.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        www.Dispose();
        www = null;
    }

    public void cardPopup(Sprite image, string name, string desc) {

        foreach(Animal animal in animalDataHandler.animalList.Animals) {
            card = Instantiate (cardPrefab, MainPanel.transform);
            Transform inner = card.transform.Find("Inner");
            Image imageSpot = inner.Find("ImagePanel").gameObject.GetComponent<Image>();
            imageSpot.sprite = image;
            
            Text nameSpot = inner.Find("NamePanel").Find("Name").gameObject.GetComponent<Text>();
            nameSpot.text = name;
            
            Text descSpot = inner.Find("DecriptionPanel").Find("Description").gameObject.GetComponent<Text>();
            descSpot.text = desc;
        }
    }
}
