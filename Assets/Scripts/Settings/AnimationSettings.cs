using UnityEngine;

[CreateAssetMenu(fileName = "AnimationSettings", menuName = "Animation Settings", order = 1)]
public class AnimationSettings : ScriptableObject {
    public AnimationCurve TransitionCurve;
    public AnimationCurve ScaleCurve;
    public float PositionUpdateSpeed = 1f;
    public float ScaleUpdateSpeed = 1f;
    public float AnimationDelay = 0.2f;
}
