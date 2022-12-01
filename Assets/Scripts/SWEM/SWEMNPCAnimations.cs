using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWEMNPCAnimations : MonoBehaviour
{
    public Animator villager1, villager2, villager3, villager4, villager5, villager6, villager7, villager8;
    public SpriteRenderer villager1sprite, villager2sprite, villager3sprite, villager4sprite, villager5sprite, villager6sprite, villager7sprite, villager8sprite;
    private float currentPopulationValue, currentWaterShareValue;

    public void NPCAnimationControl(float currentPopulation, float currentWaterShare)
    {
        currentPopulationValue = currentPopulation;
        currentWaterShareValue = 100f - currentWaterShare;
        ChangeNPC();
        ChangeNPCAnimations();
        ChangeNPCSprites();
    }

    private void ChangeNPC() // activates NPC regarding the population amount from the sliders
    {
        if (currentPopulationValue >= 33000)
        {
            villager3.gameObject.SetActive(true);
            villager4.gameObject.SetActive(true);
        }
        else
        {
            villager3.gameObject.SetActive(false);
            villager4.gameObject.SetActive(false);
        }

        if (currentPopulationValue >= 67000)
        {
            villager5.gameObject.SetActive(true);
            villager6.gameObject.SetActive(true);
        }
        else
        {
            villager5.gameObject.SetActive(false);
            villager6.gameObject.SetActive(false);
        }

        if (currentPopulationValue >= 100000)
        {
            villager7.gameObject.SetActive(true);
            villager8.gameObject.SetActive(true);
        }
        else
        {
            villager7.gameObject.SetActive(false);
            villager8.gameObject.SetActive(false);
        }
    }

    private void ChangeNPCAnimations() // changes the animations for the npc regarding the water share % of the sanitation
    {
        if (currentWaterShareValue <= 12.5)
        {
            villager1.SetBool("VillagerSchool?", false);
        }
        else if (currentWaterShareValue >= 12.6)
        {
            villager1.SetBool("VillagerSchool?", true);
        }

        if (/*currentWaterShareValue >= 12.6 && */currentWaterShareValue <= 25)
        {
            villager2.SetBool("VillagerSchool?", false);
        }
        else if (currentWaterShareValue >= 25.1)
        {
            villager2.SetBool("VillagerSchool?", true);
        }

        if (/*currentWaterShareValue >= 25.1 && */currentWaterShareValue <= 37.5)
        {
            villager3.SetBool("VillagerSchool?", false);
        }
        else if (currentWaterShareValue >= 37.6)
        {
            villager3.SetBool("VillagerSchool?", true);
        }

        if (/*currentWaterShareValue >= 37.6 && */currentWaterShareValue <= 50)
        {
            villager4.SetBool("VillagerSchool?", false);
        }
        else if (currentWaterShareValue >= 50.1)
        {
            villager4.SetBool("VillagerSchool?", true);
        }

        if (/*currentWaterShareValue >= 50.1 &&*/ currentWaterShareValue <= 62.5)
        {
            villager5.SetBool("VillagerSchool?", false);
        }
        else if (currentWaterShareValue >= 62.6)
        {
            villager5.SetBool("VillagerSchool?", true);
        }

        if (/*currentWaterShareValue >= 62.6 && */currentWaterShareValue <= 75)
        {
            villager6.SetBool("VillagerSchool?", false);
        }
        else if (currentWaterShareValue >= 75.1)
        {
            villager6.SetBool("VillagerSchool?", true);
        }

        if (/*currentWaterShareValue >= 75.1 &&*/ currentWaterShareValue <= 87.5)
        {
            villager7.SetBool("VillagerSchool?", false);
        }
        else if (currentWaterShareValue >= 87.6)
        {
            villager7.SetBool("VillagerSchool?", true);
        }

        if (/*currentWaterShareValue >= 87.6 &&*/ currentWaterShareValue <= 75)
        {
            villager8.SetBool("VillagerSchool?", false);
        }
        else if (currentWaterShareValue >= 75.1)
        {
            villager8.SetBool("VillagerSchool?", true);
        }
    }

    private void ChangeNPCSprites() // changes the health Icon of the NPC regarding the water share % for the sanitation
    {
        if (currentWaterShareValue <25)
        {
            villager1sprite.color = villager2sprite.color = villager3sprite.color = villager4sprite.color = villager5sprite.color = villager6sprite.color = villager7sprite.color = villager8sprite.color = Color.red;
        }
        else if (currentWaterShareValue >=50 && currentWaterShareValue<66)
        {
            villager1sprite.color = villager2sprite.color = villager3sprite.color = villager4sprite.color = villager5sprite.color = villager6sprite.color = villager7sprite.color = villager8sprite.color = Color.yellow;
        }
        else if (currentWaterShareValue >= 75)
        {
            villager1sprite.color = villager2sprite.color = villager3sprite.color = villager4sprite.color = villager5sprite.color = villager6sprite.color = villager7sprite.color = villager8sprite.color = Color.green;
        }
    }
}
