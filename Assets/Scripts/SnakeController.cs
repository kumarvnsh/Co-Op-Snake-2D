using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SnakeController : MonoBehaviour
{
    public GameObject snakeSegmentPrefab;
    public float moveStep = 0.5f;
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

    private Vector2 moveDirection = Vector2.up; // Start moving in positive y-axis
    private List<Transform> snakeSegments = new List<Transform>();

    public int spacing = 2;
    public bool isAlive = true;
    private bool isInitialized = false;

    private float defaultMoveStep;
    private bool isShieldActive = false;
    private bool isScoreBoostActive = false;

    public float powerUpCooldown = 3f; // Cooldown time for power-ups in seconds

    private float xMin = -8f, xMax = 8f;
    private float yMin = -4.1f, yMax = 3.7f;
    private List<Vector2> positionHistory = new List<Vector2>(); // Track positions for spacing
    public int SegmentCount => snakeSegments.Count; // Property to get the number of segments

    public float xspawnPosition = 0;
    public float yspawnPosition = 0;

    

    private void Start()
    {
        defaultMoveStep = moveStep; // Store default move step
        InitializeSnake(new Vector2(xspawnPosition, yspawnPosition)); // Starting position for the snake head
    }

    private void Update()
    {
        if (!isAlive) return;
        HandleInput();
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;
        MoveSnake();
        HandleScreenWrap();
    }

    private void InitializeSnake(Vector2 startPosition)
    {
        Transform headSegment = this.transform;  // Use the GameObject this script is attached to as the head
        headSegment.position = startPosition;
        snakeSegments.Add(headSegment);
        isInitialized = true;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(upKey) && moveDirection != Vector2.down) moveDirection = Vector2.up;
        else if (Input.GetKeyDown(downKey) && moveDirection != Vector2.up) moveDirection = Vector2.down;
        else if (Input.GetKeyDown(leftKey) && moveDirection != Vector2.right) moveDirection = Vector2.left;
        else if (Input.GetKeyDown(rightKey) && moveDirection != Vector2.left) moveDirection = Vector2.right;
    }

    private void MoveSnake()
    {
        positionHistory.Insert(0, snakeSegments[0].position);

        // Move the head in the current direction
        snakeSegments[0].position = new Vector2(
            snakeSegments[0].position.x + moveDirection.x * moveStep,
            snakeSegments[0].position.y + moveDirection.y * moveStep
        );

        // Move each segment to follow the path of the head with a spacing delay
        for (int i = 1; i < snakeSegments.Count; i++)
        {
            int index = i * spacing;
            if (index < positionHistory.Count)
            {
                snakeSegments[i].position = positionHistory[index];
            }
        }

        // Limit the history list to avoid overflow
        if (positionHistory.Count > snakeSegments.Count * spacing)
        {
            positionHistory.RemoveAt(positionHistory.Count - 1);
        }
    }

    private void HandleScreenWrap()
    {
        Vector2 headPosition = snakeSegments[0].position;

        if (headPosition.x > xMax) headPosition.x = xMin;
        else if (headPosition.x < xMin) headPosition.x = xMax;

        if (headPosition.y > yMax) headPosition.y = yMin;
        else if (headPosition.y < yMin) headPosition.y = yMax;

        snakeSegments[0].position = headPosition;
    }

    public void AddSegment()
    {
        Vector2 newSegmentPosition = snakeSegments[snakeSegments.Count - 1].position;
        Transform newSegment = Instantiate(snakeSegmentPrefab, newSegmentPosition, Quaternion.identity).transform;
        snakeSegments.Add(newSegment);
        positionHistory.Add(newSegmentPosition);
    }

    public void RemoveSegment()
    {
        if (snakeSegments.Count > 1)
        {
            Transform lastSegment = snakeSegments[snakeSegments.Count - 1];
            snakeSegments.RemoveAt(snakeSegments.Count - 1);
            Destroy(lastSegment.gameObject);
        }
    }

    // Power-Up Methods
    public void ActivateShield()
    {
        if (isShieldActive) return;
        isShieldActive = true;
        StartCoroutine(DeactivateShieldAfterCooldown());
    }

    private IEnumerator DeactivateShieldAfterCooldown()
    {
        yield return new WaitForSeconds(powerUpCooldown);
        isShieldActive = false;
    }

    public void ActivateScoreBoost()
    {
        if (isScoreBoostActive) return;
        isScoreBoostActive = true;
        StartCoroutine(DeactivateScoreBoostAfterCooldown());
    }

    private IEnumerator DeactivateScoreBoostAfterCooldown()
    {
        yield return new WaitForSeconds(powerUpCooldown);
        isScoreBoostActive = false;
    }

    public void ActivateSpeedUp()
    {
        moveStep *= 1.5f; // Increase speed by 50%
        StartCoroutine(DeactivateSpeedUpAfterCooldown());
    }

    private IEnumerator DeactivateSpeedUpAfterCooldown()
    {
        yield return new WaitForSeconds(powerUpCooldown);
        moveStep = defaultMoveStep; // Reset to default speed
    }

    // Collision Detection for Power-Ups and Food
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gainer"))
        {
            Destroy(collision.gameObject);
            AddSegment();
            // Assuming UIManager manages the score and has AddScore method
            if (isScoreBoostActive) UIManager.Instance.AddScore(2); // Double score if Score Boost is active
            else UIManager.Instance.AddScore(1);
        }
        else if (collision.CompareTag("Burner"))
        {
            Destroy(collision.gameObject);
            RemoveSegment();
        }
        else if (collision.CompareTag("Shield"))
        {
            Destroy(collision.gameObject);
            ActivateShield();
        }
        else if (collision.CompareTag("ScoreBoost"))
        {
            Destroy(collision.gameObject);
            ActivateScoreBoost();
        }
        else if (collision.CompareTag("SpeedUp"))
        {
            Destroy(collision.gameObject);
            ActivateSpeedUp();
        }
        else if (collision.CompareTag("snake"))
        {
            if (isShieldActive) return;
            isAlive = false;
            // Assuming UIManager manages the game over state
            UIManager.Instance.GameOver();
        }
    }
}
