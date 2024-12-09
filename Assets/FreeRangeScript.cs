using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class FreeRangeScript : MonoBehaviour
{
    public Button MainMenuButton;



    private void Start()
    {
        MainMenuButton.onClick.AddListener(() => LoadSpecificLevel(0));
    }

    private void LoadSpecificLevel(int sceneIndex)
    {
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError("Scene index " + sceneIndex + " is out of bounds.");
        }
    }
}
