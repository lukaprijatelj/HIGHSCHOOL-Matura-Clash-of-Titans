using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Xml;
using System.Xml.Linq;
using System.Threading;


namespace SpopadTitanov
{
    /// <summary>
    /// © Avtor: Luka Prijatelj
    /// | Maturitetni izdelek |
    ///     SPOPAD TITANOV
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        #region Spremenljivke
        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;
        SpriteEffects spriteEffectDefault;
        SpriteFont font_health, font_health2;

        /* SPREMENLJIVKE ZA SLIKE */
        Texture2D front;
        Texture2D back;
        Texture2D front_settings;
        Texture2D front_credits;
        Texture2D new_game;
        Texture2D scores;
        Texture2D[] menuButtons = new Texture2D[4];
        Texture2D regularButton;
        Texture2D on_left, off_left;
        Texture2D on_right, off_right;
        Texture2D right, left;

        /* SPREMENLJIVKE ZA RAÈUNALNIŠKO MIŠKO */
        MouseState current_mouse;
        int mouseX, mouseY;

        /* SPREMENLJIVKE ZA PISAVO */
        SpriteFont font_menu;
        SpriteFont font_settings;
        SpriteFont font_scores;

        /* SPREMENLJIVKE ZA VIDEO */
        Video vid;                //shranimo video v 'vid'
        VideoPlayer vidPlayer;    //uporabljamo za branje videa
        Rectangle vidRectangle;   //uporabljamo za nastavitev velikosti videa 
        Texture2D vidTexture;    //uporabljamo za izrisovanje videa 

        /* SPREMENLJIVKE ZA NASTAVITVE RESOLUCIJE IN ZVOKA */
        bool fullScreen;
        int bpp;
        bool sound;

        /* DRUGE SPREMENLJIVKE */
        Color myGreen = new Color(45, 157, 11);   //to je R,G,B za zeleno barvo
        Color colorDefault = Color.White;
        int[,] choose_res = new int[4, 2];
        int[,] meni_positions = new int[4, 2];
        int menuButtons0x = 1200;
        int menuButtons0y = 500;
        bool tmp1, tmp2;
        int tmp3, tmp4;
        int moved = 60;
        string text;


        /* Za kamero */
        Camera camera_backward, camera_first, camera_menu;
        Vector3 directionOfCamera;
        bool change_camera;
        
        /* Objekti in igralci */
        CModel player, computer;
        CModel stadion, ring, statue;

        /* Za resolucijo */
        Vector2 scale;
        int width, height;
        float scaleFactor;

        /* Animacije za objekte */
        ObjectAnimation head;
        ObjectAnimation left_arm, left_fist;
        ObjectAnimation right_arm, right_fist; 
        ObjectAnimation left_leg, left_knee, left_foot;
        ObjectAnimation right_leg, right_knee, right_foot;

        /* Position za objekte posameznega robota */
        Vector3 headV;
        Vector3 left_armV;
        Vector3 left_fistV;
        Vector3 right_armV;
        Vector3 right_fistV;
        Vector3 right_legV;
        Vector3 right_kneeV;
        Vector3 right_footV;
        Vector3 left_legV;
        Vector3 left_kneeV;
        Vector3 left_footV;

        /* Rotation za objekte posameznega robota */
        Vector3 headR;
        Vector3 left_armR;
        Vector3 left_fistR;
        Vector3 right_armR;
        Vector3 right_fistR;
        Vector3 right_legR;
        Vector3 right_kneeR;
        Vector3 right_footR;
        Vector3 left_legR;
        Vector3 left_kneeR;
        Vector3 left_footR;

        List<ObjectAnimation> transform = new List<ObjectAnimation>(); //zaèetne animacije za vse objekte
        List<ObjectAnimationFrame> punch_left_arm = new List<ObjectAnimationFrame>();  //premiki za left_arm
        List<ObjectAnimationFrame> punch_left_fist = new List<ObjectAnimationFrame>();  //premiki za left_fist
        List<ObjectAnimationFrame> punch_right_arm = new List<ObjectAnimationFrame>();  //premiki za left_arm
        List<ObjectAnimationFrame> punch_right_fist = new List<ObjectAnimationFrame>();  //premiki za left_fist

        List<ObjectAnimationFrame> special_left_arm = new List<ObjectAnimationFrame>();  //premiki za left_arm
        List<ObjectAnimationFrame> special_left_fist = new List<ObjectAnimationFrame>();  //premiki za left_fist

        List<ObjectAnimationFrame> walk_right_legF = new List<ObjectAnimationFrame>();  //premiki za left_fist
        List<ObjectAnimationFrame> walk_right_kneeF = new List<ObjectAnimationFrame>();  //premiki za left_fist
        List<ObjectAnimationFrame> walk_right_footF = new List<ObjectAnimationFrame>();  //premiki za left_fist
        List<ObjectAnimationFrame> walk_left_legF = new List<ObjectAnimationFrame>();  //premiki za left_fist
        List<ObjectAnimationFrame> walk_left_kneeF = new List<ObjectAnimationFrame>();  //premiki za left_fist
        List<ObjectAnimationFrame> walk_left_footF = new List<ObjectAnimationFrame>();  //premiki za left_fist
        List<ObjectAnimationFrame> walk_right_legB = new List<ObjectAnimationFrame>();  //premiki za left_fist
        List<ObjectAnimationFrame> walk_right_kneeB = new List<ObjectAnimationFrame>();  //premiki za left_fist
        List<ObjectAnimationFrame> walk_right_footB = new List<ObjectAnimationFrame>();  //premiki za left_fist
        List<ObjectAnimationFrame> walk_left_legB = new List<ObjectAnimationFrame>();  //premiki za left_fist
        List<ObjectAnimationFrame> walk_left_kneeB = new List<ObjectAnimationFrame>();  //premiki za left_fist
        List<ObjectAnimationFrame> walk_left_footB = new List<ObjectAnimationFrame>();  //premiki za left_fist

        KeyframedObjectAnimation[] punch_leftP = new KeyframedObjectAnimation[2];
        KeyframedObjectAnimation[] punch_rightP = new KeyframedObjectAnimation[2];
        KeyframedObjectAnimation[] special_punchP = new KeyframedObjectAnimation[2];
        KeyframedObjectAnimation[] walkFP = new KeyframedObjectAnimation[6];
        KeyframedObjectAnimation[] walkBP = new KeyframedObjectAnimation[6];

        KeyframedObjectAnimation[] punch_leftC = new KeyframedObjectAnimation[2];
        KeyframedObjectAnimation[] punch_rightC = new KeyframedObjectAnimation[2];
        KeyframedObjectAnimation[] special_punchC = new KeyframedObjectAnimation[2];
        KeyframedObjectAnimation[] walkFC = new KeyframedObjectAnimation[6];
        KeyframedObjectAnimation[] walkBC = new KeyframedObjectAnimation[6];

        AnimPlayer animPlayer1 = new AnimPlayer();
        AnimPlayer animPlayer2 = new AnimPlayer();
        AnimPlayer animPlayer3 = new AnimPlayer();
        AnimPlayer animPlayer4 = new AnimPlayer();
        AnimPlayer animPlayer5 = new AnimPlayer();
        AnimPlayer animPlayer6 = new AnimPlayer();
        AnimPlayer animPlayer7 = new AnimPlayer();
        AnimPlayer animPlayer8 = new AnimPlayer();

        /* Spremenljivke za robota */
        string[] Objects = { "Head", "Left_arm", "Left_fist", "Right_arm", "Right_fist", "Right_leg", "Right_knee", "Right_foot", "Left_leg", "Left_knee", "Left_foot" };
        int healthP, healthC;

        /* 2D texture */
        Texture2D[] jarvis = new Texture2D[5];
        Texture2D health_al;
        Texture2D name_score;
        Texture2D victory;
        Texture2D lost;

        /* Za funkcijo DRAW() */
        string name, temporar;
        float time;
        Keys[] temporary;

        /* Za funkcijo UPDATE() */
        BoundingSphere player_head, computer_head, player_punch_sphere, computer_punch_sphere, player_defend_sphere, computer_defend_sphere, player_special_punch_sphere, computer_special_punch_sphere;
        TimeSpan timer_one, timer_two, timer_three, timer_four, timer_five, timer_six;
        SoundEffect soundMissed, soundPunched;
        Vector3 rotChange_one, rotChange_two;
        Vector3 player_punch_sphereV;
        Song soundACDC;
        float left_arm_radius, right_arm_radius, special_left_arm_radius;
        float Z1, X1, Z2, X2;
        float angle, k;
        bool punchP, defendP, punchC, defendC, punchSP, punchSC;
        bool collision1, collision2;
        bool attack, defence;
        int walk_state;

        /* Ostale spremenljivke */
        Color zelena = new Color(24, 221, 229);
        Random rnd = new Random();

        /* USTVARIMO IZBIRO MED ZASTAVICAMI ZA PREKLOP MED MENIJI */
        enum gameState
        {
            Menu,
            NewGame_menu,
            Settings_menu,
            Credits_menu,
            Intro,
            Game,
            Scores,
        }
        gameState game_state;
        #endregion

         
        /// <summary>
        /// Konstruktor za razred Game
        /// </summary>
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            game_state = gameState.Intro;

            // Postavi glavo, roke in noge spremenljivke na default pozicijo
            setObjectsDefaults(); 
            left_arm_radius = 8.82f;
            special_left_arm_radius = 9.1f;
            right_arm_radius = 6.3f;

            resetForNewGame();     

            setResolution();
            tmp1 = sound;
            tmp2 = fullScreen;
            tmp3 = bpp;

            choose_res[3, 0] = 1920;
            choose_res[3, 1] = 1080;
            choose_res[2, 0] = 1600;
            choose_res[2, 1] = 900;
            choose_res[1, 0] = 1366;
            choose_res[1, 1] = 768;
            choose_res[0, 0] = 1280;
            choose_res[0, 1] = 720;

            // Set vertical trace with the back buffer  
            graphics.SynchronizeWithVerticalRetrace = false;

            // Use multi-sampling to smooth corners of objects  
            graphics.PreferMultiSampling = true;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Funkcija, kjer se inicializirajo vse spremenljivke
        /// </summary> 
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteEffectDefault = SpriteEffects.None;

            // Preberi animacijo za posamezne objekte, iz XML datoteke
            readXML_ForAnimations();

            // Postavi glavo, roke in noge spremenljivke na zaèetne animacije
            head = applyObjectDefaults(headV, headV, headR, headR, 0, true);
            left_arm = applyObjectDefaults(left_armV, left_armV, left_armR, left_armR, 0, true);
            left_fist = applyObjectDefaults(left_fistV, left_fistV, left_fistR, left_fistR, 0, true);
            right_arm = applyObjectDefaults(right_armV, right_armV, right_armR, right_armR, 0, true);
            right_fist = applyObjectDefaults(right_fistV, right_fistV, right_fistR, right_fistR, 0, true);
            left_leg = applyObjectDefaults(left_legV, left_legV, left_legR, left_legR, 0, true);
            left_knee = applyObjectDefaults(left_kneeV, left_kneeV, left_kneeR, left_kneeR, 0, true);
            left_foot = applyObjectDefaults(left_footV, left_footV, left_footR, left_footR, 0, true);
            right_leg = applyObjectDefaults(right_legV, right_legV, right_legR, right_legR, 0, true);
            right_knee = applyObjectDefaults(right_kneeV, right_kneeV, right_kneeR, right_kneeR, 0, true);
            right_foot = applyObjectDefaults(right_footV, right_footV, right_footR, right_footR, 0, true);

