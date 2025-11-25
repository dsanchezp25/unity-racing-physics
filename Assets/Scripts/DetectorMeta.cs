using UnityEngine;

public class DetectorMeta : MonoBehaviour
{
    private GestorDeCarrera gestor;

    void Start()
    {
        gestor = FindFirstObjectByType<GestorDeCarrera>();
        if (gestor == null) Debug.LogError("❌ ERROR CRÍTICO: No encuentro al GestorDeCarrera en la escena.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (gestor != null)
            {
                gestor.NuevaVuelta();
            }
        }
    }
}