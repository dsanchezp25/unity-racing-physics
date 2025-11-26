using UnityEngine;

public class MenuVivo : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidadRotacion = 10f; // Velocidad a la que gira la cámara (grados por segundo)
    public float oscilacionAltura = 0.05f; // Cuánto sube y baja (efecto respiración)
    public float velocidadOscilacion = 1f; // Qué tan rápido sube y baja

    private float alturaInicial;

    void Start()
    {
        // Guardamos la altura a la que pusiste la cámara al principio
        alturaInicial = transform.position.y;
    }

    void Update()
    {
        // 1. ROTACIÓN: Hacemos que este objeto gire sobre su propio eje Y
        // Como la cámara será "hija" de este objeto, orbitará alrededor del centro
        transform.Rotate(0, velocidadRotacion * Time.deltaTime, 0);

        // 2. RESPIRACIÓN (Opcional): Un movimiento suave arriba y abajo
        // Esto hace que parezca que la cámara la sostiene una persona o un dron, no un robot
        float nuevoY = alturaInicial + Mathf.Sin(Time.time * velocidadOscilacion) * oscilacionAltura;
        
        transform.position = new Vector3(transform.position.x, nuevoY, transform.position.z);
    }
}