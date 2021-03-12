using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Constants
{
    public class AnimationParameters
    {
        /// <summary>
        /// Offset for blinking animation (to prevent all characters blink at the same time)
        /// </summary>
        public const string Par_BlinkingOffset = "BlinkingOffset";
        public const string Par_IsWalking = "IsWalking";
        /// <summary>
        /// is actor facing forward?
        /// </summary>
        public const string Par_IsForward = "IsForward";
        /// <summary>
        /// is actor dead?
        /// </summary>
        public const string Par_Dead = "Dead";
        /// <summary>
        /// parameter for surprise face expression
        /// </summary>
        public const string Par_IsSurprised = "IsSurprised";

        /// <summary>
        /// trigger for attacking with a jab
        /// </summary>
        public const string Trig_Jab = "Jab";
        /// <summary>
        /// trigger for got hit in the head
        /// </summary>
        public const string Trig_HeadHit = "HeadHit";
        /// <summary>
        /// trigger for bow down
        /// </summary>
        public const string Trig_FormalBow = "FormalBow";
        public const string Trig_ShakeHeadNo = "ShakeHeadNo";
        public const string Trig_ShakeHeadYes = "ShakeHeadYes";
        public const string Trig_PointBehind = "Point";
        /// <summary>
        /// trigger for yell gestures
        /// </summary>
        public const string Trig_Yell = "Yell";
    }
}