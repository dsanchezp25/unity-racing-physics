using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
    [Header("Duración del día")]
    // Cuanto más bajo, más lento pasa el día.
    // 10 = Día muy rápido. 1 = Día lento.
    public float velocidadTiempo = 2f; 

    void Update()
    {
        // Giramos la luz alrededor del eje X (que es el horizonte)
        transform.Rotate(velocidadTiempo * Time.deltaTime, 0, 0);
    }
}