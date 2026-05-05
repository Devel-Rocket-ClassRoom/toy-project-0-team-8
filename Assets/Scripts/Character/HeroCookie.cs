using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCookie : MonoBehaviour
{
    public float jumpForce = 200f;
    public float Health = 100f;
    private int jumpCount = 0;

    private bool isGrounded = false;
    private bool isSlide = false;

    // 종료 로직과 시간을 같이 담을 튜플 ... 구조체로 받을 방법 없나? 다른 요소가 필요할 수도 있으니
    private (Action<GameObject>, float) ActiveItem;
    // 를 담을 리스트. 갱신과 종료가 각자 돼야하니
    private List<(Action<GameObject>, float)> _activeItems = new List<(Action<GameObject>, float)>();


    public bool isDead = false;
    public bool isImmune = false;

    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D col;

    private Vector2 originOffset;
    private Vector2 originSize;
    private Vector3 originPos;

    private Coroutine coFallenAnim;
    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();

    }

    void Start()
    {
        originOffset = col.offset;
        originSize = col.size;
        originPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ItemCheck();
        Jump();
        Slide();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Transformation();

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isSlide", isSlide);

    }

    private void Transformation()
    {
        animator.SetTrigger("Transformation");
    }

    void Jump()
    {

        if (Input.GetButtonDown("Fire1") && jumpCount < 2)
        {
            jumpCount++;
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool("isDouble", jumpCount == 2);

            Debug.Log("jump");

            if (isSlide)
            {
                isSlide = false;
                isGrounded = false;

                
                col.offset = new Vector2(originOffset.x, originOffset.y);
                col.size = new Vector2(originSize.x, originSize.y);
            }

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
        Vector3 targetPos = originPos;
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
            yield return null;
        }
        transform.position = targetPos;
        coFallenAnim = null;
    }
    void Slide()
    {

        if (Input.GetKey(KeyCode.S) && !isSlide && isGrounded)
        {
            isSlide = true;

            col.offset = new Vector2(col.offset.x, col.offset.y - col.size.y / 4);
            col.size = new Vector2(col.size.x, col.size.y * 0.5f);


            Debug.Log("slide");
        }
        else if (Input.GetKeyUp(KeyCode.S) && isSlide)
        {
            isSlide = false;

            col.offset = new Vector2(originOffset.x, originOffset.y);

            col.size = new Vector2(originSize.x, originSize.y);
        }


    }

    public void Die()
    {
        isDead = true;
        animator.SetTrigger("isDead");

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

    }

    public void Heal(float energy)
    {
        Health += energy;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform") && collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Dead"))
        {
            Die();
        }
    }
}
