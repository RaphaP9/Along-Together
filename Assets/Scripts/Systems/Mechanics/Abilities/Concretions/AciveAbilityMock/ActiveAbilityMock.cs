using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAbilityMock : ActiveAbility
{
    private ActiveAbilityMockSO ActiveAbilityMockSO => abilitySO as ActiveAbilityMockSO;

    #region Logic Methods
    protected override void HandleFixedUpdateLogic() { }
    protected override void HandleUpdateLogic() { }
    #endregion

    #region Abstract Methods
    protected override void OnAbilityVariantActivationMethod() { }
    protected override void OnAbilityVariantDeactivationMethod() { }
    #endregion
}
