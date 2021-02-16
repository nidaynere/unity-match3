using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class GameMember : Transition, IPointerClickHandler
{
    private Action onClick;

    public void SetClickAction(Action onClick)
    {
        this.onClick = onClick;
    }

    public void SetPosition(int X, int Y)
    {
        transform.localPosition = new Vector3(X, -Y);
    }

    public void SetTransition (int X, int Y, Action onCompleted = null)
    {
        var tPos = transform.parent.TransformPoint (new Vector3(X, -Y));

        Move(tPos, animationSettings.PositionUpdateSpeed, onCompleted);
    }

    public void Kill()
    {
        Scale(Vector3.zero, animationSettings.ScaleUpdateSpeed,  () => { gameObject.SetActive(false); });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke();
    }
}
