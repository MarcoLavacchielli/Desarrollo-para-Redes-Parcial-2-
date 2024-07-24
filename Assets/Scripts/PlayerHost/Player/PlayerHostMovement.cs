using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHostMovement : NetworkCharacterControllerPrototype
{
    [SerializeField] private NetworkMecanimAnimator _networkAnimator;

    private NetworkInputData _inputs;

    private Vector3 originalScale;
    private float originalSpeed;
    private float originalSprintSpeed;
    private float originalCrouchSpeed;
    private float originalSlideSpeed;

    [SerializeField] private float originalSlideForce;

    AudioManager audioM;

    [Header("Camera")]
    [SerializeField] private ParticleSystem attackPs;
    [SerializeField] private ParticleSystem stunPs;
    [SerializeField] private ParticleSystem boostPs;

    [Header("Camera")]
    public Camera cameraAct;

    [Header("Boosts")]
    [SerializeField] private int capsuleAmount = 3;
    private float boostTime = 5f;
    [SerializeField] private TextMeshProUGUI capsuleText;
    [Networked(OnChanged = nameof(OnBoostChanged))]
    private bool isBoosting { get; set; } 

    [SerializeField] private bool boosteanding = false;

    [Header("Crouch")]
    public float crouchSpeed = 1.0f;
    public float crouchYScale = 0.5f;

    [Header("Sprint")]
    public float sprintVelocity;

    [Header("Slider")]
    public float maxSlideTime;
    public float slideSpeed;
    public float slideForce = 10.0f;
    private bool _isSliding = false;
    private float _slideTimer = 0f;
    [SerializeField] private bool _canSlide = true;
    public float slideCooldown = 1.0f;

    [Header("Ataque")]
    [SerializeField] private float attackRadius = 1.5f;
    [SerializeField] private LayerMask objLayer;
    [SerializeField] private LayerMask playerLayer;
    public int danio;
    [Networked(OnChanged = nameof(OnAttackingChanged))]
    private bool isAttacking { get; set; }
    [SerializeField] private float cooldown = 0.5f;
    private float nextAttackTime = 0f;

    [SerializeField] private bool isStunned = false;
    [SerializeField] private float stunDuration = 1.0f;
    [Networked(OnChanged = nameof(OnStunChanged))]
    private bool isStun { get; set; }

    public override void FixedUpdateNetwork()
    {
        UpdateAnimations();

        if (GetInput(out _inputs))
        {
            HandleInputs();
        }
    }

    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasInputAuthority)
        {
            cameraAct.gameObject.SetActive(true);
        }
        else
        {
            cameraAct.gameObject.SetActive(false);
        }

        audioM = FindObjectOfType<AudioManager>();

        if (audioM == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }
    }

    protected override void Awake()
    {
        base.Awake();
        originalScale = transform.localScale;
        originalSpeed = maxSpeed;

        originalSlideForce = slideForce;
        originalSprintSpeed = sprintVelocity;
        originalCrouchSpeed = crouchSpeed;
        originalSlideSpeed = slideSpeed;
    }

    private void UpdateAnimations()
    {
        _networkAnimator.Animator.SetBool("slowRun", false);
        _networkAnimator.Animator.SetBool("crouchIdle", false);
        _networkAnimator.Animator.SetBool("fastRun", false);
        _networkAnimator.Animator.SetBool("isAttack", false);

        bool isMoving = Velocity.magnitude > 0.1f;

        if (maxSpeed == crouchSpeed && isMoving)
        {
            _networkAnimator.Animator.SetBool("crouchIdle", true);
        }
        else if (maxSpeed == sprintVelocity && isMoving)
        {
            _networkAnimator.Animator.SetBool("fastRun", true);
        }
        else if (maxSpeed > crouchSpeed && maxSpeed < sprintVelocity && isMoving)
        {
            _networkAnimator.Animator.SetBool("slowRun", true);
            audioM.PlaySFX(2);
        }

        if (isAttacking)
        {
            _networkAnimator.Animator.SetBool("isAttack", true);
        }
    }

    private void HandleInputs()
    {
        if (isStunned)
        {
            return;
        }

        if (_inputs.isJumpPressed)
        {
            audioM.PlaySFX(1);
        }
        if (_inputs.isCrouchPressed)
        {
            Crouch();
        }
        if (_inputs.isStandPressed)
        {
            Stand();
        }
        if (_inputs.isSprintPressed)
        {
            Sprint();
        }
        if (_inputs.isSlidePressed && !_isSliding)
        {
            StartSliding();
        }
        if (_isSliding)
        {
            Slide();
        }
        if (_inputs.isAttackPressed && Time.time >= nextAttackTime && !isAttacking)
        {
            Attack();
        }
    }

    public void Crouch()
    {
        transform.localScale = new Vector3(originalScale.x, crouchYScale, originalScale.z);
        Velocity += Vector3.down * 5f;
        maxSpeed = crouchSpeed;
    }

    public void Stand()
    {
        slideForce = 0f;
        transform.localScale = originalScale;
        maxSpeed = originalSpeed;
    }

    public void ApplyJumpForce(float jumpForce)
    {
        Velocity = new Vector3(Velocity.x, jumpForce, Velocity.z);
    }

    public void Sprint()
    {
        if (transform.localScale == new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z))
        {
            maxSpeed = crouchSpeed;
        }
        else
        {
            maxSpeed = sprintVelocity;
        }
    }

    public void StartSliding()
    {
        if (_canSlide)
        {
            slideForce = originalSlideForce;
            _isSliding = true;
            _slideTimer = 0f;
            maxSpeed = slideSpeed;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }
    }

    private void Slide()
    {
        if (_canSlide)
        {
            _slideTimer += Time.fixedDeltaTime;

            if (_slideTimer >= maxSlideTime)
            {
                EndSlide();
            }

            if (_isSliding)
            {
                Vector3 slideDirection = transform.forward * slideForce;
                Velocity += slideDirection;
            }
        }
    }

    private void EndSlide()
    {
        slideForce = 0f;
        _isSliding = false;
        _slideTimer = 0f;
        maxSpeed = originalSpeed;
        transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
        StartCoroutine(SlideCooldown());
    }

    private IEnumerator SlideCooldown()
    {
        _canSlide = false;
        Debug.Log("Cooldown started");
        yield return new WaitForSeconds(slideCooldown);
        Debug.Log("Cooldown ended");
        _canSlide = true;
    }

    public void Attack()
    {
        if (!isAttacking && Time.time >= nextAttackTime)
        {
            isAttacking = true;
            nextAttackTime = Time.time + cooldown;
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        StartCoroutine(ServerPerformAttack());
    }

    private IEnumerator ServerPerformAttack()
    {
        Debug.Log("Attacking");
        audioM.PlaySFX(0);
        yield return new WaitForSeconds(0.5f);
        AttackDestroyer();
        AttackFinished();
    }

    static void OnAttackingChanged(Changed<PlayerHostMovement> changed)
    {
        bool currentFiring = changed.Behaviour.isAttacking;
        changed.LoadOld();
        bool oldFiring = changed.Behaviour.isAttacking;

        if (!oldFiring && currentFiring) changed.Behaviour.TurnOnParticleSystem();
    }

    static void OnStunChanged(Changed<PlayerHostMovement> changed)
    {
        bool currentStun = changed.Behaviour.isStun;
        changed.LoadOld();
        bool oldStun = changed.Behaviour.isStun;

        if (!oldStun && currentStun) changed.Behaviour.TurnOnStunParticleSystem();
    }

    static void OnBoostChanged(Changed<PlayerHostMovement> changed)
    {
        bool currentBoosting = changed.Behaviour.isBoosting;
        changed.LoadOld();
        bool oldBoosting = changed.Behaviour.isBoosting;

        if (!oldBoosting && currentBoosting) changed.Behaviour.TurnOnBoostParticleSystem();
    }

    void TurnOnParticleSystem()
    {
        attackPs.Play();
    }

    void TurnOnStunParticleSystem()
    {
        stunPs.Play();
    }

    void TurnOnBoostParticleSystem()
    {
        boostPs.Play();
        audioM.PlaySFX(6);
    }

    private void AttackFinished()
    {
        isAttacking = false;
    }

    private void AttackDestroyer()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRadius, playerLayer);

        if (hitEnemies.Length > 0)
        {
            Debug.Log("Attack hit detected");
        }
        else
        {
            Debug.Log("No players detected in attack radius");
        }

        foreach (Collider hitObject in hitEnemies)
        {
            PlayerHostMovement enemyPlayer = hitObject.GetComponent<PlayerHostMovement>();

            if (enemyPlayer != null && enemyPlayer != this)
            {
                Debug.Log("Stunning player: " + hitObject.name);
                enemyPlayer.Stun();
            }
            else if (enemyPlayer == this)
            {
                Debug.Log("Ignoring self stun for: " + hitObject.name);
            }
            else
            {
                Fusion.NetworkObject networkObject = hitObject.GetComponent<Fusion.NetworkObject>();

                if (networkObject != null)
                {
                    Runner.Despawn(networkObject);
                }
                else
                {
                    Debug.LogWarning("El objeto " + hitObject.name + " no tiene un componente NetworkObject adjunto.");
                }
            }
        }

        Collider[] hitObj = Physics.OverlapSphere(transform.position, attackRadius, objLayer);

        foreach (Collider hitObject in hitObj)
        {
            Fusion.NetworkObject networkObject = hitObject.GetComponent<Fusion.NetworkObject>();

            if (networkObject != null)
            {
                Runner.Despawn(networkObject);
            }
            else
            {
                Debug.LogWarning("El objeto " + hitObject.name + " no tiene un componente NetworkObject adjunto.");
            }
        }
    }

    public void Stun()
    {
        if (!isStunned)
        {
            StartCoroutine(StunCoroutine());
        }
    }

    public void Boost()
    {
        if (capsuleAmount > 0 && boosteanding == false)
        {
            capsuleAmount -= 1;
            boosteanding = true;
            UpdateCapsuleText();
            SetSpeedMultiplier(2f);
            isBoosting = true;
            StartCoroutine(ResetBoost());
        }
    }

    private void UpdateCapsuleText()
    {
        capsuleText.text = capsuleAmount.ToString();
    }

    private IEnumerator ResetBoost()
    {
        yield return new WaitForSeconds(boostTime);
        ResetSpeedMultiplier();
        isBoosting = false;
        boosteanding = false;
    }

    private IEnumerator StunCoroutine()
    {
        Debug.Log("Player stunned: " + gameObject.name);
        isStunned = true;
        isStun = true;
        float originalSpeed = maxSpeed;
        maxSpeed = 0f;
        stunPs.Play(); 
        yield return new WaitForSeconds(stunDuration);
        maxSpeed = originalSpeed;
        isStunned = false;
        isStun = false;
        Debug.Log("Player recovered from stun: " + gameObject.name);
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        maxSpeed = originalSpeed * multiplier;
        sprintVelocity = originalSprintSpeed * multiplier;
        slideSpeed = originalSlideSpeed * multiplier;
        crouchSpeed = originalCrouchSpeed * multiplier;
        slideForce = originalSlideForce * multiplier;
    }

    public void ResetSpeedMultiplier()
    {
        maxSpeed = originalSpeed;
        sprintVelocity = originalSprintSpeed;
        slideSpeed = originalSlideSpeed;
        crouchSpeed = originalCrouchSpeed;
        slideForce = originalSlideForce;
    }

    [Header("Camera")]
    public GameObject victoryScreen;
    public GameObject defeatScreen;
    public GameObject Timer;
    [SerializeField] NetworkBool gano = false;
    [SerializeField] NetworkBool perdio = false;

    public void Gano()
    {
        if (Object.HasInputAuthority)
        {
            victoryScreen.SetActive(true);
            audioM.PlaySFX(3);
        }
        gano = true;
    }

    public void Perdio()
    {
        if (Object.HasInputAuthority)
        {
            defeatScreen.SetActive(true);
            audioM.PlaySFX(4);
        }
        perdio = true;
    }
}
