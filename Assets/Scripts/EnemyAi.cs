using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    NavMeshAgent AI;
    public Transform Player;
    public int Health=10;
    [SerializeField] private ParticleSystem DeadEffect;
    private PlayerController _PlayerController;

    [SerializeField] private float hitForce=1;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        _PlayerController = Player.GetComponent<PlayerController>();
        AI = GetComponent<NavMeshAgent>();
        AI.updateRotation = false;
        AI.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        AI.SetDestination(Player.position);
        if (AI.velocity.x > 0)
        {
            AI.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (AI.velocity.x < 0)
        {
            
            AI.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            StartCoroutine(_PlayerController.Damage());
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector3 dir = (AI.destination - transform.position).normalized;
            rb.AddForce(-dir * 8, ForceMode2D.Impulse);
            rb.AddForce(-dir * 8, ForceMode2D.Impulse);
        }
        

    }

    public void Damage()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Health--;
        if (Health == 0)
        {
            Instantiate(DeadEffect,transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            return;
        }
        Vector3 dir = (AI.destination - transform.position).normalized;
        rb.AddForce(-dir*hitForce,ForceMode2D.Impulse);
        
    }
}
