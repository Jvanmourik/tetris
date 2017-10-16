/// <summary>
/// Describes all tetromino's and their rotations
/// </summary>

using Microsoft.Xna.Framework;

public static class Tetrimino {

    public static Point[][] I = new Point[][] {
        ToPoints(-1, 0, 0, 0, 1, 0, 2, 0),
        ToPoints(0, -1, 0, 0, 0, 1, 0, 2) 
    };

    public static Point[][] J = new Point[][] {
        ToPoints(-1, 0, 0, 0, 1, 0, 1, 1),
        ToPoints(0, -1, 0, 0, -1, 1, 0, 1),
        ToPoints(-1, -1, -1, 0, 0, 0, 1, 0),
        ToPoints(0, -1, 1, -1, 0, 0, 0, 1)
    };

    public static Point[][] L = new Point[][] {
        ToPoints(-1, 0, 0, 0, 1, 0, -1, 1),
        ToPoints(-1, -1, 0, -1, 0, 0, 0, 1),
        ToPoints(1, -1, -1, 0, 0, 0, 1, 0),
        ToPoints(0, -1, 0, 0, 0, 1, 1, 1)
    };

    public static Point[][] O = new Point[][] {
        ToPoints(0, 0, 1, 0, 0, 1, 1, 1)
    };

    public static Point[][] S = new Point[][] {
        ToPoints(0, 0, 1, 0, -1, 1, 0, 1),
        ToPoints(-1, -1, -1, 0, 0, 0, 0, 1)
    };

    public static Point[][] T = new Point[][] {
        ToPoints(-1, 0, 0, 0, 1, 0, 0, 1),
        ToPoints(0, -1, -1, 0, 0, 0, 0, 1),
        ToPoints(0, -1, -1, 0, 0, 0, 1, 0),
        ToPoints(0, -1, 0, 0, 1, 0, 0, 1)
    };

    public static Point[][] Z = new Point[][] {
        ToPoints(-1, 0, 0, 0, 0, 1, 1, 1),
        ToPoints(0, -1, -1, 0, 0, 0, -1, 1)
    };

    public static Point[][][] All = new Point[][][] { I, J, L, O, S, T, Z };

    private static Point[] ToPoints(
        int x1, int y1,
        int x2, int y2,
        int x3, int y3,
        int x4, int y4) {
            return new Point[] {
                new Point(x1, y1),
                new Point(x2, y2),
                new Point(x3, y3),
                new Point(x4, y4)
            };
    }
}