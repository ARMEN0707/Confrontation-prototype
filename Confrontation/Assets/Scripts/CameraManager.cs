using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private tk2dCamera _camera;
    [SerializeField] private tk2dCamera _pageCamera;
    [SerializeField] private tk2dCamera _messageBoxCamera;
    [SerializeField] private tk2dCamera _popUpCamera;
    [SerializeField] private tk2dCamera _academyCamera;
    
    [SerializeField] private LayerMask _pageMask;
    [SerializeField] private LayerMask _messageBoxMask;
    [SerializeField] private LayerMask _popUpMask;
    [SerializeField] private LayerMask _academyMask;

    public static tk2dCamera GameCamera => _instance._camera;
    public static tk2dCamera PageCamera => _instance._pageCamera;
    public static tk2dCamera MessageBoxCamera => _instance._messageBoxCamera;
    public static tk2dCamera PopUpCamera => _instance._popUpCamera;
    public static tk2dCamera AcademyCamera => _instance._academyCamera;
    
    public static LayerMask PageMask => _instance._pageMask;
    public static LayerMask MessageBoxMask => _instance._messageBoxMask;
    public static LayerMask PopUpMask => _instance._popUpMask;
    public static LayerMask AcademyMask => _instance._academyMask;
    
    private static CameraManager _instance;
    
    private void Awake()
    {
        _instance = this;
    }

    public static void CameraZoom(Touch zero, Touch first, tk2dCamera camera, float minZoom, float maxZoom)
    {
        var touchZero = zero;
        var touchOne = first;

        var touchZeroLastPos = touchZero.position - touchZero.deltaPosition;
        var touchOneLastPos = touchOne.position - touchOne.deltaPosition;

        var distanceTouch = (touchZeroLastPos - touchOneLastPos).magnitude;
        var currentDistanceTouch = (touchZero.position - touchOne.position).magnitude;
            
        Zoom(camera, (currentDistanceTouch - distanceTouch) / 50f, minZoom, maxZoom);
    }

    public static void Zoom(tk2dCamera camera, float distance, float minZoom, float maxZoom)
    {
        camera.ZoomFactor = Mathf.Clamp(camera.ZoomFactor + distance, minZoom, maxZoom);
    }
    
    public static void CameraMove(tk2dCamera camera, Vector3 mouseDownPos, Vector2 center, Vector2 size)
    {
        var direction = mouseDownPos - camera.ScreenToWorldPoint(Input.mousePosition);
        var (newX, newY, _) = camera.transform.position + direction;
        if (newX < center.x -size.x / 2 || newX > center.x + size.x / 2)
            return;
        
        if (newY < center.y - size.y / 2 || newY > center.y + size.y / 2)
            return;
        
        camera.transform.position += direction;
    }
}