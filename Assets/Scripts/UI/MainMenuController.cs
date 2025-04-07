using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Referencias a los Botones")]
    [SerializeField] private Button botonJugar;
    [SerializeField] private Button botonOpciones;
    [SerializeField] private Button botonCreditos;

    private void Start()
    {
        // Asignar las funciones a cada botón
        botonJugar.onClick.AddListener(IrAJugar);
        botonOpciones.onClick.AddListener(IrAOpciones);
        botonCreditos.onClick.AddListener(IrACreditos);
    }

    /// <summary>
    /// Función para el botón "Jugar"
    /// Carga la escena principal del juego o la pantalla de selección de nivel.
    /// </summary>
    private void IrAJugar()
    {
        // Reemplaza "EscenaJuego" por el nombre real de la escena de tu juego
        SceneManager.LoadScene("Lobby");
    }

    /// <summary>
    /// Función para el botón "Opciones"
    /// Carga la escena o panel de opciones.
    /// </summary>
    private void IrAOpciones()
    {
        // Reemplaza "EscenaOpciones" por la escena o implementa un panel UI
        SceneManager.LoadScene("EscenaOpciones");
    }

    /// <summary>
    /// Función para el botón "Créditos"
    /// Carga la escena o panel de créditos.
    /// </summary>
    private void IrACreditos()
    {
        // Reemplaza "EscenaCreditos" por la escena de créditos
        SceneManager.LoadScene("EscenaCreditos");
    }
}