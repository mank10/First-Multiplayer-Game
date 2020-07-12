using Photon.Pun;
using System.Threading;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner spawner; 

    public Transform[] spawnPoints;

    [SerializeField] private GameObject playerPrefab = null;

    void Awake()
    {
        if(PlayerSpawner.spawner == null)
        {
            PlayerSpawner.spawner = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
    }

}
