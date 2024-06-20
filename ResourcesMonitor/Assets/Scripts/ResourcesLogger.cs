using System;
using UnityEngine;
using UnityEngine.Events;

public class ResourcesLogger : MonoBehaviour
{
    [field: SerializeField]
    public UnityEvent<string> LogUpdated { get; set; }

    [field: SerializeField]
    internal ResourceMonitor[] ResourceMonitors { get; set; } = Array.Empty<ResourceMonitor>();

    [SerializeField]
    private float _updateIntervalInSeconds = 1f;

    private float _lastUpdate = 0;

    private void Update()
    {
        if (Time.realtimeSinceStartup >= _lastUpdate + _updateIntervalInSeconds)
        {
            var logs = new string[ResourceMonitors.Length];
            for (int i = 0; i < ResourceMonitors.Length; i++)
            {
                logs[i] = ResourceMonitors[i].GetResourceLog();
            }

            LogUpdated.Invoke(string.Join("\r\n\r\n", logs));

            _lastUpdate = Time.realtimeSinceStartup;
        }
    }
}
