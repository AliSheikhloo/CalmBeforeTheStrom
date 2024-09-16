using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletsController : MonoBehaviour
{
    [SerializeField] private float BulletShootingPowerF;
    [SerializeField] private bool IsCartridge=false;
    
    public Vector2 InitalPosV3;
    private bool IsActive = false;
    private Rigidbody2D Rb;
    // Start is called before the first frame update

    void OnEnable()
    {
        IsActive = true;
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
        if (!IsCartridge) 
        {
            CheckForHit();
        }
        
        if (transform.position.y < InitalPosV3.y - .5f)
        {
            if (IsCartridge&& IsActive)
            {
                Rb.gravityScale = 0;
                Rb.velocity = Vector2.zero;
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void OnDisable()
    {
        if (IsCartridge)
        {
            IsActive = false;
            Rb.gravityScale = 10;
            InitalPosV3 = new Vector2(0,-10); 
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

    public void CheckForHit()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, .1f);
        foreach (var VARIABLE in cols)
        {
            if (VARIABLE.gameObject.tag == "Hitable"|| VARIABLE.gameObject.tag =="Enemy")
            {

                GameObject gm = Pooling.instance.ReturnObject("HitEffect");
                gm.transform.position = transform.position;
                gm.SetActive(true);
                StartCoroutine(BackBulletHitEffectToReposetory(gm));
                
                if (VARIABLE.gameObject.tag == "Enemy")
                {
                    VARIABLE.gameObject.GetComponent<EnemyAi>().Damage();
                    
                }
                
                Pooling.instance.BackObjectToRepository(gameObject);
                break;
            }
        }
    }
    /*private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!IsCartridge)
        {
            if (collision.gameObject.tag != "Player" && collision.gameObject.tag !="Bullet")
            {
                GameObject gm = Pooling.instance.ReturnObject("HitEffect");
                gm.transform.position = transform.position;
                gm.SetActive(true);
                StartCoroutine(BackBulletHitEffectToReposetory(gm));
                
                if (collision.gameObject.tag == "Enemy")
                {
                    collision.gameObject.GetComponent<EnemyAi>().Damage();
                }
                Pooling.instance.BackObjectToRepository(gameObject);
            }
        }

    }*/

    IEnumerator BackBulletHitEffectToReposetory(GameObject gameObject)
    {
        yield return new WaitForSeconds(2);
        Pooling.instance.BackObjectToRepository(gameObject);
    }
}
