using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject SellStoreHelp;
    [SerializeField] GameObject GunStoreHelp;
    public bool GameOverB = false;
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] ParticleSystem DeadEffect;
    public MainManager.Tools PlayerInHand = MainManager.Tools.WheatSeed;
    private MainManager _MainManager;

    private float CurrentSpeedF;
    [SerializeField] private float WalkingSpeedF;
    [SerializeField] private float RunningSpeedF;


    [SerializeField] private GameObject CornSeedPrefabG;
    [SerializeField] private GameObject WheatSeedPrefabG;

    [SerializeField] private GameObject Seeds;

    [SerializeField] private GameObject HarvestingEffectPrefabG;
    //[SerializeField] private float JumpForceF;

    private Rigidbody2D PlayerRB;
    private Animator _Animator;
    public bool IsShiftPressedB = false;
    public bool IsShooting = false;
    public bool IsPlayerLookingLeft;

    public int HealthI = 10;

    //private bool IsJumpFinished = true;

    // Start is called before the first frame update
    void Start()
    {
        
        _MainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
        PlayerRB = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();

    }

    private void FixedUpdate()
    {
        if (_MainManager.isGameStarted)
        {
            BasicMovment();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_MainManager.isGameStarted)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                _Animator.SetBool("Walk", true);
            }
            else
            {
                _Animator.SetBool("Walk", false);
            }

            if ((PlayerInHand == MainManager.Tools.WheatSeed || PlayerInHand == MainManager.Tools.CornSeed) &&
                Input.GetMouseButton(0))
            {
                PlantSeeds();
            }

            if (PlayerInHand == MainManager.Tools.sickle && Input.GetMouseButton(0))
            {
                HarvestPlants();
            }

            
            //gunning
            
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _MainManager.SwichTool(MainManager.Tools.Pistol);
                _Animator.SetTrigger("Pistol");
                SoundManager.instance.PlayerSFX.Stop();
            }
            
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                
                 _MainManager.SwichTool(MainManager.Tools.Rifle);
                 _Animator.SetTrigger("Rifle");
                  SoundManager.instance.PlayerSFX.Stop();
                
            }

            if (Input.GetKey(KeyCode.Alpha3))
            {
                
                _MainManager.SwichTool(MainManager.Tools.Shotgun);
                _Animator.SetTrigger("Shotgun");
                 SoundManager.instance.PlayerSFX.Stop();
                
            }

            
            //farming
            if (Input.GetKey(KeyCode.Alpha4))
            {
                _MainManager.SwichTool(MainManager.Tools.sickle);
                _Animator.SetTrigger("Sickle");
                SoundManager.instance.PlayerSFX.Stop();
            }

            if (Input.GetKey(KeyCode.Alpha5))
            {
                _MainManager.SwichTool(MainManager.Tools.WheatSeed);
                _Animator.SetTrigger("Wheat");
                SoundManager.instance.PlayerSFX.Stop();
            }

            if (Input.GetKey(KeyCode.Alpha6))
            {
                _MainManager.SwichTool(MainManager.Tools.CornSeed);
                _Animator.SetTrigger("Corn");
                SoundManager.instance.PlayerSFX.Stop();
            }

            
        }
    }

    void BasicMovment()
    {
        Vector3 transformLocalScaleV3 = transform.localScale;
        Vector2 MoveDir = new Vector2();
        MoveDir.x = Input.GetAxis("Horizontal");
        MoveDir.y = Input.GetAxis("Vertical");
        MoveDir.Normalize();
        //Basic movment of player
        if (!IsShooting)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                CurrentSpeedF = RunningSpeedF;
                IsShiftPressedB = true;
                _Animator.SetFloat("Run", 1.5f);
            }
            else
            {
                CurrentSpeedF = WalkingSpeedF;
                IsShiftPressedB = false;
                _Animator.SetFloat("Run", 1);
            }
        }
        else
        {
            CurrentSpeedF = WalkingSpeedF;
            _Animator.SetFloat("Run", 1);
        }

        if (MoveDir.x < 0)
        {
            if (!IsShooting)
            {
                transformLocalScaleV3.x = -1;
                IsPlayerLookingLeft = true;
            }

        }

        if (MoveDir.x > 0)
        {
            if (!IsShooting)
            {
                transformLocalScaleV3.x = 1;
                IsPlayerLookingLeft = false;
            }
        }




        PlayerRB.AddForce(MoveDir * CurrentSpeedF * Time.fixedDeltaTime * 50);


        transform.localScale = transformLocalScaleV3;

    }

    void PlantSeeds()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
        foreach (var VARIABLE in colliders)
        {
            FarmingController FC = VARIABLE.gameObject.GetComponent<FarmingController>();
            if (VARIABLE.gameObject.tag == "SeedsSpawner" && !FC.isPlant)
            {
                FC.isPlant = true;
                SoundManager.instance.PlaySound("Plant");
                switch (PlayerInHand)
                {
                    case MainManager.Tools.WheatSeed:
                        GameObject Temp1 = Pooling.instance.ReturnObject("WheatSeed");
                        Temp1.transform.position = VARIABLE.transform.position;
                        Temp1.transform.SetParent(Seeds.transform);
                        Temp1.SetActive(true);
                        FC.SeedType = "Wheat";
                        FC.daysToGrow = 1;
                        break;
                    case MainManager.Tools.CornSeed:
                        GameObject Temp2 = Pooling.instance.ReturnObject("CornSeed");
                        Temp2.transform.position = VARIABLE.transform.position;
                        Temp2.transform.SetParent(Seeds.transform);
                        Temp2.SetActive(true);
                        FC.SeedType = "Corn";
                        FC.daysToGrow = 2;
                        break;
                }

                FC.DayPlant = MainManager.CurrentDay;
            }
        }
    }

    void HarvestPlants()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

        foreach (var VARIABLE in colliders)
        {

            FarmingController FC = VARIABLE.gameObject.GetComponent<FarmingController>();
            if (VARIABLE.gameObject.tag == "SeedsSpawner" && FC.isGrown && FC.isPlant)
            {
                FC.isGrown = false;
                FC.isPlant = false;
            }

            if (VARIABLE.gameObject.tag == "Wheat")
            {
                SoundManager.instance.PlaySound("Plant");
                GameObject prefab = Instantiate(HarvestingEffectPrefabG, VARIABLE.transform.position,
                    quaternion.identity);
                StartCoroutine(DestroyHarvestingParticleSystem(prefab));
                Pooling.instance.BackObjectToRepository(VARIABLE.gameObject);
                Inventory.Wheats++;
            }

            if (VARIABLE.gameObject.tag == "Corn")
            {
                SoundManager.instance.PlaySound("Plant");
                GameObject prefab = Instantiate(HarvestingEffectPrefabG, VARIABLE.transform.position,
                    quaternion.identity);
                StartCoroutine(DestroyHarvestingParticleSystem(prefab));
                Pooling.instance.BackObjectToRepository(VARIABLE.gameObject);
                Inventory.Corns++;
            }
        }

    }

    IEnumerator DestroyHarvestingParticleSystem(GameObject prefab)
    {
        yield return new WaitForSeconds(3);
        Destroy(prefab);
    }

    public IEnumerator Damage()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        HealthI--;
        if (HealthI == 0)
        {
            GameOverB = true;
            GameOverPanel.SetActive(true);
            SoundManager.instance.PlaySound("Dead");
            Instantiate(DeadEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            _MainManager.gameObject.SetActive(false);
        }

        float mirror = -transform.localScale.x;
        PlayerRB.AddForce(((transform.right * mirror)) * 10, ForceMode2D.Impulse);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!_MainManager.IsNightB)
        {
            if (collision.name == "SellStore")
            {
                SellStoreHelp.SetActive(true);
                if (Input.GetKeyDown(KeyCode.K))
                {
                    _MainManager.Sell();
                }

            }

            if (collision.name == "GunStore")
            {
                GunStoreHelp.SetActive(true);


                if (Input.GetKeyDown(KeyCode.R))
                {
                    _MainManager.Buy("RifleAmmo");
                    _Animator.SetTrigger("Rifle");
                    SoundManager.instance.PlayerSFX.Stop();
                }

                if (Input.GetKey(KeyCode.H))
                {
                    _MainManager.Buy("ShotGunAmmo");
                    _Animator.SetTrigger("Shotgun");
                    SoundManager.instance.PlayerSFX.Stop();
                }

                
            }
        }
        else
        {
            GunStoreHelp.SetActive(false);
            SellStoreHelp.SetActive(false);
        }
    }



    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "GunStore")
        {
            GunStoreHelp.SetActive(false);
        }

        if (collision.name == "SellStore")
        {
            SellStoreHelp.SetActive(false);
        }
    }


}



public class Inventory
    {
        public static int RifleBulletsI;
        public static int ShotgunBulletsI;
        public static int Wheats;
        public static int Corns;
        public static int Coins;
        
    }

