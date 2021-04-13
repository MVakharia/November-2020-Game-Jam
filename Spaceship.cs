using System.Collections;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    #region Fields
    [SerializeField]
    protected int hullHealth;
    [SerializeField]
    protected int hullMaximumHealth;
    [SerializeField]
    protected int hullLevel;
    [SerializeField]
    protected int shieldHealth;
    [SerializeField]
    protected int shieldMaximumHealth;
    [SerializeField]
    protected GameObject shield;
    [SerializeField]
    protected MeshCollider hullCollider;
    [SerializeField]
    protected bool shieldIsFrozen;
    [SerializeField]
    protected bool shieldIsRebooting;
    #endregion

    #region Properties
    public int HullHealth => hullHealth;
    public int HullMaximumHealth => hullMaximumHealth; 
    public int ShieldHealth => shieldHealth;
    public int ShieldMaximumHealth => shieldMaximumHealth;
    #endregion

    #region Methods
    private void DeactivateShield() { shield.SetActive(false); ThawShield(); }
    public void ActivateShield() => shield.SetActive(true);
    public void ShieldHit() { if (shieldIsFrozen) { DepleteShield(); return; } DamageShield(); }
    public void DamageShield() => shieldHealth--;
    public void DepleteShield() => shieldHealth -= shieldHealth;
    public void DamageHull() => hullHealth--;
    public void FreezeShield() => shieldIsFrozen = true;
    public void ThawShield() => shieldIsFrozen = false;
    public void HullHit() { if (!shield.activeSelf && hullHealth > 0) { DamageHull(); } }
    private void SetHullCollision() => hullCollider.enabled = !shield.activeSelf;
    public void StartRebootingShield() => shieldIsRebooting = true;
    public void FinishRebootingShield() => shieldIsRebooting = false;
    public void StartFreezingShield(int length) => StartCoroutine(FreezeAndThawShield(length));
    public void ShockShield(int length) => StartCoroutine(RebootShield(length));

    public IEnumerator FreezeAndThawShield(int length)
    {
        FreezeShield();
        yield return new WaitForSeconds(length);
        ThawShield();
    }

    public void InheritedUpdateFunctionality()
    {
        if (shieldHealth <= 0)
        {
            DeactivateShield();
        }
        SetHullCollision();
    }

    public IEnumerator RebootShield(int length)
    {
        StartRebootingShield();
        DeactivateShield();

        yield return new WaitForSeconds(length);

        ActivateShield();
        FinishRebootingShield();
    }
    #endregion
}