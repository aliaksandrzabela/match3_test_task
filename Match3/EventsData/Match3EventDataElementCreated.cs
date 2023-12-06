namespace monogame_match3.Match3
{
    public class Match3EventDataElementCreated : Match3GamefieldEventData
    {
        public (int x, int y) position;
        public Match3GameElementType type;
    }
}
