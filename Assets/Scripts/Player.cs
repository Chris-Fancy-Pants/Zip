using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {



    [Header("Player Speeds")]
    public float moveSpeed = 10f;
    public Vector2 jumpForce = new Vector2(0, 100f);

    [Header("Ground Check Transforms")]
    public Transform frontGroundCheck;
    public Transform backGroundCheck;
    public LayerMask groundCheckLayerMask;
    public float groundCheckRadius = 0.1f;
    float previousDirection = 1;

    bool zipping = false;
    public float zipIndicatorRadius = 0.2f;

    public GameObject zipIndicatorObject;
    public SpriteRenderer spriteRenderer;


    bool zipIndicatorOverObject = false;
    bool doingZip = false;

    public float zipSpeed;

    Rigidbody2D _rigidbody;
    Animator _animator;

    bool jumping = false;
    public bool grounded = false;
    // Use this for initialization
    void Start () {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
	}



    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(frontGroundCheck.position, groundCheckRadius);
        Gizmos.DrawSphere(backGroundCheck.position, groundCheckRadius);
        Gizmos.DrawSphere(zipIndicatorObject.transform.position, zipIndicatorRadius);

    }
    // Update is called once per frame
    void Update () {

        float horizontal = Input.GetAxis("Horizontal");

        bool groundedFront = Physics2D.OverlapCircle(frontGroundCheck.position, groundCheckRadius, groundCheckLayerMask);
        bool groundedBack = Physics2D.OverlapCircle(backGroundCheck.position, groundCheckRadius, groundCheckLayerMask); ;


        grounded = (groundedBack || groundedFront);

        float direction = 1f;
        float directionToMove = 1f;


        if(horizontal > 0.01)
        {
            direction = 1;
            directionToMove = direction;
            spriteRenderer.flipX = false;

        } else if(horizontal < -0.01)
        {
            direction = -1;
            directionToMove = direction;
            spriteRenderer.flipX = true;

        } else
        {
            directionToMove = 0;
        }


        if (!doingZip)
        {
            _rigidbody.velocity = new Vector2(moveSpeed * directionToMove, _rigidbody.velocity.y);
            _rigidbody.gravityScale = 2;
        }


        previousDirection = direction;
       

        if (Input.GetButtonDown("Jump") && grounded)
        {
            Jump();
        }


        if(Input.GetButtonDown("Fire1") && !grounded)
        {

            StartZip();

        }

        if(Input.GetButtonUp("Fire1") && zipping)
        {
            StopZip();
        }

        

        if (zipping)
        {
            _rigidbody.gravityScale = 0;
            _rigidbody.velocity = Vector2.zero;

            zipIndicatorOverObject = Physics2D.OverlapCircle(zipIndicatorObject.transform.position, zipIndicatorRadius);

            float zipIndicatorHorizontal = Input.GetAxis("Horizontal");
            float zipIndicatorVertical = Input.GetAxis("Vertical");

            Vector2 newIndicatorPosition = new Vector2(zipIndicatorObject.transform.position.x + zipIndicatorHorizontal, zipIndicatorObject.transform.position.y + zipIndicatorVertical);
            // TODO: Add vertical movement to zipIndicator
            if (Vector2.Distance(transform.position, newIndicatorPosition) < 5)
            {
                zipIndicatorObject.transform.position = newIndicatorPosition;
            }


            if(zipIndicatorOverObject)
            {
                zipIndicatorObject.GetComponent<SpriteRenderer>().color = Color.red;
            } else
            {
                zipIndicatorObject.GetComponent<SpriteRenderer>().color = Color.white;
            }


        }

        _animator.SetBool("doindZip", doingZip);
        _animator.SetFloat("velocityX", Mathf.Abs(_rigidbody.velocity.x));
        _animator.SetBool("grounded", grounded);
        _animator.SetFloat("velocityY", _rigidbody.velocity.y);
    }


    void StartZip()
    {
        zipping = true;
        zipIndicatorObject.SetActive(true);
    }


    IEnumerator DoZip()
    {
        print("Start coroutine");
        zipIndicatorObject.transform.SetParent(null);
        _rigidbody.gravityScale = 0;
        
        while (Vector2.Distance(transform.position, zipIndicatorObject.transform.position) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, zipIndicatorObject.transform.position, zipSpeed);
            yield return new WaitForSeconds(0.1f);
        }

        print("In coroutine before 2 sec pause");
        yield return new WaitForSeconds(2f);
        print("In coroutine after 2 sec pause");
        zipIndicatorObject.transform.position = transform.position;
        doingZip = false;
        //_rigidbody.gravityScale = 2;
        _rigidbody.isKinematic = false;
    }

    void StopZip()
    {
        zipping = false;
        zipIndicatorObject.SetActive(false);
        if (!zipIndicatorOverObject)
        {
            //transform.position = zipIndicatorObject.transform.position;
            doingZip = true;
            StartCoroutine("DoZip");
        }
        


    }

    void Jump()
    {

            _rigidbody.AddForce(jumpForce);

    }
}
