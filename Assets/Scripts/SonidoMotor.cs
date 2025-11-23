using UnityEngine;

public class SonidoMotor : MonoBehaviour
{
    [Header("Configuración")]
    public float pitchMinimo = 1.0f; // Sonido al ralentí
    public float pitchMaximo = 3.0f; // Sonido a tope de revoluciones
    public float velocidadMaximaSonido = 200f; // A qué velocidad llegamos al tono máximo

    private AudioSource audioSource;
    private Rigidbody rb;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 1. Calculamos la velocidad real en km/h
        float velocidadActual = rb.linearVelocity.magnitude * 3.6f;

        // 2. Calculamos el tono (Pitch)
        // Mathf.Lerp hace una transición suave entre Min y Max según el % de velocidad
        float pitch = Mathf.Lerp(pitchMinimo, pitchMaximo, velocidadActual / velocidadMaximaSonido);

        // 3. Aplicamos el tono
        audioSource.pitch = pitch;
    }
}