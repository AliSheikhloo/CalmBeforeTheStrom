using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Tile = UnityEngine.WSA.Tile;

public class PlayerController : MonoBehaviour
{
    public MainManager.Tools PlayerInHand = MainManager.Tools.WheatSeed;
    private MainManager _MainManager;
    
    private float CurrentSpeedF;
    [SerializeField] private float WalkingSpeedF;
    [SerializeField] private float RunningSpeedF;
    
    
    public List<GameObject> UsedFarmCells;
    [SerializeField] private GameObject WheatSeedPrefabG;
    [SerializeField] private GameObject CornSeedPrefabG;
    [SerializeField] private GameObject Seeds;
    [SerializeField] private GameObject HarvestingEffectPrefabG;
    //[SerializeField] private float JumpForceF;

    private Rigidbody2D PlayerRB;
    public bool IsShiftPressedB = false;
    public bool IsShooting = false;
    public bool IsPlayerLookingLeft;
    //private bool IsJumpFinished = true;

    // Start is called before the first frame update
    void Start()
    {
        _MainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        Application.targetFrameRate = 60;
        PlayerRB = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        BasicMovment();

        if ((PlayerInHand == MainManager.Tools.WheatSeed || PlayerInHand== MainManager.Tools.CornSeed) && Input.GetKey(KeyCode.Space))
        {
            PlantSeeds();
        }

        if (PlayerInHand == MainManager.Tools.sickle&& Input.GetKey(KeyCode.Space))
        {
            HarvestPlants();
        }

        if (Input.GetKey(KeyCode.N))
        {
            _MainManager.SwichTool(MainManager.Tools.Rifle);
        }
        
        if (Input.GetKey(KeyCode.B))
        {
            _MainManager.SwichTool(MainManager.Tools.sickle);
        }
    }

    void BasicMovment()
    {
        Vector3 transformLocalScaleV3 = transform.localScale;

        //Basic movment of player
        if (!IsShooting)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                CurrentSpeedF = RunningSpeedF;
                IsShiftPressedB = true;
            }
            else
            {
                CurrentSpeedF = WalkingSpeedF;
                IsShiftPressedB = false;
            }
        }
        else
        {
            CurrentSpeedF = WalkingSpeedF;
        }

        if (Input.GetKey(KeyCode.A))
        {
            PlayerRB.AddForce(-transform.right * CurrentSpeedF * Time.deltaTime * 50);
            if (!IsShooting)
            {
                transformLocalScaleV3.x = 1;
                IsPlayerLookingLeft = true;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            PlayerRB.AddForce(-transform.up * CurrentSpeedF * Time.deltaTime * 50);
        }

        if (Input.GetKey(KeyCode.D))
        {
            PlayerRB.AddForce(transform.right * CurrentSpeedF * Time.deltaTime * 50);
            if (!IsShooting)
            {
                transformLocalScaleV3.x = -1;
                IsPlayerLookingLeft = false;
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            PlayerRB.AddForce(transform.up * CurrentSpeedF * Time.deltaTime * 50);
        }
        
        transform.localScale = transformLocalScaleV3;

    }

    void PlantSeeds()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
        foreach (var VARIABLE in colliders)
        {
            if (VARIABLE.gameObject.tag == "SeedsSpawner")
            {
                if (!UsedFarmCells.Contains(VARIABLE.gameObject))
                {
                    UsedFarmCells.Add(VARIABLE.gameObject);
                    switch (PlayerInHand)
                    {
                        case MainManager.Tools.WheatSeed:
                            Instantiate(WheatSeedPrefabG, VARIABLE.transform.position, quaternion.identity,Seeds.transform);
                            break;
                        case MainManager.Tools.CornSeed:
                            Instantiate(CornSeedPrefabG, VARIABLE.transform.position, quaternion.identity,Seeds.transform);
                            break;
                    }
                }
            }
        }
    }

    void HarvestPlants()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
            
        foreach (var VARIABLE in colliders)
        {
            if (VARIABLE.gameObject.tag == "SeedsSpawner")
            {
                if (UsedFarmCells.Contains(VARIABLE.gameObject))
                {
                    UsedFarmCells.Remove(VARIABLE.gameObject);
                }
            }

            if (VARIABLE.gameObject.tag == "WheatSeed")
            {
                Destroy(VARIABLE.gameObject);
                GameObject prefab=Instantiate(HarvestingEffectPrefabG, VARIABLE.transform.position, quaternion.identity);
                StartCoroutine(DestroyHarvestingParticleSystem(prefab));
                _MainManager.PlayerMoney += 20;

            }

            if (VARIABLE.gameObject.tag == "CornSeed")
            {
                Destroy(VARIABLE.gameObject);
                GameObject prefab=Instantiate(HarvestingEffectPrefabG, VARIABLE.transform.position, quaternion.identity);
                StartCoroutine(DestroyHarvestingParticleSystem(prefab));
                _MainManager.PlayerMoney += 50;
            }
        }

    }

    IEnumerator DestroyHarvestingParticleSystem(GameObject prefab)
    {
        yield return new WaitForSeconds(3);
        Destroy(prefab);
    }
    
}
