using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float rotationSpeed = 2.0f;
    public float finalVelocitySlow = 250.0f;
    public float finalVelocityMedium = 400.0f;
    public float finalVelocityFast = 550.0f;
    public float finalVelocityBoost = 750.0f;
    public float finalVelocityReverse = -45f;
    public float decelerationRateReverse = 15f;
    public float accelerationRateSlow = 25f;
    public float accelerationRateMedium = 40f;
    public float accelerationRateFast = 70f;
    public float boostCooldown = 5f;
    public float boostDuration = 5f;
    public float decelerationRate = 100f;
    public AudioClip engineStart, carSlow, carMedium, carFast, carReverse, carSpin, carBarricadeSpikes, carBarricade;
    public GameObject soundManager;
    public float timeToStart = 16f;
    public GameObject train;
    public GameObject soundManagerSFX;
    public float radius = 5.0F;
    public float power = 10.0F;
    public AudioClip[] bGM;
    public GameObject bankAudioSource, policeSound;

    private new Camera camera;
    //private int rotationDirection;
    private float rotationCoeff = 0;
    private float initialVelocity = 0.0f;
    public float currentVelocity = 0.0f;
    public static bool isMoving = false;
    private bool isMovingSlow = false;
    private bool isMovingMedium = false;
    private bool isMovingFast = false;
    private bool hasCooldown = false;
    private bool inBoost = false;
    //private bool isInIgnition = false;
    private AudioSource playerAudioSource;
    private bool startSound = false;
    private bool reverse = false;
    private bool isSlow, isMedium, isFast;
    private float rotationLeft = 360f;
    private float slickRotationSpeed = 450f;
    private float slickRotation; 
    private bool slickCollision = false;
    private bool calledTrain = false;
    private bool villageCalled = false;
    private bool bankAudio = false;
    //private AudioClip currentClip;
    //private bool justSlicked = false;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        camera = Camera.main;
        StartCoroutine(ActivateCooldown());
        playerAudioSource = GetComponent<AudioSource>();
        soundManager.GetComponent<AudioSource>().clip = bGM[0];
        soundManager.GetComponent<AudioSource>().Play();

    }
	
	// Update is called once per frame
	void Update () {
        if (!playerAudioSource.isPlaying && !isMoving)
        {
            playerAudioSource.clip = engineStart;
            playerAudioSource.Play();
        }
        else if (!playerAudioSource.isPlaying && isMoving && !slickCollision && !reverse)
        {
            playerAudioSource.clip = carFast;
            playerAudioSource.Play();
        }
      
        if (timeToStart >= 0)
        {
            timeToStart -= Time.deltaTime;
        }
        if (timeToStart <= 6.0f && !bankAudio)
        {
            bankAudioSource.SetActive(true);
            bankAudio = true;
        }
        else if (timeToStart <= 0 && !TrainTrigger.hasTriggered)
        {
            //Handling Rotation
            if (!slickCollision)
            {
                if (Input.GetAxisRaw("Mouse Y") > 0)
                {
                    //rotationDirection = 1;
                    rotationCoeff += Time.deltaTime * rotationSpeed;
                }
                else if (Input.GetAxisRaw("Mouse Y") < 0)
                {
                    //rotationDirection = -1;
                    rotationCoeff -= Time.deltaTime * rotationSpeed;
                }
                rotationCoeff = Mathf.Clamp(rotationCoeff, -50f, 50f);
                float rotation = -1 * rotationCoeff * Time.deltaTime;
                //Debug.Log(rotationCoeff);
                if (currentVelocity > 0)
                {
                    transform.Rotate(0, rotation, 0);
                }
                else if (currentVelocity < 0)
                {
                    transform.Rotate(0, -rotation, 0);
                }
                playerAudioSource.loop = true;

                //soundManagerSFX.GetComponent<AudioSource>().Stop();

            }
            else if (slickCollision)
            {
                SlickCollision();
            }

            if (VillageTrigger.hasTriggered && !villageCalled)
            {
                villageCalled = true;
                isMoving = false;
                isMovingMedium = false;
                isMovingSlow = false;
                isMovingFast = false;
                playerAudioSource.clip = engineStart;
                playerAudioSource.Play();
                soundManager.GetComponent<AudioSource>().clip = bGM[2];
                soundManager.GetComponent<AudioSource>().Play();
                GetComponent<PlayerMovement>().enabled = false;
            }

            if (Input.GetKeyDown(KeyCode.G) && isMoving)
            {
                isSlow = isMovingSlow;
                isMedium = isMovingMedium;
                isFast = isMovingFast;
                reverse = true;
                soundManagerSFX.GetComponent<AudioSource>().clip = carReverse;
                soundManagerSFX.GetComponent<AudioSource>().Play();
                soundManagerSFX.GetComponent<AudioSource>().loop = true;
                playerAudioSource.Stop();

            }
            if (Input.GetKey(KeyCode.G) && isMoving)
            {
                if (soundManagerSFX.GetComponent<AudioSource>().isPlaying)
                {
                    Debug.Log("Reverse Sound Playing");
                }
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                reverse = false;
                isMovingSlow = isSlow;
                isMovingMedium = isMedium;
                isMovingFast = isFast;
                currentVelocity = initialVelocity;
                soundManagerSFX.GetComponent<AudioSource>().Stop();
                playerAudioSource.Play();
                //currentVelocity += 50;
            }
            if (reverse)
            {
                Reverse();
                isMovingSlow = false;
                isMovingMedium = false;
                isMovingFast = false;
            }
            if (Input.GetKeyDown(KeyCode.A) && (!isMovingSlow || !isMoving))
            {
                currentVelocity = currentVelocity + 25f;
                isMoving = true;
                isMovingSlow = true;
                isMovingMedium = false;
                isMovingFast = false;
                playerAudioSource.clip = carFast;
                //playerAudioSource.Play();
                if (!startSound)
                {
                    startSound = true;
                    soundManager.GetComponent<AudioSource>().clip = bGM[1];
                    soundManager.GetComponent<AudioSource>().Play();
                    policeSound.SetActive(true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.A) && (isMovingSlow || isMoving))
            {
                isMoving = false;
                isMovingSlow = false;
                isMovingMedium = false;
                isMovingFast = false;
                playerAudioSource.clip = engineStart;
                playerAudioSource.Play();
            }
            if (isMovingSlow)
            {
                MovingSlow();
            }
            if (Input.GetKeyDown(KeyCode.S) && (isMovingSlow || !isMoving || !isMovingMedium))
            {
                isMoving = true;
                isMovingSlow = false;
                isMovingMedium = true;
                isMovingFast = false;
                //playerAudioSource.clip = carMedium;
                //playerAudioSource.Play();
            }
            else if (Input.GetKeyDown(KeyCode.S) && (!isMoving || isMovingMedium))
            {
                isMoving = true;
                isMovingSlow = true;
                isMovingMedium = false;
                isMovingFast = false;
                //playerAudioSource.clip = carSlow;
                //playerAudioSource.Play();
            }
            if (isMovingMedium)
            {
                MovingMedium();
            }
            if (Input.GetKeyDown(KeyCode.D) && (isMovingSlow || !isMoving || !isMovingFast || isMovingMedium))
            {
                isMoving = true;
                isMovingMedium = false;
                isMovingSlow = false;
                isMovingFast = true;
                //playerAudioSource.clip = carFast;
                //playerAudioSource.Play();
            }
            else if (Input.GetKeyDown(KeyCode.D) && (!isMoving || isMovingFast))
            {
                isMoving = true;
                isMovingMedium = true;
                isMovingSlow = false;
                isMovingFast = false;
                //playerAudioSource.clip = carMedium;
                //playerAudioSource.Play();
            }
            if (isMovingFast)
            {
                MovingFast();
            }

            if (Input.GetKeyDown(KeyCode.F) && isMoving && !hasCooldown && !inBoost)
            {
                currentVelocity = finalVelocityBoost;
                inBoost = true;
                StartCoroutine(ActivateCooldown());
                StartCoroutine(ResetMovement(isMovingSlow, isMovingMedium, isMovingFast));
            }

            if (!isMoving)
            {
                currentVelocity = currentVelocity - (decelerationRate * Time.deltaTime);
                currentVelocity = Mathf.Clamp(currentVelocity, initialVelocity, finalVelocitySlow);
            }


            gameObject.GetComponent<Rigidbody>().velocity = currentVelocity * Time.deltaTime * camera.transform.forward;

        }

        if (TrainTrigger.hasTriggered)
        {
            TrainTriggered();
        }
    }

    private void MovingSlow()
    {
        if (currentVelocity > finalVelocitySlow && !inBoost)
        {
            currentVelocity = currentVelocity - (decelerationRateReverse * Time.deltaTime);
        }
        else if (currentVelocity <= finalVelocitySlow)
        {
            currentVelocity = currentVelocity + (accelerationRateSlow * Time.deltaTime);
            currentVelocity = Mathf.Clamp(currentVelocity, initialVelocity, finalVelocitySlow);
        }
    }

    private void MovingMedium()
    {
        if (currentVelocity > finalVelocityMedium && !inBoost)
        {
            currentVelocity = currentVelocity - (decelerationRateReverse * Time.deltaTime);
        }
        else if (currentVelocity <= finalVelocityMedium)
        {
            currentVelocity = currentVelocity + (accelerationRateMedium * Time.deltaTime);
            currentVelocity = Mathf.Clamp(currentVelocity, initialVelocity, finalVelocityMedium);
        }
    }

    private void MovingFast()
    {
        if (currentVelocity > finalVelocityFast && !inBoost)
        {
            currentVelocity = currentVelocity - (decelerationRateReverse * Time.deltaTime);
        }
        else if (currentVelocity <= finalVelocityFast)
        {
            currentVelocity = currentVelocity + (accelerationRateFast * Time.deltaTime);
            currentVelocity = Mathf.Clamp(currentVelocity, initialVelocity, finalVelocityFast);
        }
    }

    private void Reverse()
    {
        currentVelocity = currentVelocity - (decelerationRate * Time.deltaTime);
        currentVelocity = Mathf.Clamp(currentVelocity, finalVelocityReverse, initialVelocity);
    }

    private void SlickCollision()
    {
        slickRotation = slickRotationSpeed * Time.deltaTime;
        if (slickRotation < rotationLeft)
        {
            rotationLeft -= slickRotation;
        }
        else
        {
            slickRotation = rotationLeft;
            rotationLeft = 0;
        }
        transform.Rotate(0, slickRotation, 0);
        if (rotationLeft == 0)
        {
            slickCollision = false;
            soundManagerSFX.GetComponent<AudioSource>().Stop();
        }
        currentVelocity = 50f;
    }

    private void TrainTriggered()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
        currentVelocity = 300f;
        gameObject.GetComponent<Rigidbody>().velocity = currentVelocity * Time.deltaTime * camera.transform.forward;
        if (!calledTrain)
        {
            calledTrain = true;
            train.GetComponent<Mover>().enabled = true;
            train.GetComponent<AudioSource>().enabled = true;
        }
    }

    private  IEnumerator ResetMovement(bool slow, bool medium, bool fast)
    {
        // wait some seconds
        yield return new WaitForSeconds(boostDuration);
        // return to normal speed
        if (slow)
        {
            currentVelocity = finalVelocitySlow;
        }
        else if (medium)
        {
            currentVelocity = finalVelocityMedium;
        }
        else if (fast)
        {
            currentVelocity = finalVelocityFast;
        }
        inBoost = false;
        Debug.Log("boost ended");
    }

    private IEnumerator ActivateCooldown()
    {
        // put some code to disable the boost-is-ready bar
        // diable the ability to use boost
        hasCooldown = true;
        // wait until the boost is ready again
        yield return new WaitForSeconds(boostCooldown);
        // make the boost usable
        hasCooldown = false;
        Debug.Log("boost ready");
        // put some code to enable the boost-is-ready bar
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Spikes")
        {
            currentVelocity = 50f;
            finalVelocityFast -= 25f;
            soundManagerSFX.GetComponent<AudioSource>().clip = carBarricadeSpikes;
            soundManagerSFX.GetComponent<AudioSource>().Play();
            soundManagerSFX.GetComponent<AudioSource>().loop = false;
            playerAudioSource.Stop();
        }

        if (collision.gameObject.tag == "Barricade")
        {
            currentVelocity = 20f;
            soundManagerSFX.GetComponent<AudioSource>().clip = carBarricade;
            soundManagerSFX.GetComponent<AudioSource>().Play();
            soundManagerSFX.GetComponent<AudioSource>().loop = false;
            playerAudioSource.Stop();
        }

        if (collision.gameObject.tag == "MilkSlick" && !collision.gameObject.GetComponent<MilkSlick>().hasCollided)
        {
            slickCollision = true;
            rotationLeft = 360f;
            soundManagerSFX.GetComponent<AudioSource>().clip = carSpin;
            soundManagerSFX.GetComponent<AudioSource>().Play();
            //currentClip = playerAudioSource.clip;
            playerAudioSource.Stop();
            soundManagerSFX.GetComponent<AudioSource>().loop = false;
            //justSlicked = true;
        }

        if (collision.gameObject.tag == "Village")
        {
            isMoving = false;
            isMovingMedium = false;
            isMovingSlow = false;
            isMovingFast = false;
        }
    }

    public IEnumerator DestroyBarricade(GameObject current, GameObject shatter) {
        current.SetActive(false);
        shatter.SetActive(true);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && rb.gameObject.tag != "Player")
            {
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
                Debug.Log("Force Added");
                rb.useGravity = true;
            }
        }
        yield return null;
    }
}
