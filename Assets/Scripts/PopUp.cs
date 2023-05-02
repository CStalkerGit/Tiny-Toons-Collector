using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public FontSprites FontSprites;
    public SpriteRenderer originSprite;

    int lastNumber = -1;
    float lifetime = 0.75f;

    void Awake()
    {

    }

    void FixedUpdate()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0) Destroy(gameObject);
        var pos = transform.position;
        pos.y += Time.deltaTime;
        transform.position = pos;
    }

    public void SetNumber(int number)
    {
        if (number == lastNumber) return;
        lastNumber = number;

        List<int> digits = new List<int>();
        while (number > 0)
        {
            digits.Add(number % 10);
            number /= 10;
        }
        digits.Reverse();

        for (int i = 0; i < digits.Count; i++)
        {
            var position = transform.position;
            const float sprWidth = 0.25f;
            position.x += ((digits.Count - 1) * sprWidth) / -2f + i * sprWidth;
            var obj = Instantiate(originSprite, position, Quaternion.identity, transform);
            obj.sprite = FontSprites.sprites[digits[i]];            
        }
    }
}
