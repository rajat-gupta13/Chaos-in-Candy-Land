using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public Rails rail;
    public float speed = 5;

    private int currentSeg;
    private float transition;
    private bool isCompleted;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!rail)
        {
            return;
        }
        if (!isCompleted)
        {
            Play();
        }

	}

    private void Play()
    {
        transition += Time.deltaTime * 1 / speed;
        if (transition > 1)
        {
            transition = 0;
            currentSeg++;
            if (currentSeg == rail.nodes.Length - 1)
            {
                isCompleted = true;
                return;
            }
        }
        else if (transition < 0)
        {
            transition = 1;
            currentSeg--;
        }
        transform.position = rail.LinearPosition(currentSeg, transition);
     
    }
}
