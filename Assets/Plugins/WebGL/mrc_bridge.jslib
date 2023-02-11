var LibraryMRCBridge = {
  SetMRCActions: function (CaptureAllKeyboardInput, EnableSpectator,
    EnableMRC, TrySetFramesDelay, TrySetThresholdMinColor,
    TrySetThresholdMaxColor, TrySetWebcamIndex) {
    window.mixedRealityCapture = window.mixedRealityCapture || {};

    window.mixedRealityCapture.captureAllKeyboardInput = function (value) {
      Module.dynCall_vi(CaptureAllKeyboardInput, value);
    }

    window.mixedRealityCapture.enableSpectator = function (value) {
      Module.dynCall_vi(EnableSpectator, value);
    }

    window.mixedRealityCapture.enableMRC = function (value) {
      Module.dynCall_vi(EnableMRC, value);
    }

    window.mixedRealityCapture.trySetFramesDelay = function (value) {
      Module.dynCall_vi(TrySetFramesDelay, value);
    }

    window.mixedRealityCapture.trySetThresholdMinColor = function (r, g, b) {
      Module.dynCall_viii(TrySetThresholdMinColor, r, g, b);
    }

    window.mixedRealityCapture.trySetThresholdMaxColor = function (r, g, b) {
      Module.dynCall_viii(TrySetThresholdMaxColor, r, g, b);
    }

    window.mixedRealityCapture.trySetWebcamIndex = function (value) {
      Module.dynCall_vi(TrySetWebcamIndex, value);
    }
  }
};
mergeInto(LibraryManager.library, LibraryMRCBridge);