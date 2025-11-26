using UnityEngine;
using UnityEngine.EventSystems; // Necesario para detectar el ratón

public class BotonEfecto : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float tamañoAlTocar = 1.1f; // Crecerá un 10%
    private Vector3 escalaOriginal;

    void Start()
    {
        // Guardamos el tamaño que tiene al principio
        escalaOriginal = transform.localScale;
    }

    // Cuando el ratón entra
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = escalaOriginal * tamañoAlTocar;
    }

    // Cuando el ratón sale
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = escalaOriginal;
    }
}