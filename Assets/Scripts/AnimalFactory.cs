using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFactory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("spawnAnimals");
    }

    //spawns animals as sphere on board
    //can make it so it constantly does it but I think that's all gonna be backend?
    IEnumerator spawnAnimals()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Spawning animals from animal factory");
        GameControl.control.GetAnimals(transform.position);
    }
}
