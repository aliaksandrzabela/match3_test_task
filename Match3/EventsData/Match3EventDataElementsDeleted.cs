using System.Collections.Generic;

namespace monogame_match3.Match3.EventsData
{
    public class Match3EventDataElementsDeleted : Match3GamefieldEventData
    {
        public List<(int col, int row)> positions;
    }
}
