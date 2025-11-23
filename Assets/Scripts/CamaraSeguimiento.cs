using UnityEngine;

public class CamaraSeguimiento : MonoBehaviour
{
    public Transform objetivo;
    public Vector3 offset = new Vector3(0, 2.8f, -6.5f); // Altura y Distancia
    public float tiempoSuavizado = 0.15f; // Menor número = más pegada. 0.1 es bueno.
    public float velocidadRotacion = 5f;

    private Vector3 velocidadActual; // Variable interna para las matemáticas

    void LateUpdate() // IMPORTANTE: LateUpdate para suavidad visual
    {
        if (objetivo == null) return;

        // 1. POSICIÓN: Usamos SmoothDamp (Cero vibraciones)
        Vector3 posicionDeseada = objetivo.TransformPoint(offset);
        transform.position = Vector3.SmoothDamp(transform.position, posicionDeseada, ref velocidadActual, tiempoSuavizado);

        // 2. ROTACIÓN: Mirar al coche suavemente
        var rotacionDeseada = Quaternion.LookRotation(objetivo.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, velocidadRotacion * Time.deltaTime);
    }
}