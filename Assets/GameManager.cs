using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI hitCounterText;
    public TextMeshProUGUI totalTargetCounterText;
    public TextMeshProUGUI timeTakenText;  // To show the time taken
    public GameObject popupPanel;          // The popup panel UI
    public Button nextLevelButton;         // The "Next Level" button
    public TextMeshProUGUI popupText;      // The text in the popup

    private int hitTargetsCount = 0;
    private int totalTargetsCount = 0;

    private float startTime;              // Time when the level started
    private bool levelComplete = false;   // To check if the level is complete

    public Button resetCurrentSceneButton; // For resetting the level
    public Button resetAllGameButton;      // For resetting all game

    private void Start()
    {
        // Initialize
        totalTargetsCount = FindObjectsOfType<Target>().Length;
        UpdateTotalTargetsText();

        // Start tracking time
        startTime = Time.time;

        // Add button listeners
        resetCurrentSceneButton.onClick.AddListener(ResetCurrentScene);
        resetAllGameButton.onClick.AddListener(ResetAllGame);
        nextLevelButton.onClick.AddListener(LoadNextLevel);

        // Hide the popup initially
        popupPanel.SetActive(false);
    }

    private void OnEnable()
    {
        Target.OnTargetHit += HandleTargetHit;
    }

    private void OnDisable()
    {
        Target.OnTargetHit -= HandleTargetHit;
    }

    private void HandleTargetHit(Target target)
    {
        if (!target.IsHit())
        {
            hitTargetsCount++;
            target.SetHitStatus(true);
            UpdateHitTargetsText();
        }

        if (hitTargetsCount >= totalTargetsCount && !levelComplete)
        {
            levelComplete = true;
            ShowPopup();
        }
    }

    private void ShowPopup()
    {
        float timeTaken = Time.time - startTime;

        string formattedTime = FormatTime(timeTaken);

        popupText.text = "Time taken: " + formattedTime;

        popupPanel.SetActive(true);
    }

    // Format the time as minutes:seconds
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Update the hit counter text UI
    private void UpdateHitTargetsText()
    {
        hitCounterText.text = "Targets Hit: " + hitTargetsCount;
    }

    // Update the total targets text UI
    private void UpdateTotalTargetsText()
    {
        totalTargetCounterText.text = "Total Targets: " + totalTargetsCount;
    }

    // Reset the current scene
    private void ResetCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Reset all game (load the first scene)
    private void ResetAllGame()
    {
        SceneManager.LoadScene(0);
    }

    // Load the next level (next scene)
    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Check if there is a next scene
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            // If no next scene, reload the current one or go to a game over screen
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}
