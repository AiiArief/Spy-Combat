using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public abstract class AbstractPowerUpController : MonoBehaviour // ipickupable?
    {
        public abstract void PickUp(EntityController_Player player);
    }
}