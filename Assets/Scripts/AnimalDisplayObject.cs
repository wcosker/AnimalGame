using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class AnimalDisplayObject : MonoBehaviour
{
    [SerializeField]
    private GameObject part;
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
        GameObject go = Instantiate(part, transform.position,transform.rotation);
        Destroy(go, 2);
        Destroy(this.gameObject);
    }
}
