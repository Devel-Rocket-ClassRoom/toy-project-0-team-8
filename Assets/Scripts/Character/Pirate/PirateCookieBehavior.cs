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
		// 한번 죽는 애니메이션 호출 및 스크롤 정지
		_controller.SetSlidingCollider();
		StartDeathAnimation();
		_gameManager.ScrollObjectsFlag = false;
		_controller.JumpEnabled = false;
		_controller.SlideEnabled = false;
		
		yield return new WaitForSeconds(1f);
		
		// 애니메이션 교체
		_animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(_ghostAnimatorPath);
		// 부활 애니메이션 나올 동안 대기
		yield return new WaitForSeconds(0.5f);
		// 추가 체력 주고, 다시 배경 스크롤
		_controller.SetStandingCollider();
		_controller.GetAdditionalHealth(30);
		_gameManager.ScrollObjectsFlag = true;
		_controller.JumpEnabled = true;
		_controller.SlideEnabled = true;
		
		_reviveCoroutine = null;
	}
}