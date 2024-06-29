using System;
using UnityEngine;

public class PaddleModifier : MonoBehaviour
{
    [SerializeField] private GameObject paddle1;
    [SerializeField] private GameObject paddle2;

    private Vector3 defaultSize;

    private void Awake()
        => defaultSize = paddle1.transform.localScale;

    public void ChangePaddleSize(int score1, int score2)
    {
        var diff = Math.Abs(score1 - score2) / 3;

        paddle1.transform.localScale = defaultSize + (score1 > score2 ? new Vector3(-diff, 0) : new Vector3(diff, 0));
        paddle2.transform.localScale = defaultSize + (score2 > score1 ? new Vector3(-diff, 0) : new Vector3(diff, 0));
    }
    
    
}
