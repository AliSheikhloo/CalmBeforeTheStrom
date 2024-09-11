using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public MainManager.GunTypeE PlayerGun=MainManager.GunTypeE.Pistol;
    [SerializeField] private float MovingSpeedF;

    //[SerializeField] private float JumpForceF;

    private Rigidbody2D PlayerRB;
    public bool IsShiftPressedB = false;
    private bool IsGunPickedUpB = true;
    public bool IsShooting = false;
    public bool IsPlayerLookingLeft;
    //private bool IsJumpFinished = true;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        PlayerRB = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        BasicMovment();
    }
    

   
    void BasicMovment()
    {
        Vector3 transformLocalScaleV3 = transform.localScale;

        //Basic movment of player
        if (Input.GetKeyDown(KeyCode.LeftShift) && !IsShiftPressedB)
        {
            MovingSpeedF *= 2;
            IsShiftPressedB = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            IsShiftPressedB = false;
            MovingSpeedF /= 2;
        }

        if (Input.GetKey(KeyCode.A))
        {
            PlayerRB.AddForce(-transform.right * MovingSpeedF * Time.deltaTime * 50);
            if (!IsShooting)
            {
                transformLocalScaleV3.x = 1;
                IsPlayerLookingLeft = true;
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            PlayerRB.AddForce(-transform.up * MovingSpeedF * Time.deltaTime * 50);
        }

        if (Input.GetKey(KeyCode.D))
        {
            PlayerRB.AddForce(transform.right * MovingSpeedF * Time.deltaTime * 50);
            if (!IsShooting)
            {
                transformLocalScaleV3.x = -1;
                IsPlayerLookingLeft = false;
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            PlayerRB.AddForce(transform.up * MovingSpeedF * Time.deltaTime * 50);
        }
        
        transform.localScale = transformLocalScaleV3;

    }
}
