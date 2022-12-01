using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

[RequireComponent(typeof(Animator))]
public class NPC_animation : MonoBehaviour
{
    public bool maleGender = false;

    //Script Reference
    private NetworkBehaviour networkBehaviour;
    private Animator m_animator;
    private Rigidbody m_rb;

    //Animation State
    const string AnimController = "AnimController";
    public enum anim { sitting, reacting, approaching};

    // - Sitting
    bool isPlaySitting = false;
    const string sitX = "sitX";
    const string sitY = "sitY";
    float sitAnimPeriod = 10f; // it will be randomize at start Function


    // - Reaction
    public int reactLevel = 0;
    [System.NonSerialized] public bool isReacting = false;
    const string reaction = "reaction";
    float reactionAnimPeriod = 10f; // it will be randomize at start Function

    // - Approaching
    [System.NonSerialized] public Transform followTarget;
    [System.NonSerialized] public bool isFollower = false;
    const string approaching = "approaching";
    enum approachEnum { getUp, walking, yelling };
    [Range(0.3f, 1.5f)]
    public float minApproachDist = 0.1f;
    public float moveSpeed = 10f;
    public float posDamp = 1f;
    public float rotDamp = 5f;
    private Vector3 oriPos; //you dont need to set it up in the editor, cause the Server position is absolute
    private Quaternion oriRot; //you dont need to set it up in the editor, cause the Server rotation is absolute
    

    //Dummy Enum
    private IEnumerator dummyCoroutine;


    private void Awake()
    {
        networkBehaviour = GetComponentInParent<NetworkBehaviour>();
        m_animator = GetComponent<Animator>();
        m_rb = GetComponent<Rigidbody>();
        m_rb.isKinematic = true;
        sitAnimPeriod = Random.Range(5, 30);
        reactionAnimPeriod = Random.Range(5, 30);
    }

    // Start is called before the first frame update
    void Start()
    {
        //set the animcoroutine
        dummyCoroutine = DummyCoroutine();

        //record the pos and Rot
        oriPos = this.transform.position;
        oriRot = this.transform.rotation;

    }


    private void Update()
    {
        //Dont do anything if it is not the server
        if (!networkBehaviour.isServer)
            return;

        int currentAnim = m_animator.GetInteger(AnimController);

        //following takes priority than the other
        if (isFollower && currentAnim == (int)anim.approaching)
        {
            if (followTarget)
                PlayApproach(followTarget);
            return;
        }

        //if this guy is not in the original places, 
        //then go back first, before doing other animation
        if (!isBackInPlace())
        {
            PlayApproach_GoingBack();
            return;
        }
            
        


        if (!isPlaySitting &&  currentAnim == (int)anim.sitting)
        {
            // Sit idle animation
            StartCoroutine(PlaySitIdleAnim());
        }


        else if (!isReacting && currentAnim == (int)anim.reacting)
        {
            // Reaction animation
            StopCoroutine(dummyCoroutine);
            dummyCoroutine = PlayReactionAnim();
            StartCoroutine(PlayReactionAnim());
        }
        


    }
    //to change the animation state
    public void ChangeAnimation(anim _state)
    {
        if(!m_animator)
            m_animator = GetComponent<Animator>();

        switch (_state)
        {
            case anim.sitting:  m_animator.SetInteger(AnimController, (int)anim.sitting); break;
            case anim.reacting:  m_animator.SetInteger(AnimController, (int)anim.reacting); break;
            case anim.approaching:  m_animator.SetInteger(AnimController, (int)anim.approaching); break;
            

            //if no animation just play the sitting anim..
            default: m_animator.SetInteger(AnimController, (int)anim.sitting); break;
        }
    }
    

    //to play the idle animation
    public IEnumerator PlaySitIdleAnim()
    {
        isPlaySitting = true;
        float randomSitX = Mathf.Round(Random.Range(-1f, 1f));
        float randomSitY = Mathf.Round(Random.Range(-1f, 1f));

        //lerp the idle value
        float currentSitX = m_animator.GetFloat(sitX);
        float currentSitY = m_animator.GetFloat(sitY);
        
        StartCoroutine(CoroutineExtensions.AnimationOverTime(m_animator, sitX, randomSitX, 3f));
        StartCoroutine(CoroutineExtensions.AnimationOverTime(m_animator, sitY, randomSitY, 3f));
        
        m_animator.SetFloat(sitX, currentSitX);
        m_animator.SetFloat(sitY, currentSitY);


        yield return new WaitForSeconds(sitAnimPeriod);
        isPlaySitting = false;
    }


    //play reaction animation
    public IEnumerator PlayReactionAnim()
    {
        isReacting = true;

        //var declaration
        int randomReaction = 0;
        float currentReaction = m_animator.GetFloat(reaction);

        //Angry
        if (reactLevel < 0)
        {
            randomReaction = Random.Range(-5, 0); 
        }
        

        //Cheering
        else if( reactLevel > 0) 
        {
            randomReaction = Random.Range(1, 6); 
        }


        StartCoroutine(CoroutineExtensions.AnimationOverTime(m_animator, reaction, randomReaction, 3f));
        m_animator.SetFloat(reaction, currentReaction);
        yield return new WaitForSeconds(reactionAnimPeriod);


        isReacting = false;
    }

