using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int powerupID; //0 = TripleShot, 1 = Speed, 2 = Shield
    [SerializeField] private AudioClip powerupSound;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y <= -8)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                switch (powerupID)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;

                    case 1:
                        player.ActivateSpeedBoost();
                        break;

                    case 2:
                        player.ActivateShield();
                        break;

                    case 3:
                        player.AddtoHealth();
                        break;

                    case 4:
                        player.UpdateAmmo();
                        break;

                    case 5:
                        player.ActivateWaveFire();
                        break;

                    default:
                        Debug.LogError("PowerupID is Invalid");
                        break;
                }
            }
            AudioSource.PlayClipAtPoint(powerupSound, transform.position);
            Destroy(this.gameObject);
        }
    }
}