            // Objekti spravi v seznam
            transform.Add(head);
            transform.Add(left_arm);
            transform.Add(left_fist);
            transform.Add(right_arm);
            transform.Add(right_fist);
            transform.Add(right_leg);
            transform.Add(right_knee);
            transform.Add(right_foot);
            transform.Add(left_leg);
            transform.Add(left_knee);
            transform.Add(left_foot);

            MediaPlayer.IsRepeating = true;

            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// Funkcija, kjer se vsi objekti (datoteke) naložijo
        /// </summary>
        protected override void LoadContent()
        {
            // Naloži vse potrebne 3D modele
            player = newCModel("player_blue", new Vector3(0, 400, 0), Vector3.Zero, new Vector3(50f), GraphicsDevice);
            computer = newCModel("player_red", new Vector3(0, 400, 0), Vector3.Zero, new Vector3(50f), GraphicsDevice);
            ring = newCModel("ring", Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice);
            stadion = newCModel("stadion", Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice);
            statue = newCModel("statue", Vector3.Zero, Vector3.Zero, Vector3.One, GraphicsDevice);

            // Naloži vse potrebne 2D texture
            front_settings = Content.Load<Texture2D>(@"Pictures\front_layer_settings");
            front_credits = Content.Load<Texture2D>(@"Pictures\front_layer_credits");
            menuButtons[0] = Content.Load<Texture2D>(@"Pictures\\New_game");
            menuButtons[1] = Content.Load<Texture2D>(@"Pictures\\Settings");
            menuButtons[2] = Content.Load<Texture2D>(@"Pictures\\Credits");
            menuButtons[3] = Content.Load<Texture2D>(@"Pictures\\Exit");
            regularButton = Content.Load<Texture2D>(@"Pictures\Button");
            new_game = Content.Load<Texture2D>(@"Pictures\\New_Game1");
            scores = Content.Load<Texture2D>(@"Pictures\\Score_menu");
            front = Content.Load<Texture2D>(@"Pictures\front_layer");   
            back = Content.Load<Texture2D>(@"Pictures\back_layer");
            on_left = Content.Load<Texture2D>(@"Pictures\on_left");
            on_right = Content.Load<Texture2D>(@"Pictures\on_right");
            off_left = Content.Load<Texture2D>(@"Pictures\off_left");
            off_right = Content.Load<Texture2D>(@"Pictures\off_right");
            jarvis[0] = Content.Load<Texture2D>("Pictures\\jarvis");
            jarvis[1] = Content.Load<Texture2D>("Pictures\\circle_in");
            jarvis[2] = Content.Load<Texture2D>("Pictures\\circle_out");
            jarvis[3] = Content.Load<Texture2D>("Pictures\\circuit");
            jarvis[4] = Content.Load<Texture2D>("Pictures\\core");
            health_al = Content.Load<Texture2D>("Pictures\\health_all");
            name_score = Content.Load<Texture2D>("Pictures\\name");
            victory = Content.Load<Texture2D>("Pictures\\victory");
            right = Content.Load<Texture2D>(@"Pictures\right");
            left = Content.Load<Texture2D>(@"Pictures\left");
            lost = Content.Load<Texture2D>("Pictures\\lost");


            // Naloži vse potrebne pisave (.ttf)
            font_menu = Content.Load<SpriteFont>("Fonts\\SpriteFont_main");
            font_health = Content.Load<SpriteFont>("Fonts\\Health");
            font_health2 = Content.Load<SpriteFont>("Fonts\\Health2");
            font_settings = Content.Load<SpriteFont>("Fonts\\SpriteFont_settings");
            font_scores = Content.Load<SpriteFont>("Fonts\\SpriteFont_scores");

            // Naloži intro video
            vidRectangle = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            vid = Content.Load<Video>("Video\\xna");    
            vidPlayer = new VideoPlayer();

            // Naloži zvoke
            soundMissed = Content.Load<SoundEffect>("Sound\\missed");
            soundPunched = Content.Load<SoundEffect>("Sound\\punched");
            soundACDC = Content.Load<Song>("Sound\\acdc");

            // Nastavitve za sprednjo in zadnjo kamero
            camera_backward = new ChaseCamera(new Vector3(0, 1500, 2000), new Vector3(0,900,0), new Vector3(0, 0, 0), GraphicsDevice); 
            camera_menu = new ChaseCamera(new Vector3(100, 30, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), GraphicsDevice);
            camera_first = new ChaseCamera(new Vector3(0, 1150, -15), new Vector3(0, 1100, -600), new Vector3(0, 0, 0), GraphicsDevice);   // first person look
            ((ChaseCamera)camera_first).distance=false;


            /* Sestavi igralca in nasprotnika */
            int j = 0;
            foreach (string obj in Objects)
            {
                transformBones(player, obj, transform[j]);
                transformBones(computer, obj, transform[j]);
                j++;
            }

            /* Nastavitve, za trk (angl. collision) */
            player_punch_sphere.Radius = 50;
            computer_punch_sphere.Radius = player_punch_sphere.Radius;
            player_special_punch_sphere.Radius = player_punch_sphere.Radius;
            computer_special_punch_sphere.Radius = player_punch_sphere.Radius;

            player_head.Center = player.Position;
            player_head.Radius = 130;
            computer_head.Center = computer.Position;
            computer_head.Radius = player_head.Radius;

            player_defend_sphere.Radius = 120;
            player_defend_sphere.Center = player.Position;
            computer_defend_sphere.Radius = player_defend_sphere.Radius;
            computer_defend_sphere.Center = computer.Position;        

            // Zaèetne pozicije za igralca in nasprotnika
            player.Position = new Vector3(1500, 0, -1500);
            player.Rotation = new Vector3(0, MathHelper.ToRadians(135), 0);
            computer.Position = new Vector3(-1500, 0, 1500);
            computer.Rotation = new Vector3(0, -MathHelper.ToRadians(45), 0);
            statue.Position = new Vector3(0, -20, 28);
            statue.Scale = new Vector3(1.5f, 1.5f, 1.5f);

            // Za kamero
            directionOfCamera = player.Position;
            directionOfCamera.Y = 900;
        }

