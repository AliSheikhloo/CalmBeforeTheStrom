using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    public int AccuracyI;
    public int FireRateI;

    [SerializeField] private GameObject BulletPrefabG;
    [SerializeField] private GameObject CartridgeG;
    private bool IsGunLoaded=true;
    
    public enum GunTypeE
    {
        Pistol,
        Rifle,
        Shotgun
    }

    public GunTypeE ThisGunType;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& IsGunLoaded)
        {
            switch (ThisGunType)
            {
                case  GunTypeE.Pistol:
                    StartCoroutine(FirePistol());
                    break;
                case GunTypeE.Rifle:
                    StartCoroutine(FireRifle());
                    break;
                case GunTypeE.Shotgun:
                    StartCoroutine(FireShotgun());
                    IsGunLoaded = false;
                    break;

            }
            
        }
    }

    IEnumerator FirePistol()
    {
        while (Input.GetKey(KeyCode.Space)){
            InstantiateBullet();
            ThrowCartridge();
            while (Input.GetKey(KeyCode.Space))
            {
                yield return null;
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator FireRifle()
    {
        while (Input.GetKey(KeyCode.Space))
        {
            InstantiateBullet();
            ThrowCartridge();
            for (int i = 0; i < 5; i++)
            {
                yield return null;
            }
        }
    }

    IEnumerator FireShotgun()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                InstantiateBullet();
            }
            yield return null;
        }
        for (int i = 0; i < 800*Time.deltaTime; i++)
        {
            yield return null;
        }
        ThrowCartridge();
        IsGunLoaded = true;
    }

    void InstantiateBullet()
    {
        Instantiate(BulletPrefabG, transform.position, Quaternion.Euler(0, 0, Random.Range(-AccuracyI,AccuracyI)));
    }
    void ThrowCartridge()
    {
        print("cartridge");
        GameObject cartridge;
        cartridge=Instantiate(CartridgeG, transform.position, Quaternion.Euler(0,0,Random.Range(80,100)));
        cartridge.GetComponent<Rigidbody2D>().AddForce(cartridge.transform.up * 20,ForceMode2D.Impulse);
    }
}
