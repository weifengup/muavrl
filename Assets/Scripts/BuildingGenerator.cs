using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour
{
    public GameObject buildingPrefab;
    public int numberOfBuildings;

    private GameObject floor;

    private void Start()
    {
        floor = GameObject.Find("Floor"); // Replace "Floor" with the actual name of your floor object.
        GenerateBuildings();
    }

    void GenerateBuildings()
    {
        if (floor == null)
        {
            Debug.LogError("Floor object not found. Make sure to set the correct name of your floor object.");
            return;
        }

        Renderer floorRenderer = floor.GetComponent<Renderer>();
        Vector3 floorSize = floorRenderer.bounds.size;

        for (int i = 0; i < numberOfBuildings; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-floorSize.x / 2, floorSize.x / 2),
                0,
                Random.Range(-floorSize.z / 2, floorSize.z / 2)
            );

            GameObject newBuilding = Instantiate(buildingPrefab, randomPosition, Quaternion.identity);
            Debug.Log("???");
            // You can add any additional building customization here.

            if (!IsOverlapping(newBuilding, floorSize))
            {
                // Building is not overlapping with existing ones and is within the floor boundaries, keep it.
            }
            else
            {
                // Building is overlapping or outside the floor boundaries, destroy it.
                Destroy(newBuilding);
                i--; // Decrement to retry this position.
            }
        }
    }

    bool IsOverlapping(GameObject building, Vector3 floorSize)
    {
        Collider buildingCollider = building.GetComponent<Collider>();
        Collider[] colliders = Physics.OverlapBox(
            buildingCollider.bounds.center,
            buildingCollider.bounds.extents,
            building.transform.rotation
        );

        // Check for overlapping with other buildings (excluding itself).
        foreach (var collider in colliders)
        {
            if (collider.gameObject != building)
            {
                return true; // Overlapping with another object.
            }
        }

        // Check if the building is within the floor boundaries.
        if (building.transform.position.x - buildingCollider.bounds.extents.x < -floorSize.x / 2 ||
            building.transform.position.x + buildingCollider.bounds.extents.x > floorSize.x / 2 ||
            building.transform.position.z - buildingCollider.bounds.extents.z < -floorSize.z / 2 ||
            building.transform.position.z + buildingCollider.bounds.extents.z > floorSize.z / 2)
        {
            return true; // Building is outside the floor boundaries.
        }

        return false; // No overlap.
    }
}
