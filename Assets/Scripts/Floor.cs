using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Bounds GetFloorBounds()
    {
        Renderer floorRenderer = this.GetComponent<Renderer>();
        Bounds bounds = floorRenderer.bounds;
        return bounds;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
