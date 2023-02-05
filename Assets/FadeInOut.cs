using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public UnityEngine.UI.Image image;
    private float alpha = 1.0f;
    public float speed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }

    // Update is called once per frame
    void Update()
    {
        alpha -= Time.deltaTime * speed;
        if (alpha <= 0)
            alpha = 0.0f;
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }
}
