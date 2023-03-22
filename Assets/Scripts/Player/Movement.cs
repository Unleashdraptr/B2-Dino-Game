using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IDataHandler
{
    //General Player settings
    public float Speed;
    public float lookSpeed;
    public float JumpSpeed;
    public DialogueManager Dialogue;
    public float Health;

    //Getting the camera and where the character's physics body and feet gameobject. Also what layer the feet will interact with.
    public LayerMask FloorMask;
    public Transform Feet;
    public Transform PlayerCamera;
    public Rigidbody rb;
    //Players input
    Vector3 PlayerMoveInput;
    Vector2 CameraMoveInput;
    GameObject Prompt;

    //Players stamina (Removing infinite dashing)
    public float Stamina = 100;

    //Bools to keep track what state they are currently in
    public bool Camouflaged;
    float RotX;
    public enum MoveState {IDLE, WALK, DASH, SNEAK, DEATH};
    public MoveState moveState;

    public void LoadData(GameData data)
    {
        transform.SetPositionAndRotation(data.Position, data.Rotation);
        Stamina = data.Stamina;
    }
    public void SaveData(ref GameData data)
    {
        data.Position = transform.position;
        data.Rotation = transform.rotation;
        data.Stamina = Stamina;
    }
    private void Start()
    {
        //Set the state to idle
        moveState = MoveState.IDLE;
        //Hide the cursor and get the rigidbody on the player when first starting
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = transform.GetComponent<Rigidbody>();
    }
    //Update once a frame
    void Update()
    {
        if (GameUIManager.Pause == false)
        {
            if (Prompt != null)
            {
                if (Prompt.transform.GetChild(0).GetChild(0).gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.E))
                {
                    Dialogue.StartDialogue(Prompt);
                }
            }
            //Gets the movement input and then runs its function
            PlayerMoveInput = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Checks if the player is moving and if not to go to Idle
            if (PlayerMoveInput.Equals(default))
            {
                moveState = MoveState.IDLE;
            }
            //Gets the camera's input and run its function
            CameraMoveInput = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            MovePlayerCamera();
            //Stamina Ending and Recharging
            if (Stamina <= 0)
            {
                //Checks what the player is currently doing to get the correct state
                if (PlayerMoveInput.Equals(default))
                {
                    moveState = MoveState.IDLE;
                }
                else
                    moveState = MoveState.WALK;
            }
            //Recharging if not running or is not currently full
            if (Stamina < 100 && moveState != MoveState.DASH)
            {
                Stamina += 5 * Time.deltaTime;
            }
            //Jump Function
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Check if the feet GameObject are on the ground that has a layer called 'Ground'
                if (Physics.CheckSphere(Feet.position, 0.1f, FloorMask))
                {
                    Jump();
                }
            }
            //Dash Function
            if (Input.GetKeyDown(KeyCode.LeftShift) && Stamina > 20)
            {
                //checks if the player is walking or not
                if (moveState == MoveState.WALK)
                {
                    moveState = MoveState.DASH;
                }
                else
                    moveState = MoveState.WALK;
            }
            //If they arent dashing, then check if they're sneaking
            else if (Input.GetKey(KeyCode.LeftControl) == true)
            {
                moveState = MoveState.SNEAK;
            }
            //If not then reset the sneak bools
            else
            {
                if (PlayerMoveInput.Equals(default))
                {
                    moveState = MoveState.IDLE;
                }
                else if (moveState != MoveState.DASH)
                {
                    moveState = MoveState.WALK;
                }
            }
        }
        else
        {
            rb.velocity = new(0, 0, 0);
            PlayerMoveInput.Equals(default);
        }
    }
    //FixedUpdate can run multiple times per frame
    private void FixedUpdate()
    {
        if (GameUIManager.Pause == false)
        {
            //Run the Physics calculations
            MovePlayer();
        }
    }
    void Jump()
    {
        rb.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
    }
    void MovePlayer()
    {
        //Total Move Direction for the frame
        Vector3 MoveDir = transform.TransformDirection(PlayerMoveInput) * Speed;
        //Setting the Y velocity aswell
        MoveDir.y = rb.velocity.y;
        //If its true then increase speed and lower stamina
        if (moveState == MoveState.DASH)
        {
            MoveDir.x *= 2;
            MoveDir.z *= 2;
            Stamina -= 10 * Time.deltaTime;
        }
        if(moveState == MoveState.SNEAK)
        {
            MoveDir.x /= 2;
            MoveDir.z /= 2;
        }
        //Calculates and then applies then input with rigidbody's Physics
        rb.MovePosition(transform.position + MoveDir * Time.deltaTime);
    }
    void MovePlayerCamera()
    {
        //Gets the Y axis of the camera input
        RotX -= CameraMoveInput.y * lookSpeed;
        //Applies it to the camera and then to the player
        transform.Rotate(0, CameraMoveInput.x * lookSpeed, 0);
        PlayerCamera.transform.localRotation = Quaternion.Euler(RotX, CameraMoveInput.x, 0);
    }

    void CheckHealth()
    {
        if (Health <= 0)
        {
            GetComponent<Movement>().moveState = MoveState.DEATH;
            GameObject.Find("Player UI").GetComponent<GameUIManager>().DeathScreen.SetActive(true);
        }
    }

    //Both Enter & Stay check if the player is currently standing on a Camoflaugeable object
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sneakable") && moveState == MoveState.SNEAK)
        {
            Camouflaged = true;
        }
        else if (collision.gameObject.CompareTag("NPC"))
        {
            Prompt = collision.gameObject;
            Prompt.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
        else
            Camouflaged = false;
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Sneakable") && moveState == MoveState.SNEAK)
        {
            Camouflaged = true;
        }
        else if (collision.gameObject.CompareTag("NPC"))
        {
            Prompt = collision.gameObject;
            Prompt.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        }
        else
            Camouflaged = false;
    }
    //Once they leave it then they are no longer camoflauged
    private void OnCollisionExit(Collision collision)
    {
        Camouflaged = false;
        if (collision.gameObject.CompareTag("NPC"))
        {
            Prompt.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
            Prompt = null;
        }
    }
}
