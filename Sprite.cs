using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    class Sprite
    {
        //The asset name for the Sprite's Texture  (stores name of the image to be loaded from Content Pipeline for this sprite)     
        public string AssetName;

        //The size of the Sprite (w/ scale applied)
        public Rectangle Size;

        //The Rectangular area from the original image that 
        //defines the Sprite. 
        Rectangle mSource;
        public Rectangle Source
        {
            get { return mSource; }
            set
            {
                mSource = value;
                Size = new Rectangle(0, 0, (int)(mSource.Width * Scale), (int)(mSource.Height * Scale));
            }
        }

        //Used to size the Sprite up or down from the original image
        private float mScale = 1.0f;

        //The current position of the Sprite        
        public Vector2 Position = new Vector2(0, 0);

        //The center of the Sprite (with respect to the screen)
        public Vector2 center = Vector2.Zero;

        //The texture object used when drawing the sprite        
        private Texture2D mSpriteTexture;

        //When the scale is modified through the property, the Size of the  
        //sprite is recalculated with the new scale applied.      
        public float Scale
        {
            get { return mScale; }
            set
            {
                mScale = value;
                //Recalculate the Size of the Sprite with the new scale  
                Size = new Rectangle(0, 0, (int)(Source.Width * Scale), (int)(Source.Height * Scale));
            }
        }

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            mSpriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            AssetName = theAssetName;
            Source = new Rectangle(0, 0, mSpriteTexture.Width, mSpriteTexture.Height);
            Size = new Rectangle(0, 0, (int)(mSpriteTexture.Width * mScale), (int)(mSpriteTexture.Height * mScale));
        }

        //Draw the sprite to the screen
        public virtual void Draw(SpriteBatch theSpriteBatch) //virtual allows an inherited class to override a method
        {
            theSpriteBatch.Draw(mSpriteTexture, Position,
                Source, Color.White,
                0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        //Update the Sprite and change its position based on the passed in speed, direction and elapsed time.   
        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            Position += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
            //theDirection = left (X -1), right (X 1), up (Y -1), down(Y 1), none (X/Y 0)
        }
    }
}
