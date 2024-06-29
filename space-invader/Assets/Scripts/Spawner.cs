using System.Collections;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Spawner : MonoBehaviour
{
    #region Variables

    [SerializeField] private Wave[] waves;
    private int currentWave;
    private int nbWaves;
    
    [SerializeField] private Transform extraHolder;
    [SerializeField] private Transform ennemiesHolder;
    [SerializeField] private Transform bulletHolder;
    [SerializeField] private Transform bunkersHolder;
    
    [SerializeField] private Sprite spriteBunker;
    
    [SerializeField] private GameObject extraEnemyPrefab;
    private GameObject extraEnemy;

    private int nbCols;
    private int nbRows;
    
    [SerializeField] private float speed;
    public float timeToMove;
    private float currentTimer;
    private float speedUp;
    
    private int direction = 1;

    private bool justWentDown;
    
    private Vector3 initialPos;

    private AudioSource audioSource;
    
    public static int EnemiesRemaining;
    public static bool AnimationIsRunning;

    #endregion

    #region Events

    private void OnEnable()
    {
        Enemy.OnWin += OnWin;
        Enemy.OnEnemyDestroyed += OnEnemyKilled;
        Enemy.OnEnemyShoot += ChangeBulletParent;
        Player.OnPlayerLose += DestroyEnnemies;
        GameHandler.Restart += Restart;
    }

    private void OnDisable()
    {
        Enemy.OnWin -= OnWin;
        Enemy.OnEnemyDestroyed -= OnEnemyKilled;
        Enemy.OnEnemyShoot -= ChangeBulletParent;
        Player.OnPlayerLose -= DestroyEnnemies;
        GameHandler.Restart -= Restart;
    }

    #endregion

    #region Init

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        initialPos = ennemiesHolder.position;
        nbWaves = waves.Length;
        timeToMove = waves[0].initialSpeed;
        currentTimer = timeToMove;
        StartCoroutine(SpawnEnemies(0));
    }

    private void Update()
    {
        if (GameHandler.hasLoose || AnimationIsRunning) return; 
        
        if (currentTimer > 0)
        {
            currentTimer -= Time.deltaTime;
            return;
        }
        
        audioSource.Play();

        // 10 % chance of spawning an extra
        Random random = new Random();
        if (random.Next(100) < 10 && !extraEnemy)
        {
            EnemiesRemaining++;
            extraEnemy = Instantiate(extraEnemyPrefab, new Vector3(-10, 4), Quaternion.identity, extraHolder);
        }

        // reset delay
        currentTimer = timeToMove;

        // gets the first enemy in this GO with position >= 7.5f or position <= 7.5f
        bool goDown = ennemiesHolder.Cast<Transform>().Any(enemy => enemy.position.x >= 7.5 || enemy.position.x <= -7.5);
        
        float min = GetLowestEnemyYPos();
        
        GameHandler.hasLoose |= ennemiesHolder.position.y + min < -3f;
        if (GameHandler.hasLoose) Player.OnLose();

        if (!justWentDown && goDown)
        {
            direction *= -1;
            
            var position = ennemiesHolder.position;
            position.y -= 0.5f;
            ennemiesHolder.position = position;
            
            justWentDown = true;
            
            return;
        }
        
        ennemiesHolder.position += Vector3.right * speed * direction;
        justWentDown = false;
    }

    #endregion

    #region Methods

    private void OnWin()
    {
        ResetBunkers();
        StartCoroutine(SpawnEnemies(0.5f));
    }

    private IEnumerator SpawnEnemies(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // no more waves : keep the last one
        if (nbWaves == currentWave) currentWave--;

        ennemiesHolder.position = initialPos;
        timeToMove = waves[currentWave].initialSpeed;
        nbCols = waves[currentWave].nbCols;
        speedUp = (waves[currentWave].initialSpeed - waves[currentWave].maxSpeed) / (waves[currentWave].enemies.Length * nbCols);
        nbRows = waves[currentWave].enemies.Length;
        EnemiesRemaining = nbCols * nbRows;
        
        InstantiateEnemies();
    }
    
    private void InstantiateEnemies()
    {
        for (var i = 0; i < nbRows; i++)
            for (var j = 0; j < nbCols; j++)
                Instantiate(waves[currentWave].enemies[i], ennemiesHolder.position + new Vector3(j * 1.4f, -i * 0.8f),
                    Quaternion.identity, ennemiesHolder);

        currentWave++;
    }


    private float GetLowestEnemyYPos()
    {
        float min = 0;
        return ennemiesHolder.Cast<Transform>().Select(enemy => enemy.localPosition.y).Prepend(min).Min();
    }
    
    private void OnEnemyKilled(int points)
    {
        // dont decrease if extra
        if (points > 50) return;
        timeToMove -= speedUp;
    }
    
    private void DestroyEnnemies()
    {
        foreach (Transform enemy in ennemiesHolder)
            Destroy(enemy.gameObject);
    }
    
    private void ResetBunkers()
    {
        foreach (Transform bunker in bunkersHolder)
        {
            bunker.gameObject.GetComponent<SpriteRenderer>().sprite = spriteBunker;
            bunker.gameObject.SetActive(true);
            bunker.GetComponent<Bunker>().lives = 2;
        }
    }

    private void ResetBullets()
    {
        foreach (Transform bullet in bulletHolder)
            Destroy(bullet.gameObject);
    }

    private void Restart()
    {
        currentWave = 0;
        currentTimer = timeToMove;

        foreach (Transform extra in extraHolder)
            Destroy(extra.gameObject);
        
        ResetBunkers();
        ResetBullets();
        DestroyEnnemies();
        StartCoroutine(SpawnEnemies(0));
    }

    private void ChangeBulletParent(GameObject bullet)
    {
        bullet.transform.parent = bulletHolder;
    }

    #endregion
}
