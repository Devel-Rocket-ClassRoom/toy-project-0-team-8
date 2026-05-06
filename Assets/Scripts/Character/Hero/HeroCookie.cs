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

    private Coroutine coFallenAnim;
    private Coroutine coSkillAnim;
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();

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

            // 점프 한번 되면서 변신
            rb.AddForce(Vector3.up * flyUpForce, ForceMode2D.Force);
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

        while (true)
        {
            if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "FlyEnd")
            {
                ChargeStack = 0;
                coSkillAnim = null;
                rb.gravityScale = 3;

                _controller.JumpEnabled = true;
                _controller.SlideEnabled = true;

                _controller.WhileJumpKeyPressed.RemoveListener(OnSkill);
                _controller.WhileSlideKeyPressed.RemoveListener(OnSkill);
                break;
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
    public void AddItem(Action<GameObject> onApply, Action<GameObject> onRemove, float duration)
    {

        // 효과 발동
        onApply?.Invoke(gameObject);

        // bool 값까지 받아서 획득 시 지속시간 초기화를 해야하나..
        _activeItems.Add((onRemove, duration));
    }

    public void ItemCheck()
    {
        for (int i = _activeItems.Count - 1; i >= 0; i--)
        {
            var item = _activeItems[i];
            item.Item2 -= Time.deltaTime;
            _activeItems[i] = item;


            // 시간이 다 되면 보관해둔 삭제 로직 실행
            if (_activeItems[i].Item2 <= 0)
            {
                _activeItems[i].Item1?.Invoke(gameObject);
                _activeItems.RemoveAt(i);
            }
        }
    }

    public override bool UseAbilityProgressBar { get; }


    public override float GetProgressbarAmount() {
        throw new NotImplementedException();
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
        rb.gravityScale = 3;

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
}
