using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    [SerializeField] private GameObject laserPrefab;

    private Animator animator;
    private Player player;
    private AudioSource audioSource;

    private float fireRate = 3f;
    private float canFire = -1;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player is NULL");
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator is NULL");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource on Enemy is NULL");
        }
    }

    void Update()
    {
        TranslateMovement();

        if (Time.time > canFire)
        {
            fireRate = Random.Range(3f, 7f);
            canFire = Time.time + fireRate;
            GameObject enemyLaser =  Instantiate(laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for(int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void TranslateMovement()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y <= -8)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 8, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (player != null)
            {
                player.Damage();
            }
            StartCoroutine(EnemyDeathSequence());
        }

        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (player != null)
            {
                player.AddtoScore(10);
            }
            StartCoroutine(EnemyDeathSequence());
        }

        else if (other.tag == "Wave")
        {
            if (player != null)
            {
                player.AddtoScore(10);
            }
            StartCoroutine(EnemyDeathSequence());
        }
    }

    IEnumerator EnemyDeathSequence()
    {
        speed = 0;
        Destroy(GetComponent<Collider2D>());
        animator.SetTrigger("OnEnemyDeath");
        audioSource.Play();
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }
}
