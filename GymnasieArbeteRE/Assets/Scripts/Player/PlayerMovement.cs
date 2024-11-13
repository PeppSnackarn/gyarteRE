using UnityEngine;
using UnityEngine.Serialization;


public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public float groundAcceleration = 1000f;
    public float airAcceleration = 500f;
    [SerializeField] private float movingDrag = 0.2f;
    [SerializeField] private float stopDrag = 10f;
    public float jumpForce = 1000f;
    public float cameraSensitivity = 1f;
    public float maxCameraAngle = 80f;
    public float extraGroundedHeight = 2f;
    public float maxHealth = 100f;
    public float maxVelocity = 10f;
    
    [Header("Player Components")]
    public Camera playerCam;
    public MeshRenderer playerMesh;
    public Transform orientation;
    [HideInInspector] public Rigidbody rb;
    
    [Header("Player Attributes")]
    public float currentHealth;
    public bool bIsMoving;
    
    //Input
    [HideInInspector] public Player_IA InputAction;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private float yRotation = 0f;
    private void Start()
    {
        //Init
        InputAction = new Player_IA();
        InputAction.Enable();

        InputAction.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        InputAction.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
       
        InputAction.Player.Camera.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        InputAction.Player.Camera.canceled += ctx => lookInput = Vector2.zero;
       
        InputAction.Player.Jump.performed += ctx => HandleJump();

        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        
        //Default settings
        Cursor.lockState = CursorLockMode.Locked;

    }

    void Update()
    { 
        orientation.rotation = Quaternion.Euler(0, playerCam.transform.rotation.y, 0);
        
        // Function calls
        HandleMovement();
        HandleDrag();
        HandleCameraMovement();
    }

    public bool bIsGrounded() // works
    {
        Vector3 lineStart = playerMesh.transform.position;
        Vector3 lineEnd = -playerMesh.transform.up;
        bool grounded = Physics.Raycast(lineStart, lineEnd, playerMesh.localBounds.size.y / 2 + extraGroundedHeight);
        return grounded;
    }
    void HandleMovement()
    {
        Vector3 forward = playerCam.transform.forward;
        forward.y = 0f;
        Vector3 right = playerCam.transform.right;
        right.y = 0f;
        Vector3 moveVector = (forward * moveInput.y + right * moveInput.x);
        if (bIsGrounded())
        {
            rb.AddForce(moveVector.normalized * (Time.deltaTime * groundAcceleration), ForceMode.Force); // X, Z
        }
        else
        {
            rb.AddForce(moveVector.normalized * (Time.deltaTime * airAcceleration), ForceMode.Force); // X, Z
        }
        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    void HandleDrag()
    {
        if (InputAction.Player.Movement.ReadValue<Vector2>() == Vector2.zero && bIsGrounded())
        {
            bIsMoving = false;
            rb.drag = stopDrag;
        }
        else
        {
            bIsMoving = true;
            rb.drag = movingDrag;
        }
    }
    void HandleJump()
    {
        Vector3 up = new Vector3(0, 1, 0);
        rb.AddForce(up * jumpForce, ForceMode.Impulse);
    }
    void HandleCameraMovement()
    {
        Vector2 mouseDelta = lookInput * cameraSensitivity;
        xRotation -= mouseDelta.y;
        yRotation += mouseDelta.x;
        xRotation = Mathf.Clamp(xRotation, -maxCameraAngle, maxCameraAngle);
        
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
