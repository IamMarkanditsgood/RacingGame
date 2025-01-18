using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnCarPositions;

    public Transform GetFreeSpawnPos()
    {
        if (_spawnCarPositions.Count > 0)
        {
            Transform position = _spawnCarPositions[_spawnCarPositions.Count - 1];
            _spawnCarPositions.Remove(position);

            return position;
        }

        Debug.LogWarning("There is no free positions");
        return null;
    }
}
