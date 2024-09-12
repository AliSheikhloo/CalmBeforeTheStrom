using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

        //Player facing system
        Vector3 transformLocalScaleV3 = transform.localScale;
        if (PlayerRB.velocity.x != 0 && !IsShooting)
        {
            if (PlayerRB.velocity.x > 0)
            {
                transformLocalScaleV3.x = -1;
                IsPlayerLookingLeft = false;
            }
            else
            {
                transformLocalScaleV3.x = 1;
                IsPlayerLookingLeft = true;

            }
        }
        

        transform.localScale = transformLocalScaleV3;
        
    }

    private void FixedUpdate()
    {
        BasicMovment();
    }


    /*
     Jump isnt used for now
     IEnumerator Jump()
    {
        Vector3 playerPositionV3 = transform.position;
        PlayerRB.AddForce(transform.up*JumpForceF,ForceMode2D.Impulse);
        while(transform.position.y>playerPositionV3.y-.001f)
        {
            PlayerRB.AddForce(-transform.up*JumpForceF*4);
            yield return null;
        }

        Vector2 playerRbVelocityV2 = PlayerRB.velocity;
        playerRbVelocityV2.y = 0;
        PlayerRB.velocity = playerRbVelocityV2;
        IsJumpFinished = true;
    }*/

    void BasicMovment()
    {
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
        }

        if (Input.GetKey(KeyCode.S))
        {
            PlayerRB.AddForce(-transform.up * MovingSpeedF * Time.deltaTime * 50);
        }

        if (Input.GetKey(KeyCode.D))
        {
            PlayerRB.AddForce(transform.right * MovingSpeedF * Time.deltaTime * 50);
        }

        if (Input.GetKey(KeyCode.W))
        {
            PlayerRB.AddForce(transform.up * MovingSpeedF * Time.deltaTime * 50);
        }

        /*if (Input.GetKeyDown(KeyCode.Space)&& IsJumpFinished)
        {
            StartCoroutine(Jump());
            IsJumpFinished = false;
        }*/

    }
}
