using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    class Paddle : Sprite
    {
        const string PADDLE_ASSETNAME = "paddle";
        const int START_POSITION_X = 125;
        const int START_POSITION_Y = 245;
        const int PADDLE_SPEED = 450;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;

        ContentManager mContentManager;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        Keys upKey = Keys.W;
        Keys downKey = Keys.S;

        KeyboardState mPreviousKeyboardState;

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, PADDLE_ASSETNAME);
            Source = new Rectangle(0, 0, Source.Width, Source.Height);
            center = new Vector2(Position.X + Source.Width / 2, Position.Y + Source.Height / 2);
        }
        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            UpdateMovement(aCurrentKeyboardState);
            mPreviousKeyboardState = aCurrentKeyboardState;
            center = new Vector2(Position.X + Source.Width / 2, Position.Y + Source.Height / 2);
            base.Update(theGameTime, mSpeed, mDirection);
        }
        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            mSpeed = Vector2.Zero;
            mDirection = Vector2.Zero;
            if (aCurrentKeyboardState.IsKeyDown(upKey) == true)
            {
                mSpeed.Y = PADDLE_SPEED; mDirection.Y = MOVE_UP;
            }
            else if (aCurrentKeyboardState.IsKeyDown(downKey) == true)
            {
                mSpeed.Y = PADDLE_SPEED; mDirection.Y = MOVE_DOWN;
            }
        }
        public void MovementCtrl(Keys up, Keys down) //change default key controls
        {
            upKey = up;
            downKey = down;
        }
        public void speedUp()
        {
            mSpeed.Y++;
        }
        public void speedReset()
        {
            mSpeed.Y = PADDLE_SPEED;
        }
    }
}
