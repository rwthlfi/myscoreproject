﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using System.Linq;

public class NpcGroupController : NetworkBehaviour
{
    [Header("Stage Variable")]
    public MeshRenderer StageMesh;
    public List<Transform> doorLocation = new List<Transform>();
    public List<AudioClip> doorAudio = new List<AudioClip>();
    public List<Transform> telephoneLocation = new List<Transform>();
    public List<AudioClip> telephoneAudio = new List<AudioClip>();

    [Header("NPC Variable")]
    public List<NPC_animation> npcList = new List<NPC_animation>();

    [Header("Audio Var")]
    public List<AudioClip> crowdAngryAudio = new List<AudioClip>();
    public List<AudioClip> crowdCheeringAudio = new List<AudioClip>();

    [Header("Equipment  Var")]
    public List<GameObject> angryObject= new List<GameObject>();
    public List<GameObject> cheeringObject= new List<GameObject>();

    public IEnumerator npcCoroutine;

    [Header("SYNCHRO Variable")]
    [Header("FollowersID")]
    [SyncVar(hook = nameof(OnFollowerChanged))]
    public string followersID; // this contains the list of the followers id.. seperated by '|'

    [Header("Cache Reaction")]
    [SyncVar(hook = nameof(OnReactionChanged))]
    public int cacheReaction = int.MaxValue; // this contains the current NPC Reaction

    private bool crowdReadyToReact = true;
    private NetworkIdentity currentPlayerBeingFollowed = null;




    private void Start()
    {
        //Store every npc in this game object
        foreach(Transform trans in this.transform)
            npcList.Add(trans.GetComponent<NPC_animation>());

        //set the Dummycoroutine
        npcCoroutine = Reaction(cacheReaction);

        //init the server exclusive npc follower
        if (isServer)
        {
            followersID = InitFollower(1);
            Debug.Log("Server " + followersID);
            cacheReaction = 0;
        }
            
    }


    private void Update()
    {
        if (isServer)
            return;

        //The animation do synchron by the Server.
        //but the throwable obj and audio sound is not.
        //Hence the if-else below is the throwing and sound
        if (cacheReaction != 0 && crowdReadyToReact)
        {
            crowdReadyToReact = false;
            StartCoroutine(CrowdReactionListener(cacheReaction));
        }
        else if (cacheReaction == 0)
            crowdReadyToReact = true;
            
            

    }


    private string InitFollower(int _total)
    {
        //if there is no follower id at the beginning of the scene
        if (followersID == "")
        {
            string str = "";
            foreach (int i in ConverterFunction.generateRandom_NoDuplicate(_total, npcList.Count))
                str += i + "|";
            followersID = str;
            return str;
        }
        else
            return followersID;
    }

    //this synchronize follower is to prevent the animation being played by the local player
    //basically the follower will be own by the server.
    void OnFollowerChanged(string _Old, string _New)
    {
        //change name
        print("followerlist " + followersID);
    }

    [Command(requiresAuthority = false)]
    public void CmdRenewingFollower(int _totalFollower) 
    { 
        followersID = RenewFollower(_totalFollower);
        Debug.Log("Follower has been renewed: " + followersID);
        if (currentPlayerBeingFollowed)
            FollowTarget(currentPlayerBeingFollowed);
    }

    //to initialize the random follower from the npcList pool

