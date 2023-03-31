using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class ArrowView : MonoBehaviour {
    [SerializeField] private Vector3 From;
    [SerializeField] private Vector3 To;
    [PropertySpace(2)]
    [SerializeField] private float HeadLength = 0.2f;
    [PropertySpace(6)]
    [SerializeField, FoldoutGroup("InnerDependencies")] private Transform Head;
    [SerializeField, FoldoutGroup("InnerDependencies")] private Transform Shaft;

    public void Start() {
        Point();
    }
    
    public void Setup(Vector3 from, Vector3 to) {
        From = from;
        To = to;
        Point();
        Debug.Log($"drawing arrow from {From} to {To}");
    }

    private void Point() {
        float magnitude = Vector3.Magnitude(To - From);
        Shaft.localScale = Vector3.one.WithZ(magnitude - HeadLength);
        Head.localScale = Vector3.one.WithZ(HeadLength);
        Head.localPosition = Head.localPosition.WithZ(Shaft.localScale.z);

        transform.position = From;
        transform.LookAt(To);

        var sequence = DOTween.Sequence();
        sequence.AppendInterval(1f).Append(transform.DOScaleX(0, 0.2f)).Join(transform.DOScaleY(0, 0.2f));
        sequence.onComplete += ()=>Destroy(gameObject);
    }
}