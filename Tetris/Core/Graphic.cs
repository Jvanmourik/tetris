using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Size {
    public int Width;
    public int Height;

    public Size(int width, int height) {
        Width = width;
        Height = height;
    }
}

public class Graphic : Node {
    public SpriteBatch spriteBatch;
    public float Width;
    public float Height;
    public Color color = Color.White;
    public float Opacity = 1;
    public bool isVisible = true;

    public Graphic(
        Game game,
        float x = 0,
        float y = 0,
        float width = 0,
        float height = 0) : base(game, x, y) {
        Width = width;
        Height = height;
    }

    public Graphic(
        Game game,
        Vector2 position,
        float width = 0,
        float height = 0) : base(game, position) {
        Width = width;
        Height = height;
    }

    public Graphic(
        Game game,
        Point position,
        float width = 0,
        float height = 0) : base(game, position) {
        Width = width;
        Height = height;
    }

    public Graphic(
        Game game,
        Vector2 position,
        Size size) : base(game, position) {
        Width = size.Width;
        Height = size.Height;
    }

    public Graphic(
        Game game,
        Point position,
        Size size) : base(game, position) {
        Width = size.Width;
        Height = size.Height;
    }

    public virtual void Draw(GameTime gameTime) {

    }

    // Get bounding box
    public Rectangle GetRectangle() {
        Vector2 position = GetWorldCoordinates();
        return new Rectangle(
            (int)position.X,
            (int)position.Y,
            (int)Width,
            (int)Height);
    }
}