    private string RenewFollower(int _total)
    {
        //Get the currentList Total
        string[] current = followersID.Split('|');

        //get the difference. "-1" is because during the split, it create an empty extra.
        int diff = _total - (current.Length - 1);


        //if the difference is extra, then add the extra follower
        if (diff > 0)
        {
            //create an exception, in order to exclude the follower that are already in the list
            List<int> exception = new List<int>();
            for (int i = 0; i < current.Length - 1; i++)
            {
                exception.Add(ConverterFunction.StringToInt(current[i]));
                //print("CurrentList: " + current[i]);
            }

            //generate the new random duplicate
            List<int> newFollowerList = ConverterFunction.generateRandom_NoDuplicate(
                                                        diff, //how many you should add
                                                        npcList.Count,  // the maximum number to be random
                                                        exception // convert string array to int
                                                        );
            string extraFollower = "";
            foreach (int i in newFollowerList)
                extraFollower += i.ToString() + "|";

            //store the new follower
            followersID += extraFollower;

            return followersID;
        }

        //if the current follower is too much, then remove the extra follower
        else if (diff < 0)
        {
            //change the difference to absolute
            int diffAbs = Mathf.Abs(diff);

            //Remove x element from the current List
            List<string> tempList = current.ToList();
            tempList.RemoveRange(0, diffAbs);
            //set the "isFollower" to false 

            
            foreach (NPC_animation npAnim in npcList)
            {
                npAnim.isFollower = false;
            }
            
            /*
            for (int i = 0; i < (current.Length - 1) - diffAbs; i++)
            {
                this.transform.GetChild(ConverterFunction.StringToInt(current[i]))
                              .GetComponent<NPC_animation>().isFollower = false;
                //npcList[ConverterFunction.StringToInt(current[i])].isFollower = false;
            }
            */



            string str = "";
            for (int i = 0; i < tempList.Count - 1; i++)
                str += tempList[i] + "|";

            //store the new follower
            followersID = str;
            return followersID;
        }

        else
        {
            //Do nothing, and just return the follower id
            //if (diff == 0)
            return followersID;
        }
    }



    //to synchronize the NPC reaction
    void OnReactionChanged(int _Old, int _New)
    {
        //change name
        print("Reaction" + cacheReaction);
        if (cacheReaction == 0)
            NoReacting();
    }

    [Command(requiresAuthority = false)]
    public void CmdReactionChanged(int _newReaction)
    {
        cacheReaction = _newReaction;
    }




    //to trigger a random door sound.
    public void RandomDoorBanging()
    {
        //get the random Location
        Transform tr = doorLocation[Random.Range(0, doorLocation.Count)];

        //get the random audio
        int randomDoorAudio = Random.Range(0, doorAudio.Count);

        //play the audio once.
        tr.GetComponent<AudioSource>().loop = false;
        tr.GetComponent<AudioSource>().clip = doorAudio[randomDoorAudio];

        tr.GetComponent<AudioSource>().Play();
    }


    //to trigger a random telephone ring
    public void RandomTelephoneRinging()
    {
        //get the random TelephoneLocation

        Transform tr = telephoneLocation[Random.Range(0, telephoneLocation.Count)];

        //get the random audio
        int randomTelpAudio = Random.Range(0, telephoneAudio.Count);

        //play the audio once.
        tr.GetComponent<AudioSource>().loop = false;
        tr.GetComponent<AudioSource>().clip = telephoneAudio[randomTelpAudio];

        tr.GetComponent<AudioSource>().Play();
    }



    public void NoReacting()
    {
        //cache the reaction
        cacheReaction = 0;
        StopCoroutine(npcCoroutine);
        foreach (NPC_animation npAnim in npcList)
        { 
            //exception for folower
            if (npAnim.isFollower)
                continue;

            //Play animation on the random Crowd from the npcList
            npAnim.reactLevel = 0;
            npAnim.ChangeAnimation(NPC_animation.anim.sitting);
            //revert the sound back
            npAnim.GetComponent<AudioSource>().Stop();
        }
        print("no reaction");
    }


    //to start the reaction 
    public void StartReaction(int _severity)
    {
        Debug.Log("Server send Reaction: " + _severity);
        //cache the reaction
        cacheReaction = _severity;

        //var declaration
        StopCoroutine(npcCoroutine);
        npcCoroutine = Reaction(cacheReaction);
        StartCoroutine(npcCoroutine);
    }

