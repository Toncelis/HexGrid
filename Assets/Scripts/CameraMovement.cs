using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField] private Camera Camera;
    
    [SerializeField] private float CameraAcceleration;
    [SerializeField] private float CameraMaxSpeed;
    [SerializeField] private float BorderWidthForCameraMovement;

    private Vector3 _cameraSpeed = Vector3.zero;
    
    private void Update() {
        BorderCheck();
    }

    private void BorderCheck() {
        Vector3 mousePosition = Input.mousePosition;
        var desiredSpeed = Vector3.zero;        
        
        if (mousePosition.x > Screen.width-BorderWidthForCameraMovement) {
            desiredSpeed += Vector3.right;
        }
        if (mousePosition.x < BorderWidthForCameraMovement) {
            desiredSpeed += Vector3.left;
        }
        if (mousePosition.y > Screen.height - BorderWidthForCameraMovement) {
            desiredSpeed += Vector3.forward;
        }
        if (mousePosition.y < BorderWidthForCameraMovement) {
            desiredSpeed += Vector3.back;
        }

        desiredSpeed = desiredSpeed.normalized * CameraMaxSpeed;
        if (_cameraSpeed != desiredSpeed) {
            _cameraSpeed = Vector3.MoveTowards(_cameraSpeed, desiredSpeed, CameraAcceleration * Time.deltaTime);
        }
        Camera.transform.position += _cameraSpeed * Time.deltaTime;
    }
}