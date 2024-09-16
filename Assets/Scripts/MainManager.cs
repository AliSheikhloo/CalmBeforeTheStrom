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
    [SerializeField] private GameObject[] Enemies;

    [SerializeField] private GameObject[] EnemySpawns;

    [SerializeField] private ParticleSystem RainEffect;
    [SerializeField] private Animator GlobalLight;
    [SerializeField] private GameObject SpotLight;
    [SerializeField] private int DayTimeI;
    [SerializeField] private int NightTimeI;



    [SerializeField] private GameObject[] UiElements;

    [SerializeField] private Text[] Texts;
    // Start is called before the first frame update
    public void StartGame()
    {
        Cursor.visible = false;
        Inventory.RifleBulletsI = 0;
        Inventory.ShotgunBulletsI = 0;


        _PlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        //InstantiateTool(_PlayerController.PlayerInHand);
        Application.targetFrameRate = 60;
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
            case "RifleAmmo":
                SwichTool(Tools.Rifle);
                if (Inventory.Coins >= 80)
                {
                    
                    Inventory.RifleBulletsI += 40;
                    Inventory.Coins -= 80;
                }


                break;
            case "ShotGunAmmo":

                SwichTool(Tools.Shotgun);
                if (Inventory.Coins >= 100)
                {
                    
                    Inventory.ShotgunBulletsI += 30;
                    Inventory.Coins -= 100;
                }


                break;


        }
    }

    public void Sell()
    {

        Inventory.Coins += Inventory.Wheats;
        Inventory.Coins += Inventory.Corns * 2;
        Inventory.Wheats = 0;
        Inventory.Corns = 0;

    }

    IEnumerator NightDayCycle()
    {
        int time;
        if (IsNightB)
        {
            time = NightTimeI;
        }
        else
        {
            time = DayTimeI;
        }
        yield return new WaitForSeconds(time);
        if (!IsNightB)
        {
            StartCoroutine(SpawnEnemies((CurrentDay + 1) * 2));
            Texts[5].gameObject.SetActive(true);
            Texts[5].text = "Night " + (CurrentDay + 1);
            SpotLight.SetActive(true);
            GlobalLight.SetTrigger("GoNight");
            SoundManager.instance.ThunderAndLightningSound();
            RainEffect.Play();
        }
        else
        {
            RainEffect.Stop();
            GlobalLight.SetTrigger("GoDay");
            CurrentDay++;
            yield return new WaitForSeconds(1);
            SpotLight.SetActive(false);
            Texts[5].gameObject.SetActive(false);
        }

        IsNightB = !IsNightB;
        StartCoroutine(NightDayCycle());
    }

    IEnumerator SpawnEnemies(int NumberOfEnemies)
    {
        for (int j = 0; j < NumberOfEnemies; j++)
        {
            yield return new WaitForSeconds(EnemySpawnRate);
            Vector3 spawn = EnemySpawns[Random.Range(0, EnemySpawns.Length)].transform.position;
            Instantiate(Enemies[Random.Range(0, Enemies.Length)],
                new Vector3(spawn.x, spawn.y + Random.Range(-5, 5)), quaternion.identity);
            print("enemty");


        }

    }


    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0); //kill current process
    }
}



