using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour
{
    #region Event

    public delegate void EnemyDestroyed(int points);
    public static event EnemyDestroyed OnEnemyDestroyed;

    public delegate void EnemyShoot(GameObject bullet);
    public static event EnemyShoot OnEnemyShoot;

    public delegate void Win();
    public static event Win OnWin;

    #endregion
    
    #region Variables

    [SerializeField] private int points;
    [SerializeField] private bool canFire;
    [SerializeField] private GameObject bulletPrefab;

    private GameObject currentBullet;
    private AudioSource audioSource;

    [SerializeField] private AudioClip enemyDeathSound;
    [SerializeField] private AudioClip enemyShootSound;

    private Animator animator;
    private ExtraEnemy extraEnemy;
    
    private static readonly int Death = Animator.StringToHash("Death");

    #endregion
    
    #region Init
    
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        extraEnemy = this as ExtraEnemy;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameHandler.hasLoose) return;
        
        Random random = new Random();
        if (canFire && random.Next(10000) < 1) Shoot();
        
        if (!extraEnemy) return;

        transform.position += Vector3.right * Time.deltaTime * 2;
        if (transform.position.x > 10)
        {
            Spawner.EnemiesRemaining--;
            if (Spawner.EnemiesRemaining == 0)
                OnWin?.Invoke();
            
            Destroy(gameObject);
        }
    }

    #endregion

    #region Methods
    
    private void Shoot()
    {
        if (currentBullet) return;
        
        audioSource.clip = enemyShootSound;
        audioSource.Play();
        
        currentBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        OnEnemyShoot?.Invoke(currentBullet);
        Destroy(currentBullet, 3f);
    }

    private void OnTriggerEnter2D(Collider2D bullet)
    {
        if (bullet.CompareTag("Player")) return;
        
        animator.SetTrigger(Death);
        GetComponentInChildren<ParticleSystem>().Play();
        Destroy(bullet.gameObject);

        Spawner.EnemiesRemaining--;
        if (Spawner.EnemiesRemaining == 0) OnWin?.Invoke();

        audioSource.clip = enemyDeathSound;
        audioSource.Play();
        
        Random random = new Random();
        OnEnemyDestroyed?.Invoke(extraEnemy ? extraEnemy.pointsList[random.Next(extraEnemy.pointsList.Length)] : points);
    }

    #endregion
}
