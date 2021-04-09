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
        // StartCoroutine("SpawnAnimals");
    }

    private IEnumerator SpawnAnimals()
    {
        // wait until map loads
        yield return new WaitForSeconds(10);

        // will repeat every 30s to gen new animals
        while (true) {
            Vector2d playerLongAndLat = map.WorldToGeoPosition(transform.position);
            GameControl.control.SpawnAnimals((float)playerLongAndLat.y, (float)playerLongAndLat.x);
            yield return new WaitForSeconds(30);
        }
    }
}