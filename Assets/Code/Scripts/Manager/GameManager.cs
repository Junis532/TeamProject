using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [Header("Manager 包访 内靛")]
    public AudioManager audioManager;

    [Header("Player 包访 内靛")]
    public PlayerController playerController;
    
}
