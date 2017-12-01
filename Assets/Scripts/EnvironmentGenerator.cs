using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour {

    private bool hasNotSpawned = false;
    public GameObject townPrefab;
    public GameObject[] villagePrefab;
    private float timeTillDestroy = 30f;
    Vector3 forward, left, right, backward;
    Vector3 forwardV, leftV, rightV, backwardV;
    private GameObject player;
    private float playerRotationY;
    private static bool firstVillageSpawn;
    // Use this for initialization
    void Start () {
        player = GameObject.Find("Player");
        playerRotationY = player.transform.eulerAngles.y;
        forward = new Vector3(transform.position.x, transform.position.y, transform.position.z + 11f);
        left = new Vector3(transform.position.x - 11f, transform.position.y, transform.position.z);
        right = new Vector3(transform.position.x + 11f, transform.position.y, transform.position.z);
        backward = new Vector3(transform.position.x, transform.position.y, transform.position.z - 11f);
        forwardV = new Vector3(transform.position.x, transform.position.y + 1.74f, transform.position.z + 11f);
        leftV = new Vector3(transform.position.x - 11f, transform.position.y + 1.74f, transform.position.z);
        rightV = new Vector3(transform.position.x + 11f, transform.position.y + 1.74f, transform.position.z);
        backwardV = new Vector3(transform.position.x, transform.position.y + 1.74f, transform.position.z - 11f);
    }
	
	// Update is called once per frame
	void Update () {
        if (PlayerMovement.isMoving)
        {
            timeTillDestroy -= Time.deltaTime;
        }
        if (timeTillDestroy < 0 && !PlayerMovement.isMoving)
        {
            Destroy(gameObject);
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Gummy")
        {
            collision.gameObject.GetComponent<Animator>().SetBool("Splat", true);
        }

        if (collision.gameObject.tag == "Player" && !hasNotSpawned && !GummyRain.isGummyRainOver)
        {
            //Debug.Log(playerRotationY);
            hasNotSpawned = true;
            if ((playerRotationY > 330 && playerRotationY <= 360) || (playerRotationY >= 0 && playerRotationY < 30))
            {
                Instantiate(townPrefab, forward, transform.rotation);
                Instantiate(townPrefab, right, transform.rotation);
                Instantiate(townPrefab, left, transform.rotation);
            }
            else if ((playerRotationY > 60 && playerRotationY < 120))
            {
                Instantiate(townPrefab, forward, transform.rotation);
                Instantiate(townPrefab, right, transform.rotation);
                Instantiate(townPrefab, backward, transform.rotation);
            }
            else if ((playerRotationY > 150 && playerRotationY < 210))
            {
                Instantiate(townPrefab, backward, transform.rotation);
                Instantiate(townPrefab, right, transform.rotation);
                Instantiate(townPrefab, left, transform.rotation);
            }
            else if ((playerRotationY > 240 && playerRotationY < 300))
            {
                Instantiate(townPrefab, forward, transform.rotation);
                Instantiate(townPrefab, backward, transform.rotation);
                Instantiate(townPrefab, left, transform.rotation);
            }

        }
        else if (collision.gameObject.tag == "Player" && !hasNotSpawned && GummyRain.isGummyRainOver)
        {
            if (!firstVillageSpawn)
            {
                firstVillageSpawn = true;
                hasNotSpawned = true;
                if ((playerRotationY > 330 && playerRotationY <= 360) || (playerRotationY >= 0 && playerRotationY < 30))
                {
                    Instantiate(villagePrefab[0], forwardV, Quaternion.Euler(0, 90, 0));
                    Instantiate(villagePrefab[1], rightV, transform.rotation);
                    Instantiate(villagePrefab[1], leftV, transform.rotation);
                }
                else if ((playerRotationY > 60 && playerRotationY < 120))
                {
                    Instantiate(villagePrefab[0], forwardV, Quaternion.Euler(0, 90, 0));
                    Instantiate(villagePrefab[1], rightV, transform.rotation);
                    Instantiate(villagePrefab[0], backwardV, Quaternion.Euler(0, 90, 0));
                }
                else if ((playerRotationY > 150 && playerRotationY < 210))
                {
                    Instantiate(villagePrefab[0], backwardV, Quaternion.Euler(0, 90, 0));
                    Instantiate(villagePrefab[1], rightV, transform.rotation);
                    Instantiate(villagePrefab[1], leftV, transform.rotation);
                }
                else if ((playerRotationY > 240 && playerRotationY < 300))
                {
                    Instantiate(villagePrefab[0], forwardV, Quaternion.Euler(0, 90, 0));
                    Instantiate(villagePrefab[0], backwardV, Quaternion.Euler(0, 90, 0));
                    Instantiate(villagePrefab[1], leftV, transform.rotation);
                }
            }
            else if (firstVillageSpawn)
            {
                hasNotSpawned = true;
                if ((playerRotationY > 330 && playerRotationY <= 360) || (playerRotationY >= 0 && playerRotationY < 30))
                {
                    Instantiate(villagePrefab[0], forward, Quaternion.Euler(0, 90, 0));

                }
                else if ((playerRotationY > 60 && playerRotationY < 120))
                {
                    Instantiate(villagePrefab[1], right, transform.rotation);

                }
                else if ((playerRotationY > 150 && playerRotationY < 210))
                {
                    Instantiate(villagePrefab[0], backward, Quaternion.Euler(0, 90, 0));

                }
                else if ((playerRotationY > 240 && playerRotationY < 300))
                {
                    Instantiate(villagePrefab[1], left, transform.rotation);

                }
            }
        }
    }
}
