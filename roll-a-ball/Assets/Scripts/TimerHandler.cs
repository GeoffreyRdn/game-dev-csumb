using System.Collections;
using TMPro;
using UnityEngine;

public class TimerHandler : MonoBehaviour
{
    public static bool run = true;
    private int timer = 0;
    [SerializeField] TextMeshProUGUI timerTMP;
    
    private void Start()
        => StartCoroutine(Timer());

    IEnumerator Timer()
    {
        while (run)
        {
            timerTMP.text = timer.ToString();
            timer++;
            yield return new WaitForSeconds(1);
        }
    }
}
