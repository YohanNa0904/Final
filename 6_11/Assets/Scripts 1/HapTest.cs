using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HapTest : MonoBehaviour
{
    private InputDevice leftController;
    private InputDevice rightController;

    void Start()
    {
        // InputDevices�� ���� ��Ʈ�ѷ��� ã���ϴ�.
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

        // ������ ��Ʈ�ѷ��� A ��ư ���¸� �����ɴϴ�.
        if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out aButtonPressed) && aButtonPressed)
        {
            Debug.Log("A ��ư�� ���Ƚ��ϴ�!");

            // ������ �߻���ŵ�ϴ�.
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
        // ������Ʈ�� �ı��� �� ������ ����ϴ�.
        rightController.StopHaptics();
    }
}
