using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class WavePooler : AbstractObjectPooler<AbstractEntityDynamicController>
    {
        public static WavePooler Instance { get; private set; }

        protected override void Awake()
        {
            Instance = this;

            base.Awake();
        }
    }
}