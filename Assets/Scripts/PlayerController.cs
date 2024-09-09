using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float MovingSpeedF;

    private Rigidbody2D PlayerRB;
    private bool IsShiftPressedB=false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = 120;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !IsShiftPressedB)
        {
            MovingSpeedF *= 2;
            IsShiftPressedB = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            IsShiftPressedB = false;
            MovingSpeedF /= 2;
        }

        if (Input.GetKey(KeyCode.A))
        {
            PlayerRB.AddForce(-transform.right * MovingSpeedF*Time.deltaTime*50);
        }

        if (Input.GetKey(KeyCode.S))
        {
            PlayerRB.AddForce(-transform.up * MovingSpeedF*Time.deltaTime*50);
        }

        if (Input.GetKey(KeyCode.D))
        {
            PlayerRB.AddForce(transform.right * MovingSpeedF*Time.deltaTime*50);
        }

        if (Input.GetKey(KeyCode.W))
        {
            PlayerRB.AddForce(transform.up * MovingSpeedF*Time.deltaTime*50);
        }
    }
}
