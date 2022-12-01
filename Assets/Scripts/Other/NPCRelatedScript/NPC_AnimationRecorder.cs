# if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

public class NPC_AnimationRecorder : MonoBehaviour
{
    GameObjectRecorder _recorder;

    #region Inspector Variables

    [Tooltip("Must start with Assets/")]
    [SerializeField] string _saveFolderLocation = "Assets/Artefacts/Animations/AnimationRecording/";

    [SerializeField] string _clipName;
    [SerializeField] float _frameRate = 15f;

    [Header("Key Bindings")]
    [SerializeField] string _startRecKey = "i";
    [SerializeField] string _stopRecKey = "o";
    [SerializeField] string _deleteRecKey = "p";
    [Tooltip("ONLY USE WHEN ALL RELATED ASSETS ARE DELETED FROM ASSETS FOLDER")]
    [SerializeField] string _deleteIndexKey = "l";

    #endregion 

    private AnimationClip _clip;
    private AnimationClip _currentClip;
    private bool _canRecord = true;
    private int _index;
    private string _currentClipName;

    private void OnEnable()
    {
        if (_clip == null)
        {
            CreateNewClip();
        }

        var savedIndex = PlayerPrefs.GetInt(gameObject.name + "Index");

        if (savedIndex != 0)
        {
            _index = savedIndex;
        }
    }

    void Start()
    {
        _recorder = new GameObjectRecorder(gameObject);
        _recorder.BindComponentsOfType<Transform>(gameObject, true);

        if (_clipName == "")
        {
            _clipName = gameObject.name + "_animation";
        }

    }

    private void Update()
    {
        ControllerInputs();
    }

    private void ControllerInputs()
    {
        if (Input.GetKeyDown(_startRecKey))
        {
            StartRecording();
        }

        if (Input.GetKeyDown(_stopRecKey))
        {
            StopRecording();
        }

        if (Input.GetKeyDown(_deleteRecKey))
        {
            DeleteRecording();
        }

        //reset index if all clips have been deleted from the assets
        //ONLY USE IF ALL ASSETS HAVE BEEN DELETED
        if (Input.GetKey(_deleteIndexKey))
        {
            PlayerPrefs.DeleteKey(gameObject.name + "Index");
            Debug.LogWarning("Clip name indexing has been reset");
            _index = 0;
        }
    }

    private void StartRecording()
    {
        _canRecord = true;
        CreateNewClip();
        Debug.Log("Animation Recording for " + gameObject.name + " has STARTED");
    }

    private void StopRecording()
    {
        Debug.Log("Animation Recording for " + gameObject.name + " has STOPPED");

        _canRecord = false;

        _recorder.SaveToClip(_currentClip);

        AssetDatabase.CreateAsset(_currentClip, _saveFolderLocation + _currentClipName + ".anim");

        AssetDatabase.SaveAssets();
    }

    private void DeleteRecording()
    {
        if (_canRecord)
        {
            Debug.LogWarning("Cannot delete when recording!");
            return;
        }

        if (!AssetDatabase.Contains(_currentClip))
        {
            Debug.LogWarning("Clip Has not been saved yet.");
            return;
        }
        AssetDatabase.DeleteAsset(_saveFolderLocation + _currentClipName + ".anim");
        Debug.Log("Clip has been DELETED");

    }

    private void LateUpdate()
    {
        if (_clip == null) return;

        if (_canRecord)
        {
            _recorder.TakeSnapshot(Time.deltaTime);
        }
    }

    private void CreateNewClip()
    {
        _clip = new AnimationClip();

        if (_clip.name.Contains(_clip.name))
        {
            _clip.name = _clipName + " " + (_index++);
            _currentClipName = _clip.name;
        }

        _clip.frameRate = _frameRate;

        _currentClip = _clip;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(gameObject.name + "Index", _index);
    }

}

#endif