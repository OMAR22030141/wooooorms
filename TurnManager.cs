using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [Header("Bichos de cada jugador (asigna en el Inspector)")]
    public BichoController[] player1Bichos;
    public BichoController[] player2Bichos;

    [Header("Estado actual del turno")]
    [SerializeField] private int currentPlayer = 1;  // 1 = Jugador 1, 2 = Jugador 2
    [SerializeField] private int currentIndexP1 = 0; // Índice del bicho actual del jugador 1
    [SerializeField] private int currentIndexP2 = 0; // Índice del bicho actual del jugador 2

    private void Start()
    {
        // Al iniciar la partida, activamos el primer bicho del Jugador 1
        DesactivarTodosLosBichos();
        currentPlayer = 1;
        currentIndexP1 = 0;
        currentIndexP2 = 0;

        if (player1Bichos.Length > 0 && player1Bichos[0] != null)
        {
            player1Bichos[0].SetControl(true);
        }
        else
        {
            Debug.LogWarning("TurnManager: No hay bichos asignados al Jugador 1.");
        }
    }

    private void Update()
    {
        // Por ahora, usamos la tecla Tab para cambiar de turno manualmente
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            NextTurn();
        }
    }

    // Cambia el turno al otro jugador y rota el bicho que le toca
    public void NextTurn()
    {
        // Primero desactivamos el control de todos
        DesactivarTodosLosBichos();

        if (currentPlayer == 1)
        {
            // Terminó el turno del Jugador 1, avanzamos su índice
            currentIndexP1 = (currentIndexP1 + 1) % player1Bichos.Length;

            // Cambiamos el turno al Jugador 2
            currentPlayer = 2;

            // Activamos el bicho actual del Jugador 2
            if (player2Bichos.Length > 0 && player2Bichos[currentIndexP2] != null)
            {
                player2Bichos[currentIndexP2].SetControl(true);
            }
        }
        else // currentPlayer == 2
        {
            // Terminó el turno del Jugador 2, avanzamos su índice
            currentIndexP2 = (currentIndexP2 + 1) % player2Bichos.Length;

            // Cambiamos el turno al Jugador 1
            currentPlayer = 1;

            // Activamos el bicho actual del Jugador 1
            if (player1Bichos.Length > 0 && player1Bichos[currentIndexP1] != null)
            {
                player1Bichos[currentIndexP1].SetControl(true);
            }
        }

        Debug.Log($"Turno del Jugador {currentPlayer}");
    }

    // Desactiva el control en todos los bichos
    private void DesactivarTodosLosBichos()
    {
        if (player1Bichos != null)
        {
            foreach (BichoController b in player1Bichos)
            {
                if (b != null)
                    b.SetControl(false);
            }
        }

        if (player2Bichos != null)
        {
            foreach (BichoController b in player2Bichos)
            {
                if (b != null)
                    b.SetControl(false);
            }
        }
    }
}
