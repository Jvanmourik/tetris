/// <summary>
/// Handles the game logic and loop
/// </summary>

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

class TetrisEngine : Node {
    private InputHelper input;
    private TetrisWell preview;
    private TetrisWell well;
    private TetrisGrid grid;
    private TetrisState state;
    private Random random;
    private Node gameOverContainer;

    private SoundEffectInstance backgroundMusicInstance;
    private SoundEffect placeSoundFX;
    private SoundEffect clearLineSoundFX;

    public State currentState = State.Playing;
    public enum State { Playing, GameOver }

    private double tickDuration = 500.0;
    private double _tickDuration = 500.0;
    private double tickElapsed = 0.0;

    private int previewIndex;

    private Text scoreText;
    private int score = 0;
    private int linesRemoved = 0;
    private int level = 0;

    private static Color[] colors = {
        new Color(102, 217, 239),
        new Color(1, 99, 198),
        new Color(246, 143, 55),
        new Color(255, 217, 63),
        new Color(99, 199, 77),
        new Color(193, 110, 241),
        new Color(247, 69, 48)
    };

    public TetrisEngine(
        Game game,
        InputHelper input,
        TetrisWell preview,
        TetrisWell well,
        TetrisGrid grid,
        Text scoreText) : base (game) {
            this.input = input;
            this.preview = preview;
            this.well = well;
            this.grid = grid;
            this.scoreText = scoreText;
            Initialize();
    }

    private void Initialize() {
        LoadContent();

        Viewport viewport = game.GraphicsDevice.Viewport;
        random = new Random();

        // MARK: GameOver overlay

        // GameOver container
        gameOverContainer = new Node(game) { active = false };
        AddChild(gameOverContainer);

        // Create background
        Sprite gameOverBackground = new Sprite(game, 0, 0, viewport.Width, viewport.Height) {
            color = Color.Black,
            Opacity = 0.5f
        };
        gameOverContainer.AddChild(gameOverBackground);

        // GameOver text
        Text gameOver = new Text(game, "monospace", "GAMEOVER", viewport.Width / 2, 8 * grid.cell.Height, "Center");
        gameOverContainer.AddChild(gameOver);
    }

    public void Run() {
        previewIndex = random.Next(0, Tetrimino.All.Length);
        NextTetrimino();
        backgroundMusicInstance.Play();
    }

    public void Stop() {
        grid.Clear();
        well.RemoveAllBlocks();
        preview.RemoveAllBlocks();

        tickDuration = _tickDuration;
        tickElapsed = score = linesRemoved = level = 0;
        scoreText.value = score.ToString().PadLeft(7, '0');

        backgroundMusicInstance.Stop();
    }

    public void Reset() {
        Stop();
        Run();
    }

    protected virtual void LoadContent() {
        placeSoundFX = game.Content.Load<SoundEffect>("Bum");
        clearLineSoundFX = game.Content.Load<SoundEffect>("ClearLine");

        SoundEffect backgroundMusic = game.Content.Load<SoundEffect>("Super Secret Tune");
        backgroundMusicInstance = backgroundMusic.CreateInstance();
        backgroundMusicInstance.IsLooped = true;
        backgroundMusicInstance.Volume = 0.5f;
    }

    public override void Update(GameTime gameTime) {
        if (currentState == State.Playing) {
            // Handle input
            if (input.IsKeyDown(Keys.Left, true) && grid.IsEmpty(state.Left())) {
                TranslateTetrimino(state, new Point(-1, 0));
            }
            if (input.IsKeyDown(Keys.Right, true) && grid.IsEmpty(state.Right())) {
                TranslateTetrimino(state, new Point(1, 0));
            }
            if (input.IsKeyDown(Keys.Down, true) && (tickElapsed >= 100.0)) {
                tickElapsed = 0.0;
                Tick(gameTime);
            }
            if (input.IsKeyPressed(Keys.A) && grid.IsEmpty(state.Rotate(false))) {
                RotateTetrimino(state, false);
            } else if (input.IsKeyPressed(Keys.D) && grid.IsEmpty(state.Rotate(true))) {
                RotateTetrimino(state, true);
            }
            if (input.IsKeyPressed(Keys.Up)) {
                DropTetrimino(state);
            }

            // Tick timer
            tickElapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (tickElapsed >= tickDuration) {
                tickElapsed -= tickDuration;
                Tick(gameTime);
            }
        }
        else if (currentState == State.GameOver) {
            if (input.IsKeyPressed(Keys.Enter)) {
                currentState = State.Playing;
                gameOverContainer.active = false;
                Reset();
            }
        }
    }

