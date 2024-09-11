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
    // Start is called before the first frame update
    void Start()
    {
        instance = GetComponent<Pooling>();

        for (int i = 0; i < PistolBullet.Count; i++)
        {
            GameObject Temp = Instantiate(PistolBullet.prefab, transform);
            Temp.SetActive(false);
            PistolBullet.Objects.Add(Temp);
        }
        for (int i = 0; i < RifleBullet.Count; i++)
        {
            GameObject Temp = Instantiate(RifleBullet.prefab, transform);
            Temp.SetActive(false);
            RifleBullet.Objects.Add(Temp);
        }
        for (int i = 0; i < ShotgunBullet.Count; i++)
        {
            GameObject Temp = Instantiate(ShotgunBullet.prefab, transform);
            Temp.SetActive(false);
            ShotgunBullet.Objects.Add(Temp);
        }
        for (int i = 0; i < Cartridge.Count; i++)
        {
            GameObject Temp = Instantiate(Cartridge.prefab, transform);
            Temp.SetActive(false);
            Cartridge.Objects.Add(Temp);
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
        Obj.SetActive(false);
        Obj.transform.SetParent(transform);
        Obj.transform.localPosition = Vector3.zero;
        Obj.transform.rotation = Quaternion.identity;
        
    }
    
    
}
[System.Serializable]
public class Prefab
{
    public GameObject prefab;
    public int Count;
    public List<GameObject> Objects;
}
