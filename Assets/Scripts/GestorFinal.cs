using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GestorFinal : MonoBehaviour
{
    [Header("Cámaras")]
    public GameObject camaraCoche;      
    public GameObject camaraPodio;      

    [Header("Actores")]
    public GameObject personajeBaile;   
    public GameObject cocheJugador; // <--- NUEVO: Arrastra tu coche aquí
    public GameObject interfazJuego;    
    
    [Header("Audio")]
    public AudioSource musicaCarrera;   
    public AudioSource musicaVictoria;  

    [Header("Configuración")]
    public float segundosDeBaile = 8.0f; 
    public string nombreEscenaMenu = "MenuPrincipal"; 

    public void ActivarFinal()
    {
        StartCoroutine(SecuenciaDeVictoria());
    }

    IEnumerator SecuenciaDeVictoria()
    {
        // 1. LIMPIEZA INTERFAZ Y MÚSICA FONDO
        if(interfazJuego != null) interfazJuego.SetActive(false);
        if(musicaCarrera != null) musicaCarrera.Stop();

        // 2. APAGAR EL COCHE (¡ADIÓS RUIDO DE MOTOR!)
        // Al desactivar el objeto entero, se apagan sus scripts y sus audios.
        if(cocheJugador != null) cocheJugador.SetActive(false);

        // 3. CAMBIAZO DE CÁMARA
        if(camaraCoche != null) camaraCoche.SetActive(false);
        if(camaraPodio != null) camaraPodio.SetActive(true);

        // 4. ¡QUE SALGA EL BAILARÍN!
        if(personajeBaile != null) personajeBaile.SetActive(true);

        // 5. ¡MÚSICA MAESTRO!
        if(musicaVictoria != null) musicaVictoria.Play();

        // 6. ESPERAR
        yield return new WaitForSeconds(segundosDeBaile);

        // 7. VOLVER AL MENÚ
        SceneManager.LoadScene(nombreEscenaMenu);
    }
}