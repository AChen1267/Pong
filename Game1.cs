using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Paddle left;
        Paddle right;
        Ball mBall;
        const float X_SCALE = 500f;
        const int CUSHION = 15;
        int rightCount;
        int leftCount;
        SpriteFont score;
        SpriteFont leftScore;
        SpriteFont rightScore;
        Vector2 fontPos; //for score title
        Vector2 lPos;
        Vector2 rPos;
        MouseState mouseStateCurrent, mouseStatePrev;
        //int tickCounter;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            left = new Paddle();
            right = new Paddle();
            mBall = new Ball();
            rightCount = leftCount = 0;// = tickCounter = 0;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            score = Content.Load<SpriteFont>("score");
            fontPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
            lPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 100, graphics.GraphicsDevice.Viewport.Height / 2 + 50);
            rPos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 + 100, graphics.GraphicsDevice.Viewport.Height / 2 + 50);
            leftScore = Content.Load<SpriteFont>("score");
            rightScore = Content.Load<SpriteFont>("score");

            left.LoadContent(this.Content);
            left.Position = new Vector2(20, 
                GraphicsDevice.Viewport.Height / 2 - left.Source.Height/2);
            right.LoadContent(this.Content);
            right.Position = new Vector2(GraphicsDevice.Viewport.Width - 20 - right.Source.Width, 
                GraphicsDevice.Viewport.Height / 2 - right.Source.Height / 2);
            right.MovementCtrl(Keys.Up, Keys.Down);
            mBall.LoadContent(this.Content);
            mBall.Position = new Vector2(GraphicsDevice.Viewport.Width / 2 - mBall.Source.Width / 2, 
                GraphicsDevice.Viewport.Height / 2 - mBall.Source.Height / 2);  //need to fix for resetting
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            //mouse resets score when clicked
            mouseStateCurrent = Mouse.GetState();

            if (mouseStateCurrent.LeftButton == ButtonState.Pressed && mouseStatePrev.LeftButton == ButtonState.Released)
                leftCount = rightCount = 0;
            mouseStatePrev = mouseStateCurrent;


            left.Update(gameTime);
            right.Update(gameTime);
            mBall.Update(gameTime);
            
            // new logic
            if (mBall.center.X - mBall.Source.Width <= left.center.X && mBall.center.X - mBall.Source.Width >= left.center.X - CUSHION) // range "in" the paddle where the paddle can return the ball
            {
                float dist = mBall.center.Y - left.center.Y + mBall.Radius();
                if (Math.Abs(dist) <= left.Source.Height / 2)
                {
                    mBall.mSpeed.Y = (float)Math.Pow(Math.Abs(dist), 1.5);
                    mBall.mDirection.X = 1;
                    if (dist > 0)
                        mBall.mDirection.Y = 1;
                    else
                        mBall.mDirection.Y = -1;
                    mBall.mSpeed.X += X_SCALE/(Math.Abs(dist)+.5f); // the .5f is there so we don't divide by 0
                }
            }
            if (mBall.center.X + mBall.Source.Width >= right.center.X && mBall.center.X + mBall.Source.Width <= right.center.X + CUSHION)
            {
                float dist = mBall.center.Y - right.center.Y + mBall.Radius();
                if (Math.Abs(dist) <= right.Source.Height / 2)
                {
                    mBall.mSpeed.Y = (float)Math.Pow(Math.Abs(dist), 1.5);
                    mBall.mDirection.X = -1;
                    if (dist > 0)
                        mBall.mDirection.Y = 1;
                    else
                        mBall.mDirection.Y = -1;
                    mBall.mSpeed.X += X_SCALE/(Math.Abs(dist)+.5f);
                }
            }

            if (mBall.Position.Y > GraphicsDevice.Viewport.Height - mBall.Source.Height || mBall.Position.Y < 0)        //top and bottom
                mBall.mDirection.Y *= -1;
            
            if (mBall.mSpeed.X >= mBall.MaxSpeed()) mBall.mSpeed.X = mBall.MaxSpeed();
            if (mBall.mSpeed.Y >= mBall.MaxSpeed()) mBall.mSpeed.Y = mBall.MaxSpeed();
            if (mBall.Position.X < 0)
            {
                leftCount++;        //score
                mBall.Position.X = GraphicsDevice.Viewport.Width / 2 - mBall.Source.Width / 2;
                mBall.Position.Y = GraphicsDevice.Viewport.Height / 2 - mBall.Source.Height / 2;
                mBall.mSpeed = Vector2.Zero;
                mBall.mDirection = Vector2.Zero;
                left.speedReset();
                right.speedReset();
            }
            if (mBall.Position.X + mBall.Source.Width > GraphicsDevice.Viewport.Width)
            {
                rightCount++;       //score
                mBall.Position.X = GraphicsDevice.Viewport.Width / 2 - mBall.Source.Width / 2;
                mBall.Position.Y = GraphicsDevice.Viewport.Height / 2 - mBall.Source.Height / 2;
                mBall.mSpeed = Vector2.Zero;
                mBall.mDirection = Vector2.Zero;
                left.speedReset();
                right.speedReset();
            }
            //disallow paddles from going off screen
            if (left.Position.Y + left.Source.Height > GraphicsDevice.Viewport.Height)
                left.Position.Y = GraphicsDevice.Viewport.Height - left.Source.Height;
            if (left.Position.Y < 0)
                left.Position.Y = 0;
            if (right.Position.Y + right.Source.Height > GraphicsDevice.Viewport.Height)
                right.Position.Y = GraphicsDevice.Viewport.Height - right.Source.Height;
            if (right.Position.Y < 0)
                right.Position.Y = 0;

            //tickCounter++;  //interval to increase paddle speed
            //if (tickCounter == 100)
            //{
            //    left.speedUp();
            //    right.speedUp();
            //    tickCounter = 0;
            //}

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // Draw score
            string title = "Score";

            // Find the center of the string
            Vector2 FontOrigin = score.MeasureString(title) / 2;
            // Draw the string (this one uses 10 args...)
            spriteBatch.DrawString(score, title, fontPos, Color.WhiteSmoke, 0, FontOrigin, 1.0f, SpriteEffects.FlipHorizontally, 0.5f);
            string lScore = "Left: " + rightCount;
            FontOrigin = leftScore.MeasureString(lScore) / 2;
            spriteBatch.DrawString(leftScore, lScore, lPos, Color.Purple, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            string rScore = "Right: " + leftCount;
            FontOrigin = rightScore.MeasureString(rScore) / 2;
            spriteBatch.DrawString(rightScore, rScore, rPos, Color.Green, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            left.Draw(this.spriteBatch);
            right.Draw(this.spriteBatch);
            mBall.Draw(this.spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
