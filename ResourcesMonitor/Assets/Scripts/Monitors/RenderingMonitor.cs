using System.Text;
using Unity.Profiling;

internal class RenderingMonitor : ResourceMonitor
{
    private readonly StringBuilder _stringBuilder = new();

    private StartedProfilerRecorder[] _startedProfilerRecorders;

    internal override string GetResourceLog()
    {
        _stringBuilder.Clear();
        _stringBuilder.Append("Rendering:");

        for (int i = 0; i < _startedProfilerRecorders.Length; i++)
        {
            _stringBuilder.AppendJoin(string.Empty, "\r\n", _startedProfilerRecorders[i].StatName, ": ", _startedProfilerRecorders[i].ProfilerRecorder.LastValue);
        }

        return _stringBuilder.ToString();
    }

    private void OnEnable()
    {
        string[] statsNames = {
            "SetPass Calls Count",
            "Draw Calls Count",
            "Total Batches Count",
            "Triangles Count",
            "Vertices Count",
            "Render Textures Count",
            "Render Textures Bytes",
            "Render Textures Changes Count",
            "Used Buffers Count",
            "Used Buffers Bytes",
            "Vertex Buffer Upload In Frame Count",
            "Vertex Buffer Upload In Frame Bytes",
            "Index Buffer Upload In Frame Count",
            "Index Buffer Upload In Frame Bytes",
            "Shadow Casters Count"
        };

        _startedProfilerRecorders = new StartedProfilerRecorder[statsNames.Length];

        for (int i = 0; i < statsNames.Length; i++)
        {
            _startedProfilerRecorders[i] = new StartedProfilerRecorder(statsNames[i], ProfilerCategory.Render);
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
