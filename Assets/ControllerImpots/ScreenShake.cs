using System.Collections;
using UnityEngine;
//using XInputDotNetPure;

public class ScreenShake : MonoBehaviour
{
    public float intensity;
    public float duration;

    private Vector3 startPos;
    
    private void Awake()
    {
        startPos = transform.position;
    }
    
    private void Update()
    {

    }

    public void ShakeScreen()
    {
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        for (int i = 0; i < 4; i++)
        {
            //GamePad.SetVibration((PlayerIndex)i, 1, 1);
        }

        float endTime = Time.time + duration;

        while (Time.time < endTime)
        {
            Vector3 shake = (Vector3)Random.insideUnitCircle /** Random.Range(0f, 1f)*/ * intensity + startPos;
            transform.position = shake;

            yield return 0;
        }

        transform.position = startPos;

        for (int i = 0; i < 4; i++)
        {
            //GamePad.SetVibration((PlayerIndex)i, 0, 0);
        }
    }
}
