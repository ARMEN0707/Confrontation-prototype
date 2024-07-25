using UnityEngine;
using Object = UnityEngine.Object;

public static class MouseManager
{
    public static Vector3 GetMousePosition(float order, tk2dCamera camera)
    {
        var mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, order);
        return camera.ScreenToWorldPoint(mousePosition);
    }

    public static T GetObject<T>(int mask, tk2dCamera camera) where T : Object
    {
        var ray = camera.ScreenCamera.ScreenPointToRay(Input.mousePosition);
        var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, mask);
        if (hit.collider != null)
            if (hit.collider.gameObject.TryGetComponent(out T obj))
                return obj;

        return null;
    }
}
