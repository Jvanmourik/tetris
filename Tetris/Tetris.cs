using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

public class Tetris : Game {
    GraphicsDeviceManager graphics;
    InputHelper input;
    TetrisGrid grid;
    TetrisEngine tetrisEngine;

    private SoundEffect selectSoundFX;

    public Scene scene;
    public Screen currentScreen = Screen.Menu;
    public enum Screen { Menu, Tetris }
    
    private Node menuContainer;
    private Node tetrisContainer;
    private Sprite menuHighlight;

    private int menuPointer = 0;

    public Tetris() {
        grid = new TetrisGrid(12, 21, 32, 32);
        graphics = new GraphicsDeviceManager(this) {
            PreferredBackBufferWidth = 12 * grid.cell.Width,
            PreferredBackBufferHeight = 25 * grid.cell.Height
        };
        Content.RootDirectory = "Content";
    }

    protected override void Initialize() {
        Viewport viewport = GraphicsDevice.Viewport;

        // Create input helper
        input = new InputHelper(100);

        // Create scene
        scene = new Scene(this);


        // MARK: Menu elements

        // Create container for the menu elements
        menuContainer = new Node(this);
        scene.rootNode.AddChild(menuContainer);

        // Create background
        Sprite menuBackground = new Sprite(this, 0, 0, viewport.Width, viewport.Height) {
            texture = CreateLinearGradient(
                new Color(6, 36, 55),
                new Color(20, 12, 51),
                new Color(4, 2, 22),
                new Color(14, 2, 22))
        };
        menuContainer.AddChild(menuBackground);

        // Create menu pointer graphic
        menuHighlight = new Sprite(this, 3 * grid.cell.Width, 7 * grid.cell.Height, 6 * grid.cell.Width, 3 * grid.cell.Height) {
                Opacity = 0.1f
        };
        menuContainer.AddChild(menuHighlight);

        // Create menu items
        Text start = new Text(this, "monospace", "START", viewport.Width / 2, 8 * grid.cell.Height, "Center");
        menuContainer.AddChild(start);
        
        Text mode = new Text(this, "monospace", "MODE", viewport.Width / 2, 11 * grid.cell.Height, "Center");
        menuContainer.AddChild(mode);

        Text quit = new Text(this, "monospace", "QUIT", viewport.Width / 2, 14 * grid.cell.Height, "Center");
        menuContainer.AddChild(quit);


        // MARK: Tetris elements

        // Create container for the game elements
        tetrisContainer = new Node(this);
        tetrisContainer.active = false;
        scene.rootNode.AddChild(tetrisContainer);

        // Create backgrounds
        Sprite tetrisBackground = new Sprite(this, 0, 0, viewport.Width, viewport.Height) {
            texture = CreateLinearGradient(
                new Color(6, 36, 55),
                new Color(20, 12, 51),
                new Color(4, 2, 22),
                new Color(14, 2, 22))
        };
        tetrisContainer.AddChild(tetrisBackground);

        Sprite tetrisHeaderBackground = new Sprite(this, 0, 0, viewport.Width, 4 * grid.cell.Height) {
            Opacity = 0.1f
        };
        tetrisContainer.AddChild(tetrisHeaderBackground);

        // Create score section
        Text scoreLabel = new Text(this, "regular", "Score", 6 * grid.cell.Width, grid.cell.Height);
        tetrisContainer.AddChild(scoreLabel);

        Text score = new Text(this, "monospace", "0000000", 6 * grid.cell.Width, 2 * grid.cell.Height);
        tetrisContainer.AddChild(score);

        // Create preview well
        TetrisWell preview = new TetrisWell(this, new TetrisGrid(4, 3, grid.cell.Width, grid.cell.Height));
        tetrisContainer.AddChild(preview);

        // Create playing field well
        TetrisWell playfield = new TetrisWell(this, 0, 4 * grid.cell.Width, grid);
        tetrisContainer.AddChild(playfield);

        // Create tetris game engine
        tetrisEngine = new TetrisEngine(this, input, preview, playfield, grid, score);
        tetrisContainer.AddChild(tetrisEngine);


        base.Initialize();
    }


    protected override void LoadContent() {
        selectSoundFX = Content.Load<SoundEffect>("Select");
    }

    protected override void Update(GameTime gameTime) {
        input.Update(gameTime);

        if (currentScreen == Screen.Menu) {
            if (input.IsKeyPressed(Keys.Escape))
                Exit();
            else if (input.IsKeyPressed(Keys.Enter)) {
                if (MenuPointer == 0) {
                    currentScreen = Screen.Tetris;
                    menuContainer.active = false;
                    tetrisContainer.active = true;
                    tetrisEngine.Run();
                }
                else if (MenuPointer == 1) {

                }
                else {
                    Exit();
                }
            }
            else if (input.IsKeyPressed(Keys.Up)) {
                MenuPointer--;
                menuHighlight.position.Y = (7 + 3 * MenuPointer) * grid.cell.Height;
                selectSoundFX.Play();
            } 
            else if (input.IsKeyPressed(Keys.Down)) {
                MenuPointer++;
                menuHighlight.position.Y = (7 + 3 * MenuPointer) * grid.cell.Height;
                selectSoundFX.Play();
            }
        } 
        else if (currentScreen == Screen.Tetris) {
            if (input.IsKeyPressed(Keys.Escape)) {
                currentScreen = Screen.Menu;
                tetrisEngine.Stop();
                tetrisContainer.active = false;
                menuContainer.active = true;
            }   
        }

        scene.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(
        GameTime gameTime) {
        scene.Draw(gameTime);
        base.Draw(gameTime);
    }

    private Texture2D CreateLinearGradient(Color c1, Color c2, Color c3, Color c4) {
        Texture2D texture = new Texture2D(graphics.GraphicsDevice, 2, 2);
        texture.SetData(new Color[] { c1, c2, c3, c4 });
        return texture;
    }

    private int MenuPointer {
        get { return menuPointer; }
        set { menuPointer = (3 + value) % 3; }
    }
}