        /// <summary>
        /// Funkcija, kjer se izvaja celotna matematièna logika igrice
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (game_state == gameState.Game)
            {
                if (healthC > 0 && healthP > 0)
                {
                    collision2 = false;
                    collision1 = false;
                    player.Rotation = new Vector3(0, player.Rotation.Y, 0);

                    /* FORMULA ZA OBRAÈANJE DRUGEGA IGRALCA */
                    {
                        Z1 = computer.Position.X;
                        X1 = computer.Position.Z;
                        Z2 = player.Position.X;
                        X2 = player.Position.Z;

                        k = (Z2 - Z1) / (X2 - X1);

                        if (X2 > X1)
                            angle = (float)(-MathHelper.ToRadians(180) + Math.Atan(k));
                        else
                            angle = (float)Math.Atan(k);

                        computer.Rotation = new Vector3(0, angle, 0);
                    }

                    // Omogoèanje izhoda iz igre menu
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        this.IsMouseVisible = true;
                        MediaPlayer.Play(soundACDC);
                        game_state = gameState.Menu;
                    }

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        punchP = true;

                    if (Mouse.GetState().RightButton == ButtonState.Pressed)
                        defendP = true;

                    if (Keyboard.GetState().IsKeyDown(Keys.Space) && !punchSP)
                        punchSP = true;

                    /* Vnaprej izraèunani premiki igralca in nasprotnika */
                    Vector3 tmp_front = player.Position;
                    {
                        Matrix rotation = Matrix.CreateFromYawPitchRoll(player.Rotation.Y, player.Rotation.X, player.Rotation.Z);
                        tmp_front += Vector3.Transform(Vector3.Forward, rotation) * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 1.8f;
                    }

                    Vector3 tmp_back = player.Position;
                    {
                        Matrix rotation = Matrix.CreateFromYawPitchRoll(player.Rotation.Y, player.Rotation.X, player.Rotation.Z);
                        tmp_back += Vector3.Transform(Vector3.Backward, rotation) * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 1.8f;
                    }

                    Vector3 tmp2_front = computer.Position;
                    {
                        Matrix rotation = Matrix.CreateFromYawPitchRoll(computer.Rotation.Y, computer.Rotation.X, computer.Rotation.Z);
                        tmp2_front += Vector3.Transform(Vector3.Forward, rotation) * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 1.8f;
                    }

                    Vector3 tmp2_back = computer.Position;
                    {
                        Matrix rotation = Matrix.CreateFromYawPitchRoll(computer.Rotation.Y, computer.Rotation.X, computer.Rotation.Z);
                        tmp2_back += Vector3.Transform(Vector3.Backward, rotation) * (float)gameTime.ElapsedGameTime.TotalMilliseconds * 1.8f;
                    }

                    computer_head.Center = new Vector3(0, 20, 0) * player.Scale + computer.Position;
                    player_head.Center = new Vector3(0, 20, 0) * player.Scale + player.Position;


                    
                    /* ---------> UMETNA INTELIGENCA ZA NASPROTNIKA <---------*/
                    #region Computer_commands
                    
                    rotChange_two = Vector3.Zero;
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                        rotChange_two += new Vector3(0, 1, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                        rotChange_two += new Vector3(0, -1, 0);
                    computer.Rotation += rotChange_two * 0.06f;

                    /* UGOTAVLJANJE, ali bo nasprotnik napadal, ali se bo umikal */
                    {
                        timer_three += gameTime.ElapsedGameTime;
                        if (timer_three > TimeSpan.FromSeconds(1))
                        {
                            walk_state = 0;
                            timer_three = TimeSpan.Zero;
                        }
                    }

                    /* NASPROTNIK - hoja naprej */
                    if (walk_state == 0 && Vector3.Distance(tmp_front, tmp2_front) > 500 && !attack)  //Keyboard.GetState().IsKeyDown(Keys.Up)
                    {
                        animPlayer4.Play(5, 0, walkFC, computer, gameTime);
                        animPlayer4.Play(6, 1, walkFC, computer, gameTime);
                        animPlayer4.Play(7, 2, walkFC, computer, gameTime);
                        animPlayer4.Play(8, 3, walkFC, computer, gameTime);
                        animPlayer4.Play(9, 4, walkFC, computer, gameTime);
                        animPlayer4.Play(10, 5, walkFC, computer, gameTime);

                        if ((tmp2_front.X < 1780) && (tmp2_front.Z < 1780) && (tmp2_front.X > -1780) && (tmp2_front.Z > -1780))
                            computer.Position = tmp2_front;
                    }

                    /* NASPROTNIK - hoja nazaj */
                    else if (walk_state == 2 && Vector3.Distance(tmp_back, tmp2_back) > 500 && !attack) //Keyboard.GetState().IsKeyDown(Keys.Down)
                    {
                        animPlayer4.Play(5, 0, walkBC, computer, gameTime);
                        animPlayer4.Play(6, 1, walkBC, computer, gameTime);
                        animPlayer4.Play(7, 2, walkBC, computer, gameTime);
                        animPlayer4.Play(8, 3, walkBC, computer, gameTime);
                        animPlayer4.Play(9, 4, walkBC, computer, gameTime);
                        animPlayer4.Play(10, 5, walkBC, computer, gameTime);

                        if ((tmp2_back.X < 1780) && (tmp2_back.Z < 1780) && (tmp2_back.X > -1780) && (tmp2_back.Z > -1780))
                            computer.Position = tmp2_back;
                    }
                    else
                    {
                        animPlayer4.ResetPlayer();
                        int st1 = 0;
                        foreach (string obj in Objects)
                        {
                            transformBones(computer, obj, transform[st1]);
                            st1++;
                        }
                    }


                    /* UGOTAVLJANJE, ali bo nasprotnik udaril, ali se bo branil */
                    {
                        timer_four += gameTime.ElapsedGameTime;
                        timer_six += gameTime.ElapsedGameTime;
                        if (Vector3.Distance(tmp_front, tmp2_front) <= 500)
                        {
                            if (timer_four > TimeSpan.FromSeconds(1))
                            {
                                punchC = true;
                                defendC = GetRandomBoolean();
                                timer_four = TimeSpan.Zero;
                            }
                            if (timer_six > TimeSpan.FromSeconds(2) && !punchC)
                            {
                                punchSC = true;
                                timer_six = TimeSpan.Zero;
                            }
                        }
                        else
                            defendC = false;
                    }


                    /* NASPROTNIK - Udarec z levo roko */
                    if (punchSC)
                    {
                        animPlayer8.Play(1, 0, special_punchC, computer, gameTime);
                        animPlayer8.Play(2, 1, special_punchC, computer, gameTime);

                        if (animPlayer8.clip == 1)
                            for (int i = 1; i < 3; i++)
                            {
                                float x = special_left_arm_radius * (float)Math.Cos(computer.Rotation.Y - MathHelper.ToRadians(-100));   // x = radij * cos(kot v radijanih)
                                float z = special_left_arm_radius * -(float)Math.Sin(computer.Rotation.Y - MathHelper.ToRadians(-100));   // y = radij * sin(kot v radijanih)
                                player_punch_sphereV = new Vector3(x, 21.2f * i, z) * computer.Scale + computer.Position;
                                computer_special_punch_sphere.Center = player_punch_sphereV;

                                if (computer_special_punch_sphere.Intersects(player_defend_sphere) && defendP)
                                {
                                    special_punchC[0].resetAnimation();
                                    special_punchC[1].resetAnimation();

                                    animPlayer8.ResetPlayer();
                                    punchSC = false;
                                    if (sound)
                                        soundMissed.Play();
                                }
                            }

                        if (animPlayer8.endPlaying)
                        {
                            for (int i = 1; i < 3; i++)
                            {
                                float x = special_left_arm_radius * (float)Math.Cos(computer.Rotation.Y - MathHelper.ToRadians(-100));   // x = radij * cos(kot v radijanih)
                                float z = special_left_arm_radius * -(float)Math.Sin(computer.Rotation.Y - MathHelper.ToRadians(-100));   // y = radij * sin(kot v radijanih)
                                player_punch_sphereV = new Vector3(x, 21.2f * i, z) * computer.Scale + computer.Position;
                                computer_special_punch_sphere.Center = player_punch_sphereV;

                                if (computer_special_punch_sphere.Intersects(player_head))
                                {
                                    collision2 = true;
                                    timer_two = gameTime.ElapsedGameTime;
                                }
                            }
                            animPlayer8.ResetPlayer();
                            punchSC = false;
                        }
                    }


                    
                    if (punchC)
                    {
                        animPlayer5.Play(1, 0, punch_leftC, computer, gameTime);
                        animPlayer5.Play(2, 1, punch_leftC, computer, gameTime);

                        if (animPlayer5.clip == 2)
                            for (int i = 150; i > 85; i--)
                            {
                                float x = left_arm_radius * (float)Math.Cos(computer.Rotation.Y - MathHelper.ToRadians(-i));   // x = radij * cos(kot v radijanih)
                                float z = left_arm_radius * -(float)Math.Sin(computer.Rotation.Y - MathHelper.ToRadians(-i));   // y = radij * sin(kot v radijanih)
                                player_punch_sphereV = new Vector3(x, 21.2f, z) * computer.Scale + computer.Position;
                                computer_punch_sphere.Center = player_punch_sphereV;

                                if (computer_punch_sphere.Intersects(player_defend_sphere) && defendP)
                                {
                                    punch_leftC[0].resetAnimation();
                                    punch_leftC[1].resetAnimation();
                                    animPlayer5.ResetPlayer();
                                    punchC = false;
                                    if(sound)
                                        soundMissed.Play();
                                }
                            }

                        if (animPlayer5.endPlaying)
                        {
                            for (int i = 150; i > 85; i--)
                            {
                                float x = left_arm_radius * (float)Math.Cos(computer.Rotation.Y - MathHelper.ToRadians(-i));   // x = radij * cos(kot v radijanih)
                                float z = left_arm_radius * -(float)Math.Sin(computer.Rotation.Y - MathHelper.ToRadians(-i));   // y = radij * sin(kot v radijanih)
                                player_punch_sphereV = new Vector3(x, 21.2f, z) * computer.Scale + computer.Position;
                                computer_punch_sphere.Center = player_punch_sphereV;

                                if (computer_punch_sphere.Intersects(player_head))
                                {
                                    collision2 = true;
                                    timer_two = gameTime.ElapsedGameTime;
                                }
                            }
                            animPlayer5.ResetPlayer();
                            punchC = false;
                        }
                    }

                    /* NASPROTNIK - Obramba z desno roko */
                    if (defendC)
                    {
                        animPlayer6.Play(3, 0, punch_rightC, computer, gameTime);
                        animPlayer6.Play(4, 1, punch_rightC, computer, gameTime);

                        if (animPlayer6.endPlaying)
                        {
                            if (defendC)
                            {
                                animPlayer6.freez = true;

                                float x = right_arm_radius * (float)Math.Cos(computer.Rotation.Y - MathHelper.ToRadians(-70));   // x = radij * cos(kot v radijanih)
                                float z = right_arm_radius * -(float)Math.Sin(computer.Rotation.Y - MathHelper.ToRadians(-70));   // y = radij * sin(kot v radijanih)
                                player_punch_sphereV = new Vector3(x, 21.2f, z) * computer.Scale + computer.Position;
                                computer_defend_sphere.Center = player_punch_sphereV;
                            }
                            else
                            {
                                animPlayer6.freez = false;
                                defendC = false;
                            }
                            animPlayer6.ResetPlayer();
                            animPlayer6.Play(3, 0, punch_rightC, computer, gameTime);
                            animPlayer6.Play(4, 1, punch_rightC, computer, gameTime);
                        }

                    }
                    #endregion


                    /*  ---------> UPRAVLJANJE GLAVNEGA IGRALCA <---------*/
                    #region Player_commands

                    rotChange_one = Vector3.Zero;
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                        rotChange_one += new Vector3(0, 1, 0);
                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                        rotChange_one += new Vector3(0, -1, 0);
                    player.Rotation += rotChange_one * 0.06f;


                    /* IGRALEC - hoja naprej */
                    if (Keyboard.GetState().IsKeyDown(Keys.W) && Vector3.Distance(tmp_front, tmp2_front) > 500 && !defence)
                    {
                        animPlayer3.Play(5, 0, walkFP, player, gameTime);
                        animPlayer3.Play(6, 1, walkFP, player, gameTime);
                        animPlayer3.Play(7, 2, walkFP, player, gameTime);
                        animPlayer3.Play(8, 3, walkFP, player, gameTime);
                        animPlayer3.Play(9, 4, walkFP, player, gameTime);
                        animPlayer3.Play(10, 5, walkFP, player, gameTime);

                        if ((tmp_front.X < 1780) && (tmp_front.Z < 1780) && (tmp_front.X > -1780) && (tmp_front.Z > -1780))
                            player.Position = tmp_front;
                    }
                    /* IGRALEC - hoja nazaj */
                    else if (Keyboard.GetState().IsKeyDown(Keys.S) && Vector3.Distance(tmp_back, tmp2_back) > 500 && !defence)
                    {
                        animPlayer3.Play(5, 0, walkBP, player, gameTime);
                        animPlayer3.Play(6, 1, walkBP, player, gameTime);
                        animPlayer3.Play(7, 2, walkBP, player, gameTime);
                        animPlayer3.Play(8, 3, walkBP, player, gameTime);
                        animPlayer3.Play(9, 4, walkBP, player, gameTime);
                        animPlayer3.Play(10, 5, walkBP, player, gameTime);

                        if ((tmp_back.X < 1780) && (tmp_back.Z < 1780) && (tmp_back.X > -1780) && (tmp_back.Z > -1780))
                            player.Position = tmp_back;
                    }
                    else
                    {
                        animPlayer3.ResetPlayer();
                        int st1 = 0;
                        foreach (string obj in Objects)
                        {
                            transformBones(player, obj, transform[st1]);
                            st1++;
                        }
                    }            

                    /* IGRALEC - Udarec z levo roko */
                    if (punchSP && !punchP)
                    {
                        animPlayer7.Play(1, 0, special_punchP, player, gameTime);
                        animPlayer7.Play(2, 1, special_punchP, player, gameTime);

                        if (animPlayer7.clip == 1)
                            for (int i = 1; i < 3; i++)
                            {
                                float x = special_left_arm_radius * (float)Math.Cos(player.Rotation.Y - MathHelper.ToRadians(-100));   // x = radij * cos(kot v radijanih)
                                float z = special_left_arm_radius * -(float)Math.Sin(player.Rotation.Y - MathHelper.ToRadians(-100));   // y = radij * sin(kot v radijanih)
                                player_punch_sphereV = new Vector3(x, 21.2f * i, z) * player.Scale + player.Position;
                                player_special_punch_sphere.Center = player_punch_sphereV;

                                if (player_special_punch_sphere.Intersects(computer_defend_sphere) && defendC)
                                {
                                    special_punchP[0].resetAnimation();
                                    special_punchP[1].resetAnimation();

                                    animPlayer7.ResetPlayer();
                                    punchSP = false;
                                    if (sound)
                                        soundMissed.Play();
                                }
                            }

                        if (animPlayer7.endPlaying)
                        {
                            for (int i = 1; i < 3; i++)
                            {
                                float x = special_left_arm_radius * (float)Math.Cos(player.Rotation.Y - MathHelper.ToRadians(-100));   // x = radij * cos(kot v radijanih)
                                float z = special_left_arm_radius * -(float)Math.Sin(player.Rotation.Y - MathHelper.ToRadians(-100));   // y = radij * sin(kot v radijanih)
                                player_punch_sphereV = new Vector3(x, 21.2f * i, z) * player.Scale + player.Position;
                                player_special_punch_sphere.Center = player_punch_sphereV;

                                if (player_special_punch_sphere.Intersects(computer_head))
                                {
                                    collision1 = true;
                                    timer_one = gameTime.ElapsedGameTime;
                                }
                            }
                            animPlayer7.ResetPlayer();
                            punchSP = false;
                        }
                    }
                    
                    
                    if (punchP)
                    {
                        animPlayer1.Play(1, 0, punch_leftP, player, gameTime);
                        animPlayer1.Play(2, 1, punch_leftP, player, gameTime);

                        if (animPlayer1.clip == 2)
                            for (int i = 150; i > 85; i--)
                            {
                                float x = left_arm_radius * (float)Math.Cos(player.Rotation.Y - MathHelper.ToRadians(-i));   // x = radij * cos(kot v radijanih)
                                float z = left_arm_radius * -(float)Math.Sin(player.Rotation.Y - MathHelper.ToRadians(-i));   // y = radij * sin(kot v radijanih)
                                player_punch_sphereV = new Vector3(x, 21.2f, z) * player.Scale + player.Position;
                                player_punch_sphere.Center = player_punch_sphereV;

                                if (player_punch_sphere.Intersects(computer_defend_sphere) && defendC)
                                {
                                    punch_leftP[0].resetAnimation();
                                    punch_leftP[1].resetAnimation();

                                    animPlayer1.ResetPlayer();
                                    punchP = false;
                                    if (sound)
                                        soundMissed.Play();
                                }
                            }

                        if (animPlayer1.endPlaying)
                        {
                            for (int i = 150; i > 85; i--)
                            {
                                float x = left_arm_radius * (float)Math.Cos(player.Rotation.Y - MathHelper.ToRadians(-i));   // x = radij * cos(kot v radijanih)
                                float z = left_arm_radius * -(float)Math.Sin(player.Rotation.Y - MathHelper.ToRadians(-i));   // y = radij * sin(kot v radijanih)
                                player_punch_sphereV = new Vector3(x, 21.2f, z) * player.Scale + player.Position;
                                player_punch_sphere.Center = player_punch_sphereV;

                                if (player_punch_sphere.Intersects(computer_head))
                                {
                                    collision1 = true;
                                    timer_one = gameTime.ElapsedGameTime;
                                }
                            }
                            animPlayer1.ResetPlayer();
                            punchP = false;
                        }
                    }

                    /* IGRALEC - Obramba z desno roko */
                    if (defendP)
                    {
                        animPlayer2.Play(3, 0, punch_rightP, player, gameTime);
                        animPlayer2.Play(4, 1, punch_rightP, player, gameTime);

                        if (animPlayer2.endPlaying)
                        {
                            if (Mouse.GetState().RightButton == ButtonState.Pressed)
                            {
                                animPlayer2.freez = true;

                                float x = right_arm_radius * (float)Math.Cos(player.Rotation.Y - MathHelper.ToRadians(-70));   // x = radij * cos(kot v radijanih)
                                float z = right_arm_radius * -(float)Math.Sin(player.Rotation.Y - MathHelper.ToRadians(-70));   // y = radij * sin(kot v radijanih)
                                player_punch_sphereV = new Vector3(x, 21.2f, z) * player.Scale + player.Position;
                                player_defend_sphere.Center = player_punch_sphereV;
                            }
                            else
                            {
                                animPlayer2.freez = false;
                                defendP = false;
                            }
                            animPlayer2.ResetPlayer();
                            animPlayer2.Play(3, 0, punch_rightP, player, gameTime);
                            animPlayer2.Play(4, 1, punch_rightP, player, gameTime);
                        }
                    }
                    #endregion


                    /* Ali je bil robot uspešno udarjen ali je udarec ubranil */
                    {
                        if (collision1)
                        {
                            if(sound)
                                soundPunched.Play();
                            attack = true;
                            healthC -= rnd.Next(5, 11);
                        }
                        if (collision2)
                        {
                            if(sound)
                                soundPunched.Play();
                            defence = true;
                            healthP -= rnd.Next(5, 11);
                        }

                        if (attack)
                        {
                            if (timer_one < gameTime.ElapsedGameTime + TimeSpan.FromSeconds(0.2f))
                            {
                                computer.Rotation = new Vector3((float)MathHelper.ToRadians(10), computer.Rotation.Y, computer.Rotation.Z);
                                timer_one += gameTime.ElapsedGameTime;
                            }
                            else
                            {
                                attack = false;
                                walk_state = 2;
                            }
                        }
                        if (defence)
                        {
                            if (timer_two < gameTime.ElapsedGameTime + TimeSpan.FromSeconds(0.2f))
                            {
                                player.Rotation = new Vector3((float)MathHelper.ToRadians(10), player.Rotation.Y, player.Rotation.Z);
                                timer_two += gameTime.ElapsedGameTime;
                            }
                            else
                                defence = false;
                        }
                    }

                    /* POSODABLJANJE KAMERE */
                    if (Keyboard.GetState().IsKeyDown(Keys.C))
                    {
                        change_camera = !change_camera;
                        Thread.Sleep(120);
                    }

                    updateCamera1(gameTime);
                    updateCamera2(gameTime);
                }
            }
            else
                updateMouse();

