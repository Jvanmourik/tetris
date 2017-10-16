using Microsoft.Xna.Framework;

public class Scene {
    public Node rootNode;
    public Color backgroundColor = Color.White;

    protected Game game;

    public Scene(Game game) {
        this.game = game;
        rootNode = new Node(game);
    }

    public void Update(GameTime gameTime) {
        foreach (Node node in rootNode.LocalChildren)
            UpdateNode(gameTime, node);
    }

    private void UpdateNode(GameTime gameTime, Node node) {
        if (node.active) {
            node.Update(gameTime);
            foreach (Node childNode in node.LocalChildren)
                UpdateNode(gameTime, childNode);
        }
    }

    public void Draw(GameTime gameTime) {
        game.GraphicsDevice.Clear(backgroundColor);
        foreach (Node node in rootNode.LocalChildren)
            DrawNode(gameTime, node);
    }

    private void DrawNode(GameTime gameTime, Node node) {
        if (node.active) {
            if (node is Graphic graphic && graphic.isVisible)
                graphic.Draw(gameTime);
            foreach (Node childNode in node.LocalChildren)
                DrawNode(gameTime, childNode);
        }
    }
}




