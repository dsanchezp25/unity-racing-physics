using UnityEngine;

public class SonidoDerrape : MonoBehaviour
{
    public AudioSource audioDerrape; // Arrastra aquí el SEGUNDO AudioSource
    public WheelCollider ruedaTrasera; // Arrastra una rueda trasera (ej: WC_RL)
    
    // A partir de cuánto deslizamiento suena (0.4 es cuando empieza el drift en tu otro script)
    public float umbralDerrape = 0.4f; 

    void Update()
    {
        WheelHit hit;
        // Preguntamos a la rueda cómo de fuerte está resbalando
        if (ruedaTrasera.GetGroundHit(out hit))
        {
            // El "SidewaysSlip" nos dice cuánto patina de lado
            float patinaje = Mathf.Abs(hit.sidewaysSlip);

            if (patinaje > umbralDerrape)
            {
                // Si patina mucho y el sonido no está sonando, ¡PLAY!
                if (!audioDerrape.isPlaying) audioDerrape.Play();
                
                // Truco Pro: Cambiamos el volumen según cuánto patina
                audioDerrape.volume = Mathf.Clamp01(patinaje - umbralDerrape);
            }
            else
            {
                // Si recupera el agarre, paramos el sonido poco a poco
                audioDerrape.volume = Mathf.Lerp(audioDerrape.volume, 0, Time.deltaTime * 10f);
                if (audioDerrape.volume < 0.05f) audioDerrape.Stop();
            }
        }
    }
}