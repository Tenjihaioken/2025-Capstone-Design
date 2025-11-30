using UnityEngine;
using System.Collections;

public enum PlayerWeaponType { KNIFE, PISTOL, NULL }

public class PlayerBehavior : MonoBehaviour
{
    Rigidbody myRigidBody;
    public float moveSpeed = 5.0f;
    public Transform hitTestPivot, gunPivot;
    public GameObject mousePointer, proyectilePrefab;
    public Animator animator;
    int hashSpeed;
    float attackTime = 0.4f;
    PlayerWeaponType currentWeapon = PlayerWeaponType.NULL;
    Misc_Timer attackTimer = new Misc_Timer();

    // 🩸 HP 시스템 ----------------------------
    [Header("Player Health Settings")]
    public float maxHP = 100f;
    private float currentHP;
    public UnityEngine.UI.Slider hpBar;
    //---------------------------------------

    // 🔫 탄약 시스템 --------------------------
    [Header("Player Ammo Settings")]
    public int maxAmmo = 30;
    private int currentAmmo;
    public UnityEngine.UI.Text ammoText;
    public UnityEngine.UI.Text ammoTextbg;
    //---------------------------------------

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip pistolFireClip;
    public AudioClip KnifeClip;
    public AudioClip ammoPickupClip;

    [Header("Footstep Settings")]
    public AudioClip footstepClip;       // 발자국 사운드
    public float footstepInterval = 0.4f; // 이동 속도에 따른 간격
    private float footstepTimer = 0f;     // 타이머

    void Awake() { }

