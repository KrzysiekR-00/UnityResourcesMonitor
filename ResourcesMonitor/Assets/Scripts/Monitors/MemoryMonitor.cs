using System;
using System.Text;
using Unity.Profiling;
using UnityEngine.Profiling;

internal class MemoryMonitor : ResourceMonitor
{
    private readonly StringBuilder _stringBuilder = new();

    private StartedProfilerRecorder[] _startedProfilerRecorders;

    internal override string GetResourceLog()
    {
        _stringBuilder.Clear();
        _stringBuilder.Append("Memory:");
        _stringBuilder.AppendJoin(string.Empty,
            Environment.NewLine, "Total reserved: ", ToMB(Profiler.GetTotalReservedMemoryLong()), " MB");
        _stringBuilder.AppendJoin(string.Empty,
            Environment.NewLine, "Allocated: ", ToMB(Profiler.GetTotalAllocatedMemoryLong()), " MB");
        _stringBuilder.AppendJoin(string.Empty,
            Environment.NewLine, "Unused reserved: ", ToMB(Profiler.GetTotalUnusedReservedMemoryLong()), " MB");
        _stringBuilder.AppendJoin(string.Empty,
            Environment.NewLine, "Mono heap: ", ToMB(Profiler.GetMonoHeapSizeLong()), " MB");
        _stringBuilder.AppendJoin(string.Empty,
            Environment.NewLine, "Mono used: ", ToMB(Profiler.GetMonoUsedSizeLong()), " MB");
        _stringBuilder.AppendJoin(string.Empty,
            Environment.NewLine, "Temp allocators: ", ToMB(Profiler.GetTempAllocatorSize()), " MB");
        _stringBuilder.AppendJoin(string.Empty,
            Environment.NewLine, "Graphics driver: ", ToMB(Profiler.GetAllocatedMemoryForGraphicsDriver()), " MB");

        for (int i = 0; i < _startedProfilerRecorders.Length; i++)
        {
            _stringBuilder.AppendJoin(
                string.Empty,
                Environment.NewLine,
                _startedProfilerRecorders[i].StatName,
                ": ",
                ToMB(_startedProfilerRecorders[i].ProfilerRecorder.LastValue),
                " MB"
                );
        }

        return _stringBuilder.ToString();
    }

    private int ToMB(long bytes)
    {
        return (int)(bytes / 1024 / 1024);
    }

    private void OnEnable()
    {
        string[] statsNames = {
            "System Used Memory",
            "Total Used Memory",
            "Total Reserved Memory",
            "GC Used Memory",
            "GC Reserved Memory",
            "Gfx Used Memory",
            "Gfx Reserved Memory",
            "Audio Used Memory",
            "Audio Reserved Memory",
            "Video Used Memory",
            "Video Reserved Memory",
            "Profiler Used Memory",
            "Profiler Reserved Memory"
            };

        _startedProfilerRecorders = new StartedProfilerRecorder[statsNames.Length];

        for (int i = 0; i < statsNames.Length; i++)
        {
            _startedProfilerRecorders[i] = new StartedProfilerRecorder(statsNames[i], ProfilerCategory.Memory);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _startedProfilerRecorders.Length; i++)
        {
            _startedProfilerRecorders[i].ProfilerRecorder.Dispose();
        }
    }
}
