using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullScreen : MonoBehaviour
{
    [SerializeField] private Material _fullScreen;

    private void Update()
    {
        if (DetectionManager.Instance != null)
        {
            float remappedActivate = DetectionManager.Instance.timer / DetectionManager.Instance.timerDeath;
            remappedActivate = Mathf.Clamp01(remappedActivate);
            _fullScreen.SetFloat("_Activate", remappedActivate);

            float remappedMask = 1 - remappedActivate;
            _fullScreen.SetFloat("_Mask", remappedMask);

            if (DetectionManager.Instance.timer > 0)
            {
                GamepadRumble.Instance.StartRumble(0.05f);
            }
            else
            {
                GamepadRumble.Instance.StopRumble();
            }

            if (DetectionManager.Instance.timer > 0)
            {
                GamepadRumble.Instance.UpdateRumble(0.05f);
            }
        }
    }
}
