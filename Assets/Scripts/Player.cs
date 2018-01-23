using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

public class Player : MonoBehaviour {



    [Header("Player Speeds")]
    public float moveSpeed = 10f;
    public Vector2 jumpForce = new Vector2(0, 100f);

    [Header("Ground Check Transforms")]
    public Transform frontGroundCheck;
    public Transform backGroundCheck;
    public Transform centreGroundCheck;
    public LayerMask groundCheckLayerMask;
    public float groundCheckRadius = 0.1f;
    float previousDirection = 1;


    LightningBoltScript lightningScript; 
    public GameObject lightningObject;

    public GameObject zipMoveObject;

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


    public AudioSource footStepAudio;


    // Use this for initialization
    void Start () {

        zipMoveObject.SetActive(false);
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        lightningScript = lightningObject.GetComponent<LightningBoltScript>();

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
        bool groundedBack = Physics2D.OverlapCircle(backGroundCheck.position, groundCheckRadius, groundCheckLayerMask);
        bool groundedCentre = Physics2D.OverlapCircle(centreGroundCheck.position, groundCheckRadius, groundCheckLayerMask);

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
            lightningScript.SetLightningPos(transform.position, zipIndicatorObject.transform.position);
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

        
        _animator.SetFloat("velocityX", Mathf.Abs(_rigidbody.velocity.x));
        _animator.SetBool("grounded", grounded);
        _animator.SetFloat("velocityY", _rigidbody.velocity.y);
    }


    void StartZip()
    {
        zipping = true;
        zipIndicatorObject.SetActive(true);
        lightningObject.SetActive(true);
        lightningScript.FireLightning(transform.position, zipIndicatorObject.transform.position);
    }


    IEnumerator DoZip()
    {


        spriteRenderer.enabled = false;
        zipMoveObject.SetActive(true);

        zipIndicatorObject.transform.SetParent(null);
        zipMoveObject.transform.SetParent(null);
        _rigidbody.gravityScale = 0;
        _rigidbody.isKinematic = true;



        
        while (Vector2.Distance(zipMoveObject.transform.position, zipIndicatorObject.transform.position) > 0.1f)
        {
            zipMoveObject.transform.position = Vector2.MoveTowards(zipMoveObject.transform.position, zipIndicatorObject.transform.position, zipSpeed);
            yield return new WaitForEndOfFrame();
        }

        zipIndicatorObject.transform.position = transform.position;
        zipIndicatorObject.transform.SetParent(transform);
        doingZip = false;
        zipMoveObject.SetActive(false);

        transform.position = zipMoveObject.transform.position;
        zipMoveObject.transform.position = transform.position;
        zipMoveObject.transform.SetParent(transform);
        spriteRenderer.enabled = transform;

        //_rigidbody.gravityScale = 2;
        _rigidbody.isKinematic = false;
        _animator.SetBool("doindZip", false);
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

        lightningScript.StopLightning();
        lightningObject.SetActive(false);

    }

    void Jump()
    {

            _rigidbody.AddForce(jumpForce);

    }

    public void PlayFootStep()
    {
        footStepAudio.Play();
    }



 

}
