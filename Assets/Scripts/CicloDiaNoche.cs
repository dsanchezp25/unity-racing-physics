using UnityEngine;

public class CicloDiaNoche : MonoBehaviour
{
    [Header("Configuración")]
    [Range(0, 24)] public float horaActual = 12f;
    public float duracionDiaEnMinutos = 10f; 
    
    [Header("Referencias")]
    public Light luzSol; 

    // MEMORIA: Empieza en -1 para que la primera vez valga cualquiera
    private int ultimoIndice = -1; 

    // DEFINIMOS LAS HORAS AQUÍ ARRIBA PARA NO REPETIRLAS
    private float[] horasPosibles = { 12f, 18f, 23f };

    void Start()
    {
        if (luzSol == null) luzSol = GetComponent<Light>();
    }

    void Update()
    {
        // Avance del tiempo
        float incremento = (24f / (duracionDiaEnMinutos * 60f)) * Time.deltaTime;
        horaActual += incremento;
        if (horaActual >= 24) horaActual = 0;

        ActualizarSol();
    }

    public void PonerHora(float nuevaHora)
    {
        horaActual = nuevaHora;
        ActualizarSol();
    }

    // --- FUNCIÓN CORREGIDA Y MEJORADA ---
    public void PonerHoraAleatoria()
    {
        // 1. Calculamos cuántas opciones tenemos (son 3)
        int totalOpciones = horasPosibles.Length;

        // 2. TRUCO MATEMÁTICO (Evita bucles infinitos):
        // Elegimos un salto aleatorio entre 1 y (Total-1).
        // Nunca sumamos 0, así que nunca repetimos el mismo sitio.
        int salto = Random.Range(1, totalOpciones); 

        // 3. Calculamos el nuevo índice usando el "Módulo" (%) para dar la vuelta al array
        // Si el último era -1 (inicio), lo tratamos como 0 para la suma
        int baseIndice = (ultimoIndice == -1) ? 0 : ultimoIndice;
        int nuevoIndice = (baseIndice + salto) % totalOpciones;

        // 4. Guardamos y aplicamos
        ultimoIndice = nuevoIndice;
        horaActual = horasPosibles[nuevoIndice];
        
        ActualizarSol();
        Debug.Log("🌞 Cambio de ambiente: Hora " + horaActual);
    }

    void ActualizarSol()
    {
        if(luzSol != null)
        {
            float rotacionX = (horaActual / 24f) * 360f - 90f;
            luzSol.transform.rotation = Quaternion.Euler(rotacionX, 170f, 0);
        }
    }
}