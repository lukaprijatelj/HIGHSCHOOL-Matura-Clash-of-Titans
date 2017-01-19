using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace SpopadTitanov
{
    public class AnimPlayer
    {
        string[] Objects = { "Head", "Left_arm", "Left_fist", "Right_arm", "Right_fist", "Right_leg", "Right_knee", "Right_foot", "Left_leg", "Left_knee", "Left_foot" };        
        public GameTime gameTime { get; private set; }
        public int clip { get; set; }
        public bool endPlaying;
        public bool freez;


        public AnimPlayer()
        {
            this.endPlaying = false;
            freez = false;
        }

        /// <summary>
        /// Predvaja animacije povezane na predmete
        /// </summary>
        public void Play(int _object, int id,  KeyframedObjectAnimation[] KOA, CModel player, GameTime gameTime)
        {
            this.gameTime = gameTime;
            if (KOA[id].endAnimation)
                endPlaying = true;
            KOA[id].freeze = this.freez;

            KOA[id].Update(gameTime.ElapsedGameTime);
            transformKeyFramedBones(player, Objects[_object], KOA[id]);
            this.clip = KOA[id].clip;
        }

        public void ResetPlayer()
        {
            this.endPlaying = false;
            this.clip = 0;
        }

        void transformKeyFramedBones(CModel robot, string Object, KeyframedObjectAnimation anim)
        {
            robot.Model.Meshes[Object].ParentBone.Transform =
                Matrix.CreateRotationY(anim.Rotation.Y) *
                Matrix.CreateRotationX(anim.Rotation.X) *
                Matrix.CreateRotationZ(anim.Rotation.Z) *
                Matrix.CreateTranslation(anim.Position);
        }
    }
}
