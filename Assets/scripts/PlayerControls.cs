using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float mouseSensitivity = 2f;

    public Transform cameraTransform; // Kamera Transform'u
    private CharacterController controller;
    private Animator animator;

    private Vector3 moveDirection;
    private float cameraRotationX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked; // Mouse'u ekran ortasýna kilitle
    }

    void Update()
    {
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        
        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -60f, 60f);
        cameraTransform.localRotation = Quaternion.Euler(cameraRotationX, 0f, 0f);

        
        transform.Rotate(Vector3.up * mouseX);

        
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 moveInput = new Vector3(moveX, 0, moveZ).normalized;
        moveDirection = transform.TransformDirection(moveInput) * moveSpeed;

        
        if (animator != null)
        {
            animator.SetFloat("speed", moveInput.magnitude);
        }

        
        controller.Move(moveDirection * Time.deltaTime);
        
    }
    void FixedUpdate()
    {
        
        Vector3 position = transform.position;
        position.y = 0f; 
        transform.position = position;
    }
}
