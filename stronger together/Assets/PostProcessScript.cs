using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessScript : MonoBehaviour
{

    private Volume v;
    private LensDistortion lD;

    // Start is called before the first frame update
    void Start()
    {
        v = GetComponent<Volume>();
        v.profile.TryGet(out lD);

    }
    private void Update()
    {
 
    }
    //WHILE X IS TRUE, FROM A CO-ROUTINE ADD X AMOUNT OF LD EVERY FRAME LD.INTENSITY = LD.INTENSITY + 0.1
    //WHILE X IS FALSE AND LD.INTENSITY DOESNT EQUAL 0, SUBTRACT 0.1 EVERY FRAME OR TIME INCREMENT
    //GET X COMPONENT FROM MOVMENT SCRIPT
    private void DistortionShiftUp()
    {

        lD.intensity.value = -0.6f;
    }
    private void DistortionNormalize()
    {

        lD.intensity.value = 0f;
    }
}