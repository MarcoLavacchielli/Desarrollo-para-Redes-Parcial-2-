using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHostMovement : NetworkCharacterControllerPrototype
{
    [SerializeField] private NetworkMecanimAnimator _mecanimAnim;

    private NetworkInputData _inputs;

    private Vector3 _originalScale;
    private float _originalSpeed;

    [SerializeField] private float originalSlideForce; // Fuerza del slide original

    public GameObject character;

    [Header("Camera")]
    public Camera camaraActivada;

    [Header("Crouch")]
    public float crouchSpeed = 1.0f;
    public float crouchYScale = 0.5f;

    [Header("Sprint")]
    public float sprintVelocity;

    [Header("Slider")]
    public float maxSlideTime;
    public float slideSpeed;
    public float slideForce = 10.0f; // Fuerza del slide
    private bool _isSliding = false;
    private float _slideTimer = 0f;
    [SerializeField] private bool _canSlide = true;
    public float slideCooldown = 1.0f;

    [Header("Ataque")]
    [SerializeField] private float attackRadius = 1.5f;
    [SerializeField] private LayerMask objLayer;
    public int danio;
    private bool isAttacking = false;
    [SerializeField] private float cooldown = 0.5f;
    private float nextAttackTime = 0f;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out _inputs))
        {
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
    }

    public override void Spawned()
    {
        base.Spawned();

        GetComponent<LifeHostHandler>().OnRespawn += () => TeleportToPosition(transform.position);
    }

    public override void Move(Vector3 direction)
    {
        var deltaTime = Runner.DeltaTime;
        var previousPos = transform.position;
        var moveVelocity = Velocity;

        // Transform the input direction to be relative to the character's rotation
        direction = character.transform.TransformDirection(direction).normalized;

        if (IsGrounded && moveVelocity.y < 0)
        {
            moveVelocity.y = 0f;
        }

        moveVelocity.y += gravity * Runner.DeltaTime;

        var horizontalVel = default(Vector3);
        horizontalVel.x = moveVelocity.x;
        horizontalVel.z = moveVelocity.z;

        if (direction == default)
        {
            horizontalVel = Vector3.Lerp(horizontalVel, default, braking * deltaTime);
        }
        else
        {
            horizontalVel = Vector3.ClampMagnitude(horizontalVel + direction * acceleration * deltaTime, maxSpeed);
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Runner.DeltaTime);
        }

        moveVelocity.x = horizontalVel.x;
        moveVelocity.z = horizontalVel.z;

        Controller.Move(moveVelocity * deltaTime);

        Velocity = (transform.position - previousPos) * Runner.Simulation.Config.TickRate;
        IsGrounded = Controller.isGrounded;
    }

    protected override void Awake()
    {
        base.Awake();
        _originalScale = transform.localScale;
        _originalSpeed = maxSpeed;

        originalSlideForce = slideForce;
    }

    public void Crouch()
    {
        transform.localScale = new Vector3(_originalScale.x, crouchYScale, _originalScale.z);
        Velocity += Vector3.down * 5f;
        maxSpeed = crouchSpeed;
    }

    public void Stand()
    {
        slideForce = 0f;

        transform.localScale = _originalScale;
        maxSpeed = _originalSpeed;
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
        if (_canSlide == true)
        {
            slideForce = originalSlideForce;
            _isSliding = true;
            _slideTimer = 0f;
            maxSpeed = slideSpeed;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }
    }

    ///// Slide
    void Slide()
    {
        if (_canSlide == true)
        {
            _slideTimer += Time.fixedDeltaTime;

            if (_slideTimer >= maxSlideTime)
            {
                EndSlide();
            }

            // Aplica la fuerza del slide solo si está deslizándose
            if (_isSliding)
            {
                // Transform the slide direction to be relative to the character's rotation
                Vector3 slideDirection = character.transform.TransformDirection(Vector3.forward) * slideForce;
                Velocity += slideDirection;
            }
        }
    }

    void EndSlide()
    {
        slideForce = 0f;
        _isSliding = false;
        _slideTimer = 0f;
        maxSpeed = _originalSpeed;
        transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
        StartCoroutine(SlideCooldown());
    }

    IEnumerator SlideCooldown()
    {
        _canSlide = false;
        Debug.Log("Cooldown started");
        yield return new WaitForSeconds(slideCooldown);
        Debug.Log("Cooldown ended");
        _canSlide = true;
    }
    /////

    public void Attack()
    {
        if (!isAttacking && Time.time >= nextAttackTime)
        {
            isAttacking = true;
            nextAttackTime = Time.time + cooldown;

            // Llamar al RPC de ataque en el servidor
            RpcPerformAttack();
        }
    }

    void RpcPerformAttack()
    {
        StartCoroutine(ServerPerformAttack());
    }

    IEnumerator ServerPerformAttack()
    {
        Debug.Log("Attacking");

        // Retraso antes de realizar el ataque (opcional)
        yield return new WaitForSeconds(0.5f);

        // Lógica de ataque
        AttackDestroyer();

        // Notificar a todos los clientes que el ataque ha terminado
        RpcAttackFinished();
    }

    void RpcAttackFinished()
    {
        isAttacking = false;
    }

    void AttackDestroyer()
    {

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackRadius, objLayer);

        foreach (Collider hitObject in hitEnemies)
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
}
