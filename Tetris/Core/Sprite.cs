using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Sprite : Graphic {
    public Texture2D texture;

    private String texturePath;

    public Sprite(
        Game game,
        float x = 0,
        float y = 0,
        float width = 0,
        float height = 0,
        String texturePath = null) : base(game, x, y, width, height) {
            this.texturePath = texturePath;
            Initialize();
    }

    public Sprite(
        Game game,
        Vector2 position,
        float width = 0,
        float height = 0,
        String texturePath = null) : base(game, position, width, height) {
            this.texturePath = texturePath;
            Initialize();
    }

    public Sprite(
        Game game,
        Point position,
        float width = 0,
        float height = 0,
        String texturePath = null) : base(game, position, width, height) {
        this.texturePath = texturePath;
        Initialize();
    }

    public Sprite(
        Game game,
        Vector2 position,
        Size size,
        String texturePath = null) : base(game, position, size) {
            this.texturePath = texturePath;
            Initialize();
    }

    public Sprite(
        Game game,
        Point position,
        Size size,
        String texturePath = null) : base(game, position, size) {
            this.texturePath = texturePath;
            Initialize();
    }

    protected virtual void Initialize() {
        LoadContent();
    }

    public virtual void LoadContent() {
        spriteBatch = new SpriteBatch(game.GraphicsDevice);

        try {
            if (texturePath != null)
                texture = game.Content.Load<Texture2D>(texturePath);
            else {
                texture = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                texture.SetData(new Color[] { Color.White });
            }
        } catch {
            Console.WriteLine("Couldn't load texture: " + texturePath);
            texture = new Texture2D(game.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture.SetData(new Color[] { Color.White });
        }
    }
    
    public override void Draw(GameTime gameTime) {
        spriteBatch.Begin();
        spriteBatch.Draw(texture, GetRectangle(), color * Opacity);
        spriteBatch.End();
    }
}
