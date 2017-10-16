using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Text : Graphic {
    public SpriteFont spriteFont;
    public String value;
    public String alignment;

    private String fontPath;

    public Text(
        Game game,
        String fontPath,
        String text,
        float x = 0,
        float y = 0,
        String align = "Left") : base(game, x, y) {
        this.fontPath = fontPath;
        value = text;
        alignment = align;
        Initialize();
    }

    public Text(
        Game game,
        String fontPath,
        String text,
        Vector2 position,
        String align = "Left") : base(game, position) {
        this.fontPath = fontPath;
        value = text;
        alignment = align;
        Initialize();
    }

    public Text(
        Game game,
        String fontPath,
        String text,
        Point position,
        String align = "Left") : base(game, position) {
        this.fontPath = fontPath;
        value = text;
        Initialize();
    }

    protected virtual void Initialize() {
        LoadContent();
    }

    public virtual void LoadContent() {
        spriteBatch = new SpriteBatch(game.GraphicsDevice);

        try {
            spriteFont = game.Content.Load<SpriteFont>(fontPath);
        } catch {
            Console.WriteLine("Couldn't load font: " + fontPath);
        }
    }

    public override void Draw(GameTime gameTime) {
        spriteBatch.Begin();
        float offset = (alignment == "Center")? spriteFont.MeasureString(value).X / 2 : 0;
        Vector2 position = new Vector2(this.position.X - offset, this.position.Y);
        spriteBatch.DrawString(spriteFont, value, position, color * Opacity);
        spriteBatch.End();
    }
}
