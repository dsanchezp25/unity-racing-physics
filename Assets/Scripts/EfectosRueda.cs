using UnityEngine;

public class EfectosRueda : MonoBehaviour
{
    public WheelCollider ruedaFisica; // El WC correspondiente
    public ParticleSystem humo;       // El sistema de partículas hijo
    public TrailRenderer marcaSuelo;  // El trail renderer
    
    public float umbralDrift = 0.5f; // Cuánto tiene que resbalar para activar efectos
    public float offsetSuelo = 0.02f; // Para levantar la marca un pelín y que no parpadee

    void Update()
    {
        WheelHit hit;
        // Preguntamos a la rueda qué está pasando
        if (ruedaFisica.GetGroundHit(out hit))
        {
            // 1. AJUSTAR POSICIÓN DE LA MARCA
            // Esto es un truco PRO: Movemos el Trail al punto exacto donde la rueda toca el suelo
            if (marcaSuelo != null)
            {
                marcaSuelo.transform.position = hit.point + (Vector3.up * offsetSuelo);
            }

            // 2. DETECTAR DRIFT
            // Si el deslizamiento lateral o frontal es alto...
            if (Mathf.Abs(hit.sidewaysSlip) > umbralDrift || Mathf.Abs(hit.forwardSlip) > 0.8f)
            {
                // ACTIVAMOS EFECTOS
                if (!humo.isPlaying) humo.Play();
                marcaSuelo.emitting = true;
            }
            else
            {
                // DESACTIVAMOS EFECTOS
                if (humo.isPlaying) humo.Stop();
                marcaSuelo.emitting = false;
            }
        }
        else
        {
            // Si estamos en el aire, apagamos todo
            if (humo.isPlaying) humo.Stop();
            marcaSuelo.emitting = false;
        }
    }
}