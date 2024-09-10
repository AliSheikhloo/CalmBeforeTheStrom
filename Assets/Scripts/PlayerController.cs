using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float MovingSpeedF;

    //[SerializeField] private float JumpForceF;
    [SerializeField] private GameObject BulletPrefabG;
    [SerializeField] private float BulletShootingPowerF;


    private Rigidbody2D PlayerRB;
    private bool IsShiftPressedB = false;
    private bool IsGunPickedUpB = true;
    private bool IsShootingB = false;

    private bool IsPlayerLookingLeft;
    //private bool IsJumpFinished = true;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = 90;

    }

    // Update is called once per frame
    void Update()
    {
        BasicMovment();

        //Player facing system
        Vector3 transformLocalScaleV3 = transform.localScale;

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

        transform.localScale = transformLocalScaleV3;


        if (Input.GetKey(KeyCode.Space) && !IsShootingB)
        {
            StartCoroutine(Shoot(BulletPrefabG.GetComponent<BulletsController>().FireRateI));
            IsShootingB = true;
        }
        else
        {
            IsShootingB = false;
        }
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

    IEnumerator Shoot(float fireRate)
    {
        int framesPerBulletI=0;
         switch (fireRate)
        {
            case 1:
                framesPerBulletI = 560;
                break;
            case 2:
                framesPerBulletI = 270;
                break;
            case 3:
                framesPerBulletI = 180;
                break;
            case 4: framesPerBulletI = 90;
                break;
        }
        
        int framesPassedI = 0;
        while (Input.GetKey(KeyCode.Space) && IsGunPickedUpB)
        {
            float result = framesPassedI % framesPerBulletI;
            if (result ==0)
            {
                GameObject bullet;
                bullet = Instantiate(BulletPrefabG, transform.GetChild(0).transform.position,
                    Quaternion.Euler(0, 0, Random.Range(-10, 10)));
                bullet.GetComponent<BulletsController>().IsPlayerLookingLeft = IsPlayerLookingLeft;
            }
            framesPassedI++;
            yield return null;
        }
    }

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
