#if(UNITY_EDITOR)
using UnityEngine;

namespace VekemanSacha
{
    [ExecuteInEditMode]
    public class ControlPoint : MonoBehaviour
    {
        public ConveyorGenerator generator;

        void OnEnable()
        {
            if (generator != null)
            {
                gameObject.name = ("Control Point " + generator.NumberControlPoint);
                generator.AddToList(this.gameObject);
            }
        }
        private void OnDisable()
        {
            if (generator != null)
            {
                generator.RemoveFromList(this.gameObject);
            }
        }
    }
}
#endif