    /// <summary>
    /// To Play the approach animation
    /// </summary>
    /// <param name="_followTarget">the target to follow</param>
    /// <returns>When false, it hasnt reach yet, when true, it has reach, and currently yeeling</returns>
    public bool PlayApproach(Transform _followTarget)
    {
        if (!_followTarget)// null reference checker
        {
            Debug.LogError("Error: no target");
            return false;
        }
            
        //the actual Moving
        // - compare the distance., if the distance still far.. then keep moving.
        if (Vector3.Distance(this.transform.position, _followTarget.position) >= minApproachDist * 2)
        {
            PlayApproach_WalkingTowards(_followTarget);
            return false;
        }
        //if reach, change to yelling.
        else
        {
            PlayApproach_Yelling(_followTarget);
            return true;
        }
            
    }


    /*
    //to play the Getting up animation
    private IEnumerator PlayApproach_GetUp(System.Action<bool> callback = null)
    {
        //0.  Play get up animation
        ChangeAnimation(anim.approaching);
        m_animator.SetFloat(approaching, ((int)approachEnum.getUp));
        yield return new WaitForSeconds(2.267f); // the length of getting up
        callback(true);
    }
    */


    //to approach the player
    private void PlayApproach_WalkingTowards(Transform _followTarget)
    {
        //1. Play Moving animation
        ChangeAnimation(anim.approaching);
        m_rb.isKinematic = false;

        //lerp the animation to have a smoother transition
        m_animator.SetFloat(approaching,
                            Mathf.Lerp(m_animator.GetFloat(approaching), (int)approachEnum.walking, Time.deltaTime * moveSpeed)
                            );

        //move this NPC towards the target
        this.transform.position = Vector3.MoveTowards(this.transform.position, 
                                                      _followTarget.position - new Vector3(minApproachDist, 0, minApproachDist), 
                                                      Time.deltaTime * posDamp);
        
        // This constructs a rotation looking in the direction of our target,
        Vector3 targetPos = new Vector3(_followTarget.position.x,
                                        this.transform.position.y,
                                        _followTarget.position.z);
        this.transform.LookAt(targetPos);

        /*
        //smoother transition, but not locked in X and Z
        Quaternion targetRotation = Quaternion.LookRotation(_followTarget.position - this.transform.position,Vector3.up);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation,
                                                  targetRotation,
                                                  Time.deltaTime * rotDamp);
        */
    }


    /// <summary>
    /// Order the NPC Character to perform Yelling while standing.
    /// </summary>
    private void PlayApproach_Yelling(Transform _followTarget)
    {
        // keep looking at the target,
        Vector3 targetPos = new Vector3(_followTarget.position.x,
                                        this.transform.position.y,
                                        _followTarget.position.z);
        this.transform.LookAt(targetPos);

        //2. Play Yelling Animation
        ChangeAnimation(anim.approaching);
        m_rb.isKinematic = false;

        //m_animator.SetFloat(approaching, ((int)approachEnum.yelling));
        //lerp it to have a smooth transition
        m_animator.SetFloat(approaching, 
                            Mathf.Lerp(m_animator.GetFloat(approaching), (int)approachEnum.yelling,Time.deltaTime * 5)
                            );

    }


    /// <summary>
    /// Order the NPC Character to go back to the seat.
    /// </summary>
    /// <returns>When false, it hasnt reach yet, 
    /// when true, the npc has reach, and sitting</returns>
    private bool PlayApproach_GoingBack()
    {
        //1. Play Moving animation
        ChangeAnimation(anim.approaching);
        m_rb.isKinematic = false;

        //lerp the animation to have a smoother transition
        m_animator.SetFloat(approaching,
                            Mathf.Lerp(m_animator.GetFloat(approaching), (int)approachEnum.walking, Time.deltaTime * moveSpeed)
                            );

        //move this NPC towards the target
        this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                      oriPos,
                                                      Time.deltaTime * posDamp);

        // This constructs a rotation looking in the direction of our target,
        Vector3 targetPos = new Vector3(oriPos.x,
                                        this.transform.position.y,
                                        oriPos.z);
        this.transform.LookAt(targetPos);


        //2.  sitting
        //check distance and play sitting animation after the distance is reached
        if (isBackInPlace())
        {
            ChangeAnimation(anim.sitting);
            this.transform.rotation = oriRot;
            m_rb.isKinematic = true;
            return true;
        }
        else 
            return false;
    }

    private bool isBackInPlace()
    {
        if (Vector3.Distance(this.transform.position, oriPos) <= 0.1f)
            return true;
        else
            return false;

    }




    //this is just a dummy in order to prevent null refenrence at the beginning.
    public IEnumerator DummyCoroutine()
    {
        yield return new WaitForSeconds(0f);
    }
}
