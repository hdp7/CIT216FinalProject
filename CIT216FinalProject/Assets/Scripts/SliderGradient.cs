using UnityEngine;
using UnityEngine.UI;
public class BoostUIController : MonoBehaviour
{
    public Gradient gradient;
    public Slider slider;
    public Image fillImage;

    public float boostAmount;
    private void Awake()
    {

    }

    private void Update()
    {
        float t = boostAmount; // 0 to 1
        fillImage.color = gradient.Evaluate(t);
    }

}
