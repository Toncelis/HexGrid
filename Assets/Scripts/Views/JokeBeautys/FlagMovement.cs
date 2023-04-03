using DG.Tweening;
using UnityEngine;

public class FlagMovement : MonoBehaviour {
    void Start() {
        var sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(Vector3.up * 10, 4f));
        sequence.Append(transform.DORotate(-Vector3.up * 10, 4f));
        sequence.SetLoops(-1);
    }
}