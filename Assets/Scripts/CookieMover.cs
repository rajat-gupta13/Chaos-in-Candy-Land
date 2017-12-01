using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookieMover : MonoBehaviour {

    public Transform target;
    public float speed;

    public void CookeMove()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
