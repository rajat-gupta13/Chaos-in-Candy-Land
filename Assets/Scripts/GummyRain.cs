using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GummyRain : MonoBehaviour {

    public GameObject gummyBearPrefab;
    public float spawnWait;
    public float startWait;
    public float hazardCount;
    public float minX, maxX, minZ, maxZ;
    public static bool isGummyRainOver = false;
    private bool isCalled = false;
    public GameObject player;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerMovement.isMoving && !isCalled)
        {
            isCalled = true;
            StartCoroutine(SpawnGummys());
        }
	}

    IEnumerator SpawnGummys()
    {
        yield return new WaitForSeconds(startWait);
        
        for (int i = 0; i < hazardCount; i++)
        {
            Vector3 spawnPosition = new Vector3(player.transform.position.x + Random.Range(minX, maxX), transform.position.y, player.transform.position.z + Random.Range(minZ, maxZ));
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(gummyBearPrefab, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(spawnWait);
        }
        isGummyRainOver = true;
        Debug.Log("Gummy Rain Over");
    }
}
