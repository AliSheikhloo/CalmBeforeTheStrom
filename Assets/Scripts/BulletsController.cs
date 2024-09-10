using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsController : MonoBehaviour
{
    [SerializeField] private float BulletShootingPowerF;
    [SerializeField] private bool IsCartridge=false;
    // Start is called before the first frame update
    void Start()
    {
        if (!IsCartridge)
        {
            int mirrorI;
            if (GameObject.FindWithTag("Player").GetComponent<PlayerController>().IsPlayerLookingLeft)
            {
                mirrorI = -1;
            }
            else
            {
                mirrorI = 1;
            }

            Vector3 transformLocalScale = transform.localScale;
            transformLocalScale.x *= mirrorI;
            transform.localScale = transformLocalScale;
            GetComponent<Rigidbody2D>().AddForce(transform.right * BulletShootingPowerF * mirrorI, ForceMode2D.Impulse);
        }
        StartCoroutine(DestroyBullet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyBullet()
    {
        for (int i = 0; i < 100; i++)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
