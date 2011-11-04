using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
namespace WindowsPhoneGame2
{
    class DrawingBoard
    {
        const int touchSize = 300;
        public Vector2[] touches;
        public double[] time;
        int insertionIndex = 0;

        public DrawingBoard()
        {
            touches = new Vector2[touchSize];

            time = new double[touchSize];
        }

        public void addTouch(float x, float y, double time)
        {
            touches[insertionIndex] = new Vector2(x, y);
            this.time[insertionIndex] = time;
            insertionIndex++;

            if (insertionIndex > touchSize - 1)
                insertionIndex = 0;
        }



        public void draw(GameTime gameTime, SpriteBatch sb, Texture2D tex)
        {
            for (int i = 0; i < touchSize; i++)
            {
                Vector2 t = touches[i];

                if (gameTime.TotalGameTime.TotalMilliseconds - time[i] < 1000)
                {
                    //System.Diagnostics.Debug.WriteLine(gameTime.TotalGameTime.Milliseconds - time[i]);
                    sb.Draw(tex, t, Color.Black);
                }
                    
            
            }

        }
    }


}
