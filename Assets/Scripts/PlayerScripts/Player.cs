using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Class Variables
    [Header("Player Variables")]
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float speedMulitplier = 2f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private int lives = 3;

    [Header("Object References")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject tripleLaserPrefab;
    [SerializeField] private GameObject shield;
    [SerializeField] private GameObject rightEngine;
    [SerializeField] private GameObject leftEngine;
    [SerializeField] private AudioClip laserSound;

    private SpawnManager spawnManager;
    private UIManager uIManager;

    private float canFire = -1f;
    private float coolDownTime = 5f;
    private float laserOffset = 1.5f;

    private bool isTripleShotActive = false;
    private bool isShieldActive = false;

    private float horizontalMove;
    private float verticalMove;
    private int score = 0;
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

        transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        TranslateMovement();

        FireLaser();
    }

    private void FireLaser()
    {

        if (Input.GetButtonDown("Fire") && Time.time > canFire)
        {
            canFire = Time.time + fireRate;

            if (isTripleShotActive == true)
            {
                Instantiate(tripleLaserPrefab, transform.position + new Vector3(0, laserOffset, 0), Quaternion.identity);
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
            isShieldActive = false;
            shield.SetActive(false);
            return;
        }

        else
        {
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

    public void ActivateSpeedBoost()
    {
        speed *= speedMulitplier;
        StartCoroutine(SpeedBoostCoolDown());
    }

    IEnumerator SpeedBoostCoolDown()
    {
        yield return new WaitForSeconds(coolDownTime);
        speed /= speedMulitplier;
    }

    public void ActivateShield()
    {
        isShieldActive = true;
        shield.SetActive(true);
    }

    public void AddtoScore(int points)
    {
        score += points;
        uIManager.UpdateScoreText(score);
    }
}
