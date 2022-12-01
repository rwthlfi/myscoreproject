using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RWTHNPCAnimations : MonoBehaviour
{
    public Animator newsgirl, couple1, couple2, observer, sightseeing1, sightseeing2, walker1, walker2, proftalker1, proftalker2, proftalker3, newsgirl2, newsboy, newsboy2, cafe1_1, cafe1_2, cafe1_3, cafe1_4, cafe1_5, cafe2_1, cafe2_2, cafe2_3, cafe2_4, klinkel;

    void Start()
    {
        newsgirl.Play("NewsGirl");
        couple1.Play("Couple1");
        couple2.Play("Couple2");
        observer.Play("Observer");
        sightseeing1.Play("SightSeeing1");
        sightseeing2.Play("SightSeeing2");
        walker1.Play("Walker1");
        walker2.Play("Walker2");
        proftalker1.Play("ProfTalkers1");
        proftalker2.Play("ProfTalkers2");
        proftalker3.Play("ProfTalkers3");

        newsgirl2.Play("NewsGirl");
        newsboy.Play("NewsGirl");
        newsboy2.Play("NewsGirl");

        cafe1_1.Play("Couple1");
        cafe1_2.Play("Couple1");
        cafe1_3.Play("Couple1");
        cafe1_4.Play("Couple1");
        cafe1_5.Play("Couple1");
        cafe2_1.Play("Cafe1");
        cafe2_2.Play("Cafe1");
        cafe2_3.Play("Cafe1");
        cafe2_4.Play("Cafe1");

        klinkel.Play("Klinkel");
    }
}
