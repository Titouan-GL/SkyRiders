using UnityEngine;

public interface MeleeCombat
{
    void UpdateCombo();
    void ceaseMelee();
    void InstantiateSlash();
    void Staggered();
    void Parried();
    void ChangeAttack();

}
