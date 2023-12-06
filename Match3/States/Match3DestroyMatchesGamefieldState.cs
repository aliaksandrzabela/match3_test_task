using monogame_match3.Match3.EventsData;
using NTC.ContextStateMachine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace monogame_match3.Match3
{
    public partial class Match3GameFieldModel
    {
        protected internal class Match3DestroyMatchesGamefieldState : State<Match3GameFieldModel>
        {
            private List<(int col, int row)> destroyBySuperElements = null;

            public Match3DestroyMatchesGamefieldState(Match3GameFieldModel stateInitializer) : base(stateInitializer)
            {
            }

            public override void OnEnter()
            {
                destroyBySuperElements = new List<(int col, int row)>();
                List<(int col, int row)> createSuper = new List<(int col, int row)>();

                foreach (List<(int col, int row)> match in Initializer.foundMatches)
                {
                    foreach ((int col, int row) position in match)
                    {
                        if (Initializer.field[position.col, position.row] == (int)Match3GameElementType.Super)
                        {
                            SuperElementDestroy(position);
                        }

                        if (destroyBySuperElements.Contains(position))
                        {
                            destroyBySuperElements.Remove(position);
                        }

                        Initializer.field[position.col, position.row] = EMPTY_VALUE;
                    }

                    Initializer.OnGamefieldUpdated?.Invoke(new Match3GameFieldEvent()
                    {
                        eventType = Match3GameFieldEventType.ElementsDeleted,
                        eventData = new Match3EventDataElementsDeleted() { positions = match }
                    });

                    if (match.Count > MIN_MATCH_COUNT)
                    {
                        (int col, int row) position = match[match.Count / 2];

                        if (!createSuper.Contains(position))
                        {
                            createSuper.Add(position);
                        }
                    }
                }

                if (destroyBySuperElements.Count > 0)
                {
                    foreach ((int col, int row) position in destroyBySuperElements)
                    {
                        Initializer.field[position.col, position.row] = EMPTY_VALUE;
                    }

                    Initializer.OnGamefieldUpdated?.Invoke(new Match3GameFieldEvent()
                    {
                        eventType = Match3GameFieldEventType.ElementsDeleted,
                        eventData = new Match3EventDataElementsDeleted() { positions = destroyBySuperElements }
                    });
                }

                for (int i = 0; i < createSuper.Count; i++)
                {
                    Initializer.field[createSuper[i].col, createSuper[i].row] = (int)Match3GameElementType.Super;

                    Initializer.OnGamefieldUpdated?.Invoke(new Match3GameFieldEvent()
                    {
                        eventType = Match3GameFieldEventType.ElementCreated,
                        eventData = new Match3EventDataElementCreated() { type = Match3GameElementType.Super, position = createSuper[i] }
                    });
                }

                Initializer.foundMatches = null;
            }

            private void SuperElementDestroy((int col, int row) position)
            {
                if (!destroyBySuperElements.Contains(position))
                {
                    destroyBySuperElements.Add(position);

                    if (position.col - 1 >= 0)
                    {
                        AddSupertilePosition((position.col - 1, position.row));

                        if (position.row - 1 >= 0)
                        {
                            AddSupertilePosition((position.col - 1, position.row - 1));
                        }

                        if (position.row + 1 < Initializer.field.GetLength(1))
                        {
                            AddSupertilePosition((position.col - 1, position.row + 1));
                        }
                    }

                    if (position.col + 1 < Initializer.field.GetLength(0))
                    {
                        AddSupertilePosition((position.col + 1, position.row));

                        if (position.row - 1 >= 0)
                        {
                            AddSupertilePosition((position.col + 1, position.row - 1));
                        }

                        if (position.row + 1 < Initializer.field.GetLength(1))
                        {
                            AddSupertilePosition((position.col + 1, position.row + 1));
                        }
                    }

                    if (position.row - 1 >= 0)
                    {
                        AddSupertilePosition((position.col, position.row - 1));
                    }

                    if (position.row + 1 < Initializer.field.GetLength(1))
                    {
                        AddSupertilePosition((position.col, position.row + 1));
                    }
                }
            }

            private void AddSupertilePosition((int col, int row) position)
            {
                if (Initializer.field[position.col, position.row] != EMPTY_VALUE)
                {
                    destroyBySuperElements.Add((position.col, position.row));
                    if (Initializer.field[position.col, position.row] == (int)Match3GameElementType.Super)
                    {
                        SuperElementDestroy((position.col, position.row));
                    }
                }
            }
        }
    }

}
