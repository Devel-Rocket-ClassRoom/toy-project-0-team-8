using System.Collections;
using UnityEngine;

public class PirateCookieBehavior : CookieBehavior {
	
	private Animator _animator;
	
	private readonly string _shipResourcePath = "Prefabs/Character/Pirate/PirateShip";
	private readonly int _isJumping = Animator.StringToHash("isJumping");
	private readonly int _isSliding = Animator.StringToHash("isSliding");
	private readonly int _isDoubleJumping = Animator.StringToHash("isDoubleJumping");
	private readonly int Disappear = Animator.StringToHash("Disappear");
	
	private GameObject _pirateShip;
	
	private readonly float _abilityPeriod = 15f;
	private readonly float _abilityDuration = 10f;
	
	private bool _isUsingAbility;
	
	private Coroutine _appearCoroutine;
	private Coroutine _disappearCoroutine;
	
	private bool _isFirstDeath;
	
	public override void Init(CookieController controller) {
		base.Init(controller);
		
		_animator = GetComponent<Animator>();
		
		// 시작 시에, PirateShip 만들고 없애두기
		_pirateShip = Instantiate(Resources.Load<GameObject>(_shipResourcePath));
		
		// 특정 초마다 실행하도록 Coroutine 시작
		_appearCoroutine = StartCoroutine(CoUseAbility());
	}
	
	private IEnumerator CoUseAbility() {
		while (true) {
			// 특정 시간마다 능력 사용하도록
			_disappearCoroutine = StartCoroutine(CoAbility());
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
		
		_disappearCoroutine = null;
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

	// 일단 체력이 다 되면 죽은걸로
	public override bool DeathCheck() {
		return base.DeathCheck();
	}
}