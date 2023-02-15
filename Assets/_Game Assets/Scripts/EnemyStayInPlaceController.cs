using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStayInPlaceController : MonoBehaviour
{
    [SerializeField] Transform m_bulletSpawn;

    [SerializeField] GunController m_gunPrefab;
    [SerializeField] Transform m_gunParent;
    GunController m_currentGun;

    [SerializeField] int m_maxHealth = 5;

    public bool isAlive { get; private set; } = true;
    public int currentHealth { get; private set; }

    private void OnEnable()
    {
        _SetupIsAlive(true);
        _SetupGun();
    }

    private void Update()
    {
        PlayerController player = PlayerController.Instance;
        Vector3 dir = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Euler(0.0f, rotation.eulerAngles.y, 0.0f);

        m_currentGun.Fire(m_bulletSpawn);
    }

    public void HitByBullet(BulletController bullet)
    {
        currentHealth = Mathf.Max(0, currentHealth - 1);
        if (currentHealth <= 0)
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

    private void _SetupGun()
    {
        if (!m_gunPrefab)
            return;

        m_currentGun = Instantiate(m_gunPrefab);
        m_currentGun.PickUp(m_gunParent);
    }
}
