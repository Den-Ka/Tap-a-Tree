using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraResizer : MonoBehaviour
{
    [SerializeField] private float targetWidthInUnits = 10f; // Желаемая ширина камеры в юнитах
    [SerializeField] private float bottomBoundaryY = -4f;    // Нижняя граница камеры в юнитах

    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        AdjustCameraSize();
    }

#if UNITY_EDITOR
    private void Update()
    {
        AdjustCameraSize();
    }
#endif
    
    private void AdjustCameraSize()
    {
        // Вычисляем требуемый размер камеры по высоте, чтобы вьюпорт совпадал с targetWidthInUnits по ширине
        float targetAspect = Screen.width / (float)Screen.height;
        float orthographicSize = (targetWidthInUnits / 2f) / targetAspect;

        // Устанавливаем размер камеры
        _camera.orthographicSize = orthographicSize;

        // Смещаем позицию камеры так, чтобы нижняя граница совпадала с bottomBoundaryY
        float cameraHeight = orthographicSize * 2f;
        float topBoundaryY = bottomBoundaryY + cameraHeight;
        _camera.transform.position = new Vector3(0, topBoundaryY - cameraHeight / 2f, _camera.transform.position.z);
    }

    private void OnValidate()
    {
        if (_camera == null) _camera = GetComponent<Camera>();
        AdjustCameraSize();
    }
}