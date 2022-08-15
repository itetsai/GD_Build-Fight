using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(SpawnPrefabList))]
public class SpawnPrefabList : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> RegisteObject;
    private void Start()
    {
        GetComponent<NetworkManager>().spawnPrefabs.AddRange(RegisteObject);
    }
}
