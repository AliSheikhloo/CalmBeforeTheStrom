using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SeedSpawns : MonoBehaviour
{
    public GameObject SeedSpawn; 
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i <= 15; i++)
        {
            Instantiate(SeedSpawn, new Vector3(transform.position.x + i, transform.position.y), quaternion.identity,transform);
        }
        for (int i = 0; i <= 15; i++)
        {
            GameObject gm=Instantiate(SeedSpawn, new Vector3(transform.position.x + i, transform.position.y-1), quaternion.identity,transform);
            gm.GetComponent<FarmingController>().inFront = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
