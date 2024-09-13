using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FarmingController : MonoBehaviour
{
    [SerializeField] private GameObject Wheat;
    [SerializeField] private GameObject Corn;

    public string SeedType;
    public bool isGrown;
    public bool isPlant;
    public bool inFront;
    
    public int DayPlant;

    private void Update()
    {
        if (isPlant)
        {
            if (MainManager.CurrentDay - DayPlant > 2)
            {
                if (!isGrown)
                {
                    Grow();
                }
            }
        }
    }

     void Grow()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, .5f);
        foreach (var VARIABLE in colliders)
        {
            if (VARIABLE.gameObject.tag == "WheatSeed" || VARIABLE.gameObject.tag == "CornSeed") 
            {
                Destroy(VARIABLE.gameObject);
            }
        }
        
        if (SeedType == "Wheat")
        {
           GameObject gm=Instantiate(Wheat, transform.position, quaternion.identity);
            if (inFront)
            {
                gm.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }
        else
        {
            GameObject gm=Instantiate(Corn, transform.position, quaternion.identity);
            if (inFront)
            {
                gm.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
            }
        }

        isGrown = true;
    }
}
