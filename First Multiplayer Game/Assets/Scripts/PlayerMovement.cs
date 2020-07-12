using Photon.Pun;
using System;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    private Rigidbody rbPlayer;
    [SerializeField] private Camera cam;
    [SerializeField] private float movementSpeed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        if(cam.isActiveAndEnabled && !photonView.IsMine)
        {
            cam.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {   
        if(photonView.IsMine)
        {
            TakeInput();
        }
    }

    private void TakeInput()
    {
        /*
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical).normalized;

        rbPlayer.velocity = movement * movementSpeed * Time.deltaTime;
        */
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * movementSpeed * Time.deltaTime;
        }

    }
}
