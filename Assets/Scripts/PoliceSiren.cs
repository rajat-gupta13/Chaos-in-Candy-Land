using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceSiren : MonoBehaviour {

    public GameObject player;
    public float multiplier = 0.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (player.GetComponent<PlayerMovement>().currentVelocity <= 200f)
        {
            GetComponent<AudioSource>().volume += multiplier * Time.deltaTime;
        }
        else if (player.GetComponent<PlayerMovement>().currentVelocity > 200f)
        {
            GetComponent<AudioSource>().volume -= multiplier * Time.deltaTime;
        }
        GetComponent<AudioSource>().volume = Mathf.Clamp(GetComponent<AudioSource>().volume, 0.1f, 0.5f);
    }
}
