using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFire : MonoBehaviour
{
    private float waveSpeed = 4f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.up * waveSpeed * Time.deltaTime);

        if (transform.position.y >= 8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
