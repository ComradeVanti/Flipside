using UnityEngine;

namespace Foxy.Flipside
{

    public class BackgroundFader : MonoBehaviour
    {

        [SerializeField] private Renderer backgroundRenderer;
        [SerializeField] private float fadeSpeed;

        private Material BackgroundMaterial =>
            backgroundRenderer.material;

        private float[] HSV
        {
            get
            {
                Color.RGBToHSV(BackgroundMaterial.color, out var r, out var g, out var b);
                return new[] { r, g, b };
            }
            set => BackgroundMaterial.color = Color.HSVToRGB(value[0], value[1], value[2]);
        }


        private float H
        {
            get => HSV[0];
            set
            {
                var hsv = HSV;
                hsv[0] = value;
                HSV = hsv;
            }
        }


        private void Update() =>
            H = Mathf.Repeat(H + Time.deltaTime * fadeSpeed, 1);

    }

}