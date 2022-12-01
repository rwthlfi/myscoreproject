using UnityEngine;
using UnityEngine.UI;

namespace AvatarCreation
{

    //To set up the height of the avatar
    public class AvatarSetting_Height : MonoBehaviour
    {
        [Header("Avatar GameObject")]
        public Transform avatarRig; // attach the main gameObject of the Avatar rig here

        //cache variable
        float cacheScale = 1f;
        Vector3 targetScale = Vector3.one;



        //to Load up the avatar Height
        public void LoadAvatarHeight()
        {
            // get the scaling and put it in the target
            cacheScale = PlayerPrefs.GetFloat(PrefDataList.avatarHeight);
            //print("cache " + cacheScale);

            targetScale = new Vector3(cacheScale, cacheScale, cacheScale);

            //lerping the scaling to the targeted scale vector
            if (avatarRig != null)
                StartCoroutine(LerpingExtensions.ScaleTo(avatarRig.transform, targetScale, 1f));
        }


        //attach this to the Height slider
        public void Ui_heightSlider(Slider _heightSlider)
        {
            //update the local scale of the avatar rig
            if (avatarRig != null)
                avatarRig.localScale = new Vector3(_heightSlider.value, _heightSlider.value, _heightSlider.value);


            //put it in the preferences
            PlayerPrefs.SetFloat(PrefDataList.avatarHeight, _heightSlider.value);
            LoadAvatarHeight();
        }


    }
}
