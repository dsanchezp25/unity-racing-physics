using UnityEngine;

public class ControladorRealista : MonoBehaviour
{
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
    public AnimationCurve curvaPotencia = new AnimationCurve(
        new Keyframe(0, 1),    
        new Keyframe(0.5f, 0.8f), 
        new Keyframe(1, 0.2f)  
    );

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

        // --- 1. POTENCIA ---
        float factorVelocidad = Mathf.Clamp01(velocidadKmh / velocidadMaxima);
        float multiplicadorPotencia = curvaPotencia.Evaluate(factorVelocidad);

        float torque = 0;
        if (velocidadKmh < velocidadMaxima && !frenoMano)
        {
            torque = v * fuerzaMotor * multiplicadorPotencia;
        }

        colFL.motorTorque = torque; colFR.motorTorque = torque;
        colRL.motorTorque = torque; colRR.motorTorque = torque;

        // --- 2. DIRECCIÓN ---
        inputGiroSuave = Mathf.MoveTowards(inputGiroSuave, h, Time.fixedDeltaTime * velocidadVolante);
        float anguloMax = Mathf.Lerp(anguloGiroLento, anguloGiroRapido, velocidadKmh / 120f);
        float steer = inputGiroSuave * anguloMax;
        colFL.steerAngle = steer; colFR.steerAngle = steer;

        // --- 3. DRIFT ---
        ControlarDerrape(colRL, frenoMano);
        ControlarDerrape(colRR, frenoMano);

        // --- 4. FRENOS ---
        float freno = 0;
        if (frenoMano) 
        {
            colRL.brakeTorque = fuerzaFrenoMano; colRR.brakeTorque = fuerzaFrenoMano;
            colFL.brakeTorque = 0; colFR.brakeTorque = 0;
        }
        else 
        {
            if (v == 0) freno = 100f;
            else if (Vector3.Dot(rb.linearVelocity, transform.forward) > 1f && v < 0) freno = fuerzaFreno;
            
            colFL.brakeTorque = freno; colFR.brakeTorque = freno;
            colRL.brakeTorque = freno; colRR.brakeTorque = freno;
        }

        // --- 5. LUCES DE FRENO ---
        bool estaFrenando = false;

        if (frenoMano) estaFrenando = true;
        else if (v < 0 && Vector3.Dot(rb.linearVelocity, transform.forward) > 1f) estaFrenando = true;
        else if (v == 0 && rb.linearVelocity.magnitude < 1f) estaFrenando = false;

        if (luzFrenoIzq != null) luzFrenoIzq.enabled = estaFrenando;
        if (luzFrenoDer != null) luzFrenoDer.enabled = estaFrenando;

        // --- ANTI-VUELO (AHORA SÍ ESTÁ DENTRO) ---
        rb.AddForce(-transform.up * downforce * rb.linearVelocity.magnitude);

    } // <--- AQUÍ se cierra el FixedUpdate correctamente

    void ControlarDerrape(WheelCollider rueda, bool activado)
    {
        WheelFrictionCurve roce = rueda.sidewaysFriction;
        roce.stiffness = activado ? agarreDrift : Mathf.MoveTowards(roce.stiffness, agarreNormal, Time.fixedDeltaTime * 5f);
        rueda.sidewaysFriction = roce;
    }

    void Update()
    {
        SincronizarRueda(colFL, visualFL); SincronizarRueda(colFR, visualFR);
        SincronizarRueda(colRL, visualRL); SincronizarRueda(colRR, visualRR);
    }

    void SincronizarRueda(WheelCollider col, Transform visual)
    {
        Vector3 pos; Quaternion rot;
        col.GetWorldPose(out pos, out rot);
        visual.position = pos; visual.rotation = rot;
    }
}