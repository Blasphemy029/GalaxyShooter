using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Class Variables
    [Header("Player Variables")]
    [SerializeField] private float baseSpeed = 3.5f;
    [SerializeField] private float bostedSpeed = 5f;
    [SerializeField] private float speedMulitplier = 2f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private int lives = 3;
    [SerializeField] private int shieldStrength = 3;

    [Header("Object References")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject tripleLaserPrefab;
    [SerializeField] private GameObject waveFirePrefab;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject rightEngine;
    [SerializeField] private GameObject leftEngine;
    [SerializeField] private AudioClip laserSound;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private ParticleSystem healthEffect;

    private SpawnManager spawnManager;
    private UIManager uIManager;
    private SpriteRenderer renderer;

    private float canFire = -1f;
    private float coolDownTime = 5f;
    private float laserOffset = 1.5f;
    public float speed = 0;
    public float thrusterFuel = 1f;
    public float maxFuel = 1f;
    private float fuelDepleteTime = 5f;
    private float fuelRegenTime = 3f;

    private bool isTripleShotActive = false;
    private bool isShieldActive = false;
    private bool isWaveActive = false;
    private bool isBoosted;
    private bool thrusting;

    private float horizontalMove;
    private float verticalMove;
    private int score = 0;
    public int currentAmmo = 15;
    private int maxAmmo = 15;
    #endregion

    void Start()
    {
        spawnManager = GameObject.Find("Spawner").GetComponent<SpawnManager>();
        if ( spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL");
        }

        uIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (uIManager == null)
        {
            Debug.LogError("UIManager is NULL");
        }

        renderer = shield.GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            Debug.LogError("SpriteRenderer is NULL");
        }

        transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        TranslateMovement();

        if (currentAmmo > 0)
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {

        if (Input.GetButtonDown("Fire") && Time.time > canFire)
        {
            canFire = Time.time + fireRate;
            currentAmmo -= 1;

            if (isTripleShotActive == true)
            {
                Instantiate(tripleLaserPrefab, transform.position + new Vector3(0, laserOffset +.25F, 0), Quaternion.identity);
                canFire = Time.time + fireRate;
            }

            else if (isWaveActive == true)
            {
                Instantiate(waveFirePrefab, transform.position + new Vector3(0, laserOffset, 0), Quaternion.identity);
                canFire = Time.time + fireRate;
            }

            else
            {
                Instantiate(laserPrefab, transform.position + new Vector3(0, laserOffset, 0), Quaternion.identity);
            }

            AudioSource.PlayClipAtPoint(laserSound, transform.position);
        }
    }

    private void TranslateMovement()
    {
        thrusting = false;

        if (Input.GetButton("Boost"))
        {
            thrusterFuel -= Time.deltaTime / fuelDepleteTime;

            if (thrusterFuel > 0f)
            {
                thrusting = true;
            }
        }
        else
        {
            thrusterFuel += Time.deltaTime / fuelRegenTime;
        }

        thrusterFuel = Mathf.Clamp01(thrusterFuel);

        if (thrusting == true)
        {
            speed = bostedSpeed;
        }

        else
        {
            speed = baseSpeed;
        }

        if (isBoosted)
        {
            speed *= speedMulitplier;
        }

        horizontalMove = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        verticalMove = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(horizontalMove, verticalMove, 0);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }

        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        if ( isShieldActive == true)
        {
            shieldStrength -= 1;
            ShieldIntegrity();
            return;
        }

        else
        {
            StartCoroutine(cameraShake.Shake(.15f, .4f));

            lives -= 1;
            if (lives == 2)
            {
                rightEngine.SetActive(true);
            }

            else if (lives ==1)
            {
                leftEngine.SetActive(true);
            }

            uIManager.UpdateLives(lives);
        }

        if (lives < 1)
        {
            spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }

    private void ShieldIntegrity()
    {
        switch(shieldStrength)
        {
            case 0:
                isShieldActive = false;
                shield.SetActive(false);
                break;

            case 1:
                renderer.color = Color.red;
                break;

            case 2:
                renderer.color = Color.yellow;
                break;

            case 3:
                renderer.color = Color.blue;
                break;

            default:
                Debug.LogError("Shield Strength index out of Bounds");
                break;
        }
    }

    public void ActivateTripleShot()
    {
        isTripleShotActive = true;
        StartCoroutine(TripleShotCoolDown());
    }

    IEnumerator TripleShotCoolDown()
    {
        yield return new WaitForSeconds(coolDownTime);
        isTripleShotActive = false;
    }

    public void ActivateWaveFire()
    {
        isWaveActive = true;
        StartCoroutine(WaveShotCoolDown());
    }

    IEnumerator WaveShotCoolDown()
    {
        yield return new WaitForSeconds(coolDownTime);
        isWaveActive = false;
    }

    public void ActivateSpeedBoost()
    {
        isBoosted = true;
        StartCoroutine(SpeedBoostCoolDown());
    }

    IEnumerator SpeedBoostCoolDown()
    {
        yield return new WaitForSeconds(coolDownTime);
        isBoosted = false;
    }

    public void ActivateShield()
    {
        if (isShieldActive == true)
        {
            return;
        }

        else
        {
            isShieldActive = true;
            shield.SetActive(true);
            shieldStrength = 3;
            ShieldIntegrity();
        }
    }

    public void AddtoScore(int points)
    {
        score += points;
        uIManager.UpdateScoreText(score);
    }

    public void AddtoHealth()
    {
        if (lives < 3)
        {
            lives += 1;
            healthEffect.Play();
            uIManager.UpdateLives(lives);

            if (lives == 3)
            {
                rightEngine.SetActive(false);
            }

            else if (lives == 2 )
            {
                leftEngine.SetActive(false);
            }
        }
    }

    public void UpdateAmmo()
    {
        currentAmmo = maxAmmo;
    }
}
