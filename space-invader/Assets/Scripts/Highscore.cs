using UnityEngine;

[CreateAssetMenu(fileName = "HighScore", menuName = "Objects/HighScore", order = 1)]
public class Highscore : ScriptableObject
{
    public string text;
    public int score;
}
