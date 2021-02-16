using UnityEngine;
using System.Collections.Generic;

public class AimArrow : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Animator aimAnimator;
    [SerializeField] private Transform focusPoint;

    [SerializeField] private List<Collider> walls;
#pragma warning restore CS0649

    public int SetDirection (Vector2 direction, int maxCrosses, Vector3[] positions)
    {
        float rayLength = 10f;
        var normalize = direction.normalized;
        var dir = new Vector3(normalize.x, normalize.y, 0);

        Vector3 startPosition = focusPoint.position;

        int crosses = 0;
        positions[crosses] = startPosition;
        while (true && crosses < maxCrosses)
        {
            RaycastHit rh;
            if (Physics.Raycast(startPosition, dir, out rh, rayLength))
            {
                if (walls.Contains(rh.collider))
                {
                    // cross the direction.
                    crosses++;
                    startPosition = rh.point;
                    dir = Vector3.Reflect(dir, rh.normal);
                    positions[crosses] = startPosition;
                }
                else
                {
                    rayLength = rh.distance;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        if (crosses < maxCrosses)
        {
            crosses++;
            positions[crosses] = startPosition + dir * rayLength;
        }

        lineRenderer.positionCount = crosses + 1;
        lineRenderer.SetPositions(positions);

        return crosses;
    }

    public void SetVisual (bool value)
    {
        aimAnimator.SetBool("IsActive", value);
    }
}
