using UnityEngine;

public class WinHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    private void OnTriggerEnter(Collider other)
    {
        animator.enabled = true;
        Timer.Instance.OnWin();
    }
}
