/// <summary>
/// Grid model to describe filled cells
/// </summary>

using Microsoft.Xna.Framework;

public class TetrisGrid {
    public int Columns;
    public int Rows;
    public Size cell;

    private bool[,] matrix;

    public TetrisGrid(
        int columns,
        int rows,
        int width,
        int height) {
            Columns = columns;
            Rows = rows;
            cell = new Size(width, height);
            matrix = new bool[columns, rows];
    }
    
    public void FillCell(int x, int y) {
        matrix[x, y] = true;
    }

    public void FillCell(Point position) {
        matrix[position.X, position.Y] = true;
    }

    public void FillCells(TetrisState state) {
        foreach (Point segment in state.Set[state.Orientation])
            FillCell(new Point(state.X, state.Y) + segment);
    }

    public void FillRegion(int x, int y, bool[,] matrix) {
        for (int dx = 0; dx < matrix.GetLength(0); dx++)
            for (int dy = 0; dy < matrix.GetLength(1); dy++)
                this.matrix[x + dx, y + dy] = matrix[dx, dy];
    }

    public void ClearCell(int x, int y) {
        matrix[x, y] = false;
    }

    public void ClearCell(Point position) {
        matrix[position.X, position.Y] = false;
    }

    public void ClearRow(int y) {
        for (int x = 0; x < matrix.GetLength(0); x++) {
            ClearCell(x, y);
        }
    }

    public void ClearRegion(int x, int y, bool[,] matrix) {
        for (int dx = 0; dx < matrix.GetLength(0); dx++)
            for (int dy = 0; dy < matrix.GetLength(1); dy++)
                ClearCell(x + dx, y + dy);
    }

    public void Clear() {
        matrix = new bool[Columns, Rows];
    }

    public bool Contains(Point position) {
        return (position.X >= 0 && position.X < matrix.GetLength(0) && position.Y >= 0 && position.Y < matrix.GetLength(1));
    }

    public bool IsEmpty(Point position) {
        if (Contains(position))
            return !matrix[position.X, position.Y];
        else
            return false;
    }

    public bool IsEmpty(Point[] positions) {
        foreach (Point position in positions)
            if (!IsEmpty(position))
                return false;
        return true;
    }

    public bool IsEmpty(TetrisState state) {
        foreach (Point segment in state.Set[state.Orientation])
            if (!IsEmpty(new Point(state.X, state.Y) + segment))
                return false;
        return true;
    }

    public bool IsRowEmpty(int y) {
        for (int x = 0; x < matrix.GetLength(0); x++)
            if (matrix[x, y])
                return false;
        return true;

    }

    public bool IsRowFilled(int y) {
        for (int x = 0; x < matrix.GetLength(0); x++)
            if (!matrix[x, y])
                return false;
        return true;

    }

    public bool[,] GetMatrix(int x, int y, int width, int height) {
        bool[,] matrix = new bool[width, height];
        for (int dx = 0; dx < width; dx++)
            for (int dy = 0; dy < height; dy++)
                matrix[dx, dy] = this.matrix[x + dx, y + dy];
        return matrix;
    }
}