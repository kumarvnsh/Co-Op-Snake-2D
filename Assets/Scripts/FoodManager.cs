using UnityEngine;

public class FoodManager : MonoBehaviour
{
    public GameObject gainerPrefab;
    public GameObject burnerPrefab;
    public float spawnInterval = 5f;

    private float xMin = -7f, xMax = 7f;
    private float yMin = -3.5f, yMax = 3.5f;

    private SnakeController snakeController;
    

    private void Start()
    {
        snakeController = FindObjectOfType<SnakeController>(); // Find the SnakeController in the scene
        InvokeRepeating(nameof(SpawnFood), 2f, spawnInterval); // Start spawning food
    }

    private void SpawnFood()
    {
        if (snakeController == null) return;

        GameObject foodPrefab;

        // Determine which food to spawn based on the segment count
        if (snakeController.SegmentCount < 2)
        {
            // If snake has less than 2 segments, only spawn Gainer
            foodPrefab = gainerPrefab;
        }
        else
        {
            // If snake has 2 or more segments, randomly choose between Gainer and Burner
            foodPrefab = Random.value > 0.5f ? gainerPrefab : burnerPrefab;
        }

        // Generate a random spawn position within the defined range
        Vector2 spawnPosition = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));

        // Instantiate the chosen food at the spawn position
        GameObject foodInstance = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);

        // Destroy the food after 5 seconds if not consumed
        Destroy(foodInstance, 7f);
    }
}
