using UnityEngine;
using System.Collections;

public class PitStopManager : MonoBehaviour
{
    [Header("Cámaras")]
    public GameObject camaraJuego; 
    public GameObject camaraBoxes; 

    [Header("Actores")]
    public GameObject equipoMecanicos; // Los mecánicos buenos (Grupo)
    public GameObject mecanicoTorpe;   // El mecánico que se cae (Suelto)

    [Header("Configuración")]
    public Transform puntoDeParada; 
    public float duracionParada = 4.0f; // Tiempo total que dura la secuencia
    public float fuerzaLanzamiento = 2000f; 

    [Header("Efectos")]
    public ParticleSystem particulasHumo; 
    public AudioSource sonidoHerramientas; 
    
    private Rigidbody cocheRB;

    // --- 1. VARIABLES PARA GUARDAR EL SITIO ORIGINAL ---
    private Vector3 posOriginalTorpe;
    private Quaternion rotOriginalTorpe;

    void Start()
    {
        // Al empezar el juego, memorizamos dónde pusiste al mecánico
        if (mecanicoTorpe != null)
        {
            posOriginalTorpe = mecanicoTorpe.transform.position;
            rotOriginalTorpe = mecanicoTorpe.transform.rotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SecuenciaBoxes(other.gameObject));
        }
    }

    IEnumerator SecuenciaBoxes(GameObject coche)
    {
        // 1. PREPARACIÓN INICIAL
        cocheRB = coche.GetComponent<Rigidbody>();
        
        // Desactivamos el control del jugador
        var controlador = coche.GetComponent<ControladorRealista>();
        if(controlador != null) controlador.enabled = false;
        
        // Quitamos físicas para moverlo nosotros
        cocheRB.isKinematic = true; 

        // 2. CAMBIAZO DE CÁMARA Y APARICIÓN
        if(camaraJuego != null) camaraJuego.SetActive(false);
        if(camaraBoxes != null) camaraBoxes.SetActive(true);

        // --- AQUÍ ESTÁ EL ARREGLO DEL BUG ---
        
        // Activamos a los buenos
        if (equipoMecanicos != null) equipoMecanicos.SetActive(true);
        
        // Activamos al torpe, PERO PRIMERO LO RESETEAMOS
        if (mecanicoTorpe != null) 
        {
            // ¡ZAS! Vuelve a tu sitio original antes de que nadie te vea
            mecanicoTorpe.transform.position = posOriginalTorpe;
            mecanicoTorpe.transform.rotation = rotOriginalTorpe;
            
            // Ahora sí, actívate y empieza la animación
            mecanicoTorpe.SetActive(true);
        }
        
        // Activamos ambiente
        if (particulasHumo != null) particulasHumo.Play();
        if (sonidoHerramientas != null) sonidoHerramientas.Play();


        // 3. EL COCHE APARCA (Mientras los mecánicos ya están ahí)
        float tiempoAparcado = 0f;
        float duracionAparcamiento = 1.2f; 
        
        Vector3 posicionInicial = coche.transform.position;
        Quaternion rotacionInicial = coche.transform.rotation;
        
        while (tiempoAparcado < duracionAparcamiento)
        {
            tiempoAparcado += Time.deltaTime; 
            float t = tiempoAparcado / duracionAparcamiento;
            t = t * t * (3f - 2f * t); 

            coche.transform.position = Vector3.Lerp(posicionInicial, puntoDeParada.position, t);
            coche.transform.rotation = Quaternion.Slerp(rotacionInicial, puntoDeParada.rotation, t);
            yield return null;
        }

        // Aseguramos posición final exacta
        coche.transform.position = puntoDeParada.position;
        coche.transform.rotation = puntoDeParada.rotation;


        // 4. TIEMPO DE REPARACIÓN
        // El coche ya está parado. Esperamos a que termine el show.
        yield return new WaitForSeconds(duracionParada);


        // 5. SALIDA Y LIMPIEZA
        if (equipoMecanicos != null) equipoMecanicos.SetActive(false);
        if (mecanicoTorpe != null) mecanicoTorpe.SetActive(false);
        
        if (particulasHumo != null) particulasHumo.Stop();
        if (sonidoHerramientas != null) sonidoHerramientas.Stop();
        
        // Devolver cámara al jugador
        if(camaraBoxes != null) camaraBoxes.SetActive(false);
        if(camaraJuego != null) camaraJuego.SetActive(true);

        // Reactivar físicas y control
        cocheRB.isKinematic = false;
        if(controlador != null) controlador.enabled = true;

        // ¡LANZAMIENTO!
        cocheRB.AddForce(coche.transform.forward * fuerzaLanzamiento, ForceMode.Impulse);
        
        Debug.Log("¡BOXES TERMINADO! SALIDA");
    }
}