using System.Collections.Generic;

public class MapContainer
{
    private List<ChunkData> _chunks;  
    
    public void Add(ChunkData chunk)
    {
        _chunks.Add(chunk);
    }
    
    public IReadOnlyList<ChunkData> Chunks => _chunks;
}