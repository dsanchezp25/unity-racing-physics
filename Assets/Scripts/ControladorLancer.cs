using UnityEngine;

public class ControladorLancer : MonoBehaviour
{
    [Header("Motor")]
    public float fuerzaMotor = 50000f;
    public float velocidadMaxima = 220f;

    [Header("Dirección")]
    public float anguloGiro = 35f;
    public float velocidadRuedasVisuales = 10f; 

    [Header("Drift - La clave de la suavidad")]
    public float resistenciaNormal = 10f;  // Agarre total
    public float resistenciaDrift = 0.5f;  // Agarre resbaladizo
    // NUEVO: Qué tan rápido vuelve el agarre al soltar espacio (1 = lento/suave, 10 = instantáneo)
    public float velocidadRecuperacion = 2f; 
    
    [Header("Anti-Vuelo")]
    public float downforce = 5000f;         
    public float distanciaRayo = 1.5f;      // Recuerda dejar esto en 1.5
    
    [Header("Referencias")]
    public Transform centroDeMasa;
    public Transform ruedaFL;
    public Transform ruedaFR;

    private Rigidbody rb;
    private float inputGiro;
    private bool tocandoSuelo; 
    private float resistenciaActual; // Variable interna para hacer la transición suave

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        
        rb.mass = 1500f;
        rb.linearDamping = 0.05f; 
        rb.angularDamping = 2.5f; 
        
        if (centroDeMasa != null) rb.centerOfMass = centroDeMasa.localPosition;
        
        // Empezamos con agarre normal
        resistenciaActual = resistenciaNormal;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        inputGiro = Mathf.MoveTowards(inputGiro, h, Time.deltaTime * velocidadRuedasVisuales);
        
        float angulo = inputGiro * anguloGiro;
        if (ruedaFL != null) ruedaFL.localRotation = Quaternion.Euler(0, angulo, 0);
        if (ruedaFR != null) ruedaFR.localRotation = Quaternion.Euler(0, angulo, 0);
    }

    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        bool frenoMano = Input.GetKey(KeyCode.Space);

        // 1. DETECTAR SUELO
        tocandoSuelo = Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, distanciaRayo);

        if (tocandoSuelo) 
        {
            // --- MOTOR ---
            if (rb.linearVelocity.magnitude * 3.6f < velocidadMaxima)
            {
                rb.AddForce(transform.forward * v * fuerzaMotor);
            }

            // --- DIRECCIÓN ---
            if (rb.linearVelocity.magnitude > 1f)
            {
                float direccion = Vector3.Dot(rb.linearVelocity, transform.forward) > 0 ? 1 : -1;
                // Al derrapar giramos un poco más para poder contravolantear
                float multiplicadorDrift = frenoMano ? 1.3f : 1f; 

                float giro = h * direccion * multiplicadorDrift;
                
                Quaternion giroDelta = Quaternion.Euler(Vector3.up * giro * anguloGiro * 1.5f * Time.fixedDeltaTime);
                rb.MoveRotation(rb.rotation * giroDelta);
            }

            // --- FÍSICA LATERAL (AQUÍ ESTÁ LA MAGIA NUEVA) ---
            
            // 1. Decidimos cuál es nuestro objetivo de agarre
            float agarreObjetivo = frenoMano ? resistenciaDrift : resistenciaNormal;

            // 2. Nos movemos hacia ese objetivo SUAVEMENTE (Lerp)
            // Si pulsas espacio, vas a resistenciaDrift rápido.
            // Si sueltas, vuelves a resistenciaNormal a la velocidad que digas.
            resistenciaActual = Mathf.Lerp(resistenciaActual, agarreObjetivo, Time.fixedDeltaTime * velocidadRecuperacion);

            // 3. Calculamos y aplicamos la fuerza lateral
            Vector3 velocidadLocal = transform.InverseTransformDirection(rb.linearVelocity);
            Vector3 fuerzaResistencia = -transform.right * velocidadLocal.x * resistenciaActual * rb.mass;
            rb.AddForce(fuerzaResistencia);

            // --- SUPER PEGAMENTO ---
            rb.AddForce(-transform.up * downforce);
        }
        else
        {
            rb.AddForce(Vector3.down * 2000f);
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (-transform.up * distanciaRayo));
    }
}