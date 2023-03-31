using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class HexView : MonoBehaviour {
    private const float APPEARANCE_DROP = 3;
    private const float APPEARANCE_JUMP = 0.2f;
    private const float APPEARANCE_BOUNCE_SCALE = 1.05f;
    private const float APPEARANCE_RAISE_TIME = 0.5f;
    private const float APPEARANCE_NORMALIZING_TIME = 0.1f;
    
    private HexController _controller;
    [SerializeField] private AssetReference HexSelection;
    [SerializeField] private AssetReference HexAvailability;

    [SerializeField] private TMP_Text TextX;
    [SerializeField] private TMP_Text TextY;

    [SerializeField] private Renderer MainRenderer;
    
    private bool _isMarked = false;
    private GameObject _mark;

    public Vector2Int index => _controller.Model.index;
    
    public void Setup(HexController controller, HexType type) {
        _controller = controller;
        TextX.text = _controller.Model.index.x.ToString();
        TextY.text = _controller.Model.index.y.ToString();
        transform.position = _controller.position;
    }

    public void SetSelected() {
        SetMark(HexSelection);
    }

    public void SetAvailable() {
        SetMark(HexAvailability);
    }

    private void SetMark(AssetReference markRef) {
        ClearMark();
        var instantiation = Addressables.InstantiateAsync(markRef);
        _isMarked = true;
        
        instantiation.Completed += (handle) => {
            _mark = handle.Result;
            _mark.transform.SetParent(transform);
            _mark.transform.localPosition = Vector3.zero;
        };
    }

    public void ClearMark() {
        if (!_isMarked) {
            return;
        }
        Destroy(_mark);
        _isMarked = false;
    }

    public void PlayAppearanceAnimation() {
        var myTransform = transform;
        var finalPosition = myTransform.position;
        myTransform.position = finalPosition + Vector3.down * APPEARANCE_DROP;
        myTransform.localScale = Vector3.zero;

        var appearance = DOTween.Sequence();
        appearance.Append(myTransform.DOMove(finalPosition + Vector3.up * APPEARANCE_JUMP, APPEARANCE_RAISE_TIME));
        appearance.Join(myTransform.DOScale(Vector3.one * APPEARANCE_BOUNCE_SCALE, APPEARANCE_RAISE_TIME));
        appearance.Append(myTransform.DOMove(finalPosition, APPEARANCE_NORMALIZING_TIME));
        appearance.Join(myTransform.DOScale(Vector3.one, APPEARANCE_NORMALIZING_TIME));
    }

    public Sequence Destroy() {
        transform.DOKill();
        
        var appearance = DOTween.Sequence();
        appearance.Append(transform.DOMove(transform.position + Vector3.down * APPEARANCE_DROP, APPEARANCE_RAISE_TIME));
        appearance.Append(transform.DOScale(Vector3.zero, APPEARANCE_RAISE_TIME));

        appearance.OnComplete(DestroyImmediate);
        return appearance;
    }

    public void DestroyImmediate() {
        GameObject.Destroy(this);
    }

    public void Free() {
        Debug.Log($"free {index}");
        MainRenderer.material.DOColor(Color.white, 1);
    }

    public void Occupy() {
        MainRenderer.material.DOColor(Color.red, 1);
    }
}
