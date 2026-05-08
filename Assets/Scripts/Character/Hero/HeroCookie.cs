using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class HeroCookie : CookieBehavior
{

    public int ChargeStack;

    [Header("Flight Settings")]
    public float flyUpForce = 5f;        // 위로 밀어주는 힘 (좀 더 강하게 수정 권장)
    public float maxUpwardVelocity = 6f; // 최대 상승 속도
    public float flightGravityScale = 0.8f; // 비행 중 낙하 속도 (천천히 떨어지게)
    public float SkillPosY = -0.5f;          // 비행 시작 높이

    private Rigidbody2D rb;
    private Animator animator;
    private float defaultGravity;
    private bool isFlying = false;

    private ChargeJellyBatch _chargeJellyBatch;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _chargeJellyBatch = GetComponent<ChargeJellyBatch>();

        
    }

    void Start()
    {
        if (_chargeJellyBatch != null)
            _chargeJellyBatch.ReplaceJellyByDistance(); // 스테이지 전환 시에도 호출 해야함

        // 컨트롤러에서 기본 중력값을 가져옵니다.
        defaultGravity = _cookieController.GravityScale;
        Debug.Log(defaultGravity);

        
    }

    void Update()
    {
        // 변신 조건: 풀스택이거나 테스트용 1번키
        if ((ChargeStack == 5 || Input.GetKeyDown(KeyCode.Alpha1)) && !isFlying)
        {
            StartTransformation();
        }
    }

    private void StartTransformation()
    {
        isFlying = true;
        ChargeStack = 6; // 변신 중 상태 유지용

        // 천장 활성화
        _cookieController.roof.SetActive(true);

        // 기존 조작 비활성화 및 스킬 리스너 등록
        _cookieController.JumpEnabled = false;
        _cookieController.SlideEnabled = false;
        _cookieController.WhileJumpKeyPressed.AddListener(OnSkill);
        _cookieController.WhileSlideKeyPressed.AddListener(OnSkill);

        StopAllCoroutines();
        StartCoroutine(coSkillRoutine());
    }

    IEnumerator coSkillRoutine()
    {
        // 변신 시작
        animator.SetTrigger("Transformation");
        _cookieController._isSkill = true;
        rb.bodyType = RigidbodyType2D.Kinematic; // 위치 고정을 위해 잠시 키네마틱

        float elapsed = 0f;
        float duration = 1.1f; // 변신 애니메이션 동안 올라가는 시간 (조절 가능)
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(transform.position.x, SkillPosY, transform.position.z);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // 부드러운 Lerp를 위해 SmoothStep 사용
            float newY = Mathf.SmoothStep(startPos.y, SkillPosY, t);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }

        // 비행
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = flightGravityScale; // 비행 전용 중력 적용
        float initSpeed = _gameManager.SkillSpeed(20f);
        // 애니메이션이 Fly 상태인 동안 대기 (FlyEnd가 시작될 때까지)
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("FlyEnd"))
        {
            yield return null;
        }

        // 종료
        // 다시 조작권 회수 및 상태 초기화
        _cookieController.WhileJumpKeyPressed.RemoveListener(OnSkill);
        _cookieController.WhileSlideKeyPressed.RemoveListener(OnSkill);
        _gameManager.SkillSpeed(initSpeed);
        // 착지 시에는 부드럽게 지면 근처로 유도 (원래 위치 y값으로)
        float returnElapsed = 0f;
        float returnDuration = 1.0f;
        Vector3 flightEndPos = transform.position;
        float targetLandingY = -2f; // 보통 0이나 바닥 높이

        while (returnElapsed < returnDuration)
        {
            returnElapsed += Time.deltaTime;
            float t = returnElapsed / returnDuration;
            // 물리엔진과 충돌하지 않게 속도를 줄이면서 Lerp
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Lerp(rb.linearVelocity.y, targetLandingY, t));
            yield return null;
        }

        //리셋
        ChargeStack = 0;
        rb.gravityScale = defaultGravity;
        _cookieController.roof.SetActive(false);

        _cookieController._isSkill = false;
        isFlying = false;
        _cookieController.JumpEnabled = true;
        _cookieController.SlideEnabled = true;

    }

    public void OnSkill()
    {
        // 비행 중 상승 로직
        if (rb.bodyType == RigidbodyType2D.Dynamic && rb.linearVelocity.y < maxUpwardVelocity)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, flyUpForce);

        }
    }


    public void Die()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

    }

    public override bool UseAbilityProgressBar => true;
    
    public override float GetProgressbarAmount() 
    {

        return ChargeStack/5f;
    }

    public override void StartJumpAnimation()
    {
        
        animator.SetBool("isGrounded", false);
    }

    public override void StartRunAnimation()
    {
        animator.SetBool("isDouble", false);
        animator.SetBool("isSlide", false);
        animator.SetBool("isDash", false);

        animator.SetBool("isGrounded", true);

    }

    public override void StartDoubleJumpAnimation()
    {
        animator.SetBool("isDouble", true);
    }

    public override void StartSlidingAnimation()
    {
        animator.SetBool("isSlide", true);
    }

    public override void StartDeathAnimation()
    {
        animator.SetTrigger("Dead");
        Die();
    }

    public override void StartDashAnimation() 
    {
        animator.SetBool("isDash",true);
    }
}
