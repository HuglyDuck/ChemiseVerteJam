using UnityEngine;

public class SpotlightChecker : MonoBehaviour
{
    public GameObject targetObject;

    private Light spotlight;

    void Start()
    {
      
        spotlight = GetComponent<Light>();

  
        if (spotlight.type != LightType.Spot)
        {
            Debug.LogError("This script should be attached to a Spotlight.");
        }
    }

    void Update()
    {
      
        if (IsInSpotlight(targetObject))
        {
            Debug.Log(targetObject.name + " In");
        }
        else
        {
            Debug.Log(targetObject.name + " Out");
        }
    }

   
    bool IsInSpotlight(GameObject obj)
    {
        if (obj == null) return false;

       
        Vector3 directionToTarget = obj.transform.position - transform.position;

        
        if (directionToTarget.magnitude > spotlight.range)
        {
            return false;
        }

        
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);
        if (angleToTarget > spotlight.spotAngle / 2)
        {
            return false;
        }

        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToTarget, out hit, spotlight.range))
        {
            
            if (hit.collider.gameObject == obj)
            {
                return true;
            }
        }

        return false;
    }
}
