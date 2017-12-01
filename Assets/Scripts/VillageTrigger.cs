using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageTrigger : MonoBehaviour {

    public static bool hasTriggered = false;
    public GameObject credits;
    public GameObject[] cookies;

    private float timeToDisplay = 5f;
    private bool hasDisplayed = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (hasTriggered && timeToDisplay >= 0f)
        {
            timeToDisplay -= Time.deltaTime;
            for (int i = 0; i < cookies.Length; i++)
            {
                cookies[i].GetComponent<CookieMover>().CookeMove();
            }
        }

        if (timeToDisplay <= 0f && !hasDisplayed)
        {
            credits.SetActive(true);
            hasDisplayed = true;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            hasTriggered = !hasTriggered;
        }
    }
}
