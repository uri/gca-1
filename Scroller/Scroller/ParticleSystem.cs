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
    class ParticleSystem
    {
        const int numberOfParticles = 300;
        Vector2[] deltas;
        List<Vector4> explosions;

        public ParticleSystem()
        {
            deltas = new Vector2[numberOfParticles];
            explosions = new List<Vector4>();

            for (int i = 0; i < numberOfParticles; i++)
            {
                Random rand = new Random();
                double angle = (rand.NextDouble() * 3.14 * 2);
                double distance = (rand.NextDouble() + 0.2);
                deltas[i] = new Vector2((float)(Math.Sin(angle) * distance), 
                    (float)(Math.Cos(angle) * distance));
            }

         
        }

        public void RegisterExplosion(GameTime gameTime, float x, float y, int type)
        {
            explosions.Add(new Vector4(x,y,type, (float)gameTime.TotalGameTime.TotalMilliseconds));
        }

        public void Draw(GameTime gameTime, SpriteBatch sb, Texture2D tex)
        {
          

            foreach (Vector4 v in explosions)
            {
                double theTime = gameTime.TotalGameTime.TotalMilliseconds - v.W;
                for (int i = 0; i < numberOfParticles; i++)
                {
                    Vector2 vec = new Vector2(  (float)(v.X + theTime * deltas[i].X ), (float)(v.Y + theTime * deltas[i].Y ));
                    sb.Draw(tex, vec, Color.Red);

                }
            }
        }


    }
}
