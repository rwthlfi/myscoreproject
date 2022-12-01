using System.Collections;
using UnityEngine;


public class FillGlasses : MonoBehaviour
{
    public GameObject Glas;
    public GameObject LiquidGeneral;
    private float FillAmount = 0.67f;
    private bool tilt;
    private int pourThreshold = 45;
    private bool isPouring = false;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = LiquidGeneral.GetComponent<Renderer>();

        ChangeVisibleAmount();
        StartCoroutine(Spilling());
    }

    void Update()
    {
        bool pourCheck = CalculatePourAngle() < pourThreshold;

        if (isPouring != pourCheck)
        {
            isPouring = pourCheck;
            if (isPouring)
            {
                tilt = true;
            }

            else
            {
                tilt = !true;
            }
        }
    }



    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == 4) //Water
        {
            if (FillAmount <= 0.7f)
            {
                FillAmount += 0.005f;
            }

            ChangeVisibleAmount();
        }
    }


    private IEnumerator Spilling()
    {
        if (tilt)
        {
            if (FillAmount >= -0.01f)
            {
                FillAmount -= 0.01f;
                ChangeVisibleAmount();
            }
        }
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(Spilling());
    }


    void ChangeVisibleAmount()
    {
        //amount fill values: 0.44(max) - 0.58(min)

        if (FillAmount <= 0f)
        {
            LiquidGeneral.GetComponent<Renderer>().material.SetFloat("_FillAmount", .58f);
        }

        else if (FillAmount >= 0.01f & FillAmount <= 0.09f)
        {
            LiquidGeneral.GetComponent<Renderer>().material.SetFloat("_FillAmount", .56f);
        }

        else if (FillAmount >= 0.1f & FillAmount <= 0.19f)
        {
            LiquidGeneral.GetComponent<Renderer>().material.SetFloat("_FillAmount", .54f);
        }

        else if (FillAmount >= 0.2f & FillAmount <= 0.29f)
        {
            LiquidGeneral.GetComponent<Renderer>().material.SetFloat("_FillAmount", .52f);
        }

        else if (FillAmount >= 0.3f & FillAmount <= 0.39f)
        {
            LiquidGeneral.GetComponent<Renderer>().material.SetFloat("_FillAmount", .51f);
        }

        else if (FillAmount >= 0.4f & FillAmount <= 0.49f)
        {
            LiquidGeneral.GetComponent<Renderer>().material.SetFloat("_FillAmount", .50f);
        }

        else if (FillAmount >= 0.5f & FillAmount <= 0.59f)
        {
            LiquidGeneral.GetComponent<Renderer>().material.SetFloat("_FillAmount", .49f);
        }

        else if (FillAmount >= 0.6f & FillAmount <= 0.69f)
        {
            LiquidGeneral.GetComponent<Renderer>().material.SetFloat("_FillAmount", .48f);
        }

        else if (FillAmount >= 0.7f & FillAmount <= 0.79f)
        {
            LiquidGeneral.GetComponent<Renderer>().material.SetFloat("_FillAmount", .46f);
        }

        else if (FillAmount >= 0.8)
        {
            LiquidGeneral.GetComponent<Renderer>().material.SetFloat("_FillAmount", .44f);
        }
    }
    private float CalculatePourAngle()
    {
        return Glas.transform.up.y * Mathf.Rad2Deg;
    }
}