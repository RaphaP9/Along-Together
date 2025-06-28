using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteTreatHandler : TreatHandler
{
    public static ExecuteTreatHandler Instance { get; private set; }

    private ExecuteTreatConfigSO ExecuteTreatConfigSO => treatConfigSO as ExecuteTreatConfigSO;

    protected override void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    protected override void OnTreatDisableMethod() { }
    protected override void OnTreatEnableMethod() { }

}
