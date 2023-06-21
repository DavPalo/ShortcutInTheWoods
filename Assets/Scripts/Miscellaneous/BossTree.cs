using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class BossTree : MonoBehaviour
{
    private LevelManager levelManager;
    private GameObject vehicle;

    public float distanceFromTrees = 5f;

    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        vehicle = GameObject.Find("Vehicle");
    }

    void Update()
    {
        if (vehicle) {
            if (((transform.position - vehicle.transform.position).magnitude < distanceFromTrees) && !levelManager.key.Value)
                levelManager.ChangeTxtServerRpc("Boss Area Locked");

        }

        if (levelManager.key.Value) {
            gameObject.SetActive(false);
        }
    }
}
