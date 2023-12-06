using NTC.ContextStateMachine;
using System.Collections.Generic;

namespace monogame_match3.Match3
{
    public partial class Match3GameFieldModel
    {
        public const int MIN_MATCH_COUNT = 3;
        private List<List<(int col, int row)>> foundMatches;
        protected internal class Match3CheckMatchesGamefieldState : State<Match3GameFieldModel>
        {

            public Match3CheckMatchesGamefieldState(Match3GameFieldModel stateInitializer) : base(stateInitializer)
            {
            }
            public override void OnEnter()
            {
                Initializer.IsNextCheckMatches = false;

                List<(int col, int row)> match = new List<(int col, int row)>();
                Initializer.foundMatches = new List<List<(int col, int row)>>();

                int elementType = EMPTY_VALUE;
                for (int x = 0; x < Initializer.field.GetLength(0); x++)//vertical check
                {
                    match = new List<(int col, int row)>();
                    for (int y = 0; y < Initializer.field.GetLength(1); y++)
                    {
                        if (y == 0)
                        {
                            elementType = Initializer.field[x, y];
                            if (elementType != EMPTY_VALUE)
                            {
                                match.Add((x, y));
                            }
                            continue;
                        }
                        if (elementType == (int)Match3GameElementType.Super && Initializer.field[x, y] != EMPTY_VALUE)
                        {
                            match.Add((x, y));
                            elementType = Initializer.field[x, y];
                        }
                        else if ((Initializer.field[x, y] == elementType || Initializer.field[x, y] == (int)Match3GameElementType.Super) && elementType != EMPTY_VALUE)
                        {
                            match.Add((x, y));
                        }
                        else
                        {
                            if (match.Count >= MIN_MATCH_COUNT)
                            {
                                Initializer.foundMatches.Add(match);
                            }
                            match = new List<(int col, int row)>();
                            elementType = Initializer.field[x, y];
                            match.Add((x, y));
                        }
                    }
                    if (match.Count >= MIN_MATCH_COUNT)
                    {
                        Initializer.foundMatches.Add(match);
                    }
                }

                elementType = EMPTY_VALUE;

                for (int y = 0; y < Initializer.field.GetLength(1); y++)//horizontal check
                {
                    match = new List<(int col, int row)>();
                    for (int x = 0; x < Initializer.field.GetLength(0); x++)
                    {
                        if (x == 0)
                        {
                            elementType = Initializer.field[x, y];
                            if (elementType != EMPTY_VALUE)
                            {
                                match.Add((x, y));
                            }
                            continue;
                        }
                        if (elementType == (int)Match3GameElementType.Super && Initializer.field[x, y] != EMPTY_VALUE)
                        {
                            match.Add((x, y));
                            elementType = Initializer.field[x, y];
                        }
                        else if ((Initializer.field[x, y] == elementType || Initializer.field[x, y] == (int)Match3GameElementType.Super) && elementType != EMPTY_VALUE)
                        {
                            match.Add((x, y));
                        }
                        else
                        {
                            if (match.Count >= MIN_MATCH_COUNT)
                            {
                                Initializer.foundMatches.Add(match);
                            }
                            match = new List<(int col, int row)>();
                            elementType = Initializer.field[x, y];
                            match.Add((x, y));
                        }
                    }
                    if (match.Count >= MIN_MATCH_COUNT)
                    {
                        Initializer.foundMatches.Add(match);
                    }
                }
            }
        }

    }
}
