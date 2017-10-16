using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

class InputHelper {
    MouseState currentMouseState, previousMouseState;
    KeyboardState currentKeyboardState, previousKeyboardState;


    double timeSinceLastKeyPress = 0;
    double keyRepeatDelay;

    public InputHelper(double keyRepeatDelay = 100) {
        this.keyRepeatDelay = keyRepeatDelay;
    }

    public void Update(GameTime gameTime) {
        // check if keys are pressed and update the timeSinceLastKeyPress variable
        Keys[] prevKeysDown = previousKeyboardState.GetPressedKeys();
        Keys[] currKeysDown = currentKeyboardState.GetPressedKeys();
        if (currKeysDown.Length != 0 && (prevKeysDown.Length == 0 || timeSinceLastKeyPress > keyRepeatDelay))
            timeSinceLastKeyPress = 0;
        else
            timeSinceLastKeyPress += gameTime.ElapsedGameTime.TotalMilliseconds;

        // update the mouse and keyboard states
        previousMouseState = currentMouseState;
        previousKeyboardState = currentKeyboardState;
        currentMouseState = Mouse.GetState();
        currentKeyboardState = Keyboard.GetState();
    }

    // returns the current mouse position
    public Vector2 MousePosition {
        get { return new Vector2(currentMouseState.X, currentMouseState.Y); }
    }

    // indicates whether the left mouse button is pressed
    public bool MouseLeftButtonPressed() {
        return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
    }

    // indicates whether the player has pressed the key k in the current update
    public bool IsKeyPressed(Keys k) {
        return currentKeyboardState.IsKeyDown(k) && (previousKeyboardState.IsKeyUp(k));
    }

    // indicates whether key k is currently down
    public bool IsKeyDown(Keys k, bool repeatDelay = false) {
        return currentKeyboardState.IsKeyDown(k) && (!repeatDelay || (timeSinceLastKeyPress > keyRepeatDelay && repeatDelay) || previousKeyboardState.IsKeyUp(k));
    }

    // indicates whether the player has released the key k in the current update
    public bool IsKeyReleased(Keys k) {
        return currentKeyboardState.IsKeyUp(k) && (previousKeyboardState.IsKeyDown(k));
    }
}