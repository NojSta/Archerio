using UnityEngine;

public class TargetManager : MonoBehaviour
{
    private int _targetsRemaining;

    private void OnEnable()
    {
        Target.OnTargetHit += TargetHit;
    }

    private void OnDisable()
    {
        Target.OnTargetHit -= TargetHit;
    }

    private void Start()
    {
        _targetsRemaining = FindObjectsOfType<Target>().Length;
    }

    private void TargetHit(Target target)
    {
        _targetsRemaining--;
        if (_targetsRemaining <= 0)
        {
            // Advance to next level
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        // Implement level-loading logic, e.g., SceneManager.LoadScene("NextLevelScene");
    }
}