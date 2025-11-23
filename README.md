# 🏎️ Unity Racing Physics

![Unity Version](https://img.shields.io/badge/Unity-2022.3%2B-000000.svg?style=flat&logo=unity)
![License](https://img.shields.io/badge/License-MIT-green.svg)
![Status](https://img.shields.io/badge/Status-In%20Development-orange.svg)

Repositorio de simulación de vehículos en Unity que implementa y compara dos arquitecturas de físicas distintas: **Arcade (Raycast)** vs **Simulación (WheelCollider)**.

🔗 **Repo URL:** [https://github.com/dsanchezp25/unity-racing-physics](https://github.com/dsanchezp25/unity-racing-physics)

## 🌟 Características Principales

El proyecto contiene dos sistemas de control independientes para explorar diferentes sensaciones de conducción:

### 1. Sistema Arcade (`ControladorLancer.cs`)
Diseñado para jugabilidad rápida, estilo *Drift* o *Karting*.
* **Física Custom (Raycast):** No utiliza las ruedas físicas de Unity, sino rayos para detectar el suelo, lo que evita comportamientos impredecibles.
* **Drift Asistido:** Implementa un sistema de agarre dinámico que interpolan (`Mathf.Lerp`) entre fricción normal y fricción de derrape al usar el freno de mano.
* **Estabilidad:** Sistema de *Downforce* artificial y recuperación de tracción ajustable para mantener el coche pegado a la pista.

### 2. Sistema Realista (`ControladorRealista.cs`)
Diseñado para simulación técnica y transferencia de pesos.
* **Unity WheelColliders:** Utiliza el sistema nativo de físicas de ruedas de Unity.
* **Curvas de Motor:** Simulación de entrega de potencia mediante `AnimationCurve` para un comportamiento no lineal del par motor.
* **Detalles Técnicos:** Incluye lógica para luces de freno reactivas, fricción lateral de neumáticos y sincronización visual de las mallas de las ruedas.

## 🕹️ Controles

Ambos controladores están mapeados al **Input Manager** estándar de Unity:

| Acción | Tecla / Input | Descripción |
| :--- | :--- | :--- |
| **Acelerar** | `W` / `Flecha Arriba` | Eje Vertical (+) |
| **Frenar / Reverso** | `S` / `Flecha Abajo` | Eje Vertical (-) |
| **Girar** | `A` / `D` / `Flechas` | Eje Horizontal |
| **Freno de Mano** | `Barra Espaciadora` | Activa el modo Drift (Arcade) o bloquea el eje trasero (Realista) |

## 🛠️ Instalación y Uso

1.  **Clonar el repositorio:**
    ```bash
    git clone [https://github.com/dsanchezp25/unity-racing-physics.git](https://github.com/dsanchezp25/unity-racing-physics.git)
    ```
2.  **Abrir en Unity:**
    * Abre **Unity Hub**.
    * Dale a `Add` y selecciona la carpeta clonada.
    * *Nota: Se recomienda Unity 2022.3 LTS o superior.*
3.  **Ejecutar:**
    * Navega a la carpeta `Assets/Scenes`.
    * Abre la escena **[NOMBRE_DE_TU_ESCENA]**.
    * Presiona el botón **Play**.

## 📂 Estructura del Proyecto

* `Assets/Scripts/`: Contiene la lógica core (`ControladorLancer.cs`, `ControladorRealista.cs`).
* `Assets/CartoonTracksPack1/`: Assets gráficos de los circuitos.
* `Assets/Prefabs/`: Vehículos pre-configurados listos para usar.

## 🤝 Contribuciones

Las contribuciones son bienvenidas. Algunas ideas para mejorar el proyecto:
* [ ] Añadir sistema de sonido para el motor basado en RPM.
* [ ] Implementar un velocímetro UI.
* [ ] Crear un sistema de cambio de cámara.

---
*Desarrollado por [dsanchezp25](https://github.com/dsanchezp25)*
*Desarrollado por [Ccrespo7](https://github.com/Ccrespo7)*
