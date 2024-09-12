using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private Vector3 Offset;
    [SerializeField] private float CameraSmooth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, Player.position + Offset, CameraSmooth * Time.deltaTime);
    }
}
