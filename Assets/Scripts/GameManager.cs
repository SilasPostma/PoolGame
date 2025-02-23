using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState
{
    Aiming,
    Shooting,
    Shop,
    Paused
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private GameState currentState;
    public GameObject cue;
    public GameObject linePredictor;
    public GameObject linePredictor2;
    public GameObject collCircle;
    private Rigidbody[] balls;

    private float shotDelay = 0.1f;  // Small delay before checking if balls are still
    public float shotTime = 0f;

    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI multText;

    public float points = 1f;
    public float mult = 1f;

    public GameState CurrentState
    {
        get { return currentState; }
        private set
        {
            currentState = value;
            OnGameStateChanged(currentState);
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SetBallList();
        SetGameState(GameState.Aiming); // Start in the Aiming state
        ScoreUpdate(points, mult);
    }

    void Update()
    {
        if (Time.time - shotTime >= shotDelay)
        {
            if (GameManager.Instance.AllBallsStill())
            {
                GameManager.Instance.SetGameState(GameState.Aiming);  // Transition to Aiming state
            }
        }
    }

    public void ScoreUpdate(float points, float mult)
    {
        pointsText.text = points.ToString();
        multText.text = mult.ToString();
    }


    private void SetBallList()
    {
        // Find all balls in the scene and initialize the Rigidbody array
        GameObject[] ballObjects = GameObject.FindGameObjectsWithTag("Ball");
        balls = new Rigidbody[ballObjects.Length];
        for (int i = 0; i < ballObjects.Length; i++)
        {
            balls[i] = ballObjects[i].GetComponent<Rigidbody>();
        }
    }

    public bool AllBallsStill()
    {
        // Check if all balls are still (not moving)
        foreach (Rigidbody ball in balls)
        {
            if (ball.velocity.magnitude > 0)
            {
                return false;
            }
        }
        return true;
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
    }

    private void OnGameStateChanged(GameState newState)
    {
        // Handle game state changes
        switch (newState)
        {
            case GameState.Aiming:
                Debug.Log("Player is aiming.");
                cue.SetActive(true);
                linePredictor.SetActive(true);
                linePredictor2.SetActive(true);
                collCircle.SetActive(true);
                break;

            case GameState.Shooting:
                Debug.Log("Player is shooting.");
                cue.SetActive(false);
                linePredictor.SetActive(false);
                linePredictor2.SetActive(false);
                collCircle.SetActive(false);
                break;

            case GameState.Shop:
                Debug.Log("Player is in the shop.");
                break;

            case GameState.Paused:
                Debug.Log("Game is paused.");
                break;
        }
    }
}
