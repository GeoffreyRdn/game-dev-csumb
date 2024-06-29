using UnityEngine;

public class Bunker : MonoBehaviour
{
    #region Variables
    
    [SerializeField] private Sprite spriteHit;
    [HideInInspector] public int lives = 2;

    #endregion
    
    #region Methods
    
    private void OnTriggerEnter2D(Collider2D bullet)
    {
        lives--;
        Destroy(bullet.gameObject);
        if (lives == 0) gameObject.SetActive(false);
        else GetComponent<SpriteRenderer>().sprite = spriteHit;
    }

    #endregion
}
