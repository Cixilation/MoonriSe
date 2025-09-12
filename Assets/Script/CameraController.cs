using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float panSpeed = 10f;
    private float panBorderThickness = 10f;
    private Vector3 minPanLimits = new Vector3(100f, 20f, 100f); 
    private Vector3 maxPanLimits = new Vector3(600f, 40f, 620f); 
    private float zoomSpeed = 20f;
    public bool isActive = false;

    void Update()
    {
        if (!isActive) return;

        Vector3 pos = transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * zoomSpeed;

        pos.x = Mathf.Clamp(pos.x, minPanLimits.x, maxPanLimits.x);
        pos.y = Mathf.Clamp(pos.y, minPanLimits.y, maxPanLimits.y);
        pos.z = Mathf.Clamp(pos.z, minPanLimits.z, maxPanLimits.z);

        transform.position = pos;
    }


    public void SetCameraPosition(Vector3 position)
    {
        transform.position = new Vector3(position.x, 24.98f, position.z);
        transform.rotation = new Quaternion(0.5817f, -0.1698f, 0.1401f, 0.7830f); 
    }
}