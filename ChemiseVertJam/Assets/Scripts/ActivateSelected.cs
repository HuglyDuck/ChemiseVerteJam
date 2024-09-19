using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSelected : MonoBehaviour
{
    [SerializeField] private ETypeScript _typeScript = ETypeScript.ObjectMovement; 

    public void Activate()
    {
        switch (_typeScript)
        {
            case ETypeScript.ObjectMovement:
                if( TryGetComponent( out ObjectMovement _scriptObjectMovement))
                {
                    _scriptObjectMovement.enabled = true;
                }

                break;
            case ETypeScript.SwitchLight:
                if (TryGetComponent(out LightSwitch _scriptLightSwitch))
                {
                    _scriptLightSwitch.enabled = true;
                }

                break;
            case ETypeScript.DownLight:
                if (TryGetComponent(out LightDown _scriptLightDown))
                {
                    _scriptLightDown.enabled = true;
                }
                break;
        }
    }

    public void Desactivate()
    {
        switch (_typeScript)
        {
            case ETypeScript.ObjectMovement:
                if (TryGetComponent(out ObjectMovement _scriptObjectMovement))
                {
                    _scriptObjectMovement.enabled = false;
                }

                break;
            case ETypeScript.SwitchLight:
                if (TryGetComponent(out LightSwitch _scriptLightSwitch))
                {
                    _scriptLightSwitch.enabled = false;
                }

                break;
            case ETypeScript.DownLight:
                if (TryGetComponent(out LightDown _scriptLightDown))
                {
                    _scriptLightDown.enabled = false;
                }
                break;
        }
    }

   
}
public enum ETypeScript
{
    ObjectMovement,
    SwitchLight,
    DownLight
}
