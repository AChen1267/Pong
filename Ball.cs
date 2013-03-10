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
    class Ball : Sprite
    {
        const string BALL_ASSETNAME = "paddle";
        const int START_POSITION_X = 0;
        const int START_POSITION_Y = 0;
        const int START_SPEED = 200;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;
        const int MAX_SPEED = 400;
        int radius;
        ContentManager mContentManager;

        public Vector2 mDirection = Vector2.Zero;
        public Vector2 mSpeed = Vector2.Zero;
        //bool atStart = true;
        bool lastMovedLeft = true;

        KeyboardState mPreviousKeyboardState;
  
        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, BALL_ASSETNAME);
            Source = new Rectangle(0, 0, Source.Width, Source.Width);
            center = new Vector2(Position.X + Source.Width / 2, Position.Y + Source.Height / 2);
            radius = Source.Width / 2;
        }
        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            UpdateMovement(aCurrentKeyboardState);
            mPreviousKeyboardState = aCurrentKeyboardState;
            //center = new Vector2(Source.Width / 2, Source.Height / 2);
            center = new Vector2(Position.X + Source.Width / 2, Position.Y + Source.Height / 2);
            base.Update(theGameTime, mSpeed, mDirection);
        }
        public int Radius() { return radius; }
        public int MaxSpeed() { return MAX_SPEED; }
        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            //if (atStart)
            if (mSpeed == Vector2.Zero && mDirection == Vector2.Zero)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;
                if (aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false) //beginning (TODO: alternate L/R per turn, make invalid unless 1 side loses
                {
                    mSpeed.X = START_SPEED;
                    if (lastMovedLeft)
                    {
                        mDirection.X = MOVE_LEFT;
                        lastMovedLeft = false;
                    }
                    else
                    {
                        mDirection.X = MOVE_RIGHT;
                        lastMovedLeft = true;
                    }
                    //atStart = false;
                }
            }
            //  reposition at start (add score later) and make atStart true
            //if (Position.X < 0 || Position.X + Source.Width > GraphicsDevice.Viewport.Width)
            //{
            //    Position.X = START_POSITION_X;
            //    Position.Y = START_POSITION_Y;
            //    atStart = true;
            //}
        }
        public override void Draw(SpriteBatch theSpriteBatch) //remember the override keyword
        {
            base.Draw(theSpriteBatch);
        }

    }
}
