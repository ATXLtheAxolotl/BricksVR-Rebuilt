using Unity.XR.Oculus;
using UnityEngine;

public class UpdateFixedTimestep : MonoBehaviour
{
    private void Start()
    {
        float frameRate = 90f;

        #if !OCULUSPLUGIN_UNSUPPORTED_PLATFORM
            SystemHeadset headsetType = Utils.GetSystemHeadsetType();

            switch (headsetType)
            {
                case SystemHeadset.Oculus_Quest:
                    frameRate = 72f;
                    break;

                case SystemHeadset.Rift_S:
                    frameRate = 80f;
                    break;

                case SystemHeadset.Oculus_Link_Quest:
                    frameRate = 90f;
                    break;

                case SystemHeadset.Rift_CV1:
                    frameRate = 90f;
                    break;

                case SystemHeadset.Rift_DK1:
                    frameRate = 60f;
                    break;

                case SystemHeadset.Rift_DK2:
                    frameRate = 75f;
                    break;
            }
        #endif

        Time.fixedDeltaTime = 1 / frameRate;

        Debug.Log($"Device is {headsetType}, framerate is {frameRate}");
    }
}
