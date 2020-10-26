using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerper : MonoBehaviour
{
    Vector3 prev;
    private void Start()
    {
        prev = transform.position;
    }
    public void LerpTo(Vector3 target, float t=3)
    {
        prev = transform.position;
        StartCoroutine(LerpBetweenPoints(transform.position, target, t));

        
    }
    public void LerpBack(float t=3)
    {
        
        StartCoroutine(LerpBetweenPoints(transform.position, prev, t));
        
    }
    IEnumerator LerpBetweenPoints(Vector3 A,Vector3 B, float t)
    {
        float startTime = Time.time;
        float endTime = t;
        float traveledTime = Time.time-startTime;
        float fraction;
        print(prev);    
        while (traveledTime<t)
        {
            traveledTime = Time.time - startTime;
            fraction = traveledTime / endTime;
            transform.position =Vector3.Lerp(A, B, fraction);
            print(prev);

            yield return new WaitForEndOfFrame();
        }
        
        
        yield return null;
    }
}
