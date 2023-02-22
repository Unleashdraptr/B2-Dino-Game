using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float Speed;
    public float lookSpeed;
    public float JumpSpeed;

    public LayerMask FloorMask;
    public Transform Feet;
    public Transform PlayerCamera;
    public Rigidbody rb;
    Vector3 PlayerMoveInput;
    Vector2 CameraMoveInput;
    float RotX;
    public float Stamina = 100;
    public bool Dash;
    public bool Sneaking;
    public bool Camoflauged;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = transform.GetComponent<Rigidbody>();
    }
    void Update()
    {
        //Gets the movement input and then runs its function
        PlayerMoveInput = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        MovePlayer();
        //Gets the camera's input and run its function
        CameraMoveInput = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        MovePlayerCamera();
        //Stamina Ending and Recharging
        if(Stamina <= 0)
        {
            Dash = false;
        }
        if(Stamina < 100 && Dash == false)
        {
            Stamina += 5 * Time.deltaTime;
        }
    }
    void MovePlayer()
    {
        //Total Move Direction for the frame
        Vector3 MoveDir = transform.TransformDirection(PlayerMoveInput) * Speed;
        //Jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Physics.CheckSphere(Feet.position, 0.1f, FloorMask))
            {
                rb.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
            }
        }
        //Dash button
        if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > 20)
        {
            //Flips the input
            if (Dash == false)
            {
                Dash = true;
            }
            else
                Dash = false;
        }
        else if (Input.GetKey(KeyCode.LeftControl) == true)
        {
            Sneaking = true;
            MoveDir.x /= 2;
            MoveDir.z /= 2;
        }
        else
        {
            Sneaking = false;
            Camoflauged = false;

        }
        //If its true then increase speed and lower stamina
        if (Dash == true)
        {
            MoveDir.x *= 2;
            MoveDir.z *= 2;
            Stamina -= 10 * Time.deltaTime;
        }
        //Calculates and then applies then input with rigidbody's Physics
        rb.velocity = new(MoveDir.x, rb.velocity.y, MoveDir.z);
    }
    void MovePlayerCamera()
    {
        //Gets the Y axis of the camera input
        RotX -= CameraMoveInput.y * lookSpeed;
        //Applies it to the camera and then to the player
        transform.Rotate(0, CameraMoveInput.x * lookSpeed, 0);
        PlayerCamera.transform.localRotation = Quaternion.Euler(RotX, CameraMoveInput.x, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Sneakable") && Sneaking == true)
        {
            Camoflauged = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sneakable") && Sneaking == true)
        {
            Camoflauged = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sneakable"))
        {
            Camoflauged = false;
        }
    }
}