    private void Tick(GameTime gameTime) {
        if (grid.IsEmpty(state.Down())) {
            TranslateTetrimino(state, new Point(0, 1));
        } else {
            AddTetriminoToGrid(state);
        }
    }

    private void NextTetrimino() {
        state = new TetrisState(previewIndex, grid.Columns / 2 - 1, 1, 0);
        AddTetrimino(state, well);
        previewIndex = random.Next(0, Tetrimino.All.Length);
        RemoveTetrimino(new TetrisState(state.Index, 2, 1, 0), preview);
        AddTetrimino(new TetrisState(previewIndex, 2, 1, 0), preview);
        if (!grid.IsEmpty(state)) {
            currentState = State.GameOver;
            gameOverContainer.active = true;
            backgroundMusicInstance.Stop();
        }
    }

    private void AddTetrimino(TetrisState state, TetrisWell well) {
        foreach (Point segment in state.Set[state.Orientation])
            well.AddBlock(new Point(state.X + segment.X, state.Y + segment.Y), colors[state.Index]);
    }

    private void AddTetriminoToGrid(TetrisState state) {
        // Place the tetrimino on the grid
        grid.FillCells(state);
        
        // Remove full Lines
        List<int> lines = GetAllFilledLines();
        RemoveLines(lines);

        // Update score based on number of lines removed
        score += (int)(Math.Pow(lines.Count, 2) * 100 + 10);
        scoreText.value = score.ToString().PadLeft(7, '0');

        // Update current level based on number of lines removed
        level = (linesRemoved - (linesRemoved % 10)) / 10;
        tickDuration = 100 + 400 / (1 + level * 0.5);

        // Play sound effect
        placeSoundFX.Play();

        // Prepare for next tetrimino
        NextTetrimino();
    }
    
    private void TranslateTetrimino(TetrisState state, Point translation) {
        foreach (Point segment in state.Set[state.Orientation])
            well.TranslateBlock(new Point(state.X + segment.X, state.Y + segment.Y), translation);
        state.X += translation.X;
        state.Y += translation.Y;
    }

    private void RotateTetrimino(TetrisState state, bool cw = true) {
        RemoveTetrimino(state, well);
        state.Orientation += ((cw) ? 1 : -1);
        AddTetrimino(state, well);
    }

    private void DropTetrimino(TetrisState state) {
        RemoveTetrimino(state, well);
        while(grid.IsEmpty(state.Down()))
            state.Y++;
        AddTetrimino(state, well);
        AddTetriminoToGrid(state);
    }

    private void RemoveTetrimino(TetrisState state, TetrisWell well) {
        foreach (Point segment in state.Set[state.Orientation])
            well.RemoveBlock(new Point(state.X + segment.X, state.Y + segment.Y));
    }

    private void RemoveLines(List<int> lines) {
        foreach (int line in lines) {
            bool[,] matrix = grid.GetMatrix(0, 0, grid.Columns, line);

            well.RemoveLine(line);
            well.TranslateRegion(0, 0, matrix, new Point(0, 1));
            grid.ClearRegion(0, 0, matrix);
            grid.FillRegion(0, 1, matrix);

            linesRemoved++;

            // Play sound effect
            clearLineSoundFX.Play();
        }
    }

    private List<int> GetAllFilledLines() {
        List<int> lines = new List<int>();
        for (int y = 0; y < grid.Rows; y++)
            if (grid.IsRowFilled(y)) {
                lines.Add(y);
            }
        return lines;
    }
}

