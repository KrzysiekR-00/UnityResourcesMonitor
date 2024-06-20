using Unity.Profiling;

internal readonly struct StartedProfilerRecorder
{
    internal StartedProfilerRecorder(string statName, ProfilerCategory profilerCategory)
    {
        StatName = statName;
        ProfilerRecorder = ProfilerRecorder.StartNew(profilerCategory, statName);
    }

    internal readonly string StatName { get; }
    internal readonly ProfilerRecorder ProfilerRecorder { get; }
}
