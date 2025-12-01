using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    public float turnSpeed = 20f;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    private AudioSource m_AudioSource;
    private float boost = 1;
    public bool shield = false;
    public GameObject shieldAsset;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }
    
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);
        
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            boost = 2;
        } else
        {
            boost = 1;
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement * boost, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);
    }
    void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + (m_Movement * boost) * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("touch");
        if (other.gameObject.tag != "Shield") return;


        Debug.Log("SHIELD");
        shield = true;
        shieldAsset.SetActive(true);
        Destroy(other.gameObject);
    }

    public void ShieldBreak()
    {
        shieldAsset.GetComponent<AudioSource>().Play();
        StartCoroutine(ShieldBuffer());
    }

    IEnumerator ShieldBuffer()
    {
        yield return new WaitForSeconds(2f);
        shield = false;
        shieldAsset.SetActive(false);
    }
}
