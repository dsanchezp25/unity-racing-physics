using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cargar el Menú Principal

public class MenuPausa : MonoBehaviour
{
    [Header("Arrastra aquí tu Panel Pausa")]
    public GameObject panelPausa;

    private bool juegoPausado = false;

    void Update()
    {
        // Si pulsas ESCAPE...
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (juegoPausado)
            {
                Reanudar(); // Si ya estaba pausado, volvemos al juego
            }
            else
            {
                Pausar(); // Si estaba jugando, pausamos
            }
        }
    }

    public void Pausar()
    {
        panelPausa.SetActive(true); // Aparece el menú
        Time.timeScale = 0f; // ¡MAGIA! El tiempo se congela (físicas quietas)
        juegoPausado = true;

        AudioListener.pause = true; // Pausa todos los sonidos del juego
    }

    public void Reanudar()
    {
        panelPausa.SetActive(false); // Se oculta el menú
        Time.timeScale = 1f; // El tiempo vuelve a correr
        juegoPausado = false;
        AudioListener.pause = false; // Reanuda todos los sonidos del juego
    }

    public void VolverAlMenu()
    {
        // IMPORTANTE: Descongelar el tiempo antes de irnos.
        // Si no, el Menú Principal estará congelado y no funcionará nada.
        Time.timeScale = 1f; 
        
        AudioListener.pause = false; // Reanuda todos los sonidos del juego
        // Cargamos la escena del Menú (que suele ser la número 0 en la lista)
        SceneManager.LoadScene(0); 
    }
}