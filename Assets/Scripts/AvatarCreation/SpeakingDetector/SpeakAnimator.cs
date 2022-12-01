using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Dissonance;

namespace AvatarCreation
{
    public class SpeakAnimator : MonoBehaviour
    {
        [Header("GameObject Animator")]
        public SkinnedMeshRenderer headAnimator;
        public SkinnedMeshRenderer eyeAnimator;

        [Header("Script References")]
        public Ui_AvatarFacialSelection facial;
        /*
        //Microphone state
        private IDissonancePlayer _player;
        private VoicePlayerState _state;

        //Animator state
        bool isReadyToSpeak = true;
        bool isReadyToBlink = true;
        bool isReadyToEyebrow = true;
        bool isReadyToEyes = true;



        //get all the component needed for speaking dtection
        private void OnEnable()
        {
            _player = GetComponent<IDissonancePlayer>();
            StartCoroutine(FindPlayerState());
            
        }

        private void OnDisable()
        {
            //StopAllCoroutines();
        }


        private IEnumerator FindPlayerState()
        {
            //Wait until player tracking has initialized
            while (!_player.IsTracking)
                yield return null;

            //Now ask Dissonance for the object which represents the state of this player
            //The loop is necessary in case Dissonance is still initializing this player into the network session
            while (_state == null)
            {
                _state = FindObjectOfType<DissonanceComms>().FindPlayer(_player.PlayerId);
                yield return null;
            }
        }


        /// <summary>
        /// is the avatar is speaking
        /// </summary>
        private bool isSpeaking
        {
            get
            {
                return _player.Type == NetworkPlayerType.Remote
                    && _state != null && _state.IsSpeaking;
            }
        }

        void Start()
        {
            isReadyToSpeak = true;
            isReadyToEyebrow = true;
            isReadyToEyes = true;
        }


        // Update is called once per frame
        void Update()
        {

            //For animating the munchy mouth
            if (isReadyToSpeak)
            {
                isReadyToSpeak = false;
                StartCoroutine(AnimateSpeak(isSpeaking));
            }


            //for animating the Eyebrows
            if (isReadyToEyebrow)
            {
                isReadyToEyebrow = false;
                StartCoroutine(AnimateEyebrows());
            }

            if (isReadyToEyes)
            {
                isReadyToEyes = false;
                StartCoroutine(AnimateEyes());

            }


            //For animating the Blinky2 eyelid
            if (isReadyToBlink)
            {
                isReadyToBlink = false;
                StartCoroutine(Blink());
            }
        }




        int mouthBlend;
        /// <summary>
        /// to animator mouth
        /// </summary>
        public IEnumerator AnimateSpeak(bool _isSpeaking)
        {
            //Open the mouth
            if (_isSpeaking)
            {
                mouthBlend = (int)Random.Range(facial.blendAnimMouth.x, facial.blendAnimMouth.y + 1);

                //print("Start Talking: ");
                //assign the blendshapes
                //open the mouth
                yield return StartCoroutine(CoroutineExtensions.blendShapesOverTime(headAnimator, mouthBlend, 0, 100, 0.1f));
                //wait a sec
                yield return new WaitForSeconds(0.025f); //-> human mouth close like 0.05 to 0.25 sec.
                //close the mouth
                yield return StartCoroutine(CoroutineExtensions.blendShapesOverTime(headAnimator, mouthBlend, 
                                                                                    headAnimator.GetBlendShapeWeight(mouthBlend), 
                                                                                    0, 0.1f));

                //reenable the speaking
                isReadyToSpeak = true;
            }


            else
            {
                //print("StopTalking ");
                isReadyToSpeak = true;
            }
        }

        int eyebrowBlend, eyebrowRand, eyebrowDuration;
        /// <summary>
        /// To animate Eyebrow
        /// </summary>
        /// <returns></returns>
        public IEnumerator AnimateEyebrows()
        {

            eyebrowBlend = (int)Random.Range(facial.blendAnimEyebrow.x, facial.blendAnimEyebrow.y + 1);
            eyebrowRand = Random.Range(0, 100);
            eyebrowDuration = Random.Range(3, 7);
            //animate eyebrow
            yield return StartCoroutine(CoroutineExtensions.blendShapesOverTime(headAnimator, eyebrowBlend, 0, eyebrowRand, 0.15f));
            yield return new WaitForSeconds(eyebrowDuration);
            yield return StartCoroutine(CoroutineExtensions.blendShapesOverTime(headAnimator, eyebrowBlend,
                                                                                headAnimator.GetBlendShapeWeight(eyebrowBlend),
                                                                                0, 0.15f));

            isReadyToEyebrow = true;
        }



        int eyeBlend, eyeDuration, eyeCycle;
        /// <summary>
        /// To animate Eyebrow
        /// </summary>
        /// <returns></returns>
        public IEnumerator AnimateEyes()
        {
            //randomize the eyes
            eyeBlend = (int)Random.Range(facial.blendAnimEye.x, facial.blendAnimEye.y + 1);
            //randomize the duration
            eyeCycle = Random.Range(3, 7);

            //set the blend shapes weight
            eyeAnimator.SetBlendShapeWeight(eyeBlend, 100);

            yield return new WaitForSeconds(Random.Range(0.5f, 2f)); // average human eye moves between 0.5-2 seconds once. // human cannot focus after all... XP

            eyeAnimator.SetBlendShapeWeight(eyeBlend, 0);

            yield return new WaitForSeconds(eyeCycle); // the next cycle 'till moving the eye again

            //animate eyes
            isReadyToEyes = true;
        }



        /// <summary>
        /// for routine of blinking
        /// </summary>
        /// <returns></returns>
        private IEnumerator Blink()
        {
            headAnimator.SetBlendShapeWeight(facial.blendAnimEye_closed, 0);
            yield return new WaitForSeconds(Random.Range(3, 7)); // average human eye blinks between 4-7 seconds once.

            headAnimator.SetBlendShapeWeight(facial.blendAnimEye_closed, 100);
            yield return new WaitForSeconds(0.2f);

            headAnimator.SetBlendShapeWeight(facial.blendAnimEye_closed, 0);

            yield return new WaitForSeconds(1f);
            isReadyToBlink = true;
        }

        
        /// <summary>
        /// to animate the eye lid
        /// </summary>
        /// <param name="_isOpened">open or close the eyelid</param>
        private void isBlinking(bool _isOpened)
        {
            if(_isOpened)
                headAnimator.SetBlendShapeWeight(facial.blendAnimEye_closed, 0);
            else
                headAnimator.SetBlendShapeWeight(facial.blendAnimEye_closed, 100);
        }
        


        /// <summary>
        /// for smoothing out the transition
        /// </summary>
        /// <returns></returns>
        private float Smoothing(float _oriValue, float _targetValue)
        {
            float time = Time.unscaledDeltaTime;
            float result;

            result = Mathf.MoveTowards(_oriValue , _targetValue, time);
            return result;
        }
        */

    }
}
