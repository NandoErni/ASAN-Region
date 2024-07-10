using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector]
    public int NumberOfSpaceshipsLeft;

    [HideInInspector]
    public int NumberOfSpaceshipsArrived;

    [HideInInspector]
    public List<GameObject> Spaceships = new();

    public float TimePassedSinceStartOfLevel { get; private set; }

    public bool IsActivatingCursor { get; private set; }

    public float MaxCursorTime { get; private set; }

    public float CursorTimeLeft { get; private set; }

    public AudioSource SpaceshipExplosion;

    public AudioSource MenuClick;

    public AudioSource SpaceshipStart;

    public AudioSource SpaceshipArrived;

    public AudioSource LevelFinished;

    public AudioSource LevelStart;

    public AudioSource BlackholeStart;

    public AudioSource BlackholeActive;

    public AudioSource Highscore;

    public AudioSource CursorCrash;

    public AudioSource CollectCoinSFX;

    public TextMeshProUGUI ScoreText;

    public TextMeshProUGUI HighscoreText;

    public Canvas LevelFinishedScreen;

    private SaveGame _saveGame = new();

    private int _score;

    private bool _hasStarted;

    private int levelNumber;

    private bool _isFinished => Spaceships.Count == 0 && NumberOfSpaceshipsLeft == 0 && _hasStarted;

    private int _collectedCoins;


    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        DisableLevelFinishedScreen();
    }

    private void Update()
    {
        TimePassedSinceStartOfLevel += Time.deltaTime;
        IsActivatingCursor = Input.GetButton("Fire1") && CursorTimeLeft > 0;

        if (Input.GetButtonDown("Fire1") && CursorTimeLeft > 0)
        {
            BlackholeStart.Play();
            BlackholeActive.Play();
        }

        if (Input.GetButtonUp("Fire1"))
        {
            BlackholeActive.Stop();
        }

        if (IsActivatingCursor)
        {
            CursorTimeLeft -= Time.deltaTime;
        }
        CheckIfIsFinished();

        if (LevelFinishedScreen.gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {
            MenuClick.Play();
        }

        CheckForRestartHotKey();
    }

    public void ShootSpaceShip()
    {
        NumberOfSpaceshipsLeft--;
    }

    public void FinishSpaceship(SpaceshipController spaceship)
    {
        Spaceships.Remove(spaceship.gameObject);
        NumberOfSpaceshipsArrived++;
        SpaceshipArrived.Play();
    }

    public void LoadLevel(int level)
    {
        StartCoroutine(LoadUnityLevelAsync(level));
    }

    private IEnumerator LoadUnityLevelAsync(int level)
    {
        var sceneLoad = SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);

        while (!sceneLoad.isDone)
        {
            yield return null;
        }

        levelNumber = level;
        StartLevel();
    }

    private void StartLevel()
    {
        var startingPoint = FindAnyObjectByType<StartingPointController>();
        if (startingPoint == null)
        {
            throw new Exception("There is no Starting point!");
        }

        _score = 0;
        _collectedCoins = 0;
        Spaceships.Clear();

        LevelFinishedScreen.gameObject.SetActive(false);
        NumberOfSpaceshipsLeft = startingPoint.NumberOfSpaceships;

        TimePassedSinceStartOfLevel = 0;
        NumberOfSpaceshipsArrived = 0;

        MaxCursorTime = startingPoint.MaxCursorTime;
        CursorTimeLeft = MaxCursorTime;
        _hasStarted = true;

        var cursor = FindAnyObjectByType<CursorController>();
        if (cursor == null)
        {
            throw new Exception("There is no Cursor!");
        }

        cursor.InitCircle();

        LevelStart.Play();

    }

    private int CalculateScore()
    {
        var cursorTimePercentage = CursorTimeLeft / MaxCursorTime;
        var score = NumberOfSpaceshipsArrived * 100 + cursorTimePercentage * NumberOfSpaceshipsArrived * 100;
        score += _collectedCoins * 250;
        return (int)Math.Round(score);
    }

    private void CheckIfIsFinished()
    {
        if (_isFinished)
        {
            _hasStarted = false;
            _score = CalculateScore();
            ScoreText.text = _score.ToString();
            _saveGame.SetHighestUnlockedLevel(levelNumber);

            if(_saveGame.IsHighscore(levelNumber, _score))
            {
                _saveGame.SaveScore(levelNumber, _score);
                Highscore.Play();
            }
            HighscoreText.text = _saveGame.GetScore(levelNumber).ToString();

            LevelFinishedScreen.gameObject.SetActive(true);
            LevelFinished.Play();
        }
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene(0);
        DisableLevelFinishedScreen();
    }

    public void GoToNextLevel()
    {
        if (levelNumber >= 10)
        {
            GoBackToMainMenu();
            return;
        }
        LoadLevel(levelNumber+1);
        DisableLevelFinishedScreen();
    }

    public void RetryLevel()
    {
        LoadLevel(levelNumber);
        DisableLevelFinishedScreen();
    }

    private void DisableLevelFinishedScreen()
    {
        LevelFinishedScreen.gameObject.SetActive(false);
    }

    private void CheckForRestartHotKey()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RetryLevel();
        }
    }

    public void CollectCoin()
    {
        _collectedCoins++;
        CollectCoinSFX.Play();
    }
}
