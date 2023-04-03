using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class CharView : MonoBehaviour {
    private CharController _controller;
    public CharController controller => _controller;
    [SerializeField] private List<Renderer> Renderers;

    public void Setup(CharController controller, HexController hex) {
        _controller = controller;
        transform.localScale = Vector3.zero;
        transform.position =hex.position + Vector3.up * 3;

        var appearance = DOTween.Sequence();
        appearance.Append(transform.DOMove(hex.position, 0.5f));
        appearance.Join(transform.DOScale(new Vector3(1.05f, 0.95f, 1.05f), 0.5f));
        appearance.Append(transform.DOScale(Vector3.one, 0.05f));
    }

    public void MoveTo(HexController newHex) {
        transform.DOLookAt(newHex.position.WithY(transform.position.y), 1f);
        transform.DOJump(newHex.position, 1f, 1, 2f);
    }
    
    public void ClearMark() {
        foreach (var render in Renderers) {
            render.material.DOColor(Color.white, 1);
        }
    }

    public void Mark() {
        foreach (var render in Renderers) {
            render.material.DOColor(Color.blue, 1);
        }
    }
}