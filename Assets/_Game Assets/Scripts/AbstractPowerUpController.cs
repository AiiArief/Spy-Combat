using System.Collections;
using UnityEngine;

namespace SpyCombat.Gameplay
{
    public abstract class AbstractPowerUpController : MonoBehaviour
    {
        [SerializeField]
        protected Collider m_collider;

        #region Unity's Callback
        public override int GetHashCode()
        {
            return GetType().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            return true;
        }
        #endregion

        public abstract void PickUp(EntityController_Player player);
    }
}