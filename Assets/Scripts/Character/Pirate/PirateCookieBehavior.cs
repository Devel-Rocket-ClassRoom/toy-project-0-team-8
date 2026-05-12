using System.Collections;
using UnityEngine;

public class PirateCookieBehavior : CookieBehavior
{
    private Animator _animator;

    private readonly string _shipResourcePath = "Prefabs/Character/Pirate/PirateShip";
    private readonly string _bombResourcePath = "Sprite/Character/Cherry/Jelly/CherryBomb";
    private readonly string _jumpSoundpath = "Sprite/Character/Pirate/Sound/Ch16jump";
    private readonly string _slideSoundpath = "Sprite/Character/Pirate/Sound/Ch16slide";
    private readonly string _reviveSoundpath = "Sprite/Character/Pirate/Sound/G_ghostmode_start";
    private readonly int _isJumping = Animator.StringToHash("isJumping");
    private readonly int _isSliding = Animator.StringToHash("isSliding");
    private readonly int _isDoubleJumping = Animator.StringToHash("isDoubleJumping");
    private readonly int _isDashing = Animator.StringToHash("isDashing");
    private readonly int Disappear = Animator.StringToHash("Disappear");
    private readonly int Death = Animator.StringToHash("death");
    private AudioClip JumpClip;
    private AudioClip SlideClip;
    private AudioClip ReviveClip;

    public AudioSource audioSource;

    private readonly string _ghostAnimatorPath =
        "Animations/Character/Pirate/GhostAnimationController";

    private GameObject _pirateShip;
    private GameObject _bombPrefab;

    private readonly float _abilityPeriod = 15f;
    private readonly float _abilityDuration = 2f;
    private readonly float _bombThrowInterval = 0.4f;
    private readonly float _bombThrowPower = 8f;
    private float _abilityTimer;

    private bool _isUsingAbility;
    private bool _isFirstDeath = true;

    private Coroutine _abilityCycleCoroutine;
    private Coroutine _abilityCoroutine;
    private Coroutine _bombThrowCoroutine;
    private Coroutine _reviveCoroutine;

    public override bool UseAbilityProgressBar => true;

    public override void Init(CookieController controller)
    {
        base.Init(controller);

        _animator = _cookieController.Animator;
        audioSource = GetComponent<AudioSource>();
        // 시작 시에, PirateShip 만들고 안보이는 상태로
        _pirateShip = Instantiate(Resources.Load<GameObject>(_shipResourcePath));
        _pirateShip.SetActive(false);
        _bombPrefab = Resources.Load<GameObject>(_bombResourcePath);
        JumpClip = Resources.Load<AudioClip>(_jumpSoundpath);
        SlideClip = Resources.Load<AudioClip>(_slideSoundpath);
        ReviveClip = Resources.Load<AudioClip>(_reviveSoundpath);

        // 특정 초마다 실행하도록 Coroutine 시작
        _abilityCycleCoroutine = StartCoroutine(CoAbilityCycle());
    }

    private IEnumerator CoAbilityCycle()
    {
        while (true)
        {
            if (_abilityCoroutine != null)
            {
                StopCoroutine(_abilityCoroutine);
            }
            _abilityCoroutine = StartCoroutine(CoAbility());

            // 특정 시간마다 능력 사용하도록
            _abilityTimer = 0f;
            while (true)
            {
                _abilityTimer += Time.deltaTime;
                yield return null;
                if (_abilityTimer >= _abilityPeriod)
                {
                    break;
                }
            }
        }
    }

    private IEnumerator CoAbility()
    {
        // 능력 시작 후 종료
        _pirateShip.SetActive(true);
        _isUsingAbility = true;

        // 능력 발동 동안 일정 간격으로 폭탄 투척
        _bombThrowCoroutine = StartCoroutine(CoThrowBombs());

        yield return new WaitForSeconds(_abilityDuration);

        if (_bombThrowCoroutine != null)
        {
            StopCoroutine(_bombThrowCoroutine);
            _bombThrowCoroutine = null;
        }
        _isUsingAbility = false;

        StartCoroutine(CoDisappear());

        _abilityCoroutine = null;
    }

    private IEnumerator CoThrowBombs()
    {
        // 능력 발동 직후 첫 폭탄을 던지고, 이후 _bombThrowInterval 간격으로 반복
        while (true)
        {
            ThrowBomb();
            yield return new WaitForSeconds(_bombThrowInterval);
        }
    }

