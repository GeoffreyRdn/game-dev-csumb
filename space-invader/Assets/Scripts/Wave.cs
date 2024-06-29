using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "Objects/Wave", order = 1)]
public class Wave : ScriptableObject
{
    public GameObject[] enemies;

    public int nbCols;
    
    public float initialSpeed;
    public float maxSpeed;
}
