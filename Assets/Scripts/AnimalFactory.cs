using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class AnimalFactory : MonoBehaviour
{
    private AbstractMap map;
    // Start is called before the first frame update
    void Start()
    {
        map = FindObjectOfType<AbstractMap>();
        StartCoroutine("spawnAnimals");
    }

    //spawns animals as sphere on board
    //can make it so it constantly does it but I think that's all gonna be backend?
    IEnumerator spawnAnimals()
    {
        yield return new WaitForSeconds(10);
        Vector2d latlong = map.WorldToGeoPosition(transform.position);
        GameControl.control.GetAnimals(transform.position, (float)latlong.x, (float)latlong.y);
    }
}