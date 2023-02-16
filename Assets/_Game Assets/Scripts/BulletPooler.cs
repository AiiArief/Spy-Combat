using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class BulletPooler : AbstractObjectPooler<BulletController>
    {
        public static BulletPooler Instance { get; private set; }

        protected override void Awake()
        {
            Instance = this;

            base.Awake();
        }
    }
}