    private void ThrowBomb()
    {
        if (_bombPrefab == null)
            return;

        // 해적선이 보이면 그 위치에서, 아니면 캐릭터 위치에서 투척
        Vector3 spawnPos =
            _pirateShip != null && _pirateShip.activeSelf
                ? _pirateShip.transform.position
                : transform.position;

        GameObject bomb = Instantiate(_bombPrefab, spawnPos, Quaternion.identity);

        // CherryBomb 프리팹 재활용. 해적 폭탄은 젤리 생성을 비활성화.
        CherryBomb bombScript = bomb.GetComponent<CherryBomb>();
        if (bombScript != null)
        {
            bombScript.spawnJellyEnabled = false;
        }

        // 앞쪽 위로 비스듬히 던지기
        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector3 throwDirection = new Vector3(1f, 0.6f, 0f).normalized;
            rb.AddForce(throwDirection * _bombThrowPower, ForceMode2D.Impulse);
        }
    }

    private IEnumerator CoDisappear()
    {
        _pirateShip.GetComponent<Animator>().SetTrigger(Disappear);

        yield return new WaitForSeconds(0.5f);

        _pirateShip.SetActive(false);
    }

    public override float GetProgressbarAmount() => _abilityTimer / _abilityPeriod;

    public override void StartJumpAnimation()
    {
        audioSource.PlayOneShot(JumpClip);
        _animator.SetBool(_isJumping, true);
    }

    public override void StartRunAnimation()
    {
        _animator.SetBool(_isDashing, false);
        _animator.SetBool(_isJumping, false);
        _animator.SetBool(_isDoubleJumping, false);
        _animator.SetBool(_isSliding, false);
    }

    public override void StartDoubleJumpAnimation()
    {
        audioSource.PlayOneShot(JumpClip);
        _animator.SetBool(_isDoubleJumping, true);
    }

    public override void StartSlidingAnimation()
    {
        audioSource.PlayOneShot(SlideClip);
        _animator.SetBool(_isSliding, true);
    }

    public override void StartDeathAnimation()
    {
        _animator.SetTrigger(Death);
    }

    public override void StartDashAnimation()
    {
        _animator.SetBool(_isDashing, true);
        _animator.SetBool(_isJumping, false);
        _animator.SetBool(_isDoubleJumping, false);
        _animator.SetBool(_isSliding, false);
    }

    public override bool DeathCheck()
    {
        // 부활해야 하는 사망
        if (base.DeathCheck() && _isFirstDeath)
        {
            _isFirstDeath = false;
            _reviveCoroutine = StartCoroutine(CoRevive());
        }
        // 진짜 사망
        else if (base.DeathCheck() && _reviveCoroutine == null && !_isFirstDeath)
        {
            StopCoroutine(_abilityCycleCoroutine);
            if (_bombThrowCoroutine != null)
            {
                StopCoroutine(_bombThrowCoroutine);
                _bombThrowCoroutine = null;
            }
            _abilityCoroutine = null;
            return true;
        }

        return false;
    }

    private IEnumerator CoRevive()
    {
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();

        // 사망 연출을 위해 콜라이더 사망처럼 변경
        _cookieController.SetSlidingCollider();
        // 사망 애니메이션 재생
        StartDeathAnimation();
        // 배경 스크롤 그만
        _gameManager.ScrollObjectsFlag = false;
        // 점프 및 슬라이딩 불가능하게
        _cookieController.JumpEnabled = false;
        _cookieController.SlideEnabled = false;
        // 잠깐 중력 연출도 정지
        rigidbody.simulated = false;

        yield return new WaitForSeconds(1f);

        // 애니메이션 교체
        _animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(
            _ghostAnimatorPath
        );
        audioSource.PlayOneShot(ReviveClip);
        // 부활 애니메이션 나올 동안 대기
        yield return new WaitForSeconds(0.5f);
        // 다시 스탠딩 콜라이더로 변경
        _cookieController.SetStandingCollider();
        // 추가 체력 주고, 다시 배경 스크롤
        _cookieController.GetAdditionalHealth(30);
        _gameManager.ScrollObjectsFlag = true;
        // 점프, 슬라이딩 가능하게
        _cookieController.JumpEnabled = true;
        _cookieController.SlideEnabled = true;
        // 중력 다시 연출
        rigidbody.simulated = true;

        // 이때부터는 체력 회복 및 충돌 불가능
        _cookieController.CollisionEnabled = false;
        _cookieController.HealEnabled = false;

        _reviveCoroutine = null;
    }
}
