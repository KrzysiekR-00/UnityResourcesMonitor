using System;
using UnityEngine;

internal class FpsMonitor : ResourceMonitor
{
    [SerializeField]
    private int _framesInHistory = 100;

    private float[] _frameTimeHistory;

    private float _currentFPS;
    private float _averageFPS;
    private float _minFrameTime;
    private float _maxFrameTime;
    private float _averageFrameTime;

    internal override string GetResourceLog()
    {
        CalculateStats();

        return $@"Fps: {Math.Round(_currentFPS, 1)}
Last {_frameTimeHistory.Length} frames:
Avg fps: {Math.Round(_averageFPS, 1)}
Min frame time: {Math.Round(_minFrameTime * 1000, 1)} ms
Max frame time: {Math.Round(_maxFrameTime * 1000, 1)} ms
Avg frame time: {Math.Round(_averageFrameTime * 1000, 1)} ms";
    }

    private void CalculateStats()
    {
        _currentFPS = 1.0f / Time.unscaledDeltaTime;

        float sum = 0;
        _minFrameTime = float.MaxValue;
        _maxFrameTime = float.MinValue;

        for (int i = 0; i < _frameTimeHistory.Length; i++)
        {
            if (_frameTimeHistory[i] == float.NaN) continue;

            sum += _frameTimeHistory[i];

            if (_frameTimeHistory[i] < _minFrameTime) _minFrameTime = _frameTimeHistory[i];
            if (_frameTimeHistory[i] > _maxFrameTime) _maxFrameTime = _frameTimeHistory[i];
        }

        float average = sum / _frameTimeHistory.Length;

        _averageFPS = 1 / average;
        _averageFrameTime = average;
    }

    private void Awake()
    {
        _frameTimeHistory = new float[_framesInHistory];

        for (int i = 0; i < _frameTimeHistory.Length; i++)
        {
            _frameTimeHistory[i] = float.NaN;
        }
    }

    private void Update()
    {
        UpdateFrameTimeHistory();
    }

    private void UpdateFrameTimeHistory()
    {
        _frameTimeHistory[Time.frameCount % _frameTimeHistory.Length] = Time.unscaledDeltaTime;
    }
}
