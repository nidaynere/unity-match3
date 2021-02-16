using UnityEngine;

[CreateAssetMenu(fileName = "AnimationSettings", menuName = "Animation Settings", order = 1)]
public class AnimationSettings : ScriptableObject
{
    public AnimationCurve TransitionCurve;
    public AnimationCurve ScaleCurve;

    public float BubbleThrowSpeed = 10f;
    public float PositionUpdateSpeed = 1f;
    public float CombineAnimationDelay = 0.2f;
    public float ScaleUpdateSpeed = 1f;
    public float FlyAwayScaleSpeed = 0.2f;
}
