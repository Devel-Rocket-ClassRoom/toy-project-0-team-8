using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class HeroCookie : CookieBehavior
{
    public float Health = 100f;
    public int ChargeStack;

    public float flyUpForce = 15f;
    public float maxUpwardVelocity = 4f;


    // 종료 로직과 시간을 같이 담을 튜플 ... 구조체로 받을 방법 없나? 다른 요소가 필요할 수도 있으니
    private (Action<GameObject>, float) ActiveItem;
    // 를 담을 리스트. 갱신과 종료가 각자 돼야하니
    private List<(Action<GameObject>, float)> _activeItems = new List<(Action<GameObject>, float)>();


    public bool isDead = false;
    public bool isImmune = false;
    public bool isDash = false;

    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D col;

    private Vector3 originPos;
    private float SkillPosY = 0;

    private Coroutine coFallenAnim;
    private Coroutine coSkillAnim;
    private float gravity;
   
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        gravity = 10; // 컨트롤러의 gravity scale 값
    }

    void Start()
    {
        originPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (ChargeStack == 5 || Input.GetKeyDown(KeyCode.Alpha1))
        {
            // 기존 조작 해제
            _controller.WhileJumpKeyPressed.AddListener(OnSkill);
            _controller.WhileSlideKeyPressed.AddListener(OnSkill);
            _controller.JumpEnabled = false;
            _controller.SlideEnabled = false;

            rb.gravityScale = 1;

            Transformation();
        }

    }

    private void Transformation()
    {
        animator.SetTrigger("Transformation");
        
        if (coSkillAnim == null)
        {
            StopAllCoroutines();
            StartCoroutine(coSkill());
        }
    }

    IEnumerator coSkill()
    {

        // 1. 변신 시작: 물리 힘에 방해받지 않도록 킨메틱 설정
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        while (true)
        {
            var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            if (clipInfo.Length > 0)
            {
                string clipName = clipInfo[0].clip.name;

                // 변신 중: Y축 0 근처로 강제 이동 (이제 물리 간섭 없음)
                if (clipName == "Transformation")
                {
                    float newY = Mathf.Lerp(transform.position.y, SkillPosY, 0.1f);
                    transform.position = new Vector3(transform.position.x, newY, transform.position.z);
                }
                // 2. 비행 시작 시점: 다시 물리(Dynamic)로 복구하여 AddForce가 먹히게 함
                else if (clipName == "Fly" || clipName == "FlyEnd")
                {
                    if (rb.bodyType != RigidbodyType2D.Dynamic)
                    {
                        rb.bodyType = RigidbodyType2D.Dynamic;
                        rb.gravityScale = 1f; // 비행 중엔 중력을 약하게 조절 가능
                    }
                }

                if (clipName == "FlyEnd")
                {
                    // 변신 종료 및 리셋 로직
                    ChargeStack = 0;
                    coSkillAnim = null;
                    rb.gravityScale = gravity;

                    _controller.JumpEnabled = true;
                    _controller.SlideEnabled = true;
                    _controller.WhileJumpKeyPressed.RemoveListener(OnSkill);
                    _controller.WhileSlideKeyPressed.RemoveListener(OnSkill);
                    break;
                }
            }
            yield return null;
        }
    }

    

    public void OnSkill()
    {
        
        if (rb.linearVelocity.y < maxUpwardVelocity)
        {
            Debug.Log("비행");
            rb.AddForce(Vector3.up * flyUpForce, ForceMode2D.Force);
        }


    }


    // 애니메이션 이벤트
    public void Fallen()
    {
        if(coFallenAnim == null)
        {
            coFallenAnim = StartCoroutine(coFallen());
        }
    }

    IEnumerator coFallen()
    {
        rb.gravityScale = 0;
        Vector3 targetPos = originPos;
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
            yield return new WaitForSeconds(0.05f);
        }

        Debug.Log("낙하");
        transform.position = targetPos;
        coFallenAnim = null;
    }

    public void Die()
    {
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

    }

    public override bool UseAbilityProgressBar => true;
    
    public override float GetProgressbarAmount() {

        return ChargeStack/5;
    }

    public override void StartJumpAnimation()
    {
        
        animator.SetBool("isGrounded", false);
    }

    public override void StartRunAnimation()
    {
        animator.SetBool("isGrounded", true);
        animator.SetBool("isDouble", false);
        animator.SetBool("isSlide", false);
        animator.SetBool("isDash", false);
        rb.gravityScale = gravity;

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
