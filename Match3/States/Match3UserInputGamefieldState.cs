using monogame_match3.Match3.EventsData;
using NTC.ContextStateMachine;
using System.Collections.Generic;

namespace monogame_match3.Match3
{
    public partial class Match3GameFieldModel
    {
        public class Match3UserInputGamefieldState : State<Match3GameFieldModel>
        {
            public Match3UserInputGamefieldState(Match3GameFieldModel stateInitializer) : base(stateInitializer)
            {
            }

            public override void OnEnter()
            {

            }

            public bool SwapElements((int col, int row) from, (int col, int row) to)
            {
                int[,] checkSwapField = (int[,])Initializer.field.Clone();
                int fromValue = checkSwapField[from.col, from.row];
                int toValue = checkSwapField[to.col, to.row];

                checkSwapField[from.col, from.row] = toValue;
                checkSwapField[to.col, to.row] = fromValue;

                bool result = FindFirstMatch(checkSwapField);

                if (result)
                {
                    Initializer.field = checkSwapField;
                    Initializer.OnGamefieldUpdated?.Invoke(new Match3GameFieldEvent
                    {
                        eventType = Match3GameFieldEventType.ElementMoved,
                        eventData = new Match3EventDataElementMoved
                        {
                            from = from,
                            to = to,
                            type = (Match3GameElementType)fromValue,
                            isSwap = true
                        }
                    });
                }

                Initializer.IsNextCheckMatches = result;
                return result;
            }

            public bool FindFirstMatch(int[,] field)
            {
                List<(int col, int row)> match = new List<(int col, int row)>();
                int elementType = EMPTY_VALUE;
                for (int x = 0; x < field.GetLength(0); x++)//vertical check
                {
                    match = new List<(int col, int row)>();
                    for (int y = 0; y < field.GetLength(1); y++)
                    {
                        if (y == 0)
                        {
                            elementType = field[x, y];
                            if (elementType != EMPTY_VALUE)
                            {
                                match.Add((x, y));
                            }
                            continue;
                        }
                        if (elementType == (int)Match3GameElementType.Super && field[x, y] != EMPTY_VALUE)
                        {
                            match.Add((x, y));
                            elementType = field[x, y];
                        }
                        else if ((field[x, y] == elementType || field[x, y] == (int)Match3GameElementType.Super) && elementType != EMPTY_VALUE)
                        {
                            match.Add((x, y));
                        }
                        else
                        {
                            if (match.Count >= MIN_MATCH_COUNT)
                            {
                                return true;
                            }
                            match = new List<(int col, int row)>();
                            elementType = field[x, y];
                            match.Add((x, y));
                        }
                    }
                    if (match.Count >= MIN_MATCH_COUNT)
                    {
                        return true;
                    }
                }

                elementType = EMPTY_VALUE;

                for (int y = 0; y < field.GetLength(1); y++)//horizontal check
                {
                    match = new List<(int col, int row)>();
                    for (int x = 0; x < field.GetLength(0); x++)
                    {
                        if (x == 0)
                        {
                            elementType = field[x, y];
                            if (elementType != EMPTY_VALUE)
                            {
                                match.Add((x, y));
                            }
                            continue;
                        }
                        if (elementType == (int)Match3GameElementType.Super && field[x, y] != EMPTY_VALUE)
                        {
                            match.Add((x, y));
                            elementType = field[x, y];
                        }
                        else if ((field[x, y] == elementType || field[x, y] == (int)Match3GameElementType.Super) && elementType != EMPTY_VALUE)
                        {
                            match.Add((x, y));
                        }
                        else
                        {
                            if (match.Count >= MIN_MATCH_COUNT)
                            {
                                return true;
                            }
                            match = new List<(int col, int row)>();
                            elementType = field[x, y];
                            match.Add((x, y));
                        }
                    }
                    if (match.Count >= MIN_MATCH_COUNT)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }

}
