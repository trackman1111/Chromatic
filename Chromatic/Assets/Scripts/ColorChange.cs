using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

namespace UnityEngine.Rendering
{
    public class ColorChange : MonoBehaviour
    {
        //UNCHECK the Color Adjustments override used by the volume.
        public Volume selvolume;
        public bool Coloring;
        private ColorAdjustments ColAdj;
        private readonly float SPEED = .03f;
        // Start is called before the first frame update
        void Start()
        {
            ColAdj = ScriptableObject.CreateInstance<ColorAdjustments>();
            ColAdj.saturation.overrideState = true;
            ColAdj.saturation.value = -100;

            selvolume.profile.components.Add(ColAdj);
        }

        // Update is called once per frame
        void Update()
        {
            
            if (Coloring == true && ColAdj.saturation.value < 0)
            {
                
                ColAdj.saturation.value += SPEED;

            }


        }

    }


}
