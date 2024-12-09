using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    public Button level1Button;
    public Button level2Button;

    private void Start()
    {
        level1Button.onClick.AddListener(() => LoadSpecificLevel(1));
        level2Button.onClick.AddListener(() => LoadSpecificLevel(2));
    }
    private void LoadSpecificLevel(int sceneIndex)
    {
        print("Loading scene " + sceneIndex);
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