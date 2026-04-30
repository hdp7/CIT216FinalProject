//4/29/26
//Herman Pagan Alvarez
//Scale the gameobject with given parameters
using System.Collections;
using UnityEngine;
public class GrowScale : MonoBehaviour
{
    public Vector3 maxSize;
    public float duration;
    private Transform tf;
    private Vector3 maxSizeVector;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ScaleRoutine());
    } 

    // Update is called once per frame

    private IEnumerator ScaleRoutine()
    {
        Vector3 initialScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.localScale = Vector3.Lerp(initialScale, maxSize, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure exact finish
        transform.localScale = maxSize;
        Destroy(gameObject, .5f);
    }
}

