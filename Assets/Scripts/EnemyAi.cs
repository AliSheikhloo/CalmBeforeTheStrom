using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    NavMeshAgent AI;
    public Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
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
            print("SDDS");
            AI.transform.localScale = new Vector3(-1, 1, 1);
        }
        CheckForHittig();
    }

    private void CheckForHittig()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
        foreach (var VARIABLE in colliders)
        {
            if (VARIABLE.gameObject.tag == "Bullet")
            {
                Destroy(gameObject);
            }
        }
    }
}
