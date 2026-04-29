using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class FPSController : MonoBehaviour
{
    #region General Variables
    [Header("Health")]
    [SerializeField] int maxHealth = 100;
    int currentHealth;

    [Header("Movement & Look")]
    [SerializeField] GameObject camHolder; //Ref al objeto que tiene como hijo la cámara (rota por la cámara)
    [SerializeField] float speed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float crouchSpeed = 3f;
    [SerializeField] float maxForce = 1f; //Fuerza máxima de aceleración
    [SerializeField] float sensitivity = 0.1f; //Sensibilidad para el input de look

    [Header("Jump & GroundCheck")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] bool isGrounded;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 0.3f;
    [SerializeField] LayerMask groundLayer;


    [Header("Player State Bools")]
    [SerializeField] bool isSprinting;
    [SerializeField] bool isCrouching;
    bool isDead = false;
    #endregion

    //Variables de referencia privadas
    Rigidbody rb; //Ref al rigidbody del player
    Animator anim; //Ref al animator del player

    public GameObject container; //pausa

    //Variables para el input
    Vector2 moveInput;
    Vector2 lookInput;
    float lookRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Lock del cursor del ratón
        Cursor.lockState = CursorLockMode.Locked; //Mueve el cursor al centro
        Cursor.visible = false; //Oculta el cursor de la vista
        currentHealth = maxHealth;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //GroundCheck
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        //Dibujar rayo ficiticio en escena para determinar la orientacion de la camara
        Debug.DrawRay(camHolder.transform.position, camHolder.transform.forward * 100f, Color.red);
        //

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            container.SetActive(true);
            Time.timeScale = 0;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }




    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {
        cameraLook();
    }

    void cameraLook()
    {
        //Rotacion horizontal del cuerpo del personaje
        transform.Rotate(Vector3.up * lookInput.x * sensitivity);
        //Rotacion vertical (la lleva la camara)
        lookRotation += (-lookInput.y * sensitivity);
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camHolder.transform.localEulerAngles = new Vector3(lookRotation, 0f, 0f);
    }

    void Movement()
    {
        Vector3 currentVelocity = rb.linearVelocity; //necesitamos calcular la velocidad actual del rb constantemente
        Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y); //velocidad a alcanzar = la direccion que pulsamos
        targetVelocity *= isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : speed);
        
        //concertir la direccion local en global
        targetVelocity = transform.TransformDirection(targetVelocity);

        //Calcular el cambio de velocidad (acelercion)
        Vector3 velocityChange = (targetVelocity - currentVelocity);
        velocityChange = new Vector3(velocityChange.x, 0f, velocityChange.z);
        velocityChange = Vector3.ClampMagnitude(velocityChange, maxForce);

        //aplicar la fuerza de movimiento/aceleracion
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void Jump()
    {
        if (isGrounded) rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    }

    public void TakeDamage(int damage)
    {
    if (isDead) return; //  evita daño después de morir

    currentHealth -= damage;

    Debug.Log("Vida actual: " + currentHealth);

    if (currentHealth <= 0 && !isDead)
    {
        isDead = true;
        Die();
    }
}


    //
    

  


    public void ResumeButton()
    {
        container.SetActive(false);
        Time.timeScale = 1;
        //Lock del cursor del ratón
        Cursor.lockState = CursorLockMode.Locked; //Mueve el cursor al centro
        Cursor.visible = false; //Oculta el cursor de la vista


    }
    public void MainMenuButton()
    {
        SceneManager.LoadSceneAsync(0);

    }



    void Die()
{
    SceneManager.LoadScene("3SCN_DEATH");
}


    #region Input Methods
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) Jump();
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouching = !isCrouching;
            anim.SetBool("isCrouching", isCrouching);
        }
    }
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed && !isCrouching) isSprinting = true;
        if (context.canceled) isSprinting = false;
    }
    #endregion
}
