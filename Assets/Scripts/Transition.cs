using UnityEngine;
using System;
using System.Collections;

public class Transition : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] protected AnimationSettings animationSettings;
#pragma warning restore CS0649

    public void Move(Vector3 target, float speed, Action onCompleted = null)
    {
        StartCoroutine(startTransition(target, speed, onCompleted, true));
    }

    public void Move(Vector3[] target, float speed, Action onCompleted = null)
    {
        Debug.Log("Move to target array.");
        StartCoroutine(startTransition(target, speed, onCompleted));
    }

    public void Scale (Vector3 target, float speed, Action onCompleted = null)
    {
        StartCoroutine(startScaling(target, speed, onCompleted));
    }

    private IEnumerator startTransition(Vector3[] target, float speed, Action onCompleted)
    {
        Debug.Log("Move to target array, path length = " + target.Length);
        int pathCount = target.Length;
        int currentPath = 0;

        bool isInTransition = false;
        while (true)
        {
            if (!isInTransition)
            {
                if (currentPath >= target.Length)
                {
                    Debug.Log("finished");
                    onCompleted?.Invoke();
                    break;
                }

                Debug.Log("Transition starting for " + currentPath);

                isInTransition = true;

                StartCoroutine(startTransition(target[currentPath], speed, () =>
                {
                    currentPath++;
                    isInTransition = false;
                    Debug.Log(currentPath + ", isintransition" + isInTransition);
                }, false));
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator startTransition(Vector3 target, float speed, Action onCompleted, bool useCurve = false)
    {
        Vector3 startPosition = transform.position;
        float progress = 0;
        float progressSpeed = speed / (Vector3.Distance(target, startPosition) + 0.001f);

        while (true)
        {
            progress = Mathf.Min(progress + progressSpeed * Time.deltaTime, 1);

            transform.position = Vector3.Lerp(startPosition, target, 
                useCurve ? animationSettings.TransitionCurve.Evaluate (progress) : progress);

            if (progress == 1)
            {
                onCompleted?.Invoke();
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator startScaling (Vector3 target, float speed, Action onCompleted)
    {
        Vector3 startScale = transform.localScale;
        float progress = 0;
        float progressSpeed = speed / (Vector3.Distance(target, startScale) + 0.001f);

        while (true)
        {
            progress = Mathf.Min(progress + progressSpeed * Time.deltaTime, 1);

            transform.localScale = Vector3.Lerp(startScale, target,
                animationSettings.ScaleCurve.Evaluate(progress));

            if (progress == 1)
            {
                onCompleted?.Invoke();
                break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
