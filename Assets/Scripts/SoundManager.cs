using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("AudioClips")]
    [SerializeField] private AudioClip Shotgun;
    [SerializeField] private AudioClip Pistol;
    [SerializeField] private AudioClip Rifle;
    [SerializeField] private AudioClip Plant;
    [SerializeField] private AudioClip Dead;
    public static SoundManager instance;
    public AudioSource PlayerSFX;
    public AudioSource WalkSFX;
    public AudioSource ThunderAndLightning;
    [SerializeField] private Rigidbody2D Player;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        WalkSound();
    }
    public void PlaySound(string SoundName)
    {
        PlayerSFX.Stop();
        switch (SoundName)
        {
            case "Shotgun":
                PlayerSFX.clip = Shotgun;
                PlayerSFX.loop = false;
                PlayerSFX.Play();
                break;
            case "Pistol":
                PlayerSFX.clip = Pistol;
                PlayerSFX.loop = false;
                PlayerSFX.Play();
                break;
            case "Rifle":
                PlayerSFX.clip = Rifle;
                PlayerSFX.loop = true;
                PlayerSFX.Play();
                break;
            case "Plant":
                print(SoundName);
                PlayerSFX.clip = Plant;
                PlayerSFX.loop = false;
                PlayerSFX.Play();
                break;
            case "Dead":
                print(SoundName);
                PlayerSFX.clip = Dead;
                PlayerSFX.loop = false;
                PlayerSFX.Play();
                break;
        }
    }
    void WalkSound()
    {
        if (Player.velocity.magnitude > 0.1f && !Player.GetComponent<PlayerController>().IsShooting)
        {
            if (!WalkSFX.isPlaying)
            {
                
                WalkSFX.Play();
            }
        }
        else
        {
            WalkSFX.Stop();
        }
    }
    public void ThunderAndLightningSound()
    {
        ThunderAndLightning.Play();
    }
    
}
