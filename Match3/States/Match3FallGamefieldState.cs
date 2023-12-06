using monogame_match3.Match3.EventsData;
using NTC.ContextStateMachine;

namespace monogame_match3.Match3
{
    public partial class Match3GameFieldModel
    {
        private bool IsNextGenerate;
        private bool IsNextCheckMatches;

        protected internal class Match3FallGamefieldState : State<Match3GameFieldModel>
        {
            public Match3FallGamefieldState(Match3GameFieldModel stateInitializer) : base(stateInitializer)
            {
            }

            public override void OnEnter()
            {
                Initializer.IsNextGenerate = false;
                Initializer.IsNextCheckMatches = false;

                if (ProcessFall())
                {
                    Initializer.IsNextGenerate = true;
                    return;
                }
                Initializer.IsNextCheckMatches = true;
            }

            private bool ProcessFall()
            {
                int cols = Initializer.Cols;
                int rows = Initializer.Rows;
                bool gamefieldUpdated = false;
                for (int row = rows - 2; row >= 0; row--) // Exclude the lowest row, because there is no place to fall.
                {
                    for (int col = cols - 1; col >= 0; col--)
                    {
                        int startRow = row;
                        if (Initializer.field[col, row] >= 0)
                        {
                            int endRow = row;
                            while (EmptyCellUnderElement(col, endRow))
                            {
                                endRow++;
                            }
                            if (endRow != startRow)
                            {
                                Initializer.field[col, endRow] = Initializer.field[col, startRow];
                                Initializer.field[col, startRow] = EMPTY_VALUE;
                                Initializer.OnGamefieldUpdated?.Invoke(new Match3GameFieldEvent
                                {
                                    eventType = Match3GameFieldEventType.ElementMoved,
                                    eventData = new Match3EventDataElementMoved
                                    {
                                        from = (col, startRow),
                                        to = (col, endRow),
                                        type = (Match3GameElementType)Initializer.field[col, endRow],
                                        isSwap = false
                                    }
                                });
                                gamefieldUpdated = true;
                            }
                        }
                        else
                        {
                            gamefieldUpdated = true;
                        }
                    }
                }
                return gamefieldUpdated;
            }

            private bool EmptyCellUnderElement(int x, int y)
            {
                if (y + 1 < Initializer.Rows)
                {
                    return Initializer.field[x, y + 1] == EMPTY_VALUE;
                }
                return false;
            }
        }
    }
}
