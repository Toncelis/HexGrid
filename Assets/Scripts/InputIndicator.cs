using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InputIndicator : MonoBehaviour {
    [SerializeField] private float TransitionDuration;

    [SerializeField] private Image MouseLeft;
    [SerializeField] private Image MouseMid;
    [SerializeField] private Image MouseRight;
    
    [SerializeField] private Image Shift;
    [SerializeField] private Image Ctrl;

    private void OnDown(Image image) {
        image.DOKill();
        image.DOColor(Color.grey, TransitionDuration);
    }

    private void OnUp(Image image) {
        image.DOKill();
        image.DOColor(Color.white, TransitionDuration);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            OnDown(Shift);
        }
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            OnDown(Ctrl);
        }
        if (Input.GetMouseButtonDown(0)) {
            OnDown(MouseLeft);
        }
        if (Input.GetMouseButtonDown(1)) {
            OnDown(MouseRight);
        }
        if (Input.GetMouseButtonDown(2)) {
            OnDown(MouseMid);
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            OnUp(Shift);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl)) {
            OnUp(Ctrl);
        }
        if (Input.GetMouseButtonUp(0)) {
            OnUp(MouseLeft);
        }
        if (Input.GetMouseButtonUp(1)) {
            OnUp(MouseRight);
        }
        if (Input.GetMouseButtonUp(2)) {
            OnUp(MouseMid);
        }
    }
}
