/// <summary>
/// Handles the creation and removal of blocks in the well
/// </summary>

using System.Collections.Generic;
using Microsoft.Xna.Framework;

public class TetrisWell : Node {
    private Size cell;
    
    private List<Sprite> blocks = new List<Sprite>();

    public TetrisWell(Tetris game, TetrisGrid grid) : base (game) {
        cell = grid.cell;
    }

    public TetrisWell(Tetris game, int x, int y, TetrisGrid grid) : base(game, x, y) {
        cell = grid.cell;
    }

    public TetrisWell(Tetris game, Point position, TetrisGrid grid) : base(game, position) {
        cell = grid.cell;
    }

    public void AddBlock(Point cellPosition, Color color) {
        Point position = new Point(cellPosition.X * cell.Width, cellPosition.Y * cell.Height);
        Sprite block = new Sprite(game, position, cell, "BlockGradient") {
            color = color
        };
        blocks.Add(block);
        AddChild(block);
    }

    public void RemoveBlock(Point cellPosition) {
        foreach (Sprite block in blocks) {
            if (block.position == new Vector2(cellPosition.X * cell.Width, cellPosition.Y * cell.Width)) {
                blocks.Remove(block);
                RemoveChild(block);
                return;
            }
        }
    }

    public void RemoveLine(int line) {
        List<Sprite> blocks = new List<Sprite>(this.blocks);
        foreach (Sprite block in blocks)
            if (block.position.Y == line * cell.Height) {
                this.blocks.Remove(block);
                RemoveChild(block);
            }
    }

    public void RemoveAllBlocks() {
        blocks.Clear();
        LocalChildren.Clear();
    }

    public void TranslateBlock(Point cellPosition, Point translation) {
        GetBlock(cellPosition).position += new Vector2(translation.X * cell.Width, translation.Y * cell.Height);
    }

    public void TranslateRegion(int x, int y, bool[,] matrix, Point translation) {
        for (int dx = 0; dx < matrix.GetLength(0); dx++)
            for (int dy = 0; dy < matrix.GetLength(1); dy++)
                if (matrix[dx, dy])
                    GetBlock(new Point(x + dx, y + dy)).position += new Vector2(translation.X * cell.Width, translation.Y * cell.Height);
    }

    public Sprite GetBlock(Point cellPosition) {
        foreach (Sprite block in blocks)
            if (block.position == new Vector2(cellPosition.X * cell.Width, cellPosition.Y * cell.Height))
                return block;
        return null;
    }
}
