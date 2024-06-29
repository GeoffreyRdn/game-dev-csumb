using System;
using Health;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicHandler : MonoBehaviour
{
    [SerializeField] private AudioClip menuAndLobbyMusic;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip bossMusic;
    
    [SerializeField] private string lobbyScene;
    [SerializeField] private string menuScene;
    [SerializeField] private string gameScene;
    [SerializeField] private string bossScene;

    private MusicHandler instance;
    private AudioSource audioSource;
    private PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        audioSource = GetComponent<AudioSource>();
        instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == gameScene && audioSource.clip != gameMusic)
        {
            Debug.Log("Game started !");
            audioSource.clip = gameMusic;
            audioSource.Play();
        }

        if ((scene.name == lobbyScene || scene.name == menuScene) && audioSource.clip != menuAndLobbyMusic)
        {
            Debug.Log("Lobby or Menu started !");
            audioSource.clip = menuAndLobbyMusic;
            audioSource.Play();

            if (scene.name == lobbyScene)
            {
                
                Debug.Log("CALL RESET PLAYERS ...");
                pv.RPC(nameof(ResetPlayers), RpcTarget.All);
            }
        }

        if (scene.name == bossScene && audioSource.clip != bossMusic)
        {
            Debug.Log("Boss started !");
            pv.RPC(nameof(ResetPlayers), RpcTarget.All);
            audioSource.clip = bossMusic;
            audioSource.Play();
        }
    }
    
    [PunRPC]
    private void ResetPlayers()
    {
        Debug.Log("IN RESET PLAYERS");
        foreach (var player in PhotonNetwork.PlayerList)
        {
            Debug.Log("RESETTING " + player.NickName);
            
            GameObject playerGO = player.TagObject as GameObject;
            var health = playerGO.GetComponent<HealthController>();
            health.ResetHealth();

            var playerController = playerGO.GetComponent<PlayerController>();
            playerController.isDead = false;
            playerController.currentState = PlayerController.IdleAnimation;
            
            playerController.UpdateHealthBar(health.Health, health.MaxHealth);
            
            InputManager.Instance.EnableControls();
        }
    }
}
