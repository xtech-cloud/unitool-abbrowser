using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using XTC.Logger;
using XTC.MVCS;

public class MouseController : Controller
{
    public const string NAME = "MouseController";

    public Transform Target { get; set; }

    public Transform canvas2D { get; set; }
    public float Distance = 10.0f;
    public float XSpeed = 100.0f;
    public float YSpeed = 100.0f;

    public float YMinLimit = -90f /* -360f*/;
    public float YMaxLimit = 90f /*/360f */;

    public float DistanceMin = 0.5f;
    public float DistanceMax = 1000f;

    private Camera camera_ { get; set; }
    private float x_ = 0.0f;
    private float y_ = 0.0f;
    private bool active_ { get; set; }

    private GameObject zeroPivot { get; set; }

    protected override void setup()
    {
        zeroPivot = new GameObject("0 pivot");
        zeroPivot.transform.position = Vector3.zero;
        Target = zeroPivot.transform;
        camera_ = Camera.main;
        SyncAngles();
        active_ = true;
    }

    protected override void dismantle()
    {
        GameObject.Destroy(zeroPivot);
    }

    public void LateUpdate()
    {
        if (null != canvas2D)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            GraphicRaycaster gr = canvas2D.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);
            if (results.Count > 0)
                return;
        }

        Rotate();
        Zoom();
    }

    public void SyncAngles()
    {
        Vector3 angles = camera_.transform.eulerAngles;
        x_ = angles.y;
        y_ = angles.x;
    }

    public void SetActive(bool _active)
    {
        active_ = _active;
    }

    public void Rotate()
    {
        if (!active_)
            return;

        if (!Input.GetMouseButton(1))
            return;

        float deltaX = Input.GetAxis("Mouse X");
        float deltaY = Input.GetAxis("Mouse Y");

        deltaX = deltaX * XSpeed*Time.deltaTime;
        deltaY = deltaY * YSpeed*Time.deltaTime;

        x_ += deltaX;
        y_ -= deltaY;
        y_ = clampAngle(y_, YMinLimit, YMaxLimit);

        Quaternion rotH = Quaternion.AngleAxis(x_, Vector3.up);
        Quaternion rotV = Quaternion.AngleAxis(y_, Vector3.left);
        Vector3 dir = rotH * rotV * Target.forward;
        camera_.transform.position = Target.position + dir* Distance;
        camera_.transform.LookAt(Target);
    }

    public void Zoom()
    {
        float mwheel = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(mwheel) < 0.01f)
            return;

        if (camera_.orthographic)
        {
            camera_.orthographicSize -= mwheel * camera_.orthographicSize;
            if (camera_.orthographicSize < 0.01f)
            {
                camera_.orthographicSize = 0.01f;
            }
        }

        Quaternion rotation = camera_.transform.rotation;
        Distance = Mathf.Clamp(Distance - mwheel * Mathf.Max(1.0f, Distance), DistanceMin, DistanceMax);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -Distance);
        Vector3 position = rotation * negDistance + Target.position;
        camera_.transform.position = position;
    }

    private static float clampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }
        if (angle > 360F)
        {
            angle -= 360F;
        }
        return Mathf.Clamp(angle, min, max);
    }
}//namespace
