using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooling : MonoBehaviour
{
    public static Pooling instance;
    public Prefab PistolBullet;
    public Prefab RifleBullet;
    public Prefab ShotgunBullet;
    public Prefab Cartridge;
    public Prefab CornSeed;
    public Prefab Corn;
    public Prefab WheatSeed;
    public Prefab Wheat;
    public Prefab HitEffect;
    // Start is called before the first frame update
    void Start()
    {
        instance = GetComponent<Pooling>();

        GameObject Temp;
        for (int i = 0; i < PistolBullet.Count; i++)
        {
            Temp = Instantiate(PistolBullet.prefab, transform);
            Temp.SetActive(false);
            PistolBullet.Objects.Add(Temp);
        }
        for (int i = 0; i < RifleBullet.Count; i++)
        {
            Temp = Instantiate(RifleBullet.prefab, transform);
            Temp.SetActive(false);
            RifleBullet.Objects.Add(Temp);
        }
        for (int i = 0; i < ShotgunBullet.Count; i++)
        {
            Temp = Instantiate(ShotgunBullet.prefab, transform);
            Temp.SetActive(false);
            ShotgunBullet.Objects.Add(Temp);
        }
        for (int i = 0; i < Cartridge.Count; i++)
        {
            Temp = Instantiate(Cartridge.prefab, transform);
            Temp.SetActive(false);
            Cartridge.Objects.Add(Temp);
        }
        for (int i = 0; i < CornSeed.Count; i++)
        {
            Temp = Instantiate(CornSeed.prefab, transform);
            Temp.SetActive(false);
            CornSeed.Objects.Add(Temp);
        }
        for (int i = 0; i < Corn.Count; i++)
        {
            Temp = Instantiate(Corn.prefab, transform);
            Temp.SetActive(false);
            Corn.Objects.Add(Temp);
        }
        for (int i = 0; i < Wheat.Count; i++)
        {
            Temp = Instantiate(Wheat.prefab, transform);
            Temp.SetActive(false);
            Wheat.Objects.Add(Temp);

        }
        for (int i = 0; i < WheatSeed.Count; i++)
        {
            Temp = Instantiate(WheatSeed.prefab, transform);
            Temp.SetActive(false);
            WheatSeed.Objects.Add(Temp);

        }
        for (int i = 0; i < HitEffect.Count; i++)
        {
            Temp = Instantiate(HitEffect.prefab, transform);
            Temp.SetActive(false);
            HitEffect.Objects.Add(Temp);

        }
    }
    public GameObject ReturnObject(string ObjectName)
    {
        Prefab UsedPrefab = null;
        switch (ObjectName)
        {
            case "PistolBullet":
                UsedPrefab = PistolBullet;
                break;
            case "RifleBullet":
                UsedPrefab = RifleBullet;
                break;
            case "ShotgunBullet":
                UsedPrefab = ShotgunBullet;
                break;
            case "Cartridge":
                UsedPrefab = Cartridge;
                break;
            case "CornSeed":
                UsedPrefab = CornSeed;
                break;
            case "Corn":
                UsedPrefab = Corn;
                break;
            case "Wheat":
                UsedPrefab = Wheat;
                break;
            case "WheatSeed":
                UsedPrefab = WheatSeed;
                break;
            case "HitEffect":
                UsedPrefab = HitEffect;
                break;
        }
        for (int i = 0; i < UsedPrefab.Count; i++)
        {
            if (!UsedPrefab.Objects[i].gameObject.activeSelf)
            {
                return UsedPrefab.Objects[i];
            }
        }
        GameObject NewPrefab = Instantiate(UsedPrefab.prefab,null);
        UsedPrefab.Count++;
        UsedPrefab.Objects.Add(NewPrefab);
        return NewPrefab;
    }
    public void BackObjectToRepository(GameObject Obj)
    {
        Obj.transform.SetParent(transform);
        Obj.transform.localPosition = Vector3.zero;
        Obj.transform.rotation = Quaternion.identity;
        Obj.SetActive(false);
    }
    
    
}
[System.Serializable]
public class Prefab
{
    public GameObject prefab;
    public int Count;
    public List<GameObject> Objects;
}
