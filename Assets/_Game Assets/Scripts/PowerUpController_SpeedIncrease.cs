using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public class PowerUpController_SpeedIncrease : AbstractPowerUpController
    {
        public static int MAX_SPEED_INCREASE = 5;

        public override void PickUp(EntityController_Player player)
        {
            player.speed = Mathf.Min(MAX_SPEED_INCREASE, player.speed + 1);
            Destroy(gameObject);
        }
    }
}