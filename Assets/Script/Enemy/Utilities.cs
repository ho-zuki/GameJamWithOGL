using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities{
    public float screenWidth, screenHeight;
    public float getDistance(GameObject obj1, GameObject obj2)
    {
        float x1 = obj1.transform.position.x;
        float y1 = obj1.transform.position.y;
        float x2 = obj2.transform.position.x;
        float y2 = obj2.transform.position.y;
        return (Mathf.Sqrt(((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2))));
    }
    public float getScreenWidth()
    {
        return screenWidth;
    }
    public float getScreenHeight()
    {
        return screenHeight;
    }
    
}
