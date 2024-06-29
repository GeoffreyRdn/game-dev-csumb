using Unity.VisualScripting;
using UnityEngine;

public class CoinBehavior : MonoBehaviour
{
    private void Update()
        => transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.GameObject());
        other.GetComponent<GameHandler>().OnCoinCollected(other.GameObject());
    }
}
