using UnityEngine;

public class SelectorDeCamino : MonoBehaviour
{
    [Header("¿Qué bloqueos activamos al pasar por aquí?")]
    public GameObject grupoBloqueoAActivar;

    [Header("Opcional: Referencias a los otros triggers para borrarlos")]
    public GameObject triggerVecino1;
    public GameObject triggerVecino2;

    private void OnTriggerEnter(Collider other)
    {
        // Comprobamos si lo que ha chocado es el coche (busca la etiqueta "Player")
        // OJO: Asegúrate de que tu coche tiene el Tag "Player" arriba a la derecha
        if (other.CompareTag("Player"))
        {
            // 1. Activamos los muros que cierran los otros caminos
            if (grupoBloqueoAActivar != null)
            {
                grupoBloqueoAActivar.SetActive(true);
            }

            Debug.Log("¡Ruta elegida! Bloqueando las demás.");

            // 2. DESTRUIMOS LOS OTROS TRIGGERS
            // Esto es para que si das la vuelta, no puedas activar otra ruta por error.
            // Una vez eliges, eliges.
            if (triggerVecino1 != null) Destroy(triggerVecino1);
            if (triggerVecino2 != null) Destroy(triggerVecino2);
            
            // 3. Nos destruimos a nosotros mismos (ya no hacemos falta)
            Destroy(gameObject);
        }
    }
}