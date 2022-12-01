using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AvatarCreation
{

    [System.Serializable]
    public class LecturerCustome
    {
        public List<Mesh> meshs = new List<Mesh>();
        public List<Texture> skinTexture = new List<Texture>();
        public List<Sprite> thumbnails = new List<Sprite>();


    }

    [System.Serializable]
    public class PreCustome
    {
        public string meshName;
        public Mesh hands;
        public Mesh clothes;
        public Mesh pants;
        public Mesh shoes;
    }


    public class Ui_AvatarLecturerSelection : MonoBehaviour
    {
        [Header("Activation Method")]
        public bool initUI = false;

        [Header("Ui Reference")]
        public Transform content;

        [Header("Lecturer Reference")]
        public Material skinMaterial;
        public LecturerCustome Lecturer;
        private List<GameObject> buttonList_Lecturer = new List<GameObject>();
        public Material clothsMaterial;

        [Header("Exception for Developer")]
        public Material myScoreClothsMaterial;

        [Header("Script Reference")]
        public AvatarSetting_Renderer avatarRenderer;
        public SimpleObjectPool simpleObjPool;




        private void Start()
        {
            //dont do anything if it is a server
            if (!initUI)
                return;


            InitAllLecturer(Lecturer, buttonList_Lecturer, CustomID.head);
        }


        //Init all the given Custom shapes
        public void InitAllLecturer(LecturerCustome _custom, List<GameObject> _list, CustomID _cID)
        {
            _list.Clear();
            //get all the mesh in the custome List
            int i = 0;
            foreach (Mesh c in _custom.meshs)
            {
                //create gameobject
                GameObject customButton = CreateLecturerButton(_custom, i, _cID);
                //set to parent
                customButton.transform.SetParent(content, false);
                //add to list
                _list.Add(customButton);
                i++;
            }

        }

        private GameObject CreateLecturerButton(LecturerCustome _c, int _id, CustomID _customID)
        {
            //get the reference
            GameObject a = simpleObjPool.GetObject();
            AvatarLecturerButtonRef buttonRef = a.GetComponent<AvatarLecturerButtonRef>();

            //assign the image
            buttonRef.image.overrideSprite = _c.thumbnails[_id];

            //assign the button Ability
            buttonRef.button.onClick.AddListener(delegate
            {
                //get which renderer for the target
                //and load the renderer up
                avatarRenderer.LoadRenderer_Lecturer(avatarRenderer.getRenderer(_customID), _c.meshs[_id], _c.skinTexture[_id] );

                //save the selection too
                UpdateLecturerSelection(_customID, _id);
            });

            //assign the text
            buttonRef.textPro.text = _c.meshs[_id].ToString().Replace(GlobalSettings.UnityEngineMeshString, "");

            //return gameObject
            return a;
        }



        
        public string UpdateLecturerSelection(CustomID _pos, int _value)
        {
            List<string> aList = ConverterFunction.splitGivenString(PlayerPrefs.GetString(PrefDataList.savedLecturerCustome), "|");

            string r = "";
            //modify the value
            aList[(int)CustomID.type] = Type.lecturer.ToString();
            aList[(int)_pos] = _value.ToString();

            foreach (string str in aList)
                r += str + "|";

            //save and return the value
            PlayerPrefs.SetString(PrefDataList.savedLecturerCustome, r); 

            //cache it
            PlayerPrefs.SetString(PrefDataList.savedPreviousCustome, PlayerPrefs.GetString(PrefDataList.savedLecturerCustome));

            return r;

        }

    }
}
