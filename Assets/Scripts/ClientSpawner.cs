using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    [SerializeField] private GameObject clientPrefab;

    [SerializeField] private Transform[] spawnPoints;

    private void SpawnClient()
    {
        int random = Random.Range(0, spawnPoints.Length);
        Instantiate(clientPrefab, spawnPoints[random].position, clientPrefab.transform.rotation);
    }
}
