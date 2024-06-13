using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHostMovement : NetworkCharacterControllerPrototype
{
    [SerializeField] private NetworkMecanimAnimator _networkAnimator;

    private NetworkInputData _inputs;

    private Vector3 _originalScale;
    private float _originalSpeed;

    [SerializeField] private float originalSlideForce; // Fuerza del slide original

    [Header("Camera")]
    public Camera cameraAct;

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
        _networkAnimator.Animator.SetBool("slowRun", false);
        _networkAnimator.Animator.SetBool("crouchIdle", false);
        _networkAnimator.Animator.SetBool("fastRun", false);
        _networkAnimator.Animator.SetBool("isAttack", false);

        if (maxSpeed == crouchSpeed)
        {
            _networkAnimator.Animator.SetBool("crouchIdle", true);
        }

        if (maxSpeed == sprintVelocity)
        {
            _networkAnimator.Animator.SetBool("fastRun", true);
        }

        if (maxSpeed == 2.5f)
        {
            _networkAnimator.Animator.SetBool("slowRun", true);
        }

        if (isAttacking == true)
        {
            _networkAnimator.Animator.SetBool("isAttack", true);
        }

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

        if (Object.HasInputAuthority)
        {
            cameraAct.gameObject.SetActive(true);
        }
        else
        {
            cameraAct.gameObject.SetActive(false);
        }

        GetComponent<LifeHostHandler>().OnRespawn += () => TeleportToPosition(transform.position);
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
                Vector3 slideDirection = transform.forward * slideForce;
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


    public GameObject victoryScreen;
    public GameObject defeatScreen;
    [SerializeField] NetworkBool gano = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Meta")
        {
            Gano();
            gano = true;
        }
    }

    public void Gano()
    {
        victoryScreen.SetActive(true);
        defeatScreen = null;
        //SceneManager.LoadScene("Victory");
        gano = true;
    }

    public void Perdio()
    {
        if (gano == false)
        {
            defeatScreen.SetActive(true);
            victoryScreen = null;
            //SceneManager.LoadScene("Defeat");
        }
        if (gano == true)
        {
            Gano();
        }
    }
}