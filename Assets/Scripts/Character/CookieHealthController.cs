using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CookieController))]
public class CookieHealthController : MonoBehaviour
{
    private readonly float UiMaxHp = 100f;
    private float MaxHp { get; set; }
    private readonly float MaxAdditionalHp = 100f;
    private float _currentHp;
    private float _additionalHp;

    private readonly float _godModeDurationAfterHit = 2f;
    private bool _isGodMode = false;
    private readonly float _healthReduceSpeed = 2f;

    [Header("=== 체력바 관련 Image ===")]
    [SerializeField]
    private Image _hpBar;

    [SerializeField]
    private Image _additionalHpBar;

    [SerializeField]
    private Image _healthBarShine;

    [HideInInspector]
    public UnityEvent OnTakeDamage;

    private SpriteRenderer _spriteRenderer;
    private Coroutine _coGodMode;

    public float CurrentHp
    {
        get => _currentHp;
        private set => _currentHp = Mathf.Clamp(value, 0, MaxHp);
    }

    public float AdditionalHp
    {
        get => _additionalHp;
        private set => _additionalHp = Mathf.Clamp(value, 0, MaxAdditionalHp);
    }

    public bool IsGodMode => _isGodMode;

    private float UiHpPercent => CurrentHp / UiMaxHp;
    private float AdditionalHpPercent => AdditionalHp / MaxAdditionalHp;

    public void Init(float maxHp, SpriteRenderer spriteRenderer)
    {
        MaxHp = maxHp;
        CurrentHp = MaxHp;
        _spriteRenderer = spriteRenderer;
    }

    public void TakeDamage(float amount)
    {
        if (_isGodMode)
            return;

        if (_additionalHp > 0)
        {
            float reducedAmount = Mathf.Clamp(amount, 0, _additionalHp);
            AdditionalHp -= reducedAmount;
            amount -= reducedAmount;
        }

        CurrentHp -= amount;

        OnTakeDamage?.Invoke();
        EnableGodMode(_godModeDurationAfterHit);
    }

    public void EnableGodMode(float duration)
    {
        if (_coGodMode != null)
            StopCoroutine(_coGodMode);
        _coGodMode = StartCoroutine(CoGodMode(duration));
    }

    public void StopGodMode()
    {
        if (_coGodMode != null)
            StopCoroutine(_coGodMode);
        _isGodMode = false;
        _spriteRenderer.enabled = true;
    }

    private IEnumerator CoGodMode(float duration)
    {
        _isGodMode = true;
        float godModeTimer = 0f;

        while (godModeTimer <= duration)
        {
            godModeTimer += Time.deltaTime;
            _spriteRenderer.enabled = (int)(godModeTimer / 0.1) % 2 != 0;
            yield return null;
        }

        _spriteRenderer.enabled = true;
        _isGodMode = false;
    }

    public void RecoverHp(float amount)
    {
        CurrentHp += amount;
    }

    public void GetAdditionalHealth(float amount)
    {
        AdditionalHp += amount;
    }

    private void Update()
    {
        if (AdditionalHp > 0)
        {
            AdditionalHp -= _healthReduceSpeed * Time.deltaTime;
        }
        else
        {
            CurrentHp -= _healthReduceSpeed * Time.deltaTime;
        }

        _hpBar.fillAmount = UiHpPercent;
        _additionalHpBar.rectTransform.anchoredPosition = new Vector2(
            _hpBar.rectTransform.anchoredPosition.x
                + _hpBar.rectTransform.sizeDelta.x * UiHpPercent,
            _additionalHpBar.rectTransform.anchoredPosition.y
        );
        _additionalHpBar.fillAmount = AdditionalHpPercent;
        _healthBarShine.rectTransform.anchoredPosition = new Vector2(
            _additionalHpBar.rectTransform.anchoredPosition.x
                + _additionalHpBar.rectTransform.sizeDelta.x * AdditionalHpPercent
                - 20,
            _healthBarShine.rectTransform.anchoredPosition.y
        );
    }
}
