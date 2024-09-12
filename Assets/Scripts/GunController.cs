using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    [SerializeField] PlayerController _PlayerController;
    public int AccuracyI;
    public int FireRateI;

    [SerializeField] private string BulletPrefabS;
    private bool IsGunLoaded = true;

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
    public void Shooting()
    {
        if (_PlayerController.IsShiftPressedB) return;

        if (Input.GetKeyDown(KeyCode.Space) && IsGunLoaded)
        {

            switch (ThisGunType)
            {
                case GunTypeE.Pistol:
                    StartCoroutine(FirePistol());
                    break;
                case GunTypeE.Rifle:
                    _PlayerController.IsShooting = true;
                    StartCoroutine(FireRifle());
                    break;
                case GunTypeE.Shotgun:
                    StartCoroutine(FireShotgun());
                    IsGunLoaded = false;
                    break;

            }

        }


    }
    // Update is called once per frame
    void Update()
    {
        Shooting();
    }

    IEnumerator FirePistol()
    {

        InstantiateBullet();
        ThrowCartridge();
        yield return null;

    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator FireRifle()
    {
        while (Input.GetKey(KeyCode.Space))
        {
            InstantiateBullet();
            ThrowCartridge();
            yield return new WaitForSeconds(.1f);
            if (_PlayerController.IsShiftPressedB)
            {
                break;
            }

            yield return null;
        }
        _PlayerController.IsShooting = false;
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
        for (int i = 0; i < 800 * Time.deltaTime; i++)
        {
            yield return null;
        }
        ThrowCartridge();
        IsGunLoaded = true;
    }

    void InstantiateBullet()
    {
        
        GameObject Bullet = Pooling.instance.ReturnObject(BulletPrefabS);
        Bullet.transform.SetParent(null);
        Bullet.transform.position = transform.position;
        Bullet.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-AccuracyI, AccuracyI));
        Bullet.SetActive(true);
    }
    void ThrowCartridge()
    {
        print("cartridge");

        GameObject cartridge = Pooling.instance.ReturnObject("Cartridge");
        cartridge.transform.SetParent(null);
        cartridge.transform.position = transform.position;
        cartridge.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-10, 10));
        cartridge.SetActive(true);
        cartridge.GetComponent<Rigidbody2D>().AddForce(cartridge.transform.up * 20, ForceMode2D.Impulse);

    }
}
