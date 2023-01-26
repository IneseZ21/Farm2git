using StarterAssets;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Default Game Manager for handling game controls and preferences.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Is On")]
    [Space(5)]
    [SerializeField] private bool isOn;

    [Space(10)]
    [Header("UI Components")]
    [Space(5)]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text scoreCounter;
    [SerializeField] private TMP_Text availableUnits;
    [SerializeField] private TMP_Text finalScore;
    [SerializeField] private GameObject activePanel;
    [SerializeField] private GameObject finalPanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject aboutPanel;

    [Space(10)]

    [Header("Game Settings")]
    [Space(5)]
    [SerializeField] private float objectsToSpawn = 2;
    [SerializeField] private float availableTime;

    [Space(10)]

    [Header("Game References")]
    [Space(5)]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject spawnPrefab;
    [SerializeField] private List<Transform> spawnTransforms;
    [SerializeField] private GameObject spawnedParent;
    [SerializeField] private Transform spawnTransform;
    
    private int currentScore = 0;
    private float defaultTime;
    private int dropThrowCount = 0;
    private bool isTimerOn;
    private StarterAssetsInputs starterAssetsInputs;
    private FirstPersonController firstPersonController;

    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);

        if (!isOn) return;

        Cursor.visible = true;
        defaultTime = availableTime;
        starterAssetsInputs = player.GetComponent<StarterAssetsInputs>();
        firstPersonController = player.GetComponent<FirstPersonController>();
        firstPersonController.enabled = false;
        player.transform.position = spawnTransform.position;
    }

    private void SpawnObjects()
    {
        if (!isOn) return;
        
        for (int i = 0; i < objectsToSpawn; i++)
        {
            var listIndex = UnityEngine.Random.Range(0, spawnTransforms.Count);
            Instantiate(spawnPrefab, spawnTransforms[listIndex].transform.position, Quaternion.Euler(Vector3.zero), spawnedParent.transform);
        }
        string availableUnitsTxt = string.Format("{0} {1}", "Available:", objectsToSpawn);
        availableUnits.SetText(availableUnitsTxt);
    }

    private void DestroyObjects()
    {
        if (!isOn) return;

        int childCount = spawnedParent.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Destroy(spawnedParent.transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        if (!isOn) return;

        if (isTimerOn)
            SetTimer();
    }

    public void Score(int objectScore)
    {
        if (!isOn) return;

        currentScore += objectScore;
        string scoreText = string.Format("{0} {1}", "Score:", currentScore);
        scoreCounter.SetText(scoreText);
    }

    private void SetTimer()
    {
        if (!isOn) return;

        availableTime -= Time.deltaTime;
        string minutes;
        string seconds;

        if (availableTime < 0)
        {
            isTimerOn = false;
            minutes = "00";
            seconds = "00";
            ProcessGameOver();
        }
        else
        {
            minutes = Math.Floor(availableTime / 60).ToString("00");
            seconds = (availableTime % 60).ToString("00");
        }

        string timerTxt = string.Format("{0}:{1}", minutes, seconds);
        timerText.SetText(timerTxt);
    }

    public void IncreaseDropThrowCount()
    {
        if (!isOn) return;

        dropThrowCount++;
        if(dropThrowCount == objectsToSpawn)
        {
            SpawnObjects();
            dropThrowCount = 0;
        }
        string availableUnitsTxt = string.Format("{0} {1}", "Available:", (objectsToSpawn - dropThrowCount));
        availableUnits.SetText(availableUnitsTxt);
    }

    private void ProcessGameOver()
    {
        if (!isOn) return;

        player.transform.SetPositionAndRotation(spawnTransform.position, Quaternion.Euler(Vector3.zero));
        Time.timeScale = 0;
        dropThrowCount = 0;
        string finalScoreTxt = string.Format("{0} {1}", "Score:", currentScore);
        finalScore.SetText(finalScoreTxt);
        activePanel.SetActive(false);
        finalPanel.SetActive(true);
        ToggleCursor();
        DestroyObjects();
        SoundManager.Instance.PlaySound(SoundManager.Instance.gameOverSound);
    }

    public void ProcesStart()
    {
        if (!isOn) return;

        SpawnObjects();
        ToggleCursor();
        availableTime = defaultTime;
        startPanel.SetActive(false);
        activePanel.SetActive(true);
        isTimerOn = true;
        currentScore = 0;
        SoundManager.Instance.PlaySound(SoundManager.Instance.clickSound);
    }

    public void ProcessRestart()
    {
        if (!isOn) return;

        finalPanel.SetActive(false);
        ProcesStart();
        string scoreText = string.Format("{0} {1}", "Score:", 0);
        scoreCounter.SetText(scoreText);
        Time.timeScale = 1f;
    }

    private void ToggleCursor()
    {
        if (!isOn) return;

        starterAssetsInputs.cursorLocked = !starterAssetsInputs.cursorLocked;
        starterAssetsInputs.cursorInputForLook = !starterAssetsInputs.cursorInputForLook;
        firstPersonController.enabled = !firstPersonController.enabled;
        Cursor.visible = !Cursor.visible;
    }

    public void OpenAbout()
    {
        if (!isOn) return;

        startPanel.SetActive(false);
        aboutPanel.SetActive(true);
        SoundManager.Instance.PlaySound(SoundManager.Instance.clickSound);
    }

    public void ReturnHome()
    {
        if (!isOn) return;

        startPanel.SetActive(true);
        aboutPanel.SetActive(false);
        SoundManager.Instance.PlaySound(SoundManager.Instance.clickSound);
    }
}
