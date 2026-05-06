using System.Collections;
using System.Runtime.InteropServices;
using Unity.Properties;
using UnityEditorInternal;
using UnityEngine;

public class CherryCookie : CookieBehavior
{
    public Animator animator;
    private readonly string _bombPath = "Sprite/Character/Cherry/Jelly/CherryBomb";
    private readonly int _isJumping = Animator.StringToHash("isJump");
    private readonly int _isGround = Animator.StringToHash("isGround");
    private readonly int _isSliding = Animator.StringToHash("isSlide");
    private readonly int _isDoubleJumping = Animator.StringToHash("isDoubleJump");
    private readonly int _isSkill = Animator.StringToHash("isSkill");
    private readonly int _isDie = Animator.StringToHash("isDie");
    private readonly int _isMiddleRun = Animator.StringToHash("MiddleRun");
    private readonly int _isHappyRun = Animator.StringToHash("HappyRun");
    private readonly string _jumpAudioClip = "Sprite/Character/Cherry/Sound/Ch28Jump";
    private readonly string _SlideAudioClip = "Sprite/Character/Cherry/Sound/Ch28slide";
    private readonly string _ThrowAudioClip = "Sprite/Character/Cherry/Sound/Cherry_Throwing_Normal";
    private readonly string _ThrowMadAudioClip = "Sprite/Character/Cherry/Sound/Cherry_Throwing_Mad";
    private AudioClip JumpClip;
    private AudioClip SlideClip;
    private AudioClip SkillMadClip;
    private AudioClip skillClip;
    public AudioSource audioSource;
    private bool normalSkill = true;
    private bool madSkill = false;
    private bool Alive = true;
    private bool hit = false;
    public GameObject bomb;
    private Coroutine cor;
    public float coolTime = 8f;
    public float maxCoolTIme = 8f;
    public float middleCoolTime = 6.5f;
    private float minCoolTIme = 5f;

    private void OnEnable()
    {
        Alive = true;
        coolTime = maxCoolTIme;

    }
    public override void Init(CookieController controller)
    {
        base.Init(controller);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        bomb = Resources.Load<GameObject>(_bombPath);
        normalSkill = true;
        madSkill = false;
        skillClip = Resources.Load<AudioClip>(_ThrowAudioClip);
        SkillMadClip = Resources.Load<AudioClip>(_ThrowMadAudioClip);
        JumpClip = Resources.Load<AudioClip>(_jumpAudioClip);
        SlideClip = Resources.Load<AudioClip>(_SlideAudioClip);
        cor = StartCoroutine(Cycle());
    }

    public override void StartJumpAnimation()
    {
       audioSource.PlayOneShot(JumpClip);
       animator.SetBool(_isGround, false);  
       animator.SetBool(_isJumping, true);
    }

    public override void StartRunAnimation()
    {
        animator.SetBool(_isJumping, false);
        animator.SetBool(_isSliding, false);
        animator.SetBool(_isDoubleJumping, false);
        animator.SetBool(_isGround, true);
    }

    public override void StartDoubleJumpAnimation()
    {
        audioSource.PlayOneShot(JumpClip);
        animator.SetBool(_isJumping, false);
        animator.SetBool(_isGround, false);
        animator.SetBool(_isDoubleJumping, true);
    }

    public override void StartSlidingAnimation()
    {
        audioSource.PlayOneShot(SlideClip);
        animator.SetBool(_isSliding, true);

    }

    public override void StartDeathAnimation()
    {
        Alive = false;
        animator.SetTrigger(_isDie);
    }
    public IEnumerator Cycle()
    {
        while (Alive)
        {
            yield return new WaitForSeconds(coolTime);
            cor = StartCoroutine(Skill());
            coolTime -= 0.2f;
            if(coolTime < minCoolTIme)
            {
                coolTime = minCoolTIme;

            }
            if(coolTime <middleCoolTime&&coolTime>minCoolTIme)
            {
                animator.SetFloat(_isMiddleRun, 1f);
            }
            if(coolTime ==minCoolTIme)
            {
                animator.SetFloat(_isMiddleRun, 0f);
                animator.SetFloat(_isHappyRun, 1f);
                normalSkill = false;
                madSkill = true;
            }
            if (hit)
            {
                coolTime = maxCoolTIme;
                hit = false;
                normalSkill = true;
                madSkill=false;
                animator.SetFloat(_isMiddleRun, 0f);
                animator.SetFloat(_isHappyRun, 0f);

            }
        }
    }

    public IEnumerator Skill()
    {
        animator.SetBool(_isSkill, true);
        yield return new WaitForSeconds(0.3f);
        if (normalSkill)
        {
            audioSource.PlayOneShot(skillClip);

        }
        else if (madSkill)
        {
            audioSource.PlayOneShot(SkillMadClip);
        }

        ThrowBomb();

        animator.SetBool(_isSkill,false);
    }

    public void ThrowBomb()
    {
        for(int i = 0; i <3;i++)
        {
            GameObject newBomb = Instantiate(bomb, transform.position, Quaternion.identity);
            Rigidbody2D rb = newBomb.GetComponent<Rigidbody2D>();
            float randomX = Random.Range(0.5f, 1f);
            float randomY = Random.Range(0.5f, 1.5f);
            Vector3 throwdirection = new Vector3(randomX, randomY, 0f).normalized;
            float throwPower = 10f;

            rb.AddForce(throwdirection * throwPower, ForceMode2D.Impulse);
        }
    }
}
