using UnityEngine;

public class WaterBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Timer.Instance.OnWin();
    }
}
