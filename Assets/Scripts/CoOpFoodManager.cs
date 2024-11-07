using UnityEngine;
using System.Collections.Generic;

public class CoOpFoodManager : MonoBehaviour
{
    public GameObject gainerPrefab;
    public GameObject burnerPrefab;
    public float spawnInterval = 5f;

    private float xMin = -7f, xMax = 7f;
    private float yMin = -3.5f, yMax = 3.5f;

    private List<CoOpSnakeController> snakeControllers = new List<CoOpSnakeController>(); // Store references to both snakes

    private void Start()
    {
        // Find both snakes in the scene
        CoOpSnakeController[] foundSnakes = FindObjectsOfType<CoOpSnakeController>();
        snakeControllers.AddRange(foundSnakes);

        InvokeRepeating(nameof(SpawnFood), 2f, spawnInterval); // Start spawning food
    }

    private void SpawnFood()
    {
        if (snakeControllers.Count == 0) return;

        GameObject foodPrefab;

        // Determine the food type based on the segment count of both snake
        bool spawnOnlyGainer = true;
        foreach (var snake in snakeControllers)
        {
            if (snake.SegmentCount >= 2)
            {
                spawnOnlyGainer = false;
                break;
            }
        }

        if (spawnOnlyGainer)
        {
            // Spawn Gainer if both snakes have less than 2 segments
            foodPrefab = gainerPrefab;
        }
        else
        {
            // Randomly choose between Gainer and Burner if either snake has 2 or more segments
            foodPrefab = Random.value > 0.5f ? gainerPrefab : burnerPrefab;
        }

        // Generate a random spawn position within the defined range
        Vector2 spawnPosition = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));

        // Instantiate the chosen food at the spawn position
        GameObject foodInstance = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);

        // Destroy the food after 7 seconds if not consumed
        Destroy(foodInstance, 7f);
    }
}
