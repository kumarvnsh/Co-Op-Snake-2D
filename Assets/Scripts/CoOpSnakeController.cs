using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoOpSnakeController : MonoBehaviour
{
    public GameObject snakeSegmentPrefab;
    public float moveStep = 0.5f;
    private Vector2 moveDirection = Vector2.right;
    private List<Transform> snakeSegments = new List<Transform>();
    private List<Vector2> positionHistory = new List<Vector2>();

    public KeyCode upKey, downKey, leftKey, rightKey; // Keys for movemen
    public bool isAlive = true;
    public int spacing = 2;
    public int playerNumber;

    private float defaultMoveStep;
    private bool isShieldActive = false;
    private bool isScoreBoostActive = false;
    public float powerUpCooldown = 3f;

    public float xspawnPosition = 0;
    public float yspawnPosition = 0;

    public int SegmentCount => snakeSegments.Count;

    private void Start()
    {
        defaultMoveStep = moveStep;
        InitializeSnake(new Vector2(xspawnPosition, yspawnPosition));
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
        Transform headSegment = this.transform;
        headSegment.position = startPosition;
        snakeSegments.Add(headSegment);
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
        snakeSegments[0].position = new Vector2(
            snakeSegments[0].position.x + moveDirection.x * moveStep,
            snakeSegments[0].position.y + moveDirection.y * moveStep
        );

        for (int i = 1; i < snakeSegments.Count; i++)
        {
            int index = i * spacing;
            if (index < positionHistory.Count)
            {
                snakeSegments[i].position = positionHistory[index];
            }
        }

        if (positionHistory.Count > snakeSegments.Count * spacing)
        {
            positionHistory.RemoveAt(positionHistory.Count - 1);
        }
    }

    private void HandleScreenWrap()
    {
        Vector2 headPosition = snakeSegments[0].position;

        if (headPosition.x > 7f) headPosition.x = -7f;
        else if (headPosition.x < -7f) headPosition.x = 7f;
        if (headPosition.y > 3.5f) headPosition.y = -3.5f;
        else if (headPosition.y < -3.5f) headPosition.y = 3.5f;

        snakeSegments[0].position = headPosition;
    }

    public void AddSegment()
    {
        Vector2 newSegmentPosition = snakeSegments[snakeSegments.Count - 1].position;
        Transform newSegment = Instantiate(snakeSegmentPrefab, newSegmentPosition, Quaternion.identity).transform;
        snakeSegments.Add(newSegment);
        positionHistory.Add(newSegmentPosition);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Gainer"))
        {
            AddSegment();
            Destroy(collision.gameObject);
            CoOpUIManager.Instance.AddScore(1, playerNumber);
        }
        else if (collision.CompareTag("Burner"))
        {
            RemoveSegment();
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("snake"))
        {
            if (isShieldActive) return;
            isAlive = false;
            CoOpUIManager.Instance.GameOver(playerNumber);
        }
    }

    public void RemoveSegment()
    {
        if (snakeSegments.Count > 1)
        {
            Destroy(snakeSegments[snakeSegments.Count - 1].gameObject);
            snakeSegments.RemoveAt(snakeSegments.Count - 1);
        }
    }
}
