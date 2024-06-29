using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    #region Variables
    
    public float speed = 5;
    private Rigidbody2D rigidbody;
    
    #endregion

    #region Init
    
    private void Awake()
        => rigidbody = GetComponent<Rigidbody2D>();

    void Start()
        => Fire();
    
    #endregion

    #region Methods
    
    private void Fire()
        => rigidbody.velocity = Vector2.down * speed;
    
    #endregion
}
