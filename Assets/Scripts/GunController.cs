using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    [SerializeField] PlayerController _PlayerController;

    public int AccuracyI;
    public int RecoilI;

    [SerializeField] private string BulletPrefabS;
    [SerializeField] private Animator CameraAnimator;
    [SerializeField] private Animator FireFlashAnimator;

    private bool IsGunLoaded = true;

    

    public MainManager.GunTypeE ThisGunType;

    // Start is called before the first frame update
    void Start()
    {
        CameraAnimator = Camera.main.GetComponent<Animator>();
        FireFlashAnimator = GameObject.Find("FireFlash").GetComponent<Animator>();
        _PlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    public void Shooting()
    {
//        if (_PlayerController.IsShiftPressedB) return;

        if (Input.GetKeyDown(KeyCode.Space) && IsGunLoaded)
        {

            switch (ThisGunType)
            {
                case MainManager.GunTypeE.Pistol:
                    StartCoroutine(FirePistol());
                    break;
                case MainManager.GunTypeE.Rifle:
                    _PlayerController.IsShooting = true;
                    StartCoroutine(FireRifle());
                    break;
                case MainManager.GunTypeE.Shotgun:
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
        Recoil();
        CameraAnimator.SetBool("CamShakeBool",true);
        CameraAnimator.SetBool("CamShakeBool",false);
        FireFlashAnimator.SetTrigger("Shoot");
        FireFlashAnimator.SetBool("IsShooting",false);
        yield return null;

    }

    // ReSharper disable Unity.PerformanceAnalysis
    IEnumerator FireRifle()
    {
        while (Input.GetKey(KeyCode.Space))
        {
            InstantiateBullet();
            ThrowCartridge();
            CameraAnimator.SetBool("CamShakeBool",true);
            FireFlashAnimator.SetBool("IsShooting",true);
            FireFlashAnimator.GetComponent<SpriteRenderer>().sortingOrder = 1;
            Recoil();
            yield return new WaitForSeconds(.1f);
            if (_PlayerController.IsShiftPressedB)
            {
                break;
            }

            yield return null;
        }
        CameraAnimator.SetBool("CamShakeBool",false);
        FireFlashAnimator.SetBool("IsShooting",false);
        FireFlashAnimator.GetComponent<SpriteRenderer>().sortingOrder = -1;
        _PlayerController.IsShooting = false;
    }

    IEnumerator FireShotgun()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                InstantiateBullet();
                Recoil();
                CameraAnimator.SetTrigger("CamShakeTrigger");
                CameraAnimator.SetBool("CamShakeBool",false);
                FireFlashAnimator.SetTrigger("Shoot");
                FireFlashAnimator.SetBool("IsShooting",false);
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

    void Recoil()
    {
        int mirror = 1;
        if (_PlayerController.IsPlayerLookingLeft)
        {
            mirror = -1;
        }
        _PlayerController.GetComponent<Rigidbody2D>().AddForce(-_PlayerController.transform.right*RecoilI*mirror,ForceMode2D.Impulse);
    }
}
