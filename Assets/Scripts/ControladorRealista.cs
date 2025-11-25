using UnityEngine;
using TMPro;           // NECESARIO para el texto
using UnityEngine.UI;  // NECESARIO para la barra

public class ControladorRealista : MonoBehaviour
{
    [Header("Interfaz Velocímetro")]
    public TMP_Text textoVelocidad; 
    public Image barraVelocidad;    
    // ---------------------------------------

    [Header("Referencias")]
    public WheelCollider colFL; public WheelCollider colFR;
    public WheelCollider colRL; public WheelCollider colRR;
    public Transform visualFL; public Transform visualFR;
    public Transform visualRL; public Transform visualRR;

    [Header("Luces")] 
    public Light luzFrenoIzq;
    public Light luzFrenoDer;

    [Header("Motor Realista")]
    public float fuerzaMotor = 2500f; 
    public float velocidadMaxima = 200f;
    public AnimationCurve curvaPotencia = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.5f, 0.8f), new Keyframe(1, 0.2f));

    [Header("Frenos y Drift")]
    public float fuerzaFreno = 6000f; 
    public float fuerzaFrenoMano = 10000f;
    [Range(0.1f, 2f)] public float agarreDrift = 0.5f; 
    [Range(1f, 3f)] public float agarreNormal = 2.0f; 

    [Header("Dirección")]
    public float anguloGiroLento = 40f; 
    public float anguloGiroRapido = 10f;
    public float velocidadVolante = 5f; 

    [Header("Estabilidad")]
    public Transform centroDeMasa;
    public float downforce = 2000f;

    private Rigidbody rb;
    private float inputGiroSuave; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1500f;
        rb.linearDamping = 0.02f; 
        rb.angularDamping = 3f;  
        if (centroDeMasa != null) rb.centerOfMass = centroDeMasa.localPosition;
    }

    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        bool frenoMano = Input.GetKey(KeyCode.Space);
        float velocidadKmh = rb.linearVelocity.magnitude * 3.6f;

        // Motor
        float factorVel = Mathf.Clamp01(velocidadKmh / velocidadMaxima);
        float torque = (velocidadKmh < velocidadMaxima && !frenoMano) ? v * fuerzaMotor * curvaPotencia.Evaluate(factorVel) : 0;
        colFL.motorTorque = torque; colFR.motorTorque = torque; colRL.motorTorque = torque; colRR.motorTorque = torque;

        // Dirección
        inputGiroSuave = Mathf.MoveTowards(inputGiroSuave, h, Time.fixedDeltaTime * velocidadVolante);
        float steer = inputGiroSuave * Mathf.Lerp(anguloGiroLento, anguloGiroRapido, velocidadKmh / 120f);
        colFL.steerAngle = steer; colFR.steerAngle = steer;

        // Drift y Frenos
        ControlarDerrape(colRL, frenoMano); ControlarDerrape(colRR, frenoMano);
        
        float freno = 0;
        if (frenoMano) { colRL.brakeTorque = fuerzaFrenoMano; colRR.brakeTorque = fuerzaFrenoMano; colFL.brakeTorque = 0; colFR.brakeTorque = 0; }
        else {
            if (v == 0) freno = 100f;
            else if (Vector3.Dot(rb.linearVelocity, transform.forward) > 1f && v < 0) freno = fuerzaFreno;
            colFL.brakeTorque = freno; colFR.brakeTorque = freno; colRL.brakeTorque = freno; colRR.brakeTorque = freno;
        }

        // Luces
        bool frenando = frenoMano || (v < 0 && Vector3.Dot(rb.linearVelocity, transform.forward) > 1f);
        if (luzFrenoIzq) luzFrenoIzq.enabled = frenando;
        if (luzFrenoDer) luzFrenoDer.enabled = frenando;

        // --- ACTUALIZAR UI (AQUÍ ESTÁ LA MAGIA) ---
        if (textoVelocidad != null) textoVelocidad.text = velocidadKmh.ToString("F0");
        if (barraVelocidad != null) {
            barraVelocidad.fillAmount = velocidadKmh / velocidadMaxima;
            barraVelocidad.color = Color.Lerp(Color.cyan, Color.red, barraVelocidad.fillAmount);
        }

        // Anti-Vuelo
        rb.AddForce(-transform.up * downforce * rb.linearVelocity.magnitude);
    }

    void ControlarDerrape(WheelCollider rueda, bool activado) {
        WheelFrictionCurve c = rueda.sidewaysFriction;
        c.stiffness = activado ? agarreDrift : Mathf.MoveTowards(c.stiffness, agarreNormal, Time.fixedDeltaTime * 5f);
        rueda.sidewaysFriction = c;
    }

    void Update() {
        Sincronizar(colFL, visualFL); Sincronizar(colFR, visualFR);
        Sincronizar(colRL, visualRL); Sincronizar(colRR, visualRR);
    }
    void Sincronizar(WheelCollider c, Transform t) { Vector3 p; Quaternion r; c.GetWorldPose(out p, out r); t.position = p; t.rotation = r; }
}