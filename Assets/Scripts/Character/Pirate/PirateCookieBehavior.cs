using System.Collections;
using UnityEngine;

public class PirateCookieBehavior : CookieBehavior {
	
	private Animator _animator;
	
	private readonly string _shipResourcePath = "Prefabs/Character/Pirate/PirateShip";
	private readonly int _isJumping = Animator.StringToHash("isJumping");
	private readonly int _isSliding = Animator.StringToHash("isSliding");
	private readonly int _isDoubleJumping = Animator.StringToHash("isDoubleJumping");
	private readonly int Disappear = Animator.StringToHash("Disappear");
	private readonly int Death = Animator.StringToHash("death");
	
	private readonly string _ghostAnimatorPath = "Animations/Character/Pirate/GhostAnimationController";
	
	private GameObject _pirateShip;
	
	private readonly float _abilityPeriod = 15f;
	private readonly float _abilityDuration = 10f;
	
	private bool _isUsingAbility;
	private bool _isFirstDeath = true;
	
	private Coroutine _abilityCycleCoroutine;
	private Coroutine _abilityCoroutine;
	private Coroutine _reviveCoroutine;
	
	public override void Init(CookieController controller) {
		base.Init(controller);
		
		_animator = GetComponent<Animator>();
		
		// 시작 시에, PirateShip 만들고 안보이는 상태로
		_pirateShip = Instantiate(Resources.Load<GameObject>(_shipResourcePath));
		
		// 특정 초마다 실행하도록 Coroutine 시작
		_abilityCycleCoroutine = StartCoroutine(CoAbilityCycle());
	}
	
	private IEnumerator CoAbilityCycle() {
		while (true) {
			// 특정 시간마다 능력 사용하도록
			_abilityCoroutine = StartCoroutine(CoAbility());
			yield return new WaitForSeconds(_abilityPeriod);	
		}
	}
	
	private IEnumerator CoAbility() {
		// 능력 시작 후 종료
		_pirateShip.SetActive(true);
		_isUsingAbility = true;
		yield return new WaitForSeconds(_abilityDuration);
		_isUsingAbility = false;
		
		StartCoroutine(CoDisappear());
		
		_abilityCoroutine = null;
	}
	
	private IEnumerator CoDisappear() {
		_pirateShip.GetComponent<Animator>().SetTrigger(Disappear);
		
		yield return new WaitForSeconds(0.5f);
		
		_pirateShip.SetActive(false);
	}
	
	public override void StartJumpAnimation() {
		_animator.SetBool(_isJumping, true);
	}
	
	public override void StartRunAnimation() {
		_animator.SetBool(_isJumping, false);
		_animator.SetBool(_isDoubleJumping, false);
		_animator.SetBool(_isSliding, false);
	}
	
	public override void StartDoubleJumpAnimation() {
		_animator.SetBool(_isDoubleJumping, true);
	}

	public override void StartSlidingAnimation() {
		_animator.SetBool(_isSliding, true);
	}

	public override void StartDeathAnimation() {
		_animator.SetTrigger(Death);
	}
	
	public override bool DeathCheck() {
		// 부활해야 하는 사망
		if (base.DeathCheck() && _isFirstDeath) {
			_isFirstDeath = false;
			_reviveCoroutine = StartCoroutine(CoRevive());
		}
		
		// 진짜 사망
		else if (base.DeathCheck() && _reviveCoroutine == null && !_isFirstDeath) {
			StopCoroutine(_abilityCycleCoroutine);
			_abilityCoroutine = null;
			return true;
		}
		
		return false;
	}
	
	private IEnumerator CoRevive() {
		Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
		
		// 사망 연출을 위해 콜라이더 사망처럼 변경
		_controller.SetSlidingCollider();
		// 사망 애니메이션 재생
		StartDeathAnimation();
		// 배경 스크롤 그만
		_gameManager.ScrollObjectsFlag = false;
		// 점프 및 슬라이딩 불가능하게
		_controller.JumpEnabled = false;
		_controller.SlideEnabled = false;
		// 잠깐 중력 연출도 정지
		rigidbody.simulated = false;
		
		
		yield return new WaitForSeconds(1f);
		
		// 애니메이션 교체
		_animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(_ghostAnimatorPath);
		// 부활 애니메이션 나올 동안 대기
		yield return new WaitForSeconds(0.5f);
		// 다시 스탠딩 콜라이더로 변경
		_controller.SetStandingCollider();
		// 추가 체력 주고, 다시 배경 스크롤
		_controller.GetAdditionalHealth(30);
		_gameManager.ScrollObjectsFlag = true;
		// 점프, 슬라이딩 가능하게
		_controller.JumpEnabled = true;
		_controller.SlideEnabled = true;
		// 중력 다시 연출
		rigidbody.simulated = true;
		
		_reviveCoroutine = null;
	}
}