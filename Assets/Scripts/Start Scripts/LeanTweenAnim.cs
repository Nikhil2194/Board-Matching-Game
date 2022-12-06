using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenAnim : MonoBehaviour
{
    [SerializeField] GameObject colourWheel,loading;

    [SerializeField] GameObject bgTitle;
    [SerializeField] GameObject title;

    [SerializeField] GameObject crossBoard, startButton;
    [SerializeField] GameObject greensphere1, bluesphere2, yellosphere3, purplesphere4, orangesphere5;

    void Start()
    {
        Object.Destroy(loading, 2.2f);
        LeanTween.rotate(colourWheel, new Vector3(0, 0, 90), 9f).setDelay(2.1f).setLoopPingPong();

        LeanTween.scale(bgTitle, new Vector3(1.78f, 1.78f, 1.78f), 0.5f).setDelay(2f).setEase(LeanTweenType.easeInOutBack);
        LeanTween.move(title.GetComponent<RectTransform>(), new Vector3(0, -140, 0), 0.19f).setDelay(2.6f);

        LeanTween.scale(crossBoard, new Vector3(1.3f, 1.4f, 0.8f), 0.19f).setDelay(3.1f);

        //Sphere Cartoon Falling Animation
        LeanTween.move(greensphere1, new Vector3(-2.3f, 3.83f), 0.9f).setDelay(4.8f).setEaseInOutBounce();
        LeanTween.move(bluesphere2, new Vector3(2.26f, 3.75f), 0.9f).setDelay(5.5f).setEaseInOutBounce();

        LeanTween.move(purplesphere4, new Vector3(-2.3f, -0.5f), 0.9f).setDelay(3.5f).setEaseInOutBounce();
        LeanTween.move(yellosphere3, new Vector3(2.18f, -0.5f), 0.9f).setDelay(4.2f).setEaseInOutBounce();
        LeanTween.move(orangesphere5, new Vector3(0, 1.76f), 0.9f).setDelay(6.2f).setEaseInOutBounce();

        LeanTween.scale(startButton, new Vector3(1f, 1f, 1f), 0.5f).setDelay(6.9f).setEase(LeanTweenType.easeInOutQuint);

       
    }

}
