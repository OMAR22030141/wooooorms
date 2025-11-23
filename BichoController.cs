using UnityEngine;

public class BichoController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;      // Velocidad horizontal
    public float jumpForce = 7f;      // Fuerza del salto

    [Header("Detección de suelo")]
    public Transform groundCheck;              // Punto debajo del bicho para revisar el suelo
    public float groundCheckRadius = 0.1f;     // Radio del círculo de chequeo
    public LayerMask groundLayer;             // Capas que cuentan como suelo (Ground)

    [Header("Control por turnos")]
    public bool canControl = false;   // ¿Este bicho puede recibir input ahora?

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;

    private void Awake()
    {
        // Obtenemos la referencia al Rigidbody2D del bicho
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Si NO es nuestro turno, no leemos input ni saltamos
        if (!canControl)
        {
            moveInput = 0f;
            return;
        }

        // Leer input horizontal (A/D, flechas izquierda/derecha)
        moveInput = Input.GetAxisRaw("Horizontal");

        // Revisar si el bicho está pisando el suelo
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        }

        // Si el jugador presiona espacio (Jump) y está en el suelo, saltar
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Reiniciamos la velocidad vertical antes de aplicar el salto para que sea consistente
            rb.velocity = new Vector2(rb.velocity.x, 0f);

            // Aplicar fuerza hacia arriba como impulso
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Si NO es nuestro turno, no movemos horizontalmente
        if (!canControl)
        {
            // Mantenemos solo la velocidad vertical (por caídas, físicas, etc.)
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        // Aplicar movimiento horizontal en FixedUpdate (mejor para física)
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    // Este método lo usará el TurnManager para activar/desactivar el control
    public void SetControl(bool value)
    {
        canControl = value;
    }

    private void OnDrawGizmosSelected()
    {
        // Esto dibuja una esfera en la escena para ver el rango de groundCheck (solo en editor)
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
