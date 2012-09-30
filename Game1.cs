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
        int rightCount;
        int leftCount;
        SpriteFont score;
        SpriteFont leftScore;
        SpriteFont rightScore;
        Vector2 fontPos; //for score title
        Vector2 lPos;
        Vector2 rPos;
        MouseState mouseStateCurrent, mouseStatePrev;
        int tickCounter;
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
            rightCount = leftCount = tickCounter = 0;
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
            
            if (mBall.Position.X == left.Position.X || mBall.Position.X == right.Position.X)
                mBall.mDirection.X = -1;
            if (mBall.Position.X + mBall.Source.Width == left.Source.Width + left.Position.X || mBall.Position.X + mBall.Source.Width == right.Source.Width + right.Position.X)
                mBall.mDirection.X = 1;
            //make multiple rectangles per paddle to change Y direction as well
            Rectangle ballRect = new Rectangle((int)mBall.Position.X, (int)mBall.Position.Y,
                                        mBall.Source.Width, mBall.Source.Height);  

            Rectangle leftRect = new Rectangle((int)left.Position.X, (int)left.Position.Y + 1,  
                                        left.Source.Width, left.Source.Height /3 - 1);
            Rectangle leftRect2 = new Rectangle((int)left.Position.X, (int)(left.Position.Y + left.Source.Height / 3),
                                        left.Source.Width, left.Source.Height / 3);
            Rectangle leftRect3 = new Rectangle((int)left.Position.X, (int)(left.Position.Y + left.Source.Height * 2 / 3),
                                        left.Source.Width, left.Source.Height / 3 - 1);
            Rectangle leftTop = new Rectangle((int)left.Position.X, (int)left.Position.Y,
                                        left.Source.Width, 1);
            Rectangle leftBottom = new Rectangle((int)left.Position.X, (int)left.Position.Y + left.Source.Height,
                                        left.Source.Width, 1);


            Rectangle rightRect = new Rectangle((int)right.Position.X, (int)right.Position.Y - 1,
                                        right.Source.Width, right.Source.Height / 3 - 1);
            Rectangle rightRect2 = new Rectangle((int)right.Position.X, (int)(right.Position.Y + right.Source.Height / 3),
                                        right.Source.Width, right.Source.Height / 3);
            Rectangle rightRect3 = new Rectangle((int)right.Position.X, (int)(right.Position.Y + right.Source.Height * 2 / 3),
                                        right.Source.Width, right.Source.Height / 3 - 1);
            Rectangle rightTop = new Rectangle((int)right.Position.X, (int)right.Position.Y,
                                        right.Source.Width, 1);
            Rectangle rightBottom = new Rectangle((int)right.Position.X, (int)right.Position.Y + right.Source.Height,
                                        right.Source.Width, 1);

            if (ballRect.Intersects(leftTop) || ballRect.Intersects(rightTop) || ballRect.Intersects(leftBottom) || ballRect.Intersects(rightBottom))
            {
                mBall.mDirection.Y *= -1;
            }
            if (ballRect.Intersects(leftRect) || ballRect.Intersects(rightRect))  
            {    
                mBall.mDirection.X *= -1;            
                mBall.mSpeed.Y += 50;
                mBall.mDirection.Y = -1;
            }
            if (ballRect.Intersects(leftRect2) && ballRect.Intersects(leftRect))
            {
                mBall.mDirection.X *= -1;
                mBall.mSpeed.Y += 25;
                mBall.mDirection.Y = -1;
            }
            if (ballRect.Intersects(rightRect2) && ballRect.Intersects(rightRect))
            {
                mBall.mDirection.X *= -1;
                mBall.mSpeed.Y += 25;
                mBall.mDirection.Y = -1;
            }
            if (ballRect.Intersects(leftRect2) || ballRect.Intersects(rightRect2))      //center
            {
                mBall.mDirection.X *= -1;
                mBall.mSpeed.X += 20;
            }
            if (ballRect.Intersects(leftRect3) && ballRect.Intersects(leftRect2))
            {
                mBall.mDirection.X *= -1;
                mBall.mSpeed.Y += 25;
                mBall.mDirection.Y = 1;
            }
            if (ballRect.Intersects(rightRect3) && ballRect.Intersects(rightRect2))
            {
                mBall.mDirection.X *= -1;
                mBall.mSpeed.Y += 25;
                mBall.mDirection.Y = 1;
            }
            if (ballRect.Intersects(leftRect3) || ballRect.Intersects(rightRect3))
            {
                mBall.mDirection.X *= -1;
                mBall.mSpeed.Y += 50;
                mBall.mDirection.Y = 1;
            }

            if (mBall.Position.Y > GraphicsDevice.Viewport.Height - mBall.Source.Height || mBall.Position.Y < 0)        //top and bottom
                mBall.mDirection.Y *= -1;

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

            tickCounter++;  //interval to increase paddle speed
            if (tickCounter == 100)
            {
                left.speedUp();
                right.speedUp();
                tickCounter = 0;
            }

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
            spriteBatch.DrawString(score, title, fontPos, Color.LightGreen, 0, FontOrigin, 1.0f, SpriteEffects.FlipHorizontally, 0.5f);
            string lScore = "Left: " + rightCount;
            FontOrigin = leftScore.MeasureString(lScore) / 2;
            spriteBatch.DrawString(leftScore, lScore, lPos, Color.LightGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            string rScore = "Right: " + leftCount;
            FontOrigin = rightScore.MeasureString(rScore) / 2;
            spriteBatch.DrawString(rightScore, rScore, rPos, Color.LightGreen, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            left.Draw(this.spriteBatch);
            right.Draw(this.spriteBatch);
            mBall.Draw(this.spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
