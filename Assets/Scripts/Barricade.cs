using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barricade : MonoBehaviour {

    public bool hasCollided = false;
    public GameObject current, shatter;

    private GameObject player;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            hasCollided = true;
            StartCoroutine(player.GetComponent<PlayerMovement>().DestroyBarricade(current, shatter));
        }
    }
}
