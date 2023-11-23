using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGenerator : MonoBehaviour
{
    public GameObject floor; // 地面对象

    void Start()
    {
        MoveRandomCube();
    }

    void MoveRandomCube()
    {
        Bounds floorBounds = GetFloorBounds();

        float x, y, z;

        x = Random.Range(floorBounds.min.x, floorBounds.max.x);
        y = transform.localScale.y / 2; // 使 Cube 位于地面上方
        y = Random.Range(transform.localScale.y / 2, 5);
        z = Random.Range(floorBounds.min.z, floorBounds.max.z);

        Vector3 newPosition = new Vector3(x, y, z);

        transform.position = newPosition; // 直接更改脚本所附加的物体的位置
    }

    Bounds GetFloorBounds()
    {
        Renderer floorRenderer = floor.GetComponent<Renderer>();
        Bounds bounds = floorRenderer.bounds;
        return bounds;
    }
    void update(){

    }
    
}
