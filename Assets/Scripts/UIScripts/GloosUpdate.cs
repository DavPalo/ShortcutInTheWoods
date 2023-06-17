using UnityEngine;
using Unity.Netcode;
using TMPro;
using UnityEngine.UI;

public class GloosUpdate : NetworkBehaviour
{
    [SerializeField] Text gloos;
    private LevelManager levelManager;
    private string toShow = "";

    private void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    private void Update()
    {
        toShow = levelManager.networkGloos.Value.ToString();
        gloos.text = toShow;
    }
}
