namespace Gra.Observer;

public class SoundPayload : IEventPayload
{
    public int SourceX { get; }
    public int SourceY { get; }
    public int Range { get; }

    public SoundPayload(int sourceX, int sourceY, int range)
    {
        SourceX = sourceX;
        SourceY = sourceY;
        Range = range;
    }
}