using System;
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
    public float maxVelocity = 10f;
    
    [Header("Player Components")]
    public Camera playerCam;
    public MeshRenderer playerMesh;
    public Transform orientation;
    private Rigidbody rb;
    
    [Header("Player Attributes")]
    public bool bIsMoving;
    
    //Input
    private Player_IA InputAction = GameManager.Instance.playerInput;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float xRotation = 0f;
    private float yRotation = 0f;

    #region Properties

    public Rigidbody rigidbody => rb;

    #endregion
    private void Start()
    {
        //Init
        //InputAction = new Player_IA();
        InputAction.Enable();

        InputAction.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        InputAction.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
       
        InputAction.Player.Camera.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        InputAction.Player.Camera.canceled += ctx => lookInput = Vector2.zero;
       
        InputAction.Player.Jump.performed += ctx => HandleJump();

        rb = GetComponent<Rigidbody>();
    }

    private void OnDestroy() // called when unloaded
    {
        InputAction.Player.Movement.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        InputAction.Player.Movement.canceled -= ctx => moveInput = Vector2.zero;
       
        InputAction.Player.Camera.performed -= ctx => lookInput = ctx.ReadValue<Vector2>();
        InputAction.Player.Camera.canceled -= ctx => lookInput = Vector2.zero;
       
        InputAction.Player.Jump.performed -= ctx => HandleJump();
    }

    void Update()
    { 
        orientation.rotation = Quaternion.Euler(0, playerCam.transform.rotation.y, 0);
        
        // Function calls
        HandleMovement();
        HandleDrag();
        HandleCameraMovement();
    }

    public bool BIsGrounded() // works
    {
        Transform meshTransform = playerMesh.transform;
        Vector3 lineStart = meshTransform.position;
        Vector3 lineEnd = -meshTransform.up;
        bool grounded = Physics.Raycast(lineStart, lineEnd, playerMesh.localBounds.size.y / 2 + extraGroundedHeight);
        return grounded;
    }
    void HandleMovement()
    {
        Transform camTransform = playerCam.transform;
        Vector3 forward = camTransform.forward;
        forward.y = 0f;
        Vector3 right = camTransform.right;
        right.y = 0f;
        Vector3 moveVector = (forward * moveInput.y + right * moveInput.x);
        if (BIsGrounded())
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
        if (InputAction.Player.Movement.ReadValue<Vector2>() == Vector2.zero && BIsGrounded())
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
        if (rb)
        {
            Vector3 up = new Vector3(0, 1, 0);
            rb.AddForce(up * jumpForce, ForceMode.Impulse);
        }
    }
    void HandleCameraMovement()
    {
        if (GameManager.Instance.currentState != GameManager.gameState.PauseState)
        {
            Vector2 mouseDelta = lookInput * cameraSensitivity;
            xRotation -= mouseDelta.y;
            yRotation += mouseDelta.x;
            xRotation = Mathf.Clamp(xRotation, -maxCameraAngle, maxCameraAngle);
        
            playerCam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }
    }
}
