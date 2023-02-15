using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] CharacterController m_characterController;
    [SerializeField] Animator m_animState;
    [SerializeField] Transform m_bulletSpawn;

    [Header("Movement")]
    [Tooltip("Player's move speed (walking)")]
    public float speed = 3f;

    [Tooltip("Speed multiplier when sprinting")]
    public float sprintMultiplier = 2.0f;

    [SerializeField] Transform m_gunParent;
    GunController m_currentGun;

    #region Unity's Callback
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _HandlePlayerFireInput();
        _HandlePlayerMovementInput();
        _HandlePlayerLookInput();
    }

    // todo : ganti ke taro di update aja
    private void OnTriggerStay(Collider other)
    {
        bool interact = Input.GetButtonDown("Interact");
        var gun = other.GetComponent<GunController>();
        if (interact && gun)
        {
            if (m_currentGun == null)
            {
                m_currentGun = gun;
                gun.PickUp(m_gunParent);
            } else
            {
                m_currentGun.Drop(new Vector3(transform.position.x, 0.0f, transform.position.z));

                m_currentGun = gun;
                gun.PickUp(m_gunParent);
            }
        }
    }
    #endregion

    private void _HandlePlayerFireInput()
    {
        bool fire = Input.GetButtonDown("Fire1");
        if (fire && m_currentGun)
        {
            m_currentGun.Fire(m_bulletSpawn);
        }
    }

    private void _HandlePlayerMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool sprint = Input.GetButton("Sprint") && ((m_currentGun) ? !m_currentGun.isFiring : true);

        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;
        float currentSprintMultiplier = (sprint) ? sprintMultiplier : 1.0f;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngleMove = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Vector3 movedir = Quaternion.Euler(0f, targetAngleMove, 0f) * Vector3.forward;
            m_characterController.Move(movedir.normalized * speed * currentSprintMultiplier * Time.deltaTime);
        }
    }

    private void _HandlePlayerLookInput()
    {
        Vector2 playerPos = Camera.main.WorldToViewportPoint(transform.position);
        Vector2 mousePosOnScreen = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector2 rotDir = mousePosOnScreen - playerPos;
        float targetAngleRot = Mathf.Atan2(rotDir.x, rotDir.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0.0f, targetAngleRot, 0.0f);
    }
}
