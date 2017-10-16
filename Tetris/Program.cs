﻿using System;

#if WINDOWS || LINUX
    public static class Program {
        [STAThread]
        static void Main() {
            using (var game = new Tetris())
                game.Run();
        }
    }
#endif
