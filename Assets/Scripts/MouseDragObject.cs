using UnityEngine;
using WebXR;

[RequireComponent(typeof(Rigidbody))]
public class MouseDragObject : MonoBehaviour
{
  public System.Action<bool> OnHeld;

  [SerializeField]
  private float m_zPos = 0.28f;
  private Camera m_currentCamera;
  private Rigidbody m_rigidbody;
  private Vector3 m_screenPoint;
  private Vector3 m_offset;
  private Vector3 m_currentVelocity;
  private Vector3 m_previousPos;
  private Vector3 m_newPos;
  private bool m_useFixedPos = false;

  void Awake()
  {
    m_rigidbody = GetComponent<Rigidbody>();
  }

  void OnEnable()
  {
    BlocksGenerator.OnWebXRStateChange += HandleOnWebXRStateChange;
  }

  void OnDisable()
  {
    BlocksGenerator.OnWebXRStateChange -= HandleOnWebXRStateChange;
  }

  void OnMouseDown()
  {
    m_currentCamera = FindCamera();
    if (m_currentCamera != null)
    {
      m_screenPoint = m_currentCamera.WorldToScreenPoint(gameObject.transform.position);
      m_offset = gameObject.transform.position - m_currentCamera.ScreenToWorldPoint(GetMousePosWithScreenZ(m_screenPoint.z));
    }
    OnHeld?.Invoke(true);
  }

  void OnMouseUp()
  {
    m_rigidbody.velocity = m_currentVelocity;
    m_currentCamera = null;
    OnHeld?.Invoke(false);
  }

  void FixedUpdate()
  {
    if (m_currentCamera != null)
    {
      Vector3 currentScreenPoint = GetMousePosWithScreenZ(m_screenPoint.z);
      m_rigidbody.velocity = Vector3.zero;
      m_newPos = m_currentCamera.ScreenToWorldPoint(currentScreenPoint) + m_offset;
      if (m_useFixedPos)
      {
        m_newPos.z = m_zPos;
      }
      m_rigidbody.MovePosition(m_newPos);
      m_currentVelocity = (transform.position - m_previousPos) / Time.deltaTime;
      m_previousPos = transform.position;
    }
  }

  Vector3 GetMousePosWithScreenZ(float screenZ)
  {
    return new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenZ);
  }

  Camera FindCamera()
  {
    Camera[] cameras = FindObjectsOfType<Camera>();
    Camera result = null;
    int camerasSum = 0;
    foreach (var camera in cameras)
    {
      if (camera.enabled)
      {
        result = camera;
        camerasSum++;
      }
    }
    if (camerasSum > 1)
    {
      result = null;
    }
    return result;
  }

  void HandleOnWebXRStateChange(WebXRState state)
  {
    UseFixedPos(state == WebXRState.NORMAL);
  }

  public void UseFixedPos(bool value)
  {
    m_useFixedPos = value;
  }
}
