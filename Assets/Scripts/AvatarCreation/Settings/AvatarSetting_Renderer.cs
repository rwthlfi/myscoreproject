using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace AvatarCreation
{
    public enum CustomID
    {
        type, arms, eyes, glassses, hair, hands, head, rig, pants, clothes, shoes
    }

    public enum Type
    {
        lecturer, customize
    }

    //get the setting of the face
    public class AvatarSetting_Renderer : MonoBehaviour
    {
        [Header("Renderer")]
        public SkinnedMeshRenderer armsRenderer;
        public SkinnedMeshRenderer eyesRenderer;
        public SkinnedMeshRenderer glassesRenderer;
        public SkinnedMeshRenderer hairRenderer;
        public SkinnedMeshRenderer handsRenderer;
        public SkinnedMeshRenderer headRenderer;
        public SkinnedMeshRenderer pantsRenderer;
        public SkinnedMeshRenderer clothesRenderer;
        public SkinnedMeshRenderer shoesRenderer;

        [Header("Meshes")]
        public Mesh arms_DarkSkin_Male;
        public Mesh arms_DarkSkin_Female;
        public Mesh arms_LightSkin_Male;
        public Mesh arms_LightSkin_Female;
        public Mesh hand_DarkSkin;
        public Mesh hand_LightSkin;

        [Header("Material")]
        public Material matLecturerSkin;
        public Material matCustomizeSkin_Male;
        public Material matCustomizeSkin_Female;


        private List<string> savedBlendShapes;

        [Header("Script reference")]
        public Ui_AvatarFacialSelection ui_avatarFacialSelection;
        public Ui_AvatarCustomeSelection ui_AvatarCustomeSelection;
        public Ui_AvatarLecturerSelection ui_avatarLecturerSelection;




        private void Start()
        {
            //CheckSavedAvatar();
        }

        /// <summary>
        /// get the renderer on which the renderer should be modified
        /// </summary>
        /// <param name="customID"></param>
        /// <returns></returns>
        public SkinnedMeshRenderer getRenderer(CustomID _customID)
        {
            switch (_customID)
            {
                case CustomID.arms: return armsRenderer; 
                case CustomID.eyes: return eyesRenderer; 
                case CustomID.glassses: return glassesRenderer; 
                case CustomID.hair: return hairRenderer; 
                case CustomID.hands: return handsRenderer; 
                case CustomID.head: return headRenderer; 
                case CustomID.pants: return pantsRenderer; 
                case CustomID.clothes: return clothesRenderer; 
                case CustomID.shoes: return shoesRenderer; 
                default: return null;
            }
        }

        public void InitFaceSetup()
        {
            //print("a");
            //get all the saved blend value
            ui_avatarFacialSelection.InitBlendValue();
            //initialize all the blendshapes
            ui_avatarFacialSelection.InitAllBlendSlider();
            //invoke everything for the first time
            ui_avatarFacialSelection.allSelection.onClick.Invoke(); // this is just for the filter selection
        }


        /// <summary>
        /// to load the face setup, but with the saved cache
        /// </summary>
        public void LoadFace()
        {
            savedBlendShapes = ConverterFunction.splitGivenString(PlayerPrefs.GetString(PrefDataList.avatarBlendshapes), "|");
            for (int i = 0; i < savedBlendShapes.Count; i++)
            {
                headRenderer.SetBlendShapeWeight(i, ConverterFunction.StringToInt(savedBlendShapes[i]));
                eyesRenderer.SetBlendShapeWeight(i, ConverterFunction.StringToInt(savedBlendShapes[i]));
            }
        }

        /// <summary>
        /// to load the face setup but with string value
        /// </summary>
        /// <param name="_value"></param>
        public void LoadFace(string _value)
        {
            List<string> cacheList = ConverterFunction.splitGivenString(_value, "|");
            for (int i = 0; i < cacheList.Count; i++)
            {
                headRenderer.SetBlendShapeWeight(i, ConverterFunction.StringToInt(cacheList[i]));
                eyesRenderer.SetBlendShapeWeight(i, ConverterFunction.StringToInt(cacheList[i]));
            }
        }


        public void CheckSavedAvatar()
        {
            EnableAllSkinnedRenderer(true);
            //get the player pref list
            List<string> strList = ConverterFunction.splitGivenString(PlayerPrefs.GetString(PrefDataList.savedAvatarCustome), "|");

            if(strList.Count <=0)
            {
                //randomize the custom avatar
                RandomizeCustomeValue();
                Load_SavedCustome(PlayerPrefs.GetString(PrefDataList.savedAvatarCustome));
                //cache it
                PlayerPrefs.SetString(PrefDataList.savedLecturerCustome, PlayerPrefs.GetString(PrefDataList.savedAvatarCustome));
                PlayerPrefs.SetString(PrefDataList.savedPreviousCustome, PlayerPrefs.GetString(PrefDataList.savedAvatarCustome));
            }

            else
            {
                //load saved custom Avatar
                print("load Custome Avatar");
                Load_SavedCustome(PlayerPrefs.GetString(PrefDataList.savedPreviousCustome));
            }
        }



        public void RandomizeCustomeValue()
        {
            print("Lets generate Random one and saved them");
            ui_AvatarCustomeSelection.RandomizeCustome();

            ui_avatarFacialSelection.RandomizeFacial();
        }

        /// <summary>
        /// Load the saved avatar
        /// </summary>
        /// <param name="_strList"></param>
        public void Load_SavedCustome(string _str)
        {
            EnableAllSkinnedRenderer(true);

            //convert string to list
            List<string> _strList = ConverterFunction.splitGivenString(_str, "|");


            //check the type
            //if Custom avatar then loads everythings up
            if (_strList[(int)CustomID.type] == Type.customize.ToString())
            {
                //getRenderer(CustomID.arms).sharedMesh = 

                //get the face and Arms
                getRenderer(CustomID.head).sharedMesh = ui_AvatarCustomeSelection.Human
                                                        .meshs[ConverterFunction.StringToInt(_strList[(int)CustomID.head])];

                bool femaleMesh = getRenderer(CustomID.head).sharedMesh.ToString().Contains("Female");
                bool lightSkin = getRenderer(CustomID.head).sharedMesh.ToString().Contains("LightSkin");

                //CustomizeSkin
                LoadSkinSet(femaleMesh, lightSkin);



                //get the eye
                getRenderer(CustomID.eyes).sharedMesh = ui_AvatarCustomeSelection.Eyes
                                                        .meshs[ConverterFunction.StringToInt(_strList[(int)CustomID.eyes])];

                //get the Hair
                getRenderer(CustomID.hair).sharedMesh = ui_AvatarCustomeSelection.Hair
                                                        .meshs[ConverterFunction.StringToInt(_strList[(int)CustomID.hair])];

                //get the Glasses
                getRenderer(CustomID.glassses).sharedMesh = ui_AvatarCustomeSelection.Glass
                                                            .meshs[ConverterFunction.StringToInt(_strList[(int)CustomID.glassses])];

                //Get the saved clothes
                getRenderer(CustomID.clothes).sharedMesh = ui_AvatarCustomeSelection.Clothes
                                                           .meshs[ConverterFunction.StringToInt(_strList[(int)CustomID.clothes])];
                //check if the mesh containts any tshirt
                if (ui_AvatarCustomeSelection.isTShirt(getRenderer(CustomID.clothes).sharedMesh))
                    armsRenderer.gameObject.SetActive(true);
                else
                    armsRenderer.gameObject.SetActive(false);


                //Get the saved pants
                getRenderer(CustomID.pants).sharedMesh = ui_AvatarCustomeSelection.Pants
                                                           .meshs[ConverterFunction.StringToInt(_strList[(int)CustomID.pants])];

                //Get the saved Shoes
                getRenderer(CustomID.shoes).sharedMesh = ui_AvatarCustomeSelection.Shoes
                                                           .meshs[ConverterFunction.StringToInt(_strList[(int)CustomID.shoes])];



            }



            //if lecturer Avatar then Loads only specific things
            else if(_strList[(int)CustomID.type] == Type.lecturer.ToString())
            {
                //disable the eye, hair, glasses, arms
                LecturerCustome lc = ui_avatarLecturerSelection.Lecturer;
                int lcID = ConverterFunction.StringToInt(_strList[(int)CustomID.head]);


                //Render the head
                LoadRenderer_Lecturer(getRenderer(CustomID.head), lc.meshs[lcID], lc.skinTexture[lcID]);


                //print("aaa: " + _str);

            }
        }




        /// <summary>
        /// To Load up the lecturer skin
        /// </summary>
        /// <param name="_target"></param>
        /// <param name="_mesh"></param>
        /// <param name="_tex"></param>
        public void LoadRenderer_Lecturer(SkinnedMeshRenderer _target, Mesh _mesh, Texture _tex)
        {
            EnableAllSkinnedRenderer(true);
            _target.material = matLecturerSkin;
            _target.material.SetTexture("_MainTex", _tex);
            _target.sharedMesh = _mesh;

            //disable certain renderer
            eyesRenderer.gameObject.SetActive(false);
            glassesRenderer.gameObject.SetActive(false);
            hairRenderer.gameObject.SetActive(false);
            armsRenderer.gameObject.SetActive(false);

            //Render the Custome..
            AssignLecturerCustome(getRenderer(CustomID.head).sharedMesh.ToString());
        }



        /// <summary>
        /// to load up the mesh and material for the head
        /// </summary>
        /// <param name="_target"></param>
        /// <param name="_mesh"></param>
        public void LoadRenderer_Customize_Head(SkinnedMeshRenderer _target, Mesh _mesh)
        {
            EnableAllSkinnedRenderer(true);
            _target.sharedMesh = _mesh;

            bool femaleMesh = _target.sharedMesh.ToString().Contains("Female");
            bool lightSkin = _target.sharedMesh.ToString().Contains("LightSkin");


            LoadSkinSet(femaleMesh, lightSkin);
        }

        private void LoadSkinSet(bool _femaleSkinVal, bool _lightSkinVal)
        {
            //CustomizeSkin
            if (_femaleSkinVal)
                getRenderer(CustomID.head).material = matCustomizeSkin_Female;
            else
                getRenderer(CustomID.head).material = matCustomizeSkin_Male;

            //assign the Hands 
            if (_lightSkinVal)
                getRenderer(CustomID.hands).sharedMesh = hand_LightSkin;
            else
                getRenderer(CustomID.hands).sharedMesh = hand_DarkSkin;


            //Assign the Arms
            if (_femaleSkinVal)
            {
                getRenderer(CustomID.arms).material = matCustomizeSkin_Female;
                if (_lightSkinVal)
                    getRenderer(CustomID.arms).sharedMesh = arms_LightSkin_Female;
                else
                    getRenderer(CustomID.arms).sharedMesh = arms_DarkSkin_Female;
            }
            else
            {
                getRenderer(CustomID.arms).material = matCustomizeSkin_Male;
                if (_lightSkinVal)
                    getRenderer(CustomID.arms).sharedMesh = arms_LightSkin_Male;
                else
                    getRenderer(CustomID.arms).sharedMesh = arms_DarkSkin_Male;

            }
        }



        /// <summary>
        /// to load up the T Shirt. since the arms needed to be disabled.
        /// </summary>
        /// <param name="_target"></param>
        /// <param name="_mesh"></param>
        public void LoadRenderer_Customize_ShirtWithoutArms(SkinnedMeshRenderer _target, Mesh _mesh)
        {
            EnableAllSkinnedRenderer(true);
            _target.sharedMesh = _mesh;
            armsRenderer.gameObject.SetActive(false);
        }


        /// <summary>
        /// to load up the mesh and material for the Rest
        /// </summary>
        /// <param name="_target"></param>
        /// <param name="_mesh"></param>
        public void LoadRenderer_Customize_TheRest(SkinnedMeshRenderer _target, Mesh _mesh)
        {
            EnableAllSkinnedRenderer(true);
            _target.sharedMesh = _mesh;

            //check the clothes, if it is NOT a  Tshirt, disable the arms,
            if (!ui_AvatarCustomeSelection.isTShirt(clothesRenderer.sharedMesh))
                armsRenderer.gameObject.SetActive(false);

        }



        /// <summary>
        /// to enable or disable all the skinned renderer
        /// </summary>
        /// <param name="_value"></param>
        private void EnableAllSkinnedRenderer(bool _value)
        {
            armsRenderer.gameObject.SetActive(_value);
            eyesRenderer.gameObject.SetActive(_value);
            glassesRenderer.gameObject.SetActive(_value);
            hairRenderer.gameObject.SetActive(_value);
            handsRenderer.gameObject.SetActive(_value);
            headRenderer.gameObject.SetActive(_value);
            pantsRenderer.gameObject.SetActive(_value);
            clothesRenderer.gameObject.SetActive(_value);
            shoesRenderer.gameObject.SetActive(_value);

        }



        public void AssignLecturerCustome(string _meshName)
        {
            string cleanName = _meshName.Replace(GlobalSettings.UnityEngineMeshString, "");

            print("load Avatar set: " + cleanName);
            Mesh setHands = hand_DarkSkin;
            Material setMat = matCustomizeSkin_Male;
            string setClothes = "";
            string setPants = "";
            string setShoes = "";

            //get the mesh name from 
            if (cleanName == "Faber")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Female;
                setClothes = "Shirt_Female_White";
                setPants = "Pants_Female_BlueDark";
                setShoes = "Shoes_Black";
            }

            else if (cleanName == "Habel")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Female;
                setClothes = "Suit_Female_Brown";
                setPants = "Pants_Female_Black";
                setShoes = "Shoes_Brown";
            }

            else if (cleanName == "Janoschka")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Shirt_Male_BlueLight";
                setPants = "Pants_Male_GreyDark";
                setShoes = "Shoes_Black";

            }

            else if (cleanName == "Klinkel")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Suit_Male_Black";
                setPants = "Pants_Male_GreyDark";
                setShoes = "Shoes_Black";
            }

            else if (cleanName == "Krieg")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Suit_Male_Black";
                setPants = "Pants_Male_Black";
                setShoes = "Shoes_Black";
            }

            else if (cleanName == "Nacken")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Suit_Male_Black";
                setPants = "Pants_Male_Black";
                setShoes = "Shoes_Black";
            }


            else if (cleanName == "NackenSr")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Sweater_Male_Blue";
                setPants = "Pants_Male_Brown";
                setShoes = "Shoes_Black";
            }

            else if (cleanName == "Nailis")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Suit_Male_Grey";
                setPants = "Pants_Male_GreyDark";
                setShoes = "Shoes_Black";
            }


            else if (cleanName == "Oeser")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Suit_Male_Grey";
                setPants = "Pants_Male_Grey";
                setShoes = "Shoes_Brown";
            }


            else if (cleanName == "Paganini")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Suit_Male_Black";
                setPants = "Pants_Male_Brown";
                setShoes = "Shoes_Black";
            }

            else if (cleanName == "Persike")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Suit_Male_Grey";
                setPants = "Pants_Male_Blue";
                setShoes = "Shoes_Black";
            }

            else if (cleanName == "Piller")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Suit_Male_Black";
                setPants = "Pants_Male_Black";
                setShoes = "Shoes_Black";
            }

            else if (cleanName == "Pohlmann")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Female;
                setClothes = "Suit_Female_Grey";
                setPants = "Pants_Female_GreyDark";
                setShoes = "Shoes_Black";
            }


            else if (cleanName == "Riccardo")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Suit_Male_Grey";
                setPants = "Pants_Male_Grey";
                setShoes = "Shoes_Brown";
            }


            else if (cleanName == "Ruediger")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Suit_Male_Blue";
                setPants = "Pants_Male_BlueDark";
                setShoes = "Shoes_Black";
            }


            else if (cleanName == "Sewilam")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Suit_Male_Blue";
                setPants = "Pants_Male_Blue";
                setShoes = "Shoes_Black";
            }


            else if (cleanName == "Weissman")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Female;
                setClothes = "Suit_Female_Black";
                setPants = "Pants_Female_Black";
                setShoes = "Shoes_Black";
            }


            else if (cleanName == "Schnelle")
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "Suit_Male_Blue";
                setPants = "Pants_Male_Blue";
                setShoes = "Shoes_Black";
            }


            else if (cleanName == "Djamel" || cleanName == "Koen" || cleanName == "Raymond" )
            {
                setHands = hand_LightSkin;
                setMat = matCustomizeSkin_Male;
                setClothes = "TShirt_Male_MyScore";
                setPants = "Pants_Male_Blue";
                setShoes = "Shoes_Black";


                int c = ui_AvatarCustomeSelection.getMeshID(CustomID.clothes, setClothes);
                int p = ui_AvatarCustomeSelection.getMeshID(CustomID.pants, setPants);
                int s = ui_AvatarCustomeSelection.getMeshID(CustomID.shoes, setShoes);

                getRenderer(CustomID.arms).sharedMesh = arms_LightSkin_Male;
                getRenderer(CustomID.hands).sharedMesh = setHands;
                getRenderer(CustomID.hands).material = setMat;

                getRenderer(CustomID.clothes).sharedMesh = ui_AvatarCustomeSelection.Clothes.meshs[c];
                getRenderer(CustomID.clothes).sharedMaterial = ui_avatarLecturerSelection.myScoreClothsMaterial;

                getRenderer(CustomID.pants).sharedMesh = ui_AvatarCustomeSelection.Pants.meshs[p];
                getRenderer(CustomID.shoes).sharedMesh = ui_AvatarCustomeSelection.Shoes.meshs[s];
                armsRenderer.gameObject.SetActive(true);

                return; // DONT NEED TO EXECUTE CODE BELOW
            }


            int clothesID = ui_AvatarCustomeSelection.getMeshID(CustomID.clothes, setClothes);
            int pantsID = ui_AvatarCustomeSelection.getMeshID(CustomID.pants, setPants);
            int shoesID = ui_AvatarCustomeSelection.getMeshID(CustomID.shoes, setShoes);


            getRenderer(CustomID.hands).sharedMesh = setHands;
            getRenderer(CustomID.hands).material = setMat;

            getRenderer(CustomID.clothes).sharedMesh = ui_AvatarCustomeSelection.Clothes
                                                       .meshs[clothesID];

            getRenderer(CustomID.clothes).sharedMaterial = ui_avatarLecturerSelection.clothsMaterial;


            getRenderer(CustomID.pants).sharedMesh = ui_AvatarCustomeSelection.Pants
                                                       .meshs[pantsID];

            getRenderer(CustomID.shoes).sharedMesh = ui_AvatarCustomeSelection.Shoes
                                                       .meshs[shoesID];


        }
    }
}