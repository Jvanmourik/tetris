/// <summary>
/// Describes the state of a single Tetromino
/// </summary>

using Microsoft.Xna.Framework;

public class TetrisState {
    public int Index;
    public int X;
    public int Y;

    public Point[][] Set => Tetrimino.All[Index];

    private int orientation;

    public TetrisState(int index, int x, int y, int orientation) {
        Index = index;
        X = x;
        Y = y;
        Orientation = orientation;
    }

    public int Orientation {
        get { return orientation; }
        set { orientation = (Set.Length + value) % Set.Length; }
    }

    public TetrisState Left() {
        return new TetrisState(Index, X - 1, Y, Orientation);
    }

    public TetrisState Right() {
        return new TetrisState(Index, X + 1, Y, Orientation);
    }

    public TetrisState Down() {
        return new TetrisState(Index, X, Y + 1, Orientation);
    }

    public TetrisState Rotate(bool cw = true) {
        return new TetrisState(Index, X, Y, Orientation + ((cw) ? 1 : -1));
    }
}