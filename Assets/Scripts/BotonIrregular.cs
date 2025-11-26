using UnityEngine;
using UnityEngine.UI; // Necesario para tocar la Imagen

public class BotonIrregular : MonoBehaviour
{
    void Start()
    {
        // Esto le dice a Unity: "Solo haz caso si el píxel es visible (no transparente)"
        // El 0.1f significa que ignorará todo lo que sea casi invisible.
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}