using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFloat : MonoBehaviour
{
    public float floatSpan = 2f;
    public float speed = 1f;

    private float startY;
    private float startX;
    
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
        startX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float newY = (float)(startY + Mathf.Sin(Time.time * speed) * floatSpan / 2.0);
        transform.position = new Vector2(startX, newY);
    }
}