using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombDefuser : MonoBehaviour {

    private bool bombDefused = false;
    public GameObject disarmText;

    private void Awake()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("true");
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Z) && !bombDefused)
        {
            bombDefused = true;
            Debug.Log("Bomb Defused");
            disarmText.SetActive(true);
        }
	}
}
