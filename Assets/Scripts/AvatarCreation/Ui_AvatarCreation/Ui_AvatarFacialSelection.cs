using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AvatarCreation
{
    public class Ui_AvatarFacialSelection : MonoBehaviour
    {
        [Header("Object Reference")]
        //public SkinnedMeshRenderer skinnedMeshRenderer;
        public List<string> savedBlendShapes;
        public Vector2 blendAnimMouth;
        public Vector2 blendAnimEyebrow;
        public Vector2 blendAnimEye; // The eye is located in the Eyes + mouth
        public int blendAnimEye_closed;

        public Vector2 faceID; // 25 - 30
        public Vector2 eyebrowsID; //31 - 34
        public Vector2 eyesID; // 35 -42
        public Vector2 cheeksID; // 43 - 44
        public Vector2 noseID; // 45 - 50
        public Vector2 mouthID; // 51 - 56
        public Vector2 chinID; // 57 - 60 

        [Header("Ui Reference")]
        public Transform content;
        public List<GameObject> blendList = new List<GameObject>();
        public Button allSelection;
        public Image mainImage;
        public Sprite imageFace;
        public Sprite imageEyebrows;
        public Sprite imageEye;
        public Sprite imageCheeks;
        public Sprite imageNose;
        public Sprite imageMouth;
        public Sprite imageChin;
        public Sprite imageAll;


        [Header("Script Reference")]
        public AvatarSetting_Renderer avatarRenderer;
        public SimpleObjectPool simpleObjectPool;

        // Start is called before the first frame update
        void Start()
        {
        }


        // Update is called once per frame
        void Update()
        {

        }


        /// <summary>
        /// Init all the blendShapes
        /// </summary>
        public void InitAllBlendSlider()
        {
            foreach (Transform child in content)
                Destroy(child.gameObject);

            string[] blends = getBlendShapeNames(avatarRenderer.headRenderer);


            //Get the blendshapes and assign them
            for (int i = 0; i < blends.Length; i++)
            {
                //create the Slider GameObject
                GameObject blendShapesGO = CreateBlendShapesSlider(blends[i], i, 100);
                blendShapesGO.transform.SetParent(content, false);
                //add it to the list for later usage
                blendList.Add(blendShapesGO);
            }
        }

        //function to create the bookmarked button in the website
        private GameObject CreateBlendShapesSlider(string _blendName, int _idx, float _value)
        {
            //get the reference
            GameObject gObj = simpleObjectPool.GetObject();
            BlendSliderRef blendRef = gObj.GetComponent<BlendSliderRef>();

            //assign the blendshapes name and current slider value for aesthetic
            blendRef.blendName.text = _blendName;
            blendRef.blendSlider.Set(ConverterFunction.StringToInt(savedBlendShapes[_idx]),false);

            //assign the ability to the slider
            blendRef.blendSlider.onValueChanged.AddListener(delegate
            {
                //put function here
                avatarRenderer.headRenderer.SetBlendShapeWeight(_idx, blendRef.blendSlider.value);

                //add ability to save the player prefs too for later usage
                UpdateSavedBlendShapes(_idx, (int)blendRef.blendSlider.value);
            });

            //return the gameObject back
            return gObj;
        }


        /// <summary>
        /// to get the saved blend data
        /// </summary>
        public void InitBlendValue()
        {
            //get the savedBlendShapes
            savedBlendShapes = ConverterFunction.splitGivenString(PlayerPrefs.GetString(PrefDataList.avatarBlendshapes), "|");
            
            //but to avoid null reference. and supposing there is no blendshapes being saved yet
            //initiate them
            if(savedBlendShapes.Count <=0)
            {
                string s = "";

                //randomize the blend; But only the facial and not the expression
                for(int i = 0; i < getBlendShapeNames(avatarRenderer.headRenderer).Count(); i++)
                {
                    if (i < (int)faceID.x) // below this is the facial, so we dont need to randomize it.
                        s += "0|" ;
                    else
                        s +=  Random.Range(0, 100 ) + "|";
                }
                
                /*
                foreach(string _str in getBlendShapeNames(avatarRenderer.headRenderer))
                    s += Random.Range(0, 100) + ",";
                */


                PlayerPrefs.SetString(PrefDataList.avatarBlendshapes, s);
                //then saved them
                savedBlendShapes = ConverterFunction.splitGivenString(PlayerPrefs.GetString(PrefDataList.avatarBlendshapes), "|");
            }

            //print("facial " + PlayerPrefs.GetString(PrefDataList.avatarBlendshapes));

        }


        /// <summary>
        /// to get all the blendshapes names
        /// </summary>
        /// <param name="_renderer"></param>
        /// <returns></returns>
        public string[] getBlendShapeNames(SkinnedMeshRenderer _renderer)
        {
            Mesh m = _renderer.sharedMesh;
            string[] arr;
            arr = new string[m.blendShapeCount];
            for (int i = 0; i < m.blendShapeCount; i++)
            {
                string s = m.GetBlendShapeName(i);
                //print("Blend Shape: " + i + " " + s); // 
                arr[i] = s;
            }
            return arr;
        }


        public string UpdateSavedBlendShapes(int _pos, int _value)
        {
            //get the save value
            List<string> aList = ConverterFunction.splitGivenString(PlayerPrefs.GetString(PrefDataList.avatarBlendshapes), "|");

            string r ="";
            //modify the value
            aList[_pos] = _value.ToString();

            foreach (string str in aList)
                r += str + "|";

            //save and return the value
            PlayerPrefs.SetString(PrefDataList.avatarBlendshapes, r);
            return r;

        }

        public void RandomizeFacial()
        {
            string s = "";
            int ran = 0;
            for (int i = 0; i < getBlendShapeNames(avatarRenderer.headRenderer).Count(); i++)
            {
                if (i < (int)faceID.x) // below this is the facial, so we dont need to randomize it.
                    s += "0|";
                else
                {
                    ran = Random.Range(0, 100);
                    //apply the blend weight
                    avatarRenderer.headRenderer.SetBlendShapeWeight(i, ran);
                    s += ran + "|";

                }
            }
            PlayerPrefs.SetString(PrefDataList.avatarBlendshapes, s);

            //apply it

        }

        public void ResetFacialToZero()
        {
            //Reset Value
            string s = "";
            int i = 0;
            foreach(string str in getBlendShapeNames(avatarRenderer.headRenderer))
            {
                avatarRenderer.headRenderer.SetBlendShapeWeight(i, 0);
                s += "0|";
                i++; 
            }
            PlayerPrefs.SetString(PrefDataList.avatarBlendshapes, s);

            //Reset UI
            foreach(GameObject go in blendList)
            {
                go.GetComponent<BlendSliderRef>().blendSlider.Set(0, false);
            }
        }


        //Ui Thingy now for the face
        public void Ui_showFaceBlend()
        {
            ShowEveryBlendShapes(false);
            for (int i = (int)faceID.x; i < (int)faceID.y; i++)
                blendList[i].SetActive(true);
            mainImage.overrideSprite = imageFace;
        }


        //Ui Thingy now for the Eyebrow
        public void Ui_showEyebrowBlend()
        {
            ShowEveryBlendShapes(false);
            for(int i = (int)eyebrowsID.x; i < (int)eyebrowsID.y; i++)
                blendList[i].SetActive(true);
            mainImage.overrideSprite = imageEyebrows;
        }


        //Ui Thingy now for the Eyes
        public void Ui_showEyesBlend()
        {
            ShowEveryBlendShapes(false);
            for (int i = (int)eyesID.x; i < (int)eyesID.y; i++)
                blendList[i].SetActive(true);
            mainImage.overrideSprite = imageEye;
        }


        //Ui Thingy now for the Cheek
        public void Ui_showCheekBlend()
        {
            ShowEveryBlendShapes(false);
            for (int i = (int)cheeksID.x; i < (int)cheeksID.y; i++)
                blendList[i].SetActive(true);
            mainImage.overrideSprite = imageCheeks;
        }



        //Ui Thingy now for the Nose
        public void Ui_showNoseBlend()
        {
            ShowEveryBlendShapes(false);
            for (int i = (int)noseID.x; i < (int)noseID.y; i++)
                blendList[i].SetActive(true);
            mainImage.overrideSprite = imageNose;
        }



        //Ui Thingy now for the Mouth
        public void Ui_showMouthBlend()
        {
            ShowEveryBlendShapes(false);
            for (int i = (int)mouthID.x; i < (int)mouthID.y; i++)
                blendList[i].SetActive(true);
            mainImage.overrideSprite = imageMouth;
        }

        //Ui Thingy now for the Chin
        public void Ui_showChinBlend()
        {
            ShowEveryBlendShapes(false);
            for (int i = (int)chinID.x; i < (int)chinID.y; i++)
                blendList[i].SetActive(true);
            mainImage.overrideSprite = imageChin;
        }



        //to show everything but not including the expression
        public void Ui_ShowAll_noExpression()
        {
            ShowEveryBlendShapes(false);
            for (int i = (int)faceID.x; i < blendList.Count; i++)
                blendList[i].SetActive(true);
            mainImage.overrideSprite = imageAll;
        }



        /// <summary>
        /// to hide or show everything
        /// </summary>
        /// <param name="_value"></param>
        public void ShowEveryBlendShapes(bool _value)
        {
            foreach (GameObject Go in blendList)
                Go.SetActive(_value);
        }
    }


}