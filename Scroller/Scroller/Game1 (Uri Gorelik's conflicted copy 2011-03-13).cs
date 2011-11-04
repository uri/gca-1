using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Random rand;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        SpriteFont floatyFont;

        // Variables 
        DrawingBoard drawingboard;
        ParticleSystem particleSystem;
        NumberSystem numberSystem;
       

        TouchCollection touchState;
        List<Block> blocks = new List<Block>();
        List<Obstacle> obstacles = new List<Obstacle>();
        Vector2[] backgroundPositions;
        Vector2 fontPos;
        Vector2 playerPosition;
        

        Texture2D[] backgrounds;
        Texture2D playerTexture;
        Texture2D enemyTexture;
        Texture2D lineTexture;
        Texture2D exploTexture;
        Texture2D obstacleTexture;

        SoundEffect explosionSound;
        SoundEffect jumpSound;


        float velY;
        public int score;

        int currentTexture = 0;
        int state = 1;

        const float defaultJumpingVel = 40;
        float jumpVel = 40;
        bool isJumping = false;

        const float defaultScrollSpeed = 10;
        float scrollingSpeed = 10;
        float lastGest = 0;         // Time since the last gesture in milliseconds
        const float playDefaultPosX = 100;
        const float playDefaultPosY = 350;

        bool hyperReady = false;



        // ------------------------------------------------------------------------------------
        // Constructor
        // ------------------------------------------------------------------------------------
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
     
        }


        // ------------------------------------------------------------------------------------
        // Initialize Everything Here
        // ------------------------------------------------------------------------------------
        protected override void Initialize()
        {

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            playerPosition = new Vector2(playDefaultPosX, playDefaultPosY);
            backgroundPositions = new Vector2[2];
            backgrounds = new Texture2D[4];
            // Textures

            backgroundPositions[0].X = 0;
            backgroundPositions[1].X = 800;
            //backgroundPositions[2].X = 1600;

            drawingboard = new DrawingBoard();
            particleSystem = new ParticleSystem();
            numberSystem = new NumberSystem();

            velY = 0;
            touchState = TouchPanel.GetState();
            fontPos.X = 0;
            fontPos.Y = 0;

            rand = new Random();

            base.Initialize();
        }


        // ------------------------------------------------------------------------------------
        // Load Content
        // ------------------------------------------------------------------------------------
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerTexture = Content.Load<Texture2D>("Player");
            enemyTexture = Content.Load<Texture2D>("Enemy");
            lineTexture = Content.Load<Texture2D>("line2");
            exploTexture = Content.Load<Texture2D>("line");
            obstacleTexture = Content.Load<Texture2D>("Obstacle");
            backgrounds[0] = Content.Load<Texture2D>("b0");
            backgrounds[1] = Content.Load<Texture2D>("b1");
            backgrounds[2] = Content.Load<Texture2D>("b2");
            backgrounds[3] = Content.Load<Texture2D>("loop");


            font = Content.Load<SpriteFont>("Font");
            floatyFont = Content.Load<SpriteFont>("FloatyFont");

            // Sounds
            explosionSound = Content.Load<SoundEffect>("explosion");
            jumpSound = Content.Load<SoundEffect>("jump");


            // TODO: use this.Content to load your game content here
        }


        protected override void UnloadContent(){ }

        // ------------------------------------------------------------------------------------
        // UPDATE
        // ------------------------------------------------------------------------------------
        protected override void Update(GameTime gameTime)
        {

            score += 3;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Scroll the background
            backgroundPositions[0].X -= scrollingSpeed;
            backgroundPositions[1].X -= scrollingSpeed;

            if (playerPosition.Y < playDefaultPosY)
            {
                velY -= 3.5f;
            }
            
            playerPosition.Y -= velY;


            if (playerPosition.Y > playDefaultPosY)
            {
                playerPosition.Y = playDefaultPosY;
                velY = 0;
                isJumping = false;
                scrollingSpeed = defaultScrollSpeed;

            }

            // Scroll the backgrounds
            for (int i = 0; i < 2; i++)
            {
                if (backgroundPositions[i].X <= -800)//+ backgroundPositions[i].X
                {
                    backgroundPositions[i].X =  800 + 800 + backgroundPositions[i].X;
                }
            }

            // Add a new block
            if (rand.NextDouble() < 0.025)
            {

                Block b = new Block(enemyTexture, (int)(rand.NextDouble() * 350));
                blocks.Add(b);

            }

            // Add a new obstacle
            if (rand.NextDouble() < 0.01)
            {

                Obstacle b = new Obstacle(obstacleTexture, 350);
                obstacles.Add(b);

            }

            // Update the blocks and obstacles
            for (int i = 0; i < blocks.Count; i++)
            {
                 Block t = blocks[i];
                t.position = new Vector2(t.position.X - scrollingSpeed, t.position.Y);
                if (t.position.X < 0)
                {
                    if (state == 4)
                    numberSystem.RegisterString(blocks[0].position, gameTime, -7453, this);
                    blocks.RemoveAt(0); 
                    i--;
                    
                }

            }

            // Update the obstacles
            for (int i = 0; i < obstacles.Count; i++)
            {
                Obstacle ob = obstacles[i];
                ob.position = new Vector2(ob.position.X - scrollingSpeed, ob.position.Y);
                if (ob.position.X < 0)
                {
                    if (ob.position.X < 0 && !isJumping)
                    {
                        if (state == 4) 
                        numberSystem.RegisterString(obstacles[0].position, gameTime, -7453, this);

                    }
                    obstacles.RemoveAt(0);
                    i--;
                }

            }


            System.Diagnostics.Debug.WriteLine("Jump: " + isJumping);
            // Check for collisions with the player
            for (int i = 0; i < obstacles.Count; i++)
            {
               
                Obstacle ob = obstacles[i];
                System.Diagnostics.Debug.WriteLine("Object x val: " + ob.position.X);
                if (ob.position.X > 170 && ob.position.X < 200 && !isJumping)
                {
                    if (state == 4) 
                        numberSystem.RegisterString(ob.position, gameTime, -7453, this);
                        obstacles.RemoveAt(0);
                        i--;
                }

            }


            // See what the gestures are doing
            HandleGestures(gameTime);

            base.Update(gameTime);
        }

        // ------------------------------------------------------------------------------------
        // DRAW
        // ------------------------------------------------------------------------------------
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // Draw the backgrounds
            for (int i = 0; i < 2; i++)
            {
                //if (backgroundPositions[i].X <= 800)
                    spriteBatch.Draw(backgrounds[currentTexture], backgroundPositions[i], Color.White);
            }

            // Draw the blocks
            foreach (Block b in blocks)
            {
                spriteBatch.Draw(b.texture, b.position, Color.White);
            }

            // Draw the obstacles
            foreach (Obstacle b in obstacles)
            {
                spriteBatch.Draw(b.texture, b.position, Color.White);
            }

            // Draw the player
            spriteBatch.Draw(playerTexture, playerPosition, Color.White);

            drawingboard.draw(gameTime, spriteBatch, lineTexture);
            particleSystem.Draw(gameTime, spriteBatch, exploTexture);

            // Draw the score
            spriteBatch.DrawString(font, "Score : " + score, fontPos, Color.Black);

            // Draw the floaty text
            numberSystem.Draw(gameTime, spriteBatch, floatyFont);

            spriteBatch.End();
            base.Draw(gameTime);
        }


        void HandleGestures(GameTime gameTime)
        {
            TouchPanel.EnabledGestures = GestureType.VerticalDrag | GestureType.HorizontalDrag | GestureType.Tap | GestureType.FreeDrag;
            while (TouchPanel.IsGestureAvailable)
            {

                GestureSample gesture = TouchPanel.ReadGesture();
                drawingboard.addTouch(gesture.Position.X,
                    gesture.Position.Y,
                    gameTime.TotalGameTime.TotalMilliseconds);


                // JUMP
                if (gesture.GestureType == GestureType.Tap)
                {
                    if (playerPosition.Y == playDefaultPosY)
                    {
                        velY = jumpVel;
                        jumpVel = defaultJumpingVel;
                        jumpSound.Play();
                        isJumping = true;
                        
                        scrollingSpeed = 15;


                        if(hyperReady){
                            scrollingSpeed = 25;
                            hyperReady = false;
                        }
                        if (state == 1)
                        {
                            state++;
                            currentTexture++;
                        }
                            
                    }
                }

                // Queue For an attack
                if (gesture.GestureType == GestureType.HorizontalDrag)
                {

                    if (blocks.Count != 0)
                    {

                        Block removed = blocks[0];
                        blocks.RemoveAt(0);
                        particleSystem.RegisterExplosion(gameTime, removed.position.X, removed.position.Y, 0);
                        // Floaty text
                        numberSystem.RegisterString(removed.position, gameTime, 10533, this);

                        if (state == 2)
                        {
                            state++;
                            currentTexture++;
                        }

                        explosionSound.Play();
                    }

                    lastGest = gesture.Timestamp.Milliseconds; // We can do a gesture


                }



                if (gesture.GestureType == GestureType.VerticalDrag)
                {

                    if (lastGest != 0 && gesture.Timestamp.Milliseconds - lastGest < 1500 && gesture.Delta.Y > 0)
                    {
                        lastGest = 0;

                        // Tutorial stuff
                        if (state == 3)
                        {

                         //   if (currentTexture == 2)
                           //     state = 0;

                            //currentTexture++;
                            jumpVel = 55;
                            state++;
                            currentTexture++;
                        }
                        else
                        {
                            jumpVel = 55;
                            hyperReady = true;
                        }
                        
                    }


                    else if (gesture.Delta.Y < 0)
                    {

                        //isJumping = true;
                    }

                }

            }
        }

    }
}
