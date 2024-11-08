using UnityEngine;
using UnityEngine.Serialization;


public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 1000;
    public float jumpForce = 1000;
    public float cameraSensitivity = 1f;
    public float maxCameraAngle = 80f;
    
    [FormerlySerializedAs("PlayerCam")] [Header("Player Attributes")]
    public Camera playerCam;
    [FormerlySerializedAs("PlayerMesh")] public MeshRenderer playerMesh;

    private Rigidbody rb;
    
    //Input
    private Player_IA InputAction;
    private Vector2 movementVector2;
    private Vector2 lookVector2;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private void Start()
    {
        //Init
        InputAction = new Player_IA();
        InputAction.Enable();

        InputAction.Player.Movement.performed += ctx => movementVector2 = ctx.ReadValue<Vector2>();
        InputAction.Player.Movement.canceled += ctx => movementVector2 = Vector2.zero;
        InputAction.Player.Camera.performed += ctx => lookVector2 = ctx.ReadValue<Vector2>();
        InputAction.Player.Camera.canceled += ctx => lookVector2 = Vector2.zero;

        rb = GetComponent<Rigidbody>();
        
        //Default settings
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    {
       HandleMovement();
       HandleCameraMovement();
    }

    bool bIsGrounded()
    {
        bool grounded = Physics.Raycast(transform.position, -transform.up * playerMesh.localBounds.size.y / 2);
        return grounded;
    }
    void HandleMovement()
    {
        int jumpValue = 0;
        if (InputAction.Player.Jump.IsPressed() && bIsGrounded())
        {
            jumpValue = 1;
        }
        Vector3 moveVector = new Vector3(movementVector2.x * moveSpeed, jumpValue * jumpForce, movementVector2.y * moveSpeed);
        rb.AddForce(moveVector * (playerCam.transform.rotation.x * Time.deltaTime), ForceMode.Acceleration); // Rotation doesnt work properly, look into it
    }
    void HandleCameraMovement()
    {
        Vector2 mouseDelta = lookVector2 * cameraSensitivity;
        xRotation -= mouseDelta.y;
        yRotation += mouseDelta.x;
        xRotation = Mathf.Clamp(xRotation, -maxCameraAngle, maxCameraAngle);
        
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
