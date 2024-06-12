using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HapTest : MonoBehaviour
{
    private InputDevice leftController;
    private InputDevice rightController;

    void Start()
    {
        // InputDevices를 통해 컨트롤러를 찾습니다.
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        if (devices.Count > 0)
        {
            leftController = devices[0];
        }

        devices.Clear();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
        {
            rightController = devices[0];
        }
    }

    void Update()
    {
        bool aButtonPressed;

        // 오른손 컨트롤러의 A 버튼 상태를 가져옵니다.
        if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out aButtonPressed) && aButtonPressed)
        {
            Debug.Log("A 버튼이 눌렸습니다!");

            // 진동을 발생시킵니다.
            HapticCapabilities capabilities;
            if (rightController.TryGetHapticCapabilities(out capabilities) && capabilities.supportsImpulse)
            {
                uint channel = 0;
                float amplitude = 0.5f;
                float duration = 0.1f;
                rightController.SendHapticImpulse(channel, amplitude, duration);
            }
        }
    }

    void OnDestroy()
    {
        // 오브젝트가 파괴될 때 진동을 멈춥니다.
        rightController.StopHaptics();
    }
}
