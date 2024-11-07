using UnityEngine;
using System.Collections;

public class PowerUpManager : MonoBehaviour
{
    public GameObject shieldPrefab;
    public GameObject scoreBoostPrefab;
    public GameObject speedUpPrefab;

    public float minSpawnInterval = 5f; // Minimum time interval between spawns
    public float maxSpawnInterval = 10f; // Maximum time interval between spawns

    private SnakeController snakeController;

    private void Start()
    {
        snakeController = FindObjectOfType<SnakeController>(); // Find the SnakeController in the scene
        StartCoroutine(SpawnPowerUpRandomly()); // Start spawning power-ups randomly
    }

    private IEnumerator SpawnPowerUpRandomly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

            GameObject powerUpPrefab = ChooseRandomPowerUp();
            Vector2 spawnPosition = new Vector2(Random.Range(-7f, 7f), Random.Range(-3.5f, 3.5f));

            GameObject powerUpInstance = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);

            // Destroy power-up if not picked up within 5 seconds
            Destroy(powerUpInstance, 5f);
        }
    }

    private GameObject ChooseRandomPowerUp()
    {
        float randomValue = Random.value;
        if (randomValue < 0.33f) return shieldPrefab;
        else if (randomValue < 0.66f) return scoreBoostPrefab;
        else return speedUpPrefab;
    }
}
