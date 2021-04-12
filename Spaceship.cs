using System.Collections;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
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

    public int HullHealth { get { return hullHealth; } protected set { hullHealth = value; } }
    public int HullMaximumHealth { get => hullMaximumHealth; protected set => hullMaximumHealth = value; }
    public int ShieldHealth { get { return shieldHealth; } protected set { shieldHealth = value; } }
    public int ShieldMaximumHealth { get => shieldMaximumHealth; protected set => shieldMaximumHealth = value; }
    public GameObject Shield { get => shield; protected set => shield = value; }
    public MeshCollider HullCollider { get => hullCollider; protected set => hullCollider = value; }
    public bool ShieldIsFrozen { get => shieldIsFrozen; protected set => shieldIsFrozen = value; }
    public bool ShieldIsRebooting { get => shieldIsRebooting; protected set => shieldIsRebooting = value; }
    private void DeactivateShield() { Shield.SetActive(false); ThawShield(); }
    public void ActivateShield() { Shield.SetActive(true); }
    public void ShieldHit() { if (ShieldIsFrozen) { DepleteShield(); return; } DamageShield(); }
    public void DamageShield() { ShieldHealth--; }
    public void DepleteShield() { ShieldHealth -= ShieldHealth; }
    public void DamageHull() { HullHealth--; }
    public void FreezeShield() { ShieldIsFrozen = true; }
    public void ThawShield() { ShieldIsFrozen = false; }
    public void HullHit() { if (!Shield.activeSelf && HullHealth > 0) { DamageHull(); } }
    private void SetHullCollision() { hullCollider.enabled = !Shield.activeSelf; }
    public IEnumerator FreezeAndThawShield(int length)
    {
        FreezeShield();
        yield return new WaitForSeconds(length);
        ThawShield();
    }

    public void InheritedUpdateFunctionality()
    {
        if (ShieldHealth <= 0)
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

    public void StartRebootingShield()
    {
        ShieldIsRebooting = true;
    }

    public void FinishRebootingShield()
    {
        ShieldIsRebooting = false;
    }

    public void StartFreezingShield(int length) { StartCoroutine(FreezeAndThawShield(length)); }

    public void ShockShield(int length) { StartCoroutine(RebootShield(length)); }
}