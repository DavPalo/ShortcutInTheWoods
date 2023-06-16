using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTree : MonoBehaviour
{
    LevelManager levelManager;
    GameObject vehicle;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        vehicle = GameObject.Find("Vehicle");
    }

    // Update is called once per frame
    void Update()
    {
        if (((transform.position - vehicle.transform.position).magnitude < 5) && !levelManager.Key.Value)
            canvas.enabled = true;
        else
            canvas.enabled = false;
        if (levelManager.Key.Value) {
            gameObject.SetActive(false);
        }
    }
}
