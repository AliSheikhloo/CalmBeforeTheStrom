using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    public PlayerController _PlayerController;
    public int PlayerMoney;
    
    public enum Tools
    {
        Pistol,
        Rifle,
        Shotgun,
        WheatSeed,
        CornSeed,
        sickle
    }
    public List<GameObject> ToolsList;

    public bool IsNightB;
    public static int CurrentDay;
    public int EnemySpawnRate;
    public bool isGameStarted;
    [SerializeField] private GameObject Enemy;
    [SerializeField] private GameObject[] EnemySpawns;
    
    [SerializeField] private ParticleSystem RainEffect;
    [SerializeField] private Animator GlobalLight;
    [SerializeField] private GameObject SpotLight;
    [SerializeField] private int NightTimeI;
    [SerializeField] private int DayTimeI;



    [SerializeField] private GameObject[] UiElements;

    [SerializeField] private Text[] Texts; 
    // Start is called before the first frame update
    public void StartGame()
    {
        Inventory.RifleBulletsI = 100;
        Inventory.ShotgunBulletsI = 10;
        Inventory.isRifleBought=true;
        Inventory.isShotgunBought = true;

        _PlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        //InstantiateTool(_PlayerController.PlayerInHand);
        Application.targetFrameRate = 120;
        StartCoroutine(NightDayCycle());
        
        UiElements[0].SetActive(false);
        UiElements[1].SetActive(true);
        isGameStarted = true;
        
    }

    private void Update()
    {
        Texts[0].text = Inventory.RifleBulletsI.ToString();
        Texts[1].text = Inventory.ShotgunBulletsI.ToString();
        Texts[2].text = Inventory.Wheats.ToString();
        Texts[3].text = Inventory.Corns.ToString();
        Texts[4].text = Inventory.Coins.ToString();
    }

    public void SwichTool(Tools NextTool)
    {
        Destroy(_PlayerController.transform.GetChild(2).gameObject);
        InstantiateTool(NextTool);
        _PlayerController.PlayerInHand = NextTool;
    }

    void InstantiateTool(Tools toolTypeE)
    {
        switch (toolTypeE)
        {
            case Tools.Pistol:
                Instantiate(ToolsList[0], _PlayerController.transform);
                break;
            case Tools.Rifle:
                Instantiate(ToolsList[1], _PlayerController.transform);
                break;
            case Tools.Shotgun:
                Instantiate(ToolsList[2], _PlayerController.transform);
                break;
            case Tools.WheatSeed:
                Instantiate(ToolsList[3], _PlayerController.transform);
                break;
            case Tools.CornSeed:
                Instantiate(ToolsList[4], _PlayerController.transform);
                break;
            case Tools.sickle:
                Instantiate(ToolsList[5], _PlayerController.transform);
                break;
        }
    }

    public void Buy(string obj)
    {
        switch (obj)
        {
            case "Rifle":
                SwichTool(Tools.Rifle);
                break;
            case "ShotGun":
                SwichTool(Tools.Shotgun);
                break;
            case "CornSeed":
                SwichTool(Tools.CornSeed);
                break;
            case "RifleBullet":
                Inventory.RifleBulletsI += 100;
                break;
            case "ShotgunBullet":
                Inventory.ShotgunBulletsI += 10;
                break;
            
        }
    }

    public void Sell(string obj)
    {
        switch (obj)
        {
            case "Wheats" :
                Inventory.Coins += Inventory.Wheats;
                Inventory.Wheats = 0;
                break;
            case "Corns" :
                Inventory.Coins += Inventory.Corns*2;
                Inventory.Corns = 0;
                break;

        }
    }

    IEnumerator NightDayCycle()
    {
        int time;
        if (IsNightB)
        {
            time = DayTimeI;
        }
        else
        {
            time = NightTimeI;
        }
        yield return new WaitForSeconds(time);
        if (!IsNightB)
        {
            SpotLight.SetActive(true);
            GlobalLight.SetTrigger("GoNight");
            SoundManager.instance.ThunderAndLightningSound();
            RainEffect.Play();
            StartCoroutine(SpawnEnemies(CurrentDay*2));
        }
        else
        {
            RainEffect.Stop();
            GlobalLight.SetTrigger("GoDay");
            CurrentDay++;
            yield return new WaitForSeconds(1);
            SpotLight.SetActive(false);
        }

        IsNightB = !IsNightB;
        StartCoroutine(NightDayCycle());
    }

    IEnumerator SpawnEnemies(int NumberOfEnemies)
    {
        for (int j = 0; j < NumberOfEnemies; j++)
        {
            for (int i = 0; i < EnemySpawnRate; i++)
            {
                yield return null;
            }

            Vector3 spawn = EnemySpawns[Random.Range(0, EnemySpawns.Length)].transform.position;
            Instantiate(Enemy, new Vector3(spawn.x, spawn.y + Random.Range(-5, 5)), quaternion.identity);

        }

    }
}



