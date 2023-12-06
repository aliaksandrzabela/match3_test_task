namespace monogame_match3.Match3.EventsData
{
    public class Match3EventDataElementMoved : Match3GamefieldEventData
    {
        public (int col, int row) from;
        public (int col, int row) to;
        public Match3GameElementType type;
        public bool isSwap = false;
    }
}
