using UnityEngine;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    private int _targetsRemaining;

    public GameObject targetPrefab; // Prefab for targets
    public List<Transform> spawnPoints; // List of positions to spawn targets

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
        SpawnTargets();
    }

    private void TargetHit(Target target)
    {
        _targetsRemaining--;
        if (_targetsRemaining <= 0)
        {
            // All targets are hit; respawn targets
            SpawnTargets();
        }
    }

    private void SpawnTargets()
    {
        // Clear previous targets (if not already handled elsewhere)
        foreach (Transform spawnPoint in spawnPoints)
        {
            // Instantiate new targets at each spawn point
            Instantiate(targetPrefab, spawnPoint.position, spawnPoint.rotation);
        }

        // Update the counter for remaining targets
        _targetsRemaining = spawnPoints.Count;
    }
}