using UnityEngine;
using UnityEngine.Animations;




[RequireComponent(typeof(LookAtConstraint))]
public class LookAtCamere : MonoBehaviour
{
    private void Awake()
    {
        var lookAtConstraint = GetComponent<LookAtConstraint>();
        var source = new ConstraintSource();
        source.sourceTransform = Camera.main.transform;
        source.weight = 1;
        
        lookAtConstraint.AddSource(source);
        lookAtConstraint.constraintActive = true;




    }
}
