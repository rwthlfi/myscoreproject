using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Fresenius
{
    public class CategoryBall : MonoBehaviour
    {
        public TextMeshPro categoryText;

        /// <summary>
        /// get the name of the category
        /// </summary>
        /// <returns></returns>
        public string GetCategoryText()
        {
            return categoryText.text;
        }

        /// <summary>
        /// set the category text
        /// </summary>
        /// <param name="_str">what to write here</param>
        public void SetCategoryText(string _str)
        {
            categoryText.text = _str;
        }

    }

}