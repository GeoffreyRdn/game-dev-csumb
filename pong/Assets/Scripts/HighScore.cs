using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/HighScore", order = 1)]
public class HighScore : ScriptableObject
{
    public string highScore;
    public int diff;
    public string text;
}
