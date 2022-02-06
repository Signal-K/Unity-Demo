namespace GameCreator.UIComponents
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using GameCreator.Core;
    using GameCreator.Characters;
    using GameCreator.Core.Hooks;
    using GameCreator.Variables;


    public class ColourPickerComponent : MonoBehaviour
    {
        public Image colorPalette;
       
        public Image thumb;
        
          
        [HideInInspector]
       
        private Vector2 SpectrumXY; 
        private Bounds PictureBounds; 
        private Vector3 Max; 
        private Vector3 Min;
    
        private CanvasScaler myScale;

        [VariableFilter(Variable.DataType.Color)]
        public VariableProperty variable = null;

      
        private void Start()
        {


            myScale = colorPalette.canvas.transform.GetComponent<CanvasScaler>();


            SpectrumXY = new Vector2(colorPalette.GetComponent<RectTransform>().rect.width * myScale.transform.localScale.x, colorPalette.GetComponent<RectTransform>().rect.height * myScale.transform.localScale.y);
            PictureBounds = colorPalette.GetComponent<Collider2D>().bounds;
            Max = PictureBounds.max;
            Min = PictureBounds.min;


        }

        public static Vector3 MultiplyVectors(Vector3 V1, Vector3 V2)
        {
            float[] X = { V1.x, V2.x };
            float[] Y = { V1.y, V2.y };
            float[] Z = { V1.z, V2.z };
            return new Vector3(X[0] * X[1], Y[0] * Y[1], Z[0] * Z[1]);

        }

   
        
        public void OnMouseDown()
        {
            UpdateThumbPosition();
         
        }
      
        public void OnMouseDrag()
        {
            UpdateThumbPosition();
         

        }

        public void OnMouseUp()
        {

            this.variable.Set(Getcolor());
        }

       

        private Color Getcolor()
        {
            Vector2 spectrumScreenPosition = colorPalette.transform.position;
            Vector2 thumbScreenPosition = thumb.transform.position;
            Vector2 position = thumbScreenPosition - spectrumScreenPosition + SpectrumXY * 0.5f;
            Texture2D texture = colorPalette.mainTexture as Texture2D;
            
                myScale = colorPalette.canvas.transform.GetComponent<CanvasScaler>();


                SpectrumXY = new Vector2(colorPalette.GetComponent<RectTransform>().rect.width * myScale.transform.localScale.x, colorPalette.GetComponent<RectTransform>().rect.height * myScale.transform.localScale.y);
                PictureBounds = colorPalette.GetComponent<Collider2D>().bounds;
                Max = PictureBounds.max;
                Min = PictureBounds.min;

                position = new Vector2((position.x / (colorPalette.GetComponent<RectTransform>().rect.width)),
                                        (position.y / (colorPalette.GetComponent<RectTransform>().rect.height)));
                Color circularSelectedcolor = texture.GetPixelBilinear(position.x / myScale.transform.localScale.x, position.y / myScale.transform.localScale.y);
                circularSelectedcolor.a = 1;
                return circularSelectedcolor;

            
          
        }
        
        private void UpdateThumbPosition()
        {

                
                SpectrumXY = new Vector2(colorPalette.GetComponent<RectTransform>().rect.width * myScale.transform.localScale.x, colorPalette.GetComponent<RectTransform>().rect.height * myScale.transform.localScale.y);
                PictureBounds = colorPalette.GetComponent<Collider2D>().bounds;
                Max = PictureBounds.max;
                Min = PictureBounds.min;

                float x = Mathf.Clamp(Input.mousePosition.x, Min.x, Max.x + 1);
                float y = Mathf.Clamp(Input.mousePosition.y, Min.y, Max.y);
                Vector3 newPos = new Vector3(x, y, transform.position.z);

         
                if (thumb.transform.position != newPos)
                {
                    thumb.transform.position = newPos;
		
                }
            
        }
        
    }
}
