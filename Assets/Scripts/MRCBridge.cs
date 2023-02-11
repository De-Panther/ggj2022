using AOT;
using System.Runtime.InteropServices;
using UnityEngine;
using WebXR.Interactions;

public class MRCBridge : MonoBehaviour
{
  private static MRCBridge instance;

  public SpectatorCamera spectatorCamera;
  public MixedRealityCaptureController mRCController;
  public PlayWebcam playWebcam;

  private delegate void dVoidBool(bool value);
  private delegate void dVoidInt(int value);
  private delegate void dVoidIntIntInt(int value1, int value2, int value3);

#if !UNITY_EDITOR && UNITY_WEBGL
  [DllImport("__Internal")]
  private static extern void SetMRCActions(dVoidBool captureAllKeyboardInput,
    dVoidBool enableSpectator, dVoidBool enableMRC, dVoidInt trySetFramesDelay,
    dVoidIntIntInt trySetThresholdMinColor, dVoidIntIntInt trySetThresholdMaxColor,
    dVoidInt trySetWebcamIndex);
#endif


  void Awake()
  {
    instance = this;
#if !UNITY_EDITOR && UNITY_WEBGL
    SetMRCActions(CaptureAllKeyboardInput, EnableSpectator, EnableMRC,
      TrySetFramesDelay, TrySetThresholdMinColor, TrySetThresholdMaxColor,
      TrySetWebcamIndex);
#endif
  }

  [MonoPInvokeCallback(typeof(dVoidBool))]
  static void CaptureAllKeyboardInput(bool value)
  {
    WebGLInput.captureAllKeyboardInput = value;
  }

  [MonoPInvokeCallback(typeof(dVoidBool))]
  static void EnableSpectator(bool value)
  {
    instance.spectatorCamera.EnableCameraInXR(value);
  }

  [MonoPInvokeCallback(typeof(dVoidBool))]
  static void EnableMRC(bool value)
  {
    instance.mRCController.EnableInXR(value);
  }

  [MonoPInvokeCallback(typeof(dVoidInt))]
  static void TrySetFramesDelay(int value)
  {
    instance.mRCController.TrySetFramesDelay(value);
  }

  [MonoPInvokeCallback(typeof(dVoidIntIntInt))]
  static void TrySetThresholdMinColor(int r, int g, int b)
  {
    instance.playWebcam.TrySetThresholdMinColor(
      new Color((float)Mathf.Clamp(r, 0, 255) / 255f,
      (float)Mathf.Clamp(g, 0, 255) / 255f,
      (float)Mathf.Clamp(b, 0, 255) / 255f));
  }

  [MonoPInvokeCallback(typeof(dVoidIntIntInt))]
  static void TrySetThresholdMaxColor(int r, int g, int b)
  {
    instance.playWebcam.TrySetThresholdMaxColor(
      new Color((float)Mathf.Clamp(r, 0, 255) / 255f,
      (float)Mathf.Clamp(g, 0, 255) / 255f,
      (float)Mathf.Clamp(b, 0, 255) / 255f));
  }

  [MonoPInvokeCallback(typeof(dVoidInt))]
  static void TrySetWebcamIndex(int value)
  {
    instance.playWebcam.TrySetWebcamIndex(value.ToString());
  }
}
