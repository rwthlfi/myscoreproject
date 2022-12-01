using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace AvatarCreation
{

    [System.Serializable]
    public class Custome
    {
        public List<Mesh> meshs = new List<Mesh>();
        public List<Sprite> thumbnails = new List<Sprite>();

    }


    public class Ui_AvatarCustomeSelection : MonoBehaviour
    {
        [Header("Activation Method")]
        public bool initUI = false;

        [Header("Ui Reference")]
        public Transform content;
        public Button allSelectionButton;

        [Header("Skin Hair Eye Selection")]
        public Button filterButton_human;
        public Custome Human;
        private List<GameObject> buttonList_Human = new List<GameObject>();
        public Button filterButton_hair;
        public Custome Hair;
        private List<GameObject> buttonList_Hair = new List<GameObject>();
        public Button filterButton_eyes;
        public Custome Eyes;
        private List<GameObject> buttonList_Eyes = new List<GameObject>();
        public Button filterButton_SkinHairAll;

        [Header("Custome Selection")]
        public Button filterButton_glasses;
        public Custome Glass;
        private List<GameObject> buttonList_Glass = new List<GameObject>();
        public Button filterButton_pants;
        public Custome Pants;
        private List<GameObject> buttonList_Pants = new List<GameObject>();
        public Button filterButton_clothes;
        public Custome Clothes;
        private List<GameObject> buttonList_Clothes = new List<GameObject>();
        public Button filterButton_shoes;
        public Custome Shoes;
        private List<GameObject> buttonList_Shoes = new List<GameObject>();
        public Button filterButton_CustomeAll;


        [Header("Script Reference")]
        public AvatarSetting_Renderer avatarRenderer;
        public SimpleObjectPool simpleObjPool;


        // Start is called before the first frame update
        void Start()
        {
            //dont do anything if it is a server
            if (!initUI)
                return;


            InitAllCustome(Glass, buttonList_Glass, CustomID.glassses);
            InitAllCustome(Hair, buttonList_Hair, CustomID.hair);
            InitAllCustome(Eyes, buttonList_Eyes, CustomID.eyes);
            InitAllCustome(Human, buttonList_Human, CustomID.head);
            InitAllCustome(Pants, buttonList_Pants, CustomID.pants);
            InitAllCustome(Clothes, buttonList_Clothes, CustomID.clothes);
            InitAllCustome(Shoes, buttonList_Shoes, CustomID.shoes);
            
            //invoke the button selection
            allSelectionButton.onClick.Invoke();

        }

        // Update is called once per frame
        void Update()
        {

        }

        //Init all the given Custom shapes
        public void InitAllCustome(Custome _custom, List<GameObject> _list, CustomID _cID)
        {
            _list.Clear();
            //get all the mesh in the custome List
            int i = 0;
            foreach(Mesh c in _custom.meshs)
            {
                //create gameobject
                GameObject customButton = CreateCustomeButton(_custom, i, _cID);
                //set to parent
                customButton.transform.SetParent(content, false);
                //add to list
                _list.Add(customButton);
                i++;
            }

            //create all the button from the mesh
            // - with the image and naming


        }

        private GameObject CreateCustomeButton(Custome _c, int _id, CustomID _customID)
        {
            //get the reference
            GameObject a = simpleObjPool.GetObject();
            AvatarCustomButtonRef buttonRef = a.GetComponent<AvatarCustomButtonRef>();

            //assign the image
            buttonRef.image.overrideSprite = _c.thumbnails[_id];

            //assign the button Ability
            buttonRef.button.onClick.AddListener(delegate
            {
                //get which renderer for the target
                //and load the renderer up
                if(_customID == CustomID.head)
                {
                    avatarRenderer.LoadRenderer_Customize_Head(avatarRenderer.getRenderer(_customID), _c.meshs[_id]);
                }

                //if it is clothes, then it is NOT a TShirt
                else if( _customID ==  CustomID.clothes 
                         && !isTShirt(_c.meshs[_id]))
                {
                    avatarRenderer.LoadRenderer_Customize_ShirtWithoutArms(avatarRenderer.getRenderer(_customID), _c.meshs[_id]);
                }

                else
                {   
                    avatarRenderer.LoadRenderer_Customize_TheRest(avatarRenderer.getRenderer(_customID), _c.meshs[_id]);
                }

                //Saved the Player Pref
                UpdateSavedCustome(_customID, _id);

                print("Load!");
            });


            //return gameObject
            return a;
        }

        public string UpdateSavedCustome(CustomID _pos, int _value)
        {
            List<string> aList = ConverterFunction.splitGivenString(PlayerPrefs.GetString(PrefDataList.savedAvatarCustome), "|");
        
            string r ="";
            //modify the value
            aList[(int)CustomID.type] = Type.customize.ToString();
            aList[(int)_pos] = _value.ToString();

            foreach (string str in aList)
                r += str + "|";

            //save and return the value
            PlayerPrefs.SetString(PrefDataList.savedAvatarCustome, r);

            //cache it
            PlayerPrefs.SetString(PrefDataList.savedPreviousCustome, PlayerPrefs.GetString(PrefDataList.savedAvatarCustome));
            
            return r;

        }


        /// <summary>
        /// to check if the mesh is s t-shirt or not., because then the arms need to be disabled.
        /// </summary>
        /// <param name="_mesh"></param>
        /// <returns></returns>
        public bool isTShirt(Mesh _mesh)
        {
            
            if (ConverterFunction.ContainsAny(_mesh.ToString(), GlobalSettings.string_custome_TShirt))
                return true;
            
            else
                return false;
        }


        public int getMeshID(CustomID _customeID, string _meshName)
        {
            Custome _custome = null;
            
            switch (_customeID)
            {
                case CustomID.clothes: _custome = Clothes; break;
                case CustomID.pants: _custome = Pants; break;
                case CustomID.shoes: _custome = Shoes; break;
            }


            for (int i = 0; i < _custome.meshs.Count; i++)
            {
                if (_custome.meshs[i].name == _meshName)
                    return i;
            }


            Debug.LogError("Mesh not found..., so i just return the first array");
            return 0;

        }


        public void RandomizeCustome()
        {
            string strType = Type.customize + "|";
            string strArms = 0.ToString() + "|";
            string strEyes = Random.Range(0, Eyes.meshs.Count).ToString() + "|";
            string strGlasses = Random.Range(0, Glass.meshs.Count).ToString() + "|";
            string strHair = Random.Range(0, Hair.meshs.Count).ToString() + "|";
            string strHands = 0.ToString() + "|";
            string strHead = Random.Range(0, Human.meshs.Count).ToString() + "|";
            string strRig = 0 + "|";
            string strPants = Random.Range(0, Pants.meshs.Count).ToString() + "|";
            string strClothes = Random.Range(0, Clothes.meshs.Count).ToString() + "|";
            string strShoes = Random.Range(0, Shoes.meshs.Count).ToString() + "|";

            PlayerPrefs.SetString(PrefDataList.savedAvatarCustome, strType + strArms + strEyes
                                                                   + strGlasses + strHair + strHands
                                                                   + strHead + strRig + strPants + strClothes + strShoes);

            //cache it
            PlayerPrefs.SetString(PrefDataList.savedPreviousCustome, PlayerPrefs.GetString(PrefDataList.savedAvatarCustome));

        }


        //the ui is do to not being able to have List as an argument.
        //thus copy pasting it....


        public void Ui_Filter_Hair()
        {
            ShowEveryButton(false);
            ShowButton(buttonList_Hair, true);
        }


        public void Ui_Filter_Eyes()
        {
            ShowEveryButton(false);
            ShowButton(buttonList_Eyes, true);
        }

        public void Ui_Filter_Skin()
        {
            ShowEveryButton(false);
            ShowButton(buttonList_Human, true);
        }


        public void Ui_Filter_glasses()
        {
            ShowEveryButton(false);
            ShowButton(buttonList_Glass, true);
        }


        public void Ui_Filter_Pants()
        {
            ShowEveryButton(false);
            ShowButton(buttonList_Pants, true);
        }

        public void Ui_Filter_Clothes()
        {
            ShowEveryButton(false);
            ShowButton(buttonList_Clothes, true);
        }

        public void Ui_Filter_Shoes()
        {
            ShowEveryButton(false);
            ShowButton(buttonList_Shoes, true);
        }

        public void Ui_All_Custome()
        {
            ShowEveryButton(false);
            ShowEveryFilterButton(false);
            ShowButton(buttonList_Glass, true);
            ShowButton(buttonList_Pants, true);
            ShowButton(buttonList_Clothes, true);
            ShowButton(buttonList_Shoes, true);
            filterButton_glasses.gameObject.SetActive(true);
            filterButton_pants.gameObject.SetActive(true);
            filterButton_clothes.gameObject.SetActive(true);
            filterButton_shoes.gameObject.SetActive(true);
            filterButton_CustomeAll.gameObject.SetActive(true);
        }

        public void Ui_All_Faces()
        {
            ShowEveryButton(false);
            ShowEveryFilterButton(false);
            ShowButton(buttonList_Human, true);
            ShowButton(buttonList_Hair, true);
            ShowButton(buttonList_Eyes, true);
            filterButton_human.gameObject.SetActive(true);
            filterButton_hair.gameObject.SetActive(true);
            filterButton_eyes.gameObject.SetActive(true);
            filterButton_SkinHairAll.gameObject.SetActive(true);
        }



        /// <summary>
        /// To show all the button
        /// </summary>
        /// <param name="_a"></param>
        private void ShowButton(List<GameObject> _a, bool _value)
        {
            foreach (GameObject a in _a)
                a.SetActive(_value);
        }


        private void ShowEveryButton(bool _value)
        {
            ShowButton(buttonList_Human, _value);
            ShowButton(buttonList_Hair, _value);
            ShowButton(buttonList_Eyes, _value);
            ShowButton(buttonList_Glass, _value);
            ShowButton(buttonList_Pants, _value);
            ShowButton(buttonList_Clothes, _value);
            ShowButton(buttonList_Shoes, _value);
        }

        private void ShowEveryFilterButton(bool _value)
        {
            filterButton_clothes.gameObject.SetActive(_value);
            filterButton_CustomeAll.gameObject.SetActive(_value);
            filterButton_eyes.gameObject.SetActive(_value);
            filterButton_glasses.gameObject.SetActive(_value);
            filterButton_hair.gameObject.SetActive(_value);
            filterButton_human.gameObject.SetActive(_value);
            filterButton_pants.gameObject.SetActive(_value);
            filterButton_shoes.gameObject.SetActive(_value);
            filterButton_SkinHairAll.gameObject.SetActive(_value);
        }
    }

}