    void Start()
    {
        SetWeapon(PlayerWeaponType.KNIFE);
        myRigidBody = GetComponent<Rigidbody>();
        hashSpeed = Animator.StringToHash("Speed");
        attackTimer.StartTimer(0.1f);

        // HP 초기화
        currentHP = maxHP;
        UpdateHealthUI();

        // Ammo 초기화
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void Update()
    {
        animator.SetFloat(hashSpeed, myRigidBody.linearVelocity.magnitude);
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        Vector3 newVelocity = new Vector3(inputVertical * moveSpeed, 0.0f, inputHorizontal * -moveSpeed);
        myRigidBody.linearVelocity = newVelocity;

        // 🔊 걸음 소리 처리
        HandleFootsteps(inputHorizontal, inputVertical);


        switch (currentWeapon)
        {
            case PlayerWeaponType.KNIFE:
                if (Input.GetMouseButton(0) && attackTimer.IsFinished())
                    Attack();
                break;

            case PlayerWeaponType.PISTOL:
                if (Input.GetMouseButtonDown(0) && attackTimer.IsFinished())
                    Attack();
                break;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            SetWeapon(PlayerWeaponType.KNIFE);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SetWeapon(PlayerWeaponType.PISTOL);

        attackTimer.UpdateTimer();
        UpdateAim();
    }

    // 🔫 UI 업데이트
    void UpdateAmmoUI()
    {
        string ammoString = currentAmmo + " / " + maxAmmo;

        if (ammoText != null)
            ammoText.text = ammoString;

        if (ammoText != null)
            ammoTextbg.text = ammoString;
    }

    // 🩸 피격 처리 ---------------------------------
    public void TakeDamage(float amount)
    {
        if (currentHP <= 0f) return;

        currentHP -= amount;
        UpdateHealthUI();

        if (currentHP <= 0f)
        {
            currentHP = 0f;
            DamagePlayer();
        }
    }

    public void Heal(float amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (hpBar != null)
            hpBar.value = currentHP / maxHP;
    }
    //------------------------------------------------

    public void DamagePlayer()
    {
        animator.SetBool("Dead", true);
        animator.transform.parent = null;
        this.enabled = false;
        myRigidBody.isKinematic = true;

        // 🔊 죽을 때 피튀기는 소리 + 신음 소리 재생
        GameManager.PlayEnemyDeathSounds(4.0f, 4.0f);

        GameManager.RegisterPlayerDeath();
        gameObject.GetComponent<Collider>().enabled = false;
        GameCamera.ToggleShake(0.3f);
        Vector3 pos = animator.transform.position;
        pos.y = 0.2f;
        animator.transform.position = pos;
    }

    void UpdateAim()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.y = transform.position.y;
        mousePointer.transform.position = mousePos;
        float deltaY = mousePos.z - transform.position.z;
        float deltaX = mousePos.x - transform.position.x;
        float angleInDegrees = Mathf.Atan2(deltaY, deltaX) * 180 / Mathf.PI;
        transform.eulerAngles = new Vector3(0, -angleInDegrees, 0);
    }

    public void Attack()
    {
        switch (currentWeapon)
        {
            case PlayerWeaponType.KNIFE:
                Invoke("DoHitTest", 0.2f);

                // 🔊 나이프 사운드 재생
                if (audioSource != null && KnifeClip != null)
                    audioSource.PlayOneShot(KnifeClip, 3.5f);
                break;

            case PlayerWeaponType.PISTOL:
                // 🔫 탄약 없으면 발사 불가
                if (currentAmmo <= 0)
                {
                    Debug.Log("🔫 탄약이 없습니다!");
                    return;
                }

                // 🔊 발사 사운드 재생
                if (audioSource != null && pistolFireClip != null)
                    audioSource.PlayOneShot(pistolFireClip);

                // 탄약 감소
                currentAmmo--;
                UpdateAmmoUI();

                GameCamera.ToggleShake(0.1f);
                GameObject bullet = Instantiate(proyectilePrefab, gunPivot.position, gunPivot.rotation);
                bullet.transform.LookAt(mousePointer.transform);
                bullet.transform.Rotate(0, Random.Range(-7.5f, 7.5f), 0);
                AlertEnemies();
                break;
        }

        animator.SetBool("Attack", true);
        CancelInvoke("AttackOver");
        Invoke("AttackOver", attackTime);
        attackTimer.StartTimer(attackTime);
    }

    void AlertEnemies()
    {
        RaycastHit[] hits = Physics.SphereCastAll(hitTestPivot.position, 20.0f, hitTestPivot.up);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                hit.collider.GetComponent<NPC_Enemy>().SetAlertPos(transform.position);
            }
        }
    }

    public void DoHitTest()
    {
        RaycastHit[] hits = Physics.SphereCastAll(hitTestPivot.position, 2.0f, hitTestPivot.up);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null && hit.collider.tag == "Enemy")
            {
                RaycastHit forwardHit = new RaycastHit();
                Physics.Raycast(hitTestPivot.position, hit.transform.position - transform.position, out forwardHit);
                if (forwardHit.collider != null && forwardHit.collider.tag == "Enemy")
                {
                    forwardHit.collider.GetComponent<NPC_Enemy>().Damage();
                }
            }
        }
    }

    void AttackOver()
    {
        animator.SetBool("Attack", false);
    }

    void SetWeapon(PlayerWeaponType weaponType)
    {
        if (weaponType != currentWeapon)
        {
            currentWeapon = weaponType;
            animator.SetTrigger("WeaponChange");
            switch (weaponType)
            {
                case PlayerWeaponType.KNIFE:
                    attackTime = 0.4f;
                    animator.SetInteger("WeaponType", 0);
                    break;

                case PlayerWeaponType.PISTOL:
                    attackTime = 0.1f;
                    animator.SetInteger("WeaponType", 3);
                    break;
            }
        }
        GameManager.SelectWeapon(weaponType);
    }

    // 🔫 탄약 회복 (탄약 아이템에서 호출)
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(maxAmmo, currentAmmo + amount);
        UpdateAmmoUI();

        // 🔊 탄약 획득 사운드 재생
        if (audioSource != null && ammoPickupClip != null)
            audioSource.PlayOneShot(ammoPickupClip, 2.5f); // 볼륨 조절 가능
    }

    void HandleFootsteps(float inputH, float inputV)
    {
        // 키 입력 기반으로 이동 체크
        if (Mathf.Abs(inputH) > 0.1f || Mathf.Abs(inputV) > 0.1f)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                if (audioSource != null && footstepClip != null)
                    audioSource.PlayOneShot(footstepClip, 4.5f); // 볼륨 조절

                // 발자국 재생 간격 초기화
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f; // 멈추면 초기화
        }
    }

}


