using DG.Tweening;
using UnityEngine;

public class CubeEyeAnimations : MonoBehaviour {
    [SerializeField] private Transform Eyes;
    [SerializeField] private Transform LeftEye;
    [SerializeField] private Transform RightEye;

    private float eyesScale;
    private float leftEyeScale;
    private float rightEyeScale;

    void Start() {
        eyesScale = Eyes.localScale.y;
        leftEyeScale = LeftEye.localScale.y;
        rightEyeScale = RightEye.localScale.y;

        RunEyeWinkSequence();
    }

    private void RunEyeWinkSequence() {
        var sequence = DOTween.Sequence();
        sequence.AppendInterval(Random.Range(2f, 10f));
        switch (Random.Range(0, 5)) {
            case 0:
            case 1:
                break;
            case 2:
                sequence.Append(Eyes.DOScaleY(0.2f * eyesScale, 0.2f));
                sequence.Append(Eyes.DOScaleY(eyesScale, 0.2f));
                break;
            case 3:
                sequence.Append(LeftEye.DOScaleY(0.25f * leftEyeScale, 0.2f));
                sequence.Join(RightEye.DOScaleY(1.1f * rightEyeScale, 0.2f));
                sequence.AppendInterval(0.1f);
                sequence.Append(LeftEye.DOScaleY(leftEyeScale, 0.2f));
                sequence.Join(RightEye.DOScaleY(rightEyeScale, 0.2f));
                break;
            case 4:
                sequence.Append(RightEye.DOScaleY(0.25f * rightEyeScale, 0.2f));
                sequence.Join(LeftEye.DOScaleY(1.1f * leftEyeScale, 0.2f));
                sequence.AppendInterval(0.1f);
                sequence.Append(RightEye.DOScaleY(rightEyeScale, 0.2f));
                sequence.Join(LeftEye.DOScaleY(leftEyeScale, 0.2f));
                break;
        }
        sequence.onComplete += RunEyeWinkSequence;
    }
}