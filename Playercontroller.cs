using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    public Transform viewPoint;
    public float mouseSensitivity = 1f;
    private float verticalRotStore;
    private Vector2 mouseInput;
    private Vector3 moveDir, movement;
    public float moveSpeed = 5f, runspeed = 10f;
    private float activeMoveSpeed;
    public CharacterController charCon;
    private Camera cam;
    public bool invertLook;
    private float yVelocity;
    public float jumpForce = 12f, gravityMod = 2.5f;
    public Transform groundCheckPoint;
    private bool isGrounded;
    public LayerMask groundCheck;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        verticalRotStore += mouseInput.y;
        verticalRotStore = Mathf.Clamp(verticalRotStore, -60f, 60f);

        if (invertLook)
        {
            viewPoint.rotation = Quaternion.Euler(verticalRotStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);
        }
        else
        {
            viewPoint.rotation = Quaternion.Euler(-verticalRotStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);
        }
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift))
        {
            activeMoveSpeed = runspeed;
        }
        else
        {
            activeMoveSpeed = moveSpeed;
        }
        float yVelocity = movement.y;
        movement = ((transform.forward * moveDir.z) + (transform.right * moveDir.x)).normalized * activeMoveSpeed;
        movement.y = yVelocity;

        charCon.Move(movement * activeMoveSpeed * Time.deltaTime);
        movement.y += Physics.gravity.y * Time.deltaTime * gravityMod;

        if (charCon.isGrounded)
        {
            movement.y = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Cursor.lockState == CursorLockMode.None)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, .25f);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            movement.y = jumpForce;


        }


    }




    private void LateUpdate()
    {
        cam.transform.position = viewPoint.position;
        cam.transform.rotation = viewPoint.rotation;
    }

   
}