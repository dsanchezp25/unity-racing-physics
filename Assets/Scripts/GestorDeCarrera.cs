using UnityEngine;
using TMPro;

public class GestorDeCarrera : MonoBehaviour
{
    [Header("Configuración")]
    public int totalVueltas = 3;
    
    [Header("UI (Textos)")]
    public TMP_Text textoCronoActual;
    public TMP_Text textoMejorVuelta;
    public TMP_Text textoVueltas;
    public GameObject panelVictoria;

    [Header("Muros (Grupos)")]
    public GameObject barreraTapaA; 
    public GameObject barreraTapaB; 
    public GameObject barreraTapaC; 

    [Header("Referencias Extra")]
    public GameObject grupoTriggersManuales;
    public CicloDiaNoche controladorSol; 

    [Header("CONEXIÓN CINEMÁTICA")] // --- NUEVO ---
    public GestorFinal gestorFinal; // Aquí arrastrarás tu objeto MANAGER_FINAL

    [Header("Audio")]
    public AudioClip sonidoArranque;

    private AudioSource audioSource;

    // Variables internas
    private bool haArrancado = false;
    private bool carreraTerminada = false;
    private int vueltaActual = 1;
    private float tiempoVueltaActual = 0f;
    private float mejorTiempo = Mathf.Infinity;
    private int modoJuego;
    private int ultimaRuta = -1;

    void Start()
    {
        // 1. INICIALIZACIÓN UI
        if(panelVictoria) panelVictoria.SetActive(false);
        if(textoCronoActual) textoCronoActual.text = "00:00.000";
        if(textoMejorVuelta) textoMejorVuelta.text = "--:--.---";
        ActualizarTextoVueltas();

        // 2. BUSCAR SOL AUTOMÁTICAMENTE
        if (controladorSol == null) controladorSol = FindFirstObjectByType<CicloDiaNoche>();

        // 3. CONFIGURAR PISTA
        modoJuego = PlayerPrefs.GetInt("ModoJuego", 0);
        Debug.Log("🏎️ GESTOR LISTO. Modo de juego: " + modoJuego);

        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && sonidoArranque != null)
        {
            audioSource.PlayOneShot(sonidoArranque);
        }

        if (modoJuego == 0) // Cambiante
        {
            if(grupoTriggersManuales) grupoTriggersManuales.SetActive(false);
            CambiarEntornoAleatorio();
        }
        else // Fijo
        {
            ConfigurarRuta(modoJuego);
            ConfigurarAmbiente(modoJuego);
        }
    }

    void Update()
    {
        if (carreraTerminada) return;

        // --- LÓGICA DE ARRANQUE ---
        if (!haArrancado)
        {
            // Detectamos si el jugador acelera (W o Flecha Arriba)
            if (Input.GetAxis("Vertical") > 0.1f || Input.GetKeyDown(KeyCode.W))
            {
                haArrancado = true;
                Debug.Log("🚦 ¡SALIDA! El crono empieza a correr.");
            }
            return; // No hacemos nada más hasta que arranque
        }

        // --- CRONÓMETRO ---
        tiempoVueltaActual += Time.deltaTime;
        if (textoCronoActual != null) textoCronoActual.text = FormatearTiempo(tiempoVueltaActual);
    }

    // --- ESTA FUNCIÓN LA LLAMA LA META ---
    public void NuevaVuelta()
    {
        if (carreraTerminada) return;

        Debug.Log("🏁 VUELTA " + vueltaActual + " COMPLETADA");

        // 1. Gestión de Récords
        if (tiempoVueltaActual < mejorTiempo)
        {
            mejorTiempo = tiempoVueltaActual;
            if (textoMejorVuelta)
            {
                textoMejorVuelta.text = FormatearTiempo(mejorTiempo);
                textoMejorVuelta.color = Color.green;
            }
        }
        tiempoVueltaActual = 0f; // Reset reloj

        // 2. Gestión de Vueltas
        vueltaActual++;
        
        if (vueltaActual > totalVueltas)
        {
            TerminarCarrera();
        }
        else
        {
            ActualizarTextoVueltas();
            // Si es modo cambiante, cambiamos todo otra vez
            if (modoJuego == 0) CambiarEntornoAleatorio();
        }
    }

    void CambiarEntornoAleatorio()
    {
        BarajarRuta();
        if(controladorSol) controladorSol.PonerHoraAleatoria();
    }

    // --- MODIFICADO: AHORA LLAMA AL GESTOR FINAL ---
    void TerminarCarrera()
    {
        carreraTerminada = true;
        Debug.Log("🏆 CARRERA TERMINADA");

        // Si tenemos conectado el gestor de cinemáticas, lo usamos
        if (gestorFinal != null)
        {
            gestorFinal.ActivarFinal();
        }
        else 
        {
            // Plan B por si se te olvidó conectar el gestor (para que no de error)
            if(textoVueltas) textoVueltas.text = "FIN!";
            if(panelVictoria) panelVictoria.SetActive(true);
        }
    }

    // --- UTILIDADES ---
    string FormatearTiempo(float t) {
        int m = Mathf.FloorToInt(t / 60F); int s = Mathf.FloorToInt(t % 60F); int ms = Mathf.FloorToInt((t * 1000) % 1000);
        return string.Format("{0:00}:{1:00}.{2:000}", m, s, ms);
    }
    void ActualizarTextoVueltas() { if(textoVueltas) textoVueltas.text = "VUELTA " + vueltaActual + " / " + totalVueltas; }

    // --- LÓGICA DE MUROS ---
    void BarajarRuta() { 
        int rutaAzar = Random.Range(1, 4); 
        if (rutaAzar == ultimaRuta) rutaAzar = (rutaAzar % 3) + 1; 
        ultimaRuta = rutaAzar;
        ConfigurarRuta(rutaAzar);
    }
    void ConfigurarRuta(int r) { 
        Activar(barreraTapaA, false); Activar(barreraTapaB, false); Activar(barreraTapaC, false);
        switch (r) {
            case 1: Activar(barreraTapaA, true); break;
            case 2: Activar(barreraTapaB, true); break;
            case 3: Activar(barreraTapaC, true); break;
        }
    }
    void ConfigurarAmbiente(int m) { if(!controladorSol) return; if(m==1) controladorSol.PonerHora(12); if(m==2) controladorSol.PonerHora(18.5f); if(m==3) controladorSol.PonerHora(0); }
    void Activar(GameObject m, bool e) { if (m) m.SetActive(e); }
}