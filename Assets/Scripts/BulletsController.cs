using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsController : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField] private float BulletShootingPowerF;
    [SerializeField] private bool IsCartridge=false;

    private Vector2 InitalPosV3;

    private Rigidbody2D Rb;
    // Start is called before the first frame update
    void OnEnable()
    {
        Rb = GetComponent<Rigidbody2D>();
        InitalPosV3 = GameObject.Find("Player").transform.position;
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
        if (transform.position.y < InitalPosV3.y - .1f)
        {
            if (IsCartridge)
            {
                Rb.gravityScale = 0;
                Rb.velocity = Vector2.zero;
            }
        }
    }

    IEnumerator DestroyBullet()
    {
        for (int i = 0; i < 100; i++)
        {
            yield return null;
        }
        Pooling.instance.BackObjectToRepository(gameObject);
    }
}
