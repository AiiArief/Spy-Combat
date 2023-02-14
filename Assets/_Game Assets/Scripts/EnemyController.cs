using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int m_maxHealth = 5;

    public bool isAlive { get; private set; } = true;
    public int currentHealth { get; private set; }

    private void OnEnable()
    {
        _SetupIsAlive(true);
    }

    public void HitByBullet(BulletController bullet)
    {
        currentHealth = Mathf.Max(0, currentHealth - 1);
        if(currentHealth <= 0)
        {
            _SetupIsAlive(false);
        }
    }

    private void _SetupIsAlive(bool isAlive)
    {
        this.isAlive = isAlive;
        gameObject.SetActive(isAlive);
        currentHealth = (isAlive) ? m_maxHealth : 0;
    }
}
