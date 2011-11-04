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
    class NumberSystem
    {
        List<Vector2> positionList;
        List<int> strings;
        List<Double> gameTimes;
        bool addScore = true;


        Game1 game;

        const int removeLocation = 200;

        public NumberSystem()
        {
            positionList = new List<Vector2>();
            strings = new List<int>();
            gameTimes = new List<double>();
        }

        public void RegisterString(Vector2 vec, GameTime gt, int stringToDisp, Game1 g)
        {
            game = g;
            positionList.Add(vec);
            strings.Add(stringToDisp);
            gameTimes.Add(gt.TotalGameTime.TotalMilliseconds);
            if (stringToDisp < 0)
                addScore = false;
        }

        public void Update(GameTime gt)
        {
            // Update loop

           
                for (int i = 0; i < positionList.Count; i++)
                {
                    //if (positionList[i].X < removeLocation)

                    if (strings[i] > 0)
                    {
                        if (positionList[i].X < removeLocation)
                        {
                            game.score += strings[i];
                            positionList.RemoveAt(i);
                            gameTimes.RemoveAt(i);
                            strings.RemoveAt(i);

                            i--;
                        }
                    }

                    else
                    {
                        if (gt.TotalGameTime.TotalMilliseconds - gameTimes[i] > 1100)
                        {
                            game.score += strings[i];
                            positionList.RemoveAt(i);
                            gameTimes.RemoveAt(i);
                            strings.RemoveAt(i);

                            i--;
                        }
                    }


                    
                }

                for (int i = 0; i < positionList.Count; i++)
                {
                    Vector2 removedVec = positionList[i];
                    int removedString = strings[i];
                    Double removedTime = gameTimes[i];

                    if (strings[i] > 0)
                    {
                        removedVec.X = positionList[i].X + (positionList[i].X / -30.0f);
                        removedVec.Y = positionList[i].Y + (positionList[i].Y / -10.0f);
                    }

                    else
                    {
                        removedVec.X = positionList[i].X + (positionList[i].X / 30.0f);
                        removedVec.Y = positionList[i].Y + (positionList[i].Y / -10.0f);
                    }


                    positionList.RemoveAt(i);
                    strings.RemoveAt(i);
                    gameTimes.RemoveAt(i);

                    positionList.Add(removedVec);
                    strings.Add(removedString);
                    gameTimes.Add(removedTime);


                }
            


            
        }

        public void Draw(GameTime gameTime, SpriteBatch sb, SpriteFont font)
        {

            Update(gameTime);
            for (int i = 0; i < positionList.Count; i++)
            {
                if (strings[i] > 0)
                    sb.DrawString(font, strings[i] + "", positionList[i], Color.Yellow);
                else
                    sb.DrawString(font, ""+strings[i], positionList[i], Color.Red);
            }
            
            
        }
    }
}
