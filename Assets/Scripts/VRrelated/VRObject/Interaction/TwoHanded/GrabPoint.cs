using UnityEngine;
public class GrabPoint : MonoBehaviour
{
    //The way that Rotation Restriction works is by comparing the rotation of the GrabPoint and controler and if the angle between them is less than the RotationLimit then it is grabbable.
    public bool RestrictByRotation;//Set's if you want to restrict the grip point by a rotation
    public float RotationLimit;//set's the angle that you want to restrict it to
    public bool HelperGrip;//Replaced Subgrip and reworked it so that it works like a nomal grip unless double gripped 
    public Vector3 Offset;//Added in offset varibles so that the Grabpoints don't have to be direct children of a game object with an interactible script
    public Quaternion RotationOffset;

    public bool Gripped;
    public Interactable ParentInteractable;
    private void Awake()//auto set ParentInteractible if we can and update the offset
    {

        if (!ParentInteractable && transform.parent.GetComponent<Interactable>())
        {
            ParentInteractable = transform.parent.GetComponent<Interactable>();
        }
        UpdateOffset();

    }
    public void UpdateOffset()
    {
        Offset = Quaternion.Inverse(ParentInteractable.transform.rotation) * (-ParentInteractable.transform.position + transform.position);
        RotationOffset = Quaternion.Inverse(ParentInteractable.transform.rotation) * transform.rotation;
    }

}