    private IEnumerator Reaction(int _severity)
    {
        //print("Severity " + _severity);
        //means; no reaction needed, then just revert to sitting
        if(_severity == 0)
        {
            NoReacting();
            yield return null;
        }

        float sec;
        //get random crowd & manipulate them
        foreach(NPC_animation npAnim in npcList)
        {

            //exception for folower
            if (npAnim.isFollower)
                continue;


            //Reset the animation first
            npAnim.reactLevel = 0;
            npAnim.ChangeAnimation(NPC_animation.anim.sitting);
            npAnim.GetComponent<AudioSource>().Stop();

            // - Determine to React or not 
            npAnim.isReacting = false; //restarting the npc reaction

            // convert to abs value
            int absSeverity = Mathf.Abs(_severity);
            //percentage based on the severity level(currently we have 5)
            //therefore by converting the severity to abs, and randomize it between the max. SeverityLevel, you can get the percentage
            //e.g. : _severity 1 will have about 20% of the crowd affected, cause its randomize between 1,2,3,4,5
            //if it hits 5 then the npc will get triggered.
            //e.g. : _severity 2 will have about 40% and so on.
            int affected = Random.Range(absSeverity, 6);

            //Debug.Log("Severtiy " + _severity);
            //if it is affected
            if(affected == 5)
            {
                //Play animation on the random Crowd from the npcList
                npAnim.reactLevel = _severity;
                npAnim.ChangeAnimation(NPC_animation.anim.reacting);

                /*
                //get and assign the sound to the selected crowd
                if (_severity < 0) //angry
                {
                    int randomAudio = Random.Range(0, crowdAngryAudio.Count);
                    npAnim.GetComponent<AudioSource>().loop = true;
                    npAnim.GetComponent<AudioSource>().clip = crowdAngryAudio[randomAudio];
                }

                else if(_severity > 0) // Cheering
                {
                    int randomAudio = Random.Range(0, crowdCheeringAudio.Count);
                    npAnim.GetComponent<AudioSource>().loop = true;
                    npAnim.GetComponent<AudioSource>().clip = crowdCheeringAudio[randomAudio];
                }

                //Play sound on the crowd
                npAnim.GetComponent<AudioSource>().Play();
                */
                /*
                //Throws things when the level reach 4
                if (_severity <= -4)
                {
                    CrowdThrowsGoodObj(false);
                }
                else if (_severity >= 4)
                    CrowdThrowsGoodObj(true);
                */
            }

            //throws a new waiting time.
            sec = Random.Range(0.05f, 0.25f);
            yield return new WaitForSeconds(sec);
        }


        //Recursive function
        //Loop the reaction back
        if (cacheReaction != 0)
        {
            StopCoroutine(npcCoroutine);
            npcCoroutine = Reaction(cacheReaction);
            StartCoroutine(npcCoroutine);
        }
        else if (cacheReaction == 0)
        {
            StopCoroutine(npcCoroutine);
        }
        yield return null;
    }

    private IEnumerator CrowdReactionListener(int _severity)
    {
        //means; no reaction needed, then just revert to sitting
        if (_severity == 0)
        {
            NoReacting();
            yield return null;
        }

        float sec;
        //get random crowd & manipulate them
        foreach (NPC_animation npAnim in npcList)
        {
            //exception for follower
            if (npAnim.isFollower)
                continue;

            int absSeverity = Mathf.Abs(_severity);
            int affected = Random.Range(absSeverity, 6);

            //Play Audio & throw things when it reach level 4
            if (affected == 5 && cacheReaction < 0)
            {
                CrowdCheering(false, true); // angry audio
                if (_severity <= -4) 
                    CrowdThrowsGoodObj(false);
            }
            else if (affected == 5 && cacheReaction > 0)
            {
                CrowdCheering(true, true); // happy audio
                if (_severity >= 4)
                    CrowdThrowsGoodObj(true);
            }

            //throws a new waiting time.
            sec = Random.Range(0.05f, 0.25f);
            yield return new WaitForSeconds(sec);
        }

        crowdReadyToReact = true;
    }
    


    /// <summary>
    /// to Play a cheer - happy/angry sound
    /// </summary>
    /// <param name="_isCheering">happy is true, angry is false</param>
    /// <param name="_isLooping"> should the sound be looping ?</param>
    public void CrowdCheering(bool _isCheering, bool _isLooping)
    {
        //get the random NPC
        GameObject npc = npcList[Random.Range(0, npcList.Count)].gameObject;
        //get random Audio
        int randomAudio = Random.Range(0, crowdCheeringAudio.Count);
        //trigger the audio Component
        npc.GetComponent<AudioSource>().loop = _isLooping;

        //get the audio list
        if (_isCheering) // happy cheering
            npc.GetComponent<AudioSource>().clip = crowdCheeringAudio[randomAudio];
        else // angry cheering
            npc.GetComponent<AudioSource>().clip = crowdAngryAudio[randomAudio];
        
        //Play the audio
        npc.GetComponent<AudioSource>().Play();
    }



    public void CrowdThrowsGoodObj(bool _good)
    {
        int randomObj = Random.Range(0, cheeringObject.Count);
        if (_good)
        {
            GameObject go = Instantiate(cheeringObject[randomObj]);
            ThrowsObj(go);
            Destroy(go, 10f);
        }
        else if (!_good)
        {
            GameObject go = Instantiate(angryObject[randomObj]);
            ThrowsObj(go);
            Destroy(go, 10f);
        }
    }


    /// <summary>
    /// This is to throw random Object
    /// </summary>
    private void ThrowsObj(GameObject _go)
    {
        //start position
        Vector3 s = getRandomNpcPos();
        _go.transform.position = getRandomNpcPos();
        //target
        Vector3 p = getRandomStageTarget();

        //default
        float gravity = Physics.gravity.magnitude;


        // Selected angle in radians
        float angle = RandomAngle() * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(s.x, 0, s.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = s.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion) * (p.x > transform.position.x ? 1 : -1);

        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;


        // Fire!
        //if it is not available.
        if (float.IsNaN(finalVelocity.x) || float.IsNaN(finalVelocity.y) || float.IsNaN(finalVelocity.z))
        {
            Debug.Log("The force is not available");
            return;
        }


        _go.GetComponent<Rigidbody>().velocity = finalVelocity;

        float torque = Random.Range(-45, 45f);
        _go.GetComponent<Rigidbody>().AddTorque(torque, torque, torque);
        // Alternative way:
        //rigid.AddForce(finalVelocity * rigid.mass, ForceMode.Impulse);
    }

    private Vector3 getRandomNpcPos()
    {
        int randomNPC = Random.Range(0, npcList.Count);

        //print("originName " + npcList[randomNPC].name);
        //print("originPos " + npcList[randomNPC].transform.position);
        return npcList[randomNPC].transform.position + new Vector3(0,2f,0); //+ the height of the NPC
    }

    private Vector3 getRandomStageTarget()
    {
        Bounds bounds = StageMesh.bounds;
        float margin = 0f;
        Vector3 randomPos = new Vector3(Random.Range(bounds.min.x + margin, bounds.max.x - margin),
                                        bounds.center.y,
                                        Random.Range(bounds.min.z + margin, bounds.max.z - margin)
                                        );
        return randomPos;
    }

    private float RandomAngle()
    {
        return Random.Range(10f, 30f);
    }


    //To make the npc follow the player
    //this is not a local function but a function that is executed by the server.
    public void FollowTarget(NetworkIdentity _netId)
    {
        //logic
        print("yeaah!!! ");
        
        //get the assigned NPC to follow
        string[] str = followersID.Split('|');

        //dont take the last one
        for(int i = 0; i < str.Length -1; i++)
        {
            //Debug.Log("follower " + str[i]);
            NPC_animation t = this.transform.GetChild(ConverterFunction.StringToInt(str[i]))
                                            .GetComponent<NPC_animation>();
            //set the target player
            t.followTarget = _netId.transform;
            //set to a follower
            t.isFollower = true;
            //get the NPC Animator and change it to walking.
            t.ChangeAnimation(NPC_animation.anim.approaching);
            
        }
        //register the player
        currentPlayerBeingFollowed = _netId;
    }

    public void UnFollowTarget()
    {
        print("no ");
        //get all the npc and set it to not a follower
        foreach(NPC_animation npAnim in npcList)
        {
            //NPC_animation t = npc.GetComponent<NPC_animation>();
            npAnim.isFollower = false;


            //exception, if severity is not 0, change it to reaction
            //npAnim.ChangeAnimation(NPC_animation.anim.sitting);
        }
        //de-register the player
        currentPlayerBeingFollowed = null;
    }



    //this is just a dummy in order to prevent null refenrence at the beginning.
    public IEnumerator DummyCoroutine()
    {
        yield return new WaitForSeconds(0f);
    }
}
