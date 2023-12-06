using NTC.ContextStateMachine;
using System;

namespace monogame_match3.Match3
{
    public partial class Match3GameFieldModel
    {
        public const int EMPTY_VALUE = -1;
        public int Cols { get; private set; }
        public int Rows { get; private set; }

        public event Action<Match3GameFieldEvent> OnGamefieldUpdated;

        private int[,] field;
        private Random random = new Random();

        private StateMachine<Match3GameFieldModel> stateMachine = new StateMachine<Match3GameFieldModel>();
        private Match3GenerateGamefieldState generateGamefieldState;
        private Match3FallGamefieldState fallGamefieldState;
        private Match3CheckMatchesGamefieldState checkMatchesGamefieldState;
        private Match3DestroyMatchesGamefieldState match3DestroyMatchesGamefieldState;
        private Match3UserInputGamefieldState userInputGameFieldState;
        public Match3GameFieldModel(int width, int height)
        {
            field = new int[width, height];
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[i, j] = EMPTY_VALUE;
                }
            }

            Cols = width;
            Rows = height;

            InitStates();
        }

        private void InitStates()
        {
            generateGamefieldState = new Match3GenerateGamefieldState(this);
            fallGamefieldState = new Match3FallGamefieldState(this);
            checkMatchesGamefieldState = new Match3CheckMatchesGamefieldState(this);
            match3DestroyMatchesGamefieldState = new Match3DestroyMatchesGamefieldState(this);
            userInputGameFieldState = new Match3UserInputGamefieldState(this);

            stateMachine.AddStates(generateGamefieldState, fallGamefieldState, checkMatchesGamefieldState, match3DestroyMatchesGamefieldState, userInputGameFieldState);

            stateMachine.AddTransition<Match3GenerateGamefieldState, Match3FallGamefieldState>(() => generateGamefieldState.IsFinish);
            stateMachine.AddTransition<Match3FallGamefieldState, Match3GenerateGamefieldState>(() => IsNextGenerate);
            stateMachine.AddTransition<Match3FallGamefieldState, Match3CheckMatchesGamefieldState>(() => IsNextCheckMatches);
            stateMachine.AddTransition<Match3CheckMatchesGamefieldState, Match3DestroyMatchesGamefieldState>(() => foundMatches?.Count > 0);
            stateMachine.AddTransition<Match3CheckMatchesGamefieldState, Match3UserInputGamefieldState>(() => foundMatches == null || foundMatches.Count == 0);
            stateMachine.AddTransition<Match3UserInputGamefieldState, Match3CheckMatchesGamefieldState>(() => IsNextCheckMatches);
            stateMachine.AddTransition<Match3DestroyMatchesGamefieldState, Match3FallGamefieldState>(() => true);
        }

        public void Start()
        {
            stateMachine.SetState<Match3FallGamefieldState>();
        }

        public void Run()
        {
            stateMachine.Run();
        }

        public int GetFieldValue(int x, int y)
        {
            if (x >= field.GetLength(0) || x < 0 || y >= field.GetLength(1) || y < 0)
            {
                throw new ArgumentOutOfRangeException($"x:{x}, y:{y} out of bounds field");
            }
            return field[x, y];
        }

        public bool SwapElements((int col, int row) from, (int col, int row) to)
        {
            if (!userInputGameFieldState.IsActive)
            {
                return false;
            }
            return userInputGameFieldState.SwapElements(from, to);
        }

        private int GetRandomElement()
        {
            return random.Next(0, Enum.GetNames(typeof(Match3GameElementType)).Length - 1);
        }
    }
}
