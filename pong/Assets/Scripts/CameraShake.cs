using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private Color[] colors;

    private void Awake()
        => cam = GetComponent<Camera>();

    #region Methods
    public IEnumerator Shake(float duration, float shaking)
    {
        var camPos = transform.position;
        float timer = 0f;
        
        while (timer < duration)
        {
            float newX = Random.Range(-1f, 1f) * shaking + camPos.x;
            float newY = Random.Range(-1f, 1f) * shaking + camPos.y;

            transform.position = new Vector3(newX, newY, camPos.z);
            timer += Time.deltaTime;
            yield return 0;
        }
        
        transform.position = camPos;
    }

    public void ChangeBackground()
    {
        Color newColor = colors[Random.Range(0, colors.Length)];
        while (newColor == cam.backgroundColor)
            newColor = colors[Random.Range(0, colors.Length)];
        
        cam.backgroundColor = newColor;
    }
    
    #endregion
}
