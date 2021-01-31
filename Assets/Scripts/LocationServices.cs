using UnityEngine;
using System.Collections;

public class LocationServices : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3);
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("enable pls");
            yield break;
        }
        Debug.Log("Status: " + Input.location.status);
        

        // Start service before querying location
        Input.location.Start();

        yield return new WaitForSeconds(3);
        Debug.Log("Status: " + Input.location.status);
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            Debug.Log("[" + Time.time + "] Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude);
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }
}