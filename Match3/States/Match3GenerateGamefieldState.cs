using NTC.ContextStateMachine;

namespace monogame_match3.Match3
{
    public partial class Match3GameFieldModel
    {
        protected internal class Match3GenerateGamefieldState : State<Match3GameFieldModel>
        {
            public bool IsFinish { get; private set; }
            public Match3GenerateGamefieldState(Match3GameFieldModel stateInitializer) : base(stateInitializer)
            {
                IsFinish = false;
            }

            public override void OnEnter()
            {
                Initializer.IsNextGenerate = false;
                /*for (int y = 0; y < Initializer.field.GetLength(1); y++)
                {
                    for (int x = 0; x < Initializer.field.GetLength(0); x++)
                    {
                        if (y == 0 && x > 1 && x < 4)
                        {
                            Initializer.field[x, y] = 1;
                            Initializer.OnGamefieldUpdated?.Invoke(new Match3GameFieldEvent
                            {
                                eventType = Match3GameFieldEventType.ElementCreated,
                                eventData = new Match3EventDataElementCreated() { position = (x, y), type = (Match3GameElementType)1 }
                            });
                        }
                        else
                        {
                            Initializer.field[x, y] = ((x + y) % 5);
                            Initializer.OnGamefieldUpdated?.Invoke(new Match3GameFieldEvent
                            {
                                eventType = Match3GameFieldEventType.ElementCreated,
                                eventData = new Match3EventDataElementCreated() { position = (x, y), type = (Match3GameElementType)((x + y) % 5) }
                            });
                        }
                    }
                }

                IsFinish = true;
                return;*/

                int cols = Initializer.Cols;
                for (int col = 0; col < cols; col++)
                {
                    if (Initializer.field[col, 0] >= 0)
                    {
                        continue;
                    }
                    int newGameElement = Initializer.GetRandomElement();
                    Initializer.field[col, 0] = newGameElement;

                    Initializer.OnGamefieldUpdated?.Invoke(new Match3GameFieldEvent
                    {
                        eventType = Match3GameFieldEventType.ElementCreated,
                        eventData = new Match3EventDataElementCreated() { position = (col, 0), type = (Match3GameElementType)newGameElement }
                    });
                }

                IsFinish = true;
            }
        }
    }

}