            updateCamera_menu(gameTime);

            // Posodobi èas
            base.Update(gameTime);
        }

        /// <summary>
        /// Funkcija, kjer se vsi 3D objekti izrisujejo
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            // Poèisti zaslon
            GraphicsDevice.Clear(Color.CornflowerBlue);
            ResetGraphic();
            BeginRender3D();

            switch (game_state)
            {
                case gameState.Menu:
                    Meni(gameTime); 
                    break;
                case gameState.NewGame_menu:
                    new_Game_menu();
                    break;
                case gameState.Game:
                    mainGame(gameTime);
                    break;
                case gameState.Settings_menu:
                    Settings_menu(); 
                    break;
                case gameState.Credits_menu:
                    Credits_menu(); 
                    break;
                case gameState.Intro:
                    introVideo(); 
                    break;
                case gameState.Scores:
                    Scores_menu(); 
                    break;
            }

            // Posodobi èas
            base.Draw(gameTime);
        }


        public void ResetGraphic()
        {
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.AnisotropicWrap;
        }
        public void BeginRender3D()
        {
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        void updateCamera1(GameTime gameTime)
        {
            ((ChaseCamera)camera_backward).Move(player.Position, player.Rotation);
            camera_backward.Update();
        }
        void updateCamera2(GameTime gameTime)
        {
            ((ChaseCamera)camera_first).Move(player.Position, player.Rotation);
            camera_first.Update();
        }
        void updateCamera_menu(GameTime gameTime)
        {
            ((ChaseCamera)camera_menu).Move(Vector3.Zero, Vector3.Zero);
            camera_menu.Update();
        }

        public static bool GetRandomBoolean()
        {
            return new Random().Next(100) % 2 == 0;
        }
        void jarvis_view(GameTime gameTime)
        {
            float rotationJarvis = (float)gameTime.TotalGameTime.TotalSeconds;
            spriteBatch.Begin();

            /* SLIKE */
            apply_OnlyPicture(jarvis[0], new Vector2(0, 0));
            apply_Picture(jarvis[1], new Vector2(377, 942), Color.White, rotationJarvis, new Vector2(jarvis[1].Width / 2, jarvis[1].Height / 2));
            apply_Picture(jarvis[4], new Vector2(1562, 307), Color.White, rotationJarvis, new Vector2(jarvis[4].Width / 2, jarvis[4].Height / 2));
            apply_Picture(jarvis[3], new Vector2(187, 148), Color.White, rotationJarvis, new Vector2(jarvis[3].Width / 2, jarvis[3].Height / 2 + 3));
            apply_Picture(jarvis[2], new Vector2(377, 942), Color.White, rotationJarvis * -1f, new Vector2(jarvis[2].Width / 2, jarvis[2].Height / 2));

            /* BESEDILO */
            apply_OnlyText(font_health, new Vector2(155, 107), zelena, healthP.ToString());
            apply_Text(font_health, new Vector2(140, 615), zelena, new Vector2(0.30f, 0.30f), healthP.ToString());
            apply_Text(font_health, new Vector2(250, 260), zelena, new Vector2(0.30f, 0.30f), DateTime.Now.ToString("HH:mm tt"));
            apply_Text(font_health, new Vector2(290, 260), zelena, new Vector2(0.20f, 0.20f), DateTime.Now.ToString("ss tt"));
            apply_OnlyText(font_health, new Vector2(135, 320), zelena, DateTime.Now.ToString("dd"));
            apply_Text(font_health, new Vector2(260, 290), zelena, new Vector2(0.30f, 0.30f), DateTime.Now.ToString("MMMM tt"));

            /* KOORDINATE */
            text = player.Position.X.ToString("0.0") + "k";
            apply_Text(font_health, new Vector2(360, 910), zelena, new Vector2(0.30f, 0.30f), text); // x pozicija
            text = player.Position.Y.ToString("0.0") + "k";
            apply_Text(font_health, new Vector2(360, 930), zelena, new Vector2(0.30f, 0.30f), text); // y pozicija
            text = player.Position.Z.ToString("0.0") + "k";
            apply_Text(font_health, new Vector2(360, 950), zelena, new Vector2(0.30f, 0.30f), text); // z pozicija

            /* TIPKE */
            Keys[] theKeys;
            theKeys = Keyboard.GetState().GetPressedKeys();
            text = " ";
            int i = 0;
            foreach (Keys b in theKeys)
            {
                text = text + Convert.ToString(b);
                if (theKeys.Length > 1 && i < (theKeys.Length - 1))
                    text = text + " + ";
                i++;
            }
            apply_Text(font_health, new Vector2(400, 249), zelena, new Vector2(0.15f, 0.15f), text); // sql > pritisnjene tipke
            spriteBatch.End();
        }
        void health_all_function()
        {
            spriteBatch.Begin();
            {
                apply_OnlyPicture(health_al, new Vector2(0, 0));
                apply_Text(font_health2, new Vector2(150, 948), Color.White, new Vector2(0.9f, 0.9f), "Health: " + healthP);
                apply_Text(font_health2, new Vector2(1530, 948), Color.White, new Vector2(0.9f, 0.9f), "Health: " + healthC);
            }
            spriteBatch.End();
        }
    
        void apply_PictureAndText(Texture2D picture, Vector2 pic_position, SpriteFont font, Vector2 text_position, Color text_color, string text = null)
        {
            pic_position.X *= scaleFactor;
            pic_position.Y *= scaleFactor;
            text_position.X *= scaleFactor;
            text_position.Y *= scaleFactor;

            spriteBatch.Draw(picture, pic_position, null, colorDefault, 0f, Vector2.Zero, scale, spriteEffectDefault, 0f);
            if (text != null)
                spriteBatch.DrawString(font, text, text_position, text_color, 0f, Vector2.Zero, scale, spriteEffectDefault, 0f);
        }
        void apply_Picture(Texture2D picture, Vector2 pic_position, Color color, float rotation, Vector2 center_origin)
        {
            pic_position.X *= scaleFactor;
            pic_position.Y *= scaleFactor;
            spriteBatch.Draw(picture, pic_position, null, color, rotation, center_origin, scale, spriteEffectDefault, 0f);
        }
        void apply_Text(SpriteFont font, Vector2 text_position, Color text_color, Vector2 scale2, string text = null)
        {
            text_position.X *= scaleFactor;
            text_position.Y *= scaleFactor;

            if (text == null)
                text = " ";
            spriteBatch.DrawString(font, text, text_position, text_color, 0f, Vector2.Zero, (scale * scale2), spriteEffectDefault, 0f);
        }
        void apply_OnlyText(SpriteFont font, Vector2 text_position, Color text_color, string text = null)
        {
            text_position.X *= scaleFactor;
            text_position.Y *= scaleFactor;

            if (text == null)
                text = " ";
            spriteBatch.DrawString(font, text, text_position, text_color, 0f, Vector2.Zero, scale, spriteEffectDefault, 0f);
        }
        void apply_Text(SpriteFont font, Vector2 text_position, Color text_color, string text = null)
        {
            text_position.X *= scaleFactor;
            text_position.Y *= scaleFactor;

            if (text == null)
                text = " ";
            spriteBatch.DrawString(font, text, text_position, text_color, 0f, Vector2.Zero, (scale * Vector2.One), spriteEffectDefault, 0f);
        }
        void apply_OnlyPicture(Texture2D picture, Vector2 pic_position)
        {
            pic_position.X *= scaleFactor;
            pic_position.Y *= scaleFactor;
            spriteBatch.Draw(picture, pic_position, null, Color.White, 0f, Vector2.Zero, scale, spriteEffectDefault, 0f);
        }
        void apply_Picture(Texture2D picture, Vector2 pic_position)
        {
            pic_position.X *= scaleFactor;
            pic_position.Y *= scaleFactor;
            spriteBatch.Draw(picture, pic_position, null, colorDefault, 0f, Vector2.Zero, scale, spriteEffectDefault, 0f);
        }

        ObjectAnimation applyObjectDefaults(Vector3 startPosition, Vector3 endPosition, Vector3 startRotation, Vector3 endRotation, float duration, bool loop)
        {
            ObjectAnimation temp;
            temp = new ObjectAnimation(startPosition, endPosition, startRotation, endRotation, TimeSpan.FromSeconds(duration), loop);
            return temp;
        }
        CModel newCModel(string model, Vector3 position, Vector3 rotation, Vector3 scale, GraphicsDevice graphicsDevice)
        {
            CModel temp;
            temp = new CModel(Content.Load<Model>(model), position, rotation, scale, graphicsDevice);
            return temp;
        }
        public void transformBones(CModel robot, string Object, ObjectAnimation anim)
        {
            robot.Model.Meshes[Object].ParentBone.Transform =
                Matrix.CreateRotationY(anim.Rotation.Y) *
                Matrix.CreateRotationX(anim.Rotation.X) *
                Matrix.CreateRotationZ(anim.Rotation.Z) *
                Matrix.CreateTranslation(anim.Position);
        }

        void setResolution()
        {
            // Prebere loèljivost iz XML datoteke
            readFromXML(ref height, ref width, ref bpp, ref sound, ref fullScreen);

            // Potrdi spremembe za grafièno kartico
            graphics.PreferredBackBufferHeight = height;
            graphics.PreferredBackBufferWidth = width;
            if (bpp == 32)
                graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
            else
                graphics.PreferredBackBufferFormat = SurfaceFormat.Bgr565;
            graphics.IsFullScreen = fullScreen;

            switch (width)
            {
                case 1920: tmp4 = 3; break;
                case 1600: tmp4 = 2; break;
                case 1366: tmp4 = 1; break;
                case 1280: tmp4 = 0; break;
            }

            // Nastavi scaleFactor za slike 
            scaleFactor = (1920 / (float)width);
            scaleFactor = (float)(1 / scaleFactor);
            moved *= Convert.ToInt32(scaleFactor);
            scale.X = scaleFactor;
            scale.Y = scaleFactor;

            graphics.ApplyChanges();
        }
        void setObjectsDefaults()
        {
            headV = new Vector3(0, 23.5f, 0);
            left_armV = new Vector3(-3.3f, 20, 0);
            left_fistV = new Vector3(0, -0.05f, -6);
            right_armV = new Vector3(3.3f, 20, 0);
            right_fistV = new Vector3(0.2f, 0, -6);
            right_legV = new Vector3(2.2f, 13.4f, 0);
            right_kneeV = new Vector3(0, 0.3f, -6);
            right_footV = new Vector3(-0.1f, 0, -6.4f);
            left_legV = new Vector3(-2.2f, 13.4f, 0);
            left_kneeV = new Vector3(0, 0.3f, -6);
            left_footV = new Vector3(0.1f, 0, -6.4f);

            headR = new Vector3(0, 0, 0);
            left_armR = new Vector3(MathHelper.ToRadians(-10), MathHelper.ToRadians(30), MathHelper.ToRadians(53));
            left_fistR = new Vector3(MathHelper.ToRadians(14), MathHelper.ToRadians(116), MathHelper.ToRadians(180));
            right_armR = new Vector3(MathHelper.ToRadians(-42), MathHelper.ToRadians(-28), MathHelper.ToRadians(-10));
            right_fistR = new Vector3(MathHelper.ToRadians(180), MathHelper.ToRadians(-50), MathHelper.ToRadians(151));
            right_legR = new Vector3(-MathHelper.PiOver2, 0, 0);
            right_kneeR = new Vector3(0, 0, 0);
            right_footR = new Vector3(0, 0, 0);
            left_legR = new Vector3(-MathHelper.PiOver2, 0, 0);
            left_kneeR = new Vector3(0, 0, 0);
            left_footR = new Vector3(0, 0, 0);
        }
        void resetForNewGame()
        {
            healthP = 100;
            healthC = 100;

            punchP = false;
            defendP = false;
            punchC = false;
            defendC = false;
            change_camera = false;
            attack = false;
            defence = false;

            name = null;
            time = 0;
            walk_state = 4;

            timer_one = TimeSpan.Zero;
            timer_two = TimeSpan.Zero;
            timer_three = TimeSpan.Zero;
            timer_four = TimeSpan.Zero;
            timer_five = TimeSpan.Zero;
            timer_six = TimeSpan.Zero;

            rotChange_one = Vector3.Zero;
            rotChange_two = Vector3.Zero;

            this.IsMouseVisible = false;
        }
        void updateMouse()
        {
            current_mouse = Mouse.GetState();
            mouseX = current_mouse.X;
            mouseY = current_mouse.Y;
        }

        static void readFromXML(ref int h, ref int w, ref int b, ref bool s, ref bool f)
        {
            XmlTextReader textReader = new XmlTextReader(@"Content\graphic_settings.xml");

            textReader.Read();
            while (textReader.Read())
            {
                textReader.MoveToElement(); 
                switch (textReader.Name)
                {
                    case "Height":
                        h = textReader.ReadElementContentAsInt();
                        break;
                    case "Width":
                        w = textReader.ReadElementContentAsInt();
                        break;
                    case "BPP":
                        b = textReader.ReadElementContentAsInt();
                        break;
                    case "Sound":
                        if (textReader.ReadElementContentAsString() == "yes")
                            s = true;
                        else
                            s = false;
                        break;
                    case "Full-screen":
                        if (textReader.ReadElementContentAsString() == "no")
                            f = false;
                        else
                            f = true;
                        break;

                }
                textReader.MoveToNextAttribute();
            }
            textReader.Close();
        }
        void writeToXML(int h, int w, int b, bool s, bool f)
        {
            XmlTextWriter textWriter = new XmlTextWriter(@"Content/graphic_settings.xml", null); 
            textWriter.Formatting = Formatting.Indented;   
            textWriter.WriteStartDocument(); 
            textWriter.WriteComment("DEFAULT NASTAVITVE ZA IGRICO");  
            textWriter.WriteStartElement("Settings");  

            /* ZAPIŠI NASLEDNJE ELEMENTE */
            textWriter.WriteStartElement("Width");
            textWriter.WriteValue(w);
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("Height");
            textWriter.WriteValue(h);
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("BPP");
            textWriter.WriteValue(b);
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("Full-screen");
            if (f == true)
                textWriter.WriteString("yes");
            else
                textWriter.WriteString("no");
            textWriter.WriteEndElement();
            textWriter.WriteStartElement("Sound");
            if (s == true)
                textWriter.WriteString("yes");
            else
                textWriter.WriteString("no");
            textWriter.WriteEndElement();

            textWriter.WriteEndDocument(); 
            textWriter.Close(); 
        }
        void readScores_FromXML(string addname, float addtime)
        {
            XmlTextReader textReader = new XmlTextReader(@"Content\scores.xml");
            string[] name = new string[11];
            float[] time = new float[11];
            float t = 0;
            int i = 0;
            string s;

            textReader.Read();
            while (textReader.Read())
            {
                textReader.MoveToElement();
                switch (textReader.Name)
                {
                    case "name":
                        name[i] = textReader.ReadElementContentAsString();
                        break;
                    case "time":
                        time[i] = textReader.ReadElementContentAsFloat();
                        i++;
                        break;
                }
                textReader.MoveToNextAttribute();
            }
            textReader.Close();
            
            name[10] = addname;
            time[10] = addtime;

            /* BUBBLE SORT */
            for (i = 0; i < 11 - 1; i++)
                for (int j = 11 - 1; j > 0; j--)
                {
                    if ((time[j] < time[j - 1]) || (time[j - 1] == 0))
                    {
                        t = time[j];
                        s = name[j];

                        time[j] = time[j - 1];
                        name[j] = name[j - 1];

                        time[j - 1] = t;
                        name[j - 1] = s;
                    }
                }
            writeScores_ToXML(name, time);
        }
        void writeScores_ToXML(string[] name, float[] time)
        {
            XmlTextWriter textWriter = new XmlTextWriter(@"Content\scores.xml", null);
            textWriter.Formatting = Formatting.Indented;

            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("scores");
            for (int i = 0; i < 10; i++)
            {
                textWriter.WriteStartElement("name");
                textWriter.WriteValue(name[i]);
                textWriter.WriteEndElement();
                textWriter.WriteStartElement("time");
                textWriter.WriteValue(time[i]);
                textWriter.WriteEndElement();
            }
            textWriter.WriteEndDocument();
            textWriter.Close();
        }
        public void readXML_ForAnimations()
        {
            XmlTextReader textReader = new XmlTextReader(@"Content/animations.xml");
            float id = 0, rotx = 0, roty = 0, rotz = 0, time = 0, def = 0;

            textReader.Read();
            while (textReader.Read())
            {
                switch (textReader.Name)
                {
                    case "default":
                        def = textReader.ReadElementContentAsInt();
                        break;
                    case "rotx":
                        rotx = textReader.ReadElementContentAsFloat();
                        break;
                    case "roty":
                        roty = textReader.ReadElementContentAsFloat();
                        break;
                    case "rotz":
                        rotz = textReader.ReadElementContentAsFloat();
                        break;
                    case "time":
                        time = textReader.ReadElementContentAsFloat();
                        break;
                    case "listid":
                        id = textReader.ReadElementContentAsInt();
                        if (id == 1)
                        {
                            if (def == 0)
                                punch_left_arm.Add(new ObjectAnimationFrame(left_armV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                punch_left_arm.Add(new ObjectAnimationFrame(left_armV, left_armR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 2)
                        {
                            if (def == 0)
                                punch_left_fist.Add(new ObjectAnimationFrame(left_fistV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                punch_left_fist.Add(new ObjectAnimationFrame(left_fistV, left_fistR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 3)
                        {
                            if (def == 0)
                                punch_right_arm.Add(new ObjectAnimationFrame(right_armV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                punch_right_arm.Add(new ObjectAnimationFrame(right_armV, right_armR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 4)
                        {
                            if (def == 0)
                                punch_right_fist.Add(new ObjectAnimationFrame(right_fistV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                punch_right_fist.Add(new ObjectAnimationFrame(right_fistV, right_fistR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 5)
                        {
                            if (def == 0)
                                walk_right_legF.Add(new ObjectAnimationFrame(right_legV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                walk_right_legF.Add(new ObjectAnimationFrame(right_legV, right_legR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 6)
                        {
                            if (def == 0)
                                walk_right_kneeF.Add(new ObjectAnimationFrame(right_kneeV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                walk_right_kneeF.Add(new ObjectAnimationFrame(right_kneeV, right_kneeR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 7)
                        {
                            if (def == 0)
                                walk_right_footF.Add(new ObjectAnimationFrame(right_footV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                walk_right_footF.Add(new ObjectAnimationFrame(right_footV, right_footR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 8)
                        {
                            if (def == 0)
                                walk_left_legF.Add(new ObjectAnimationFrame(left_legV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                walk_left_legF.Add(new ObjectAnimationFrame(left_legV, left_legR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 9)
                        {
                            if (def == 0)
                                walk_left_kneeF.Add(new ObjectAnimationFrame(left_kneeV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                walk_left_kneeF.Add(new ObjectAnimationFrame(left_kneeV, left_kneeR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 10)
                        {
                            if (def == 0)
                                walk_left_footF.Add(new ObjectAnimationFrame(left_footV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                walk_left_footF.Add(new ObjectAnimationFrame(left_footV, left_footR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 11)
                        {
                            if (def == 0)
                                walk_right_legB.Add(new ObjectAnimationFrame(right_legV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                walk_right_legB.Add(new ObjectAnimationFrame(right_legV, right_legR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 12)
                        {
                            if (def == 0)
                                walk_right_kneeB.Add(new ObjectAnimationFrame(right_kneeV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                walk_right_kneeB.Add(new ObjectAnimationFrame(right_kneeV, right_kneeR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 13)
                        {
                            if (def == 0)
                                walk_right_footB.Add(new ObjectAnimationFrame(right_footV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                walk_right_footB.Add(new ObjectAnimationFrame(right_footV, right_footR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 14)
                        {
                            if (def == 0)
                                walk_left_legB.Add(new ObjectAnimationFrame(left_legV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                walk_left_legB.Add(new ObjectAnimationFrame(left_legV, left_legR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 15)
                        {
                            if (def == 0)
                                walk_left_kneeB.Add(new ObjectAnimationFrame(left_kneeV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                walk_left_kneeB.Add(new ObjectAnimationFrame(left_kneeV, left_kneeR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 16)
                        {
                            if (def == 0)
                                walk_left_footB.Add(new ObjectAnimationFrame(left_footV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                walk_left_footB.Add(new ObjectAnimationFrame(left_footV, left_footR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 17)
                        {
                            if (def == 0)
                                special_left_arm.Add(new ObjectAnimationFrame(left_armV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                special_left_arm.Add(new ObjectAnimationFrame(left_armV, left_armR, TimeSpan.FromSeconds(0)));
                        }
                        if (id == 18)
                        {
                            if (def == 0)
                                special_left_fist.Add(new ObjectAnimationFrame(left_fistV, new Vector3(MathHelper.ToRadians(rotx), MathHelper.ToRadians(roty), MathHelper.ToRadians(rotz)), TimeSpan.FromSeconds(time)));
                            else
                                special_left_fist.Add(new ObjectAnimationFrame(left_fistV, left_fistR, TimeSpan.FromSeconds(0)));
                        }
                        break;
                }
                textReader.MoveToNextAttribute();
            }
            textReader.Close();


            /* IGRALEC */
            punch_leftP[0] = new KeyframedObjectAnimation(punch_left_arm, true);
            punch_leftP[1] = new KeyframedObjectAnimation(punch_left_fist, true);
            punch_rightP[0] = new KeyframedObjectAnimation(punch_right_arm, false);
            punch_rightP[1] = new KeyframedObjectAnimation(punch_right_fist, false);
            special_punchP[0] = new KeyframedObjectAnimation(special_left_arm, false);
            special_punchP[1] = new KeyframedObjectAnimation(special_left_fist, false);

            walkFP[0] = new KeyframedObjectAnimation(walk_right_legF, false);
            walkFP[1] = new KeyframedObjectAnimation(walk_right_kneeF, false);
            walkFP[2] = new KeyframedObjectAnimation(walk_right_footF, false);
            walkFP[3] = new KeyframedObjectAnimation(walk_left_legF, false);
            walkFP[4] = new KeyframedObjectAnimation(walk_left_kneeF, false);
            walkFP[5] = new KeyframedObjectAnimation(walk_left_footF, false);

            walkBP[0] = new KeyframedObjectAnimation(walk_right_legB, false);
            walkBP[1] = new KeyframedObjectAnimation(walk_right_kneeB, false);
            walkBP[2] = new KeyframedObjectAnimation(walk_right_footB, false);
            walkBP[3] = new KeyframedObjectAnimation(walk_left_legB, false);
            walkBP[4] = new KeyframedObjectAnimation(walk_left_kneeB, false);
            walkBP[5] = new KeyframedObjectAnimation(walk_left_footB, false);


            /* RAÈUNALNIK */
            punch_leftC[0] = new KeyframedObjectAnimation(punch_left_arm, true);
            punch_leftC[1] = new KeyframedObjectAnimation(punch_left_fist, true);
            punch_rightC[0] = new KeyframedObjectAnimation(punch_right_arm, false);
            punch_rightC[1] = new KeyframedObjectAnimation(punch_right_fist, false);
            special_punchC[0] = new KeyframedObjectAnimation(special_left_arm, false);
            special_punchC[1] = new KeyframedObjectAnimation(special_left_fist, false);

            walkFC[0] = new KeyframedObjectAnimation(walk_right_legF, false);
            walkFC[1] = new KeyframedObjectAnimation(walk_right_kneeF, false);
            walkFC[2] = new KeyframedObjectAnimation(walk_right_footF, false);
            walkFC[3] = new KeyframedObjectAnimation(walk_left_legF, false);
            walkFC[4] = new KeyframedObjectAnimation(walk_left_kneeF, false);
            walkFC[5] = new KeyframedObjectAnimation(walk_left_footF, false);

            walkBC[0] = new KeyframedObjectAnimation(walk_right_legB, false);
            walkBC[1] = new KeyframedObjectAnimation(walk_right_kneeB, false);
            walkBC[2] = new KeyframedObjectAnimation(walk_right_footB, false);
            walkBC[3] = new KeyframedObjectAnimation(walk_left_legB, false);
            walkBC[4] = new KeyframedObjectAnimation(walk_left_kneeB, false);
            walkBC[5] = new KeyframedObjectAnimation(walk_left_footB, false);
        }



        // Predvajanje Intro videa
        public void introVideo()
        {
            vidPlayer.Play(vid);
            vidTexture = vidPlayer.GetTexture();

            spriteBatch.Begin();
            spriteBatch.Draw(vidTexture, vidRectangle, Color.White);
            spriteBatch.End();

            if (vidPlayer.PlayPosition > (vid.Duration - TimeSpan.FromSeconds(0.5f)))
            {
                vidPlayer.IsLooped = false;
                vidPlayer.Stop();
                if(sound)
                    MediaPlayer.Play(soundACDC);
                game_state = gameState.Menu;
            }
        }

        // Glavni menu funkcija
        public void Meni(GameTime gameTime)
        {
            spriteBatch.Begin();
            {
                apply_Picture(back, Vector2.Zero);
                if (mouseX > (menuButtons0x * scaleFactor) && mouseX < ((menuButtons0x + 535) * scaleFactor) && mouseY > (menuButtons0y * scaleFactor) && mouseY < ((menuButtons0y + 60) * scaleFactor))
                {
                    apply_PictureAndText(menuButtons[0], new Vector2((menuButtons0x - 50), (menuButtons0y)), font_menu, new Vector2((menuButtons0x + 100 - moved), (menuButtons0y)), colorDefault, "NOVA IGRA");
                    if (current_mouse.LeftButton == ButtonState.Pressed)
                        game_state = gameState.NewGame_menu;
                }
                else
                    apply_PictureAndText(menuButtons[0], new Vector2((menuButtons0x), (menuButtons0y)), font_menu, new Vector2((menuButtons0x + 100), (menuButtons0y)), colorDefault, "NOVA IGRA");

                if (mouseX > ((menuButtons0x + 50) * scaleFactor) && mouseX < (((menuButtons0x + 50) * scaleFactor) + (495 * scaleFactor)) && mouseY > ((menuButtons0y + 80) * scaleFactor) && mouseY < ((menuButtons0y + 80 + 60) * scaleFactor))
                {
                    apply_PictureAndText(menuButtons[1], new Vector2((menuButtons0x + 50 - moved), (menuButtons0y + 80)), font_menu, new Vector2((menuButtons0x + 150 - moved), (menuButtons0y + 80)), colorDefault, "NASTAVITVE");
                    if (current_mouse.LeftButton == ButtonState.Pressed)
                        game_state = gameState.Settings_menu;
                }
                else
                    apply_PictureAndText(menuButtons[1], new Vector2((menuButtons0x + 50), (menuButtons0y + 80)), font_menu, new Vector2((menuButtons0x + 150), (menuButtons0y + 80)), colorDefault, "NASTAVITVE");


                if (mouseX > ((menuButtons0x + 70) * scaleFactor) && mouseX < (((menuButtons0x + 70) * scaleFactor) + (480 * scaleFactor)) && mouseY > ((menuButtons0y + 160) * scaleFactor) && mouseY < ((menuButtons0y + 160 + 60) * scaleFactor))
                {
                    apply_PictureAndText(menuButtons[2], new Vector2((menuButtons0x + 70 - moved), (menuButtons0y + 160)), font_menu, new Vector2((menuButtons0x + 170 - moved), (menuButtons0y + 160)), colorDefault, "VIZITKA");
                    if (current_mouse.LeftButton == ButtonState.Pressed)
                        game_state = gameState.Credits_menu;
                }
                else
                    apply_PictureAndText(menuButtons[2], new Vector2((menuButtons0x + 70), (menuButtons0y + 160)), font_menu, new Vector2((menuButtons0x + 170), (menuButtons0y + 160)), colorDefault, "VIZITKA");

                if (mouseX > ((menuButtons0x + 90) * scaleFactor) && mouseX < (((menuButtons0x + 90) * scaleFactor) + (460 * scaleFactor)) && mouseY > ((menuButtons0y + 240) * scaleFactor) && mouseY < ((menuButtons0y + 240 + 60) * scaleFactor))
                {
                    apply_PictureAndText(menuButtons[3], new Vector2((menuButtons0x + 90 - moved), (menuButtons0y + 240)), font_menu, new Vector2((menuButtons0x + 190 - moved), (menuButtons0y + 240)), colorDefault, "IZHOD");
                    if (current_mouse.LeftButton == ButtonState.Pressed)
                        Exit();
                }
                else
                    apply_PictureAndText(menuButtons[3], new Vector2((menuButtons0x + 90), (menuButtons0y + 240)), font_menu, new Vector2((menuButtons0x + 190), (menuButtons0y + 240)), colorDefault, "IZHOD");
            }
            apply_Picture(front, Vector2.Zero);
            spriteBatch.End();


            ResetGraphic();
            BeginRender3D();
            statue.Rotation = new Vector3(0, -(float)(gameTime.TotalGameTime.TotalSeconds * 0.5), 0);
            statue.Draw(camera_menu.View, camera_menu.Projection);
        }

        // Menu za nastavitve
        void Settings_menu()
        {
            text = Convert.ToString(choose_res[tmp4, 0]) + "x" + Convert.ToString(choose_res[tmp4, 1]);

            spriteBatch.Begin();
            {
                apply_Picture(back, Vector2.Zero);
                if (mouseX > (1270 * scaleFactor) && mouseX < ((1270 * scaleFactor) + (480 * scaleFactor)) && mouseY > (800 * scaleFactor) && mouseY < ((800 + 60) * scaleFactor))
                {
                    apply_PictureAndText(regularButton, new Vector2(1270 - moved, 800), font_menu, new Vector2(1370 - moved, 800), colorDefault, "SHRANI");
                    if (current_mouse.LeftButton == ButtonState.Pressed)
                    {
                        if (!sound)
                            MediaPlayer.Stop();
                        else
                        {
                            MediaPlayer.Stop();
                            MediaPlayer.Play(soundACDC);
                        }
                        writeToXML(choose_res[tmp4, 1], choose_res[tmp4, 0], tmp3, tmp1, tmp2);
                        setResolution();
                        Thread.Sleep(120);
                        game_state = gameState.Menu;
                    }
                }
                else
                    apply_PictureAndText(regularButton, new Vector2(1270, 800), font_menu, new Vector2(1370, 800), colorDefault, "SHRANI");
                if (mouseX > (1290 * scaleFactor) && mouseX < ((1290 * scaleFactor) + (460 * scaleFactor)) && mouseY > (880 * scaleFactor) && mouseY < ((880 + 60) * scaleFactor))
                {
                    apply_PictureAndText(regularButton, new Vector2(1290 - moved, 880), font_menu, new Vector2(1390 - moved, 880), colorDefault, "NAZAJ");
                    if (current_mouse.LeftButton == ButtonState.Pressed)
                    {
                        Thread.Sleep(120);
                        game_state = gameState.Menu;
                    }
                }
                else
                    apply_PictureAndText(regularButton, new Vector2(1290, 880), font_menu, new Vector2(1390, 880), colorDefault, "NAZAJ");


                apply_Picture(front_settings, Vector2.Zero);
                if (mouseX > (1000 * scaleFactor) && mouseX < ((980 + off_left.Width) * scaleFactor) && mouseY > (409 * scaleFactor) && mouseY < ((407 + off_left.Height) * scaleFactor))
                {
                    apply_PictureAndText(on_left, new Vector2(1000, 414), font_menu, new Vector2((float)(1000 + off_left.Width * 0.30), 405), myGreen, "16");
                    if (current_mouse.LeftButton == ButtonState.Pressed && tmp3 == 32)
                        tmp3 = 16;
                }
                else
                {
                    if (tmp3 == 16)
                        apply_PictureAndText(on_left, new Vector2(1000, 414), font_menu, new Vector2((float)(1000 + on_left.Width * 0.30), 405), myGreen, "16");
                    else
                        apply_PictureAndText(off_left, new Vector2(1000, 414), font_menu, new Vector2((float)(1000 + off_left.Width * 0.30), 405), Color.White, "16");
                }
                if (mouseX > (1220 * scaleFactor) && mouseX < ((1220 + off_right.Width) * scaleFactor) && mouseY > (409 * scaleFactor) && mouseY < ((407 + off_right.Height) * scaleFactor))
                {
                    apply_PictureAndText(on_right, new Vector2(1200, 414), font_menu, new Vector2((float)(1200 + on_right.Width * 0.40), 405), myGreen, "32");
                    if (current_mouse.LeftButton == ButtonState.Pressed && tmp3 == 16)
                        tmp3 = 32;
                }
                else
                {
                    if (tmp3 == 32)
                        apply_PictureAndText(on_right, new Vector2(1200, 414), font_menu, new Vector2((float)(1200 + on_right.Width * 0.40), 405), myGreen, "32");
                    else
                        apply_PictureAndText(off_right, new Vector2(1200, 414), font_menu, new Vector2((float)(1200 + on_right.Width * 0.40), 405), Color.White, "32");
                }


                if (mouseX > (1000 * scaleFactor) && mouseX < ((980 + off_left.Width) * scaleFactor) && mouseY > (585 * scaleFactor) && mouseY < ((585 + off_left.Height) * scaleFactor))
                {
                    apply_PictureAndText(on_left, new Vector2(1000, 590), font_menu, new Vector2((float)(1000 + off_left.Width * 0.30), 580), myGreen, "DA");
                    if (current_mouse.LeftButton == ButtonState.Pressed && tmp1 == false)
                    {
                        sound = true;
                        tmp1 = true;
                    }
                }
                else
                {
                    if (tmp1 == true)
                        apply_PictureAndText(on_left, new Vector2(1000, 590), font_menu, new Vector2((float)(1000 + on_left.Width * 0.30), 580), myGreen, "DA");
                    else
                        apply_PictureAndText(off_left, new Vector2(1000, 590), font_menu, new Vector2((float)(1000 + off_left.Width * 0.30), 580), Color.White, "DA");
                }
                if (mouseX > (1220 * scaleFactor) && mouseX < ((1200 + off_right.Width) * scaleFactor) && mouseY > (585 * scaleFactor) && mouseY < ((585 + off_right.Height) * scaleFactor))
                {
                    apply_PictureAndText(on_right, new Vector2(1200, 590), font_menu, new Vector2((float)(1200 + on_right.Width * 0.40), 580), myGreen, "NE");
                    if (current_mouse.LeftButton == ButtonState.Pressed && tmp1 == true)
                    {
                        tmp1 = false;
                        sound = false;
                    }
                }
                else
                {
                    if (tmp1 == false)
                        apply_PictureAndText(on_right, new Vector2(1200, 590), font_menu, new Vector2((float)(1200 + on_right.Width * 0.40), 580), myGreen, "NE");
                    else
                        apply_PictureAndText(off_right, new Vector2(1200, 590), font_menu, new Vector2((float)(1200 + on_right.Width * 0.40), 580), Color.White, "NE");
                }


                if (mouseX > (1000 * scaleFactor) && mouseX < ((980 + off_left.Width) * scaleFactor) && mouseY > (497 * scaleFactor) && mouseY < ((495 + off_left.Height) * scaleFactor))
                {
                    apply_PictureAndText(on_left, new Vector2(1000, 502), font_menu, new Vector2((float)(1000 + off_left.Width * 0.30), 492), myGreen, "DA");
                    if (current_mouse.LeftButton == ButtonState.Pressed && tmp2 == false)
                        tmp2 = true;
                }
                else
                {
                    if (tmp2 == true)
                        apply_PictureAndText(on_left, new Vector2(1000, 502), font_menu, new Vector2((float)(1000 + off_left.Width * 0.30), 492), myGreen, "DA");
                    else
                        apply_PictureAndText(off_left, new Vector2(1000, 502), font_menu, new Vector2((float)(1000 + off_left.Width * 0.30), 492), Color.White, "DA");
                }
                if (mouseX > (1220 * scaleFactor) && mouseX < ((1200 + off_right.Width) * scaleFactor) && mouseY > (497 * scaleFactor) && mouseY < ((495 + off_right.Height) * scaleFactor))
                {
                    apply_PictureAndText(on_right, new Vector2(1200, 502), font_menu, new Vector2((float)(1200 + off_left.Width * 0.40), 492), myGreen, "NE");
                    if (current_mouse.LeftButton == ButtonState.Pressed && tmp2 == true)
                        tmp2 = false;
                }
                else
                {
                    if (tmp2 == false)
                        apply_PictureAndText(on_right, new Vector2(1200, 502), font_menu, new Vector2((float)(1200 + off_left.Width * 0.40), 492), myGreen, "NE");
                    else
                        apply_PictureAndText(off_right, new Vector2(1200, 502), font_menu, new Vector2((float)(1200 + off_left.Width * 0.40), 492), Color.White, "NE");
                }

                apply_Text(font_settings, new Vector2(1100, 335), colorDefault, text);
                apply_Picture(left, new Vector2(1060, 335));
                apply_Picture(right, new Vector2(1300, 335));
                if (mouseX > (1060 * scaleFactor) && mouseX < ((1060 + left.Width) * scaleFactor) && mouseY > (335 * scaleFactor) && mouseY < ((335 + left.Height) * scaleFactor))
                    if (current_mouse.LeftButton == ButtonState.Pressed)
                        if (tmp4 < 3)
                        {
                            tmp4++;
                            Thread.Sleep(120);
                        }
                if (mouseX > (1300 * scaleFactor) && mouseX < ((1300 + left.Width) * scaleFactor) && mouseY > (335 * scaleFactor) && mouseY < ((335 + left.Height) * scaleFactor))
                    if (current_mouse.LeftButton == ButtonState.Pressed)
                        if (tmp4 > 0)
                        {
                            tmp4--;
                            Thread.Sleep(120);
                        }
            }

            apply_Text(font_settings, new Vector2(510, 335), colorDefault, "RESOLUCIJA:");
            apply_Text(font_settings, new Vector2(510, 418), colorDefault, "BARVNA GLOBINA:");
            apply_Text(font_settings, new Vector2(510, 505), colorDefault, "CELOZASLONSKI NACIN:");
            apply_Text(font_settings, new Vector2(510, 590), colorDefault, "ZVOK:");
            spriteBatch.End();
        }

        // Menu za credits
        void Credits_menu()
        {
            spriteBatch.Begin();
            {
                apply_Picture(back, Vector2.Zero);
                if (mouseX > (1290 * scaleFactor) && mouseX < ((1290 * scaleFactor) + (460 * scaleFactor)) && mouseY > (880 * scaleFactor) && mouseY < ((880 + 60) * scaleFactor))
                {
                    apply_PictureAndText(regularButton, new Vector2(1290 - moved, 880), font_menu, new Vector2(1390 - moved, 880), colorDefault, "NAZAJ");
                    if (current_mouse.LeftButton == ButtonState.Pressed)
                        game_state = gameState.Menu;
                }
                else
                    apply_PictureAndText(regularButton, new Vector2(1290, 880), font_menu, new Vector2(1390, 880), colorDefault, "NAZAJ");

                apply_Picture(front_credits, Vector2.Zero);
                apply_Text(font_settings, new Vector2(300, 335), Color.Gray, "Igra je koncni izdelek za 4. predmet iz mature. Narejena je v programskem\njeziku C# in uporablja graficni vmesnik XNA. Ozadja, texture in slike so bile ustvarjene\nv programski opremi Adobe Photoshop. 3D Modeli v igri so bili izdelani v programski\nopremi Autodesk Maya, texture pa so bile dodelane v programu Adobe Photoshop.\n\n\nLuka Prijatelj R4A (2012/2013)");
            }
            spriteBatch.End();
        }

        // Menu za novo igro
        void new_Game_menu()
        {
            spriteBatch.Begin();
            apply_Picture(back, Vector2.Zero);
            if (mouseX > (1270 * scaleFactor) && mouseX < ((1270 * scaleFactor) + (480 * scaleFactor)) && mouseY > (720 * scaleFactor) && mouseY < ((720 + 60) * scaleFactor))
            {
                apply_PictureAndText(regularButton, new Vector2(1270 - moved, 720), font_menu, new Vector2(1370 - moved, 720), colorDefault, "IGRAJ");
                if (current_mouse.LeftButton == ButtonState.Pressed)
                {
                    resetForNewGame();
                    player.Position = new Vector3(1500, 0, -1500);
                    player.Rotation = new Vector3(0, MathHelper.ToRadians(135), 0);
                    computer.Position = new Vector3(-1500, 0, 1500);
                    computer.Rotation = new Vector3(0, -MathHelper.ToRadians(45), 0);
                    if (sound)
                        MediaPlayer.Stop();
                    game_state = gameState.Game;
                }
            }
            else
                apply_PictureAndText(regularButton, new Vector2(1270, 720), font_menu, new Vector2(1370, 720), colorDefault, "IGRAJ");

            if (mouseX > (1270 * scaleFactor) && mouseX < ((1270 * scaleFactor) + (480 * scaleFactor)) && mouseY > (800 * scaleFactor) && mouseY < ((800 + 60) * scaleFactor))
            {
                apply_PictureAndText(regularButton, new Vector2(1270 - moved, 800), font_menu, new Vector2(1370 - moved, 800), colorDefault, "REZULTATI");
                if (current_mouse.LeftButton == ButtonState.Pressed)
                {
                    game_state = gameState.Scores;
                    Thread.Sleep(200);
                }
            }
            else
                apply_PictureAndText(regularButton, new Vector2(1270, 800), font_menu, new Vector2(1370, 800), colorDefault, "REZULTATI");
            if (mouseX > (1290 * scaleFactor) && mouseX < ((1290 * scaleFactor) + (460 * scaleFactor)) && mouseY > (880 * scaleFactor) && mouseY < ((880 + 60) * scaleFactor))
            {
                apply_PictureAndText(regularButton, new Vector2(1290 - moved, 880), font_menu, new Vector2(1390 - moved, 880), colorDefault, "NAZAJ");
                if (current_mouse.LeftButton == ButtonState.Pressed)
                    game_state = gameState.Menu;
            }
            else
                apply_PictureAndText(regularButton, new Vector2(1290, 880), font_menu, new Vector2(1390, 880), colorDefault, "NAZAJ");
            apply_Picture(new_game, Vector2.Zero);
            
            spriteBatch.End();
        }

        // Menu za rezultate
        void Scores_menu()
        {
            XmlTextReader textReader = new XmlTextReader(@"Content\scores.xml");
            string[] name = new string[11];
            float[] time = new float[11];
            int i = 0, j = 1;
            string text;

            textReader.Read();
            while (textReader.Read())
            {
                textReader.MoveToElement();
                switch (textReader.Name)
                {
                    case "name":
                        name[i] = textReader.ReadElementContentAsString();
                        break;
                    case "time":
                        time[i] = textReader.ReadElementContentAsFloat();
                        i++;
                        break;
                }
                textReader.MoveToNextAttribute();
            }
            textReader.Close();

            spriteBatch.Begin();
            {
                apply_Picture(back, Vector2.Zero);
                apply_Text(font_scores, new Vector2(500, 200), Color.Gray, "St.    Ime        Cas");

                for (i = 0; i < 10; i++)
                {
                    text = j + ".   " + name[i] + " : " + time[i];
                    if (time[i] != 0)
                    {
                        apply_Text(font_scores, new Vector2(500, 200 + (70 * j)), Color.Gray, text);
                        j++;
                    }
                }

                if (mouseX > (1270 * scaleFactor) && mouseX < ((1270 * scaleFactor) + (480 * scaleFactor)) && mouseY > (800 * scaleFactor) && mouseY < ((800 + 60) * scaleFactor))
                {
                    apply_PictureAndText(regularButton, new Vector2(1270 - moved, 800), font_menu, new Vector2(1370 - moved, 800), colorDefault, "IZBRIS");
                    if (current_mouse.LeftButton == ButtonState.Pressed)
                    {
                        writeScores_ToXML(new string[11], new float[11]);
                        Thread.Sleep(200);
                    }
                }
                else
                    apply_PictureAndText(regularButton, new Vector2(1270, 800), font_menu, new Vector2(1370, 800), colorDefault, "IZBRIS");

                if (mouseX > (1290 * scaleFactor) && mouseX < ((1290 * scaleFactor) + (460 * scaleFactor)) && mouseY > (880 * scaleFactor) && mouseY < ((880 + 60) * scaleFactor))
                {
                    apply_PictureAndText(regularButton, new Vector2(1290 - moved, 880), font_menu, new Vector2(1390 - moved, 880), colorDefault, "NAZAJ");
                    if (current_mouse.LeftButton == ButtonState.Pressed)
                    {
                        game_state = gameState.NewGame_menu;
                        Thread.Sleep(200);
                    }
                }
                else
                    apply_PictureAndText(regularButton, new Vector2(1290, 880), font_menu, new Vector2(1390, 880), colorDefault, "NAZAJ");
                apply_Picture(scores, Vector2.Zero);
            }
            spriteBatch.End();
        }

        // GLAVNA IGRA - IGRANJE
        void mainGame(GameTime gameTime)
        {
            if (healthC <= 0)
            {
                timer_five += gameTime.ElapsedGameTime;

                /* Vpisovanje imena zmagovalca */
                spriteBatch.Begin();
                if (timer_five > TimeSpan.FromSeconds(3))
                {
                    apply_OnlyPicture(name_score, Vector2.Zero);
                    if (name == null)
                    {
                        apply_Text(font_menu, new Vector2(750, 550), Color.White, new Vector2(0.9f, 0.9f), "(tvoje ime)");
                        temporary = Keyboard.GetState().GetPressedKeys();
                        if (temporary.Length != 0 && Convert.ToChar(temporary[0]) >= 'A' && Convert.ToChar(temporary[0]) <= 'Z')
                            name += Convert.ToString(temporary[0]);
                        Thread.Sleep(120);
                    }
                    else
                    {
                        temporary = Keyboard.GetState().GetPressedKeys();
                        if (temporary.Length != 0)
                        {
                            if (temporary[0] == Keys.Space && name.Length < 16)
                                name += " ";
                            else if (temporary[0] == Keys.Back)
                            {
                                temporar = string.Empty;
                                int b = 0;
                                foreach (char a in name)
                                {
                                    if (b < name.Length - 1)
                                        temporar += a;
                                    b++;
                                }
                                name = temporar;
                            }
                            else if (temporary[0] == Keys.Enter)
                            {
                                if (sound)
                                    MediaPlayer.Play(soundACDC);
                                this.IsMouseVisible = true;
                                readScores_FromXML(name, time);
                                game_state = gameState.Menu;
                            }
                            else if (name.Length < 16 && Convert.ToChar(temporary[0]) >= 'A' && Convert.ToChar(temporary[0]) <= 'Z')
                                name += Convert.ToString(temporary[0]);
                            Thread.Sleep(120);
                        }

                        apply_Text(font_menu, new Vector2(745, 550), Color.White, new Vector2(0.9f, 0.9f), name);
                        temporary = null;
                    }
                }
                else
                    apply_OnlyPicture(victory, Vector2.Zero);
                spriteBatch.End();
            }
            else if (healthP <= 0)
            {
                timer_five += gameTime.ElapsedGameTime;
                if (timer_five < TimeSpan.FromSeconds(3))
                {
                    spriteBatch.Begin();
                    apply_OnlyPicture(lost, Vector2.Zero);
                    spriteBatch.End();
                }
                else
                {
                    this.IsMouseVisible = true;
                    if(sound)
                        MediaPlayer.Play(soundACDC);
                    game_state = gameState.Menu;
                }
            }
            else
            {
                if (healthC < 0)
                    healthC = 0;
                if (healthP < 0)
                    healthP = 0;

                if (change_camera)
                {
                    // Izris 3D objektov in igralcev
                    player.Draw(camera_first.View, camera_first.Projection);
                    computer.Draw(camera_first.View, camera_first.Projection);

                    ring.Scale = new Vector3(200, 200, 200);
                    ring.Draw(camera_first.View, camera_first.Projection);

                    stadion.Scale = new Vector3(200, 200, 200);
                    stadion.Draw(camera_first.View, camera_first.Projection);

                    // Prikaz pogleda iz èelade igralca
                    jarvis_view(gameTime);
                }
                else
                {
                    // Izris 3D objektov in igralcev
                    player.Draw(camera_backward.View, camera_backward.Projection);
                    computer.Draw(camera_backward.View, camera_backward.Projection);

                    ring.Scale = new Vector3(200, 200, 200);
                    ring.Draw(camera_backward.View, camera_backward.Projection);

                    stadion.Scale = new Vector3(200, 200, 200);
                    stadion.Draw(camera_backward.View, camera_backward.Projection);

                    // Prikaz zdravja nasprotnika
                    health_all_function();
                }
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

    } // Konec razreda Game()
} // Konec Namespace: SpopadTitanov
