using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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

    [SerializeField] private ParticleSystem RainEffect;
    [SerializeField] private Light2D GlobalLight;
    // Start is called before the first frame update
    void Start()
    {
        _PlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        //InstantiateTool(_PlayerController.PlayerInHand);
        Application.targetFrameRate = 120;
        StartCoroutine(NightDayCycle());
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
        }
    }

    IEnumerator NightDayCycle()
    {
        int time;
        if (IsNightB)
        {
            time = 5;
        }
        else
        {
            time = 2;
        }
        yield return new WaitForSeconds(time);
        if (!IsNightB)
        {
            for (int i = 0; i < 95; i++)
            {
                GlobalLight.intensity -= .01f;
                yield return null;
            }
            RainEffect.Play();
        }
        else
        {
            RainEffect.Stop();
            for (int i = 0; i < 95; i++)
            {
                GlobalLight.intensity += .01f;
                yield return null;
            }
            CurrentDay++;
        }

        IsNightB = !IsNightB;
        StartCoroutine(NightDayCycle());
    }
}

