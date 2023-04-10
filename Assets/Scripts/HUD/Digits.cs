using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Digits : MonoBehaviour
{
    public Sprite[] sprites;
    public int test = 0;
    int lastNumber;

    Image[] images;

    // Start is called before the first frame update
    void Start()
    {
        images = GetComponentsInChildren<Image>();

        lastNumber = -1;
        Set(0);
    }

    public void Set(int number)
    {
        if (number == lastNumber) return;
        lastNumber = number;

        if (number < 0) number = 0;

        if (number.ToString().Length > images.Length)
        {
            foreach(var image in images)
            {
                image.gameObject.SetActive(true);
                image.sprite = sprites[9];
            }
            return;
        }

        List<int> digits = new List<int>();
        while (number > 9)
        {
            digits.Add(number % 10);
            number /= 10;
        }
        digits.Add(number);
        //digits.Reverse();

        for (int i = 0; i < images.Length; i++)
        {
            Image image = images[images.Length - i - 1];
            image.gameObject.SetActive(i < digits.Count);
            if (i < digits.Count)
                image.sprite = sprites[digits[i]];
        }
    }
}
