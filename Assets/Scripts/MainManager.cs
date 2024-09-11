using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public PlayerController _PlayerController;
    public enum GunTypeE
    {
        Pistol,
        Rifle,
        Shotgun
    }
    
    public List<GameObject> Guns;
    // Start is called before the first frame update
    void Start()
    {
        _PlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        InstantiateGun(_PlayerController.PlayerGun);
    }


    public void SwichGun(GunTypeE nextGun)
    {
        Destroy(_PlayerController.transform.GetChild(2).gameObject);
        InstantiateGun(nextGun);
    }

    void InstantiateGun(GunTypeE gunTypeE)
    {
        switch (gunTypeE)
        {
            case GunTypeE.Pistol:
                Instantiate(Guns[0], _PlayerController.transform);
                break;
            case GunTypeE.Rifle:
                Instantiate(Guns[1], _PlayerController.transform);
                break;
            case GunTypeE.Shotgun:
                Instantiate(Guns[2], _PlayerController.transform);
                break;
        }
    }
}
