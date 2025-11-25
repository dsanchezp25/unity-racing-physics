using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.UI; // NECESARIO para controlar la imagen
using System.Collections; // NECESARIO para las Coroutines
using UnityEngine.Audio;
using System.Threading.Tasks;

public class MenuManager : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreEscenaJuego = "Circuito_Version_Realista";

    [Header("Efecto de Transición")] // NUEVO
    public Image panelFundido; // Arrastra tu PanelFundido aquí
    public float duracionFundido = 0.8f; // Cuánto tarda en irse a negro
    public AudioSource musicaMenu;
    public void CargarModo(int numeroModo)
    {
        // El botón ya no carga la escena directo, sino que llama al fundido.
        StartCoroutine(FadeOutAndLoad(numeroModo)); 
    }

    // --- COROUTINE: El Corazón de la Transición ---
    IEnumerator FadeOutAndLoad(int numeroModo)
    {
        // 1. FUNDIDO A NEGRO (Fade Out)
        float tiempo = 0;
        float volumenInicial = musicaMenu != null ? musicaMenu.volume : 0;

        while (tiempo < duracionFundido)
        {
            tiempo += Time.deltaTime;
            // Lerp va moviendo el color del panelFundido de invisible (0) a negro total (1)
            panelFundido.color = Color.Lerp(Color.clear, Color.black, tiempo / duracionFundido);

            if (musicaMenu != null) musicaMenu.volume = Mathf.Lerp(volumenInicial, 0f, tiempo / duracionFundido);
            yield return null; // Espera un frame
        }

        // 2. Guardar y Cargar
        PlayerPrefs.SetInt("ModoJuego", numeroModo);
        SceneManager.LoadScene(nombreEscenaJuego);
    }

    public void SalirJuego()
    {
        Debug.Log("Cerrando juego..."); 
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}