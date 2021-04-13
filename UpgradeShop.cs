using UnityEngine;
using TMPro;

public class UpgradeShop : MonoBehaviour
{
    #region Fields    
    [SerializeField]
    private TMP_Text shopMessageText;
    [SerializeField]
    private TMP_Text metalText;
    [SerializeField]
    private TMP_Text shieldLevelText;
    [SerializeField]
    private TMP_Text hullLevelText;
    [SerializeField]
    private TMP_Text laserLevelText;
    [SerializeField]
    private TMP_Text shieldHealthText;
    [SerializeField]
    private TMP_Text hullHealthText;
    [SerializeField]
    private TMP_Text upgradeShieldText;
    [SerializeField]
    private TMP_Text downgradeShieldText;
    [SerializeField]
    private TMP_Text upgradeHullText;
    [SerializeField]
    private TMP_Text downgradeHullText;
    [SerializeField]
    private TMP_Text upgradeLaserText;
    [SerializeField]
    private TMP_Text downgradeLaserText;
    #endregion

    #region Properties
    private GameManager GameMgr => GameManager.Singleton;
    private PlayerSpaceship PlayerShip => PlayerSpaceship.Singleton;
    public int ShieldCost() => PlayerShip.ShieldLevel * 1000;
    public string ShieldCostText() => "Upgrade: " + ShieldCost() + " metal";
    public int ShieldRefundAmount() => (ShieldCost() - 1000) / 2;
    public string ShieldRefundText() => "Downgrade: +" + ShieldRefundAmount() + " metal";
    public int ShieldRepairCost => 500;
    public int HullCost => PlayerShip.HullLevel * 1000;
    public string HullCostText() { return "Upgrade: " + HullCost + " metal"; }
    public int HullRefund => (HullCost - 1000) / 2;
    public string HullRefundText => "Downgrade: +" + HullRefund + " metal";
    public int HullRepairCost => 500;
    public int LaserCost => PlayerShip.LaserLevel * 1000; public string LaserCostText() { return "Upgrade: " + LaserCost + " metal"; }
    public int LaserRefund => (LaserCost - 1000) / 2;
    public string LaserRefundText => "Downgrade: +" + LaserRefund + " metal";
    
    #endregion

    #region Methods
    private void PlayMessage(string message) => shopMessageText.text = message;
    private void SetShieldLevelText() => shieldLevelText.text = "Shield Level: " + PlayerShip.ShieldLevel;
    private void SetHullLevelText() => hullLevelText.text = "Hull Level: " + PlayerShip.HullLevel;
    private void SetLaserLevelText() => laserLevelText.text = "Laser Level: " + PlayerShip.LaserLevel;
    private void SetShieldHealthText() => shieldHealthText.text = "Shield Health: \n" + PlayerShip.ShieldHealth + "/ " + PlayerShip.ShieldMaximumHealth;
    private void SetHullHealthText() => hullHealthText.text = "Hull Health: \n" + PlayerShip.HullHealth + "/ " + PlayerShip.HullMaximumHealth;
    private void SetShieldUpgradeText() => upgradeShieldText.text = ShieldCostText();
    private void SetShieldDowngradeText() => downgradeShieldText.text = ShieldRefundText();
    private void SetHullUpgradeText() => upgradeHullText.text = HullCostText();
    private void SetHullDowngradeText() => downgradeHullText.text = HullRefundText;
    private void SetLaserUpgradeText() => upgradeLaserText.text = LaserCostText();
    private void SetLaserDowngradeText() => downgradeLaserText.text = LaserRefundText;
    public void UpgradeShields()
    {
        if (ShieldCost() <= GameMgr.Metal)
        {
            if (PlayerShip.ShieldLevel < 10)
            {
                GameMgr.RemoveFromCurrency(ShieldCost()); PlayerShip.UpgradeShield();
            }
            else
            {
                PlayMessage("You can't upgrade a level 10 shield!");
            }
        }
        else
        {
            PlayMessage("You don't have enough metal to pay for upgrades to your shield.");
        }
    }
    public void DowngradeShields()
    {
        if (PlayerShip.ShieldLevel > 1)
        {
            GameMgr.AddToCurrency(ShieldRefundAmount());
            PlayerShip.DowngradeShield();
        }
        else
        {
            PlayMessage("You can't downgrade a Level 1 shield!");
        }
    }
    public void RepairShields()
    {
        if (ShieldRepairCost< GameMgr.Metal)
        {
            if (PlayerShip.ShieldHealth < PlayerShip.ShieldMaximumHealth)
            {
                GameMgr.RemoveFromCurrency(ShieldRepairCost);
                PlayerShip.RepairShield();
            }
            else
            {
                PlayMessage("Your shields are at full capacity.");
            }
        }
        else
        {
            PlayMessage("You don't have enough metal to pay for repairs to your shield.");
        }
    }

    public void UpgradeHull()
    {
        if (HullCost <= GameMgr.Metal)
        {
            if (PlayerShip.HullLevel < 10)
            {
                GameMgr.RemoveFromCurrency(HullCost); PlayerShip.UpgradeHull();
            }
            else
            {
                PlayMessage("You can't upgrade a Level 10 hull!");
            }
        }
        else
        {
            PlayMessage("You don't have enough metal to pay for upgrades to your hull.");
        }
    }

    public void DowngradeHull()
    {
        if (PlayerShip.HullLevel > 1)
        {
            GameMgr.AddToCurrency(HullRefund);
            PlayerShip.DowngradeHull();
        }
        else
        {
            PlayMessage("You can't downgrade a Level 1 hull!");
        }
    }

    public void RepairHull()
    {
        if (HullRepairCost < GameMgr.Metal)
        {
            if (PlayerShip.HullHealth < PlayerShip.HullMaximumHealth)
            {
                GameMgr.RemoveFromCurrency(HullRepairCost);
                PlayerShip.RepairHull();
            }
            else
            {
                PlayMessage("Your hull is fully intact.");
            }
        }
        else
        {
            PlayMessage("You don't have enough metal to pay for repairs to your hull.");
        }
    }

    public void UpgradeLaser()
    {
        if (LaserCost <= GameMgr.Metal)
        {
            if (PlayerShip.LaserLevel < 10)
            {
                GameMgr.RemoveFromCurrency(LaserCost); PlayerShip.UpgradeLaser();
            }
            else
            {
                PlayMessage("You can't upgrade a level 10 laser!");
            }
        }
        else
        {
            PlayMessage("You don't have enough metal to pay for upgrades to your laser.");
        }
    }
    public void DowngradeLaser()
    {
        if (PlayerShip.LaserLevel > 1)
        {
            GameMgr.AddToCurrency(LaserRefund);
            PlayerShip.DowngradeLaser();
        }
        else
        {
            PlayMessage("You can't downgrade a Level 1 laser!");
        }
    }

    private void SetMetalAmountText()
    {
        if (GameMgr.CurrentPhase == GamePhase.StartScreen
            || GameMgr.CurrentPhase == GamePhase.Loss
            || GameMgr.CurrentPhase == GamePhase.Boss)
        {
            metalText.text = "";

        }
        else
        {
            metalText.text = "Metal: " + GameMgr.Metal;
        }
    }
    #endregion

    private void Update()
    {
        SetMetalAmountText();

        SetShieldLevelText();

        SetHullLevelText();

        SetLaserLevelText();

        SetShieldHealthText();

        SetHullHealthText();

        SetShieldUpgradeText();

        SetShieldDowngradeText();

        SetHullUpgradeText();

        SetHullDowngradeText();

        SetLaserUpgradeText();

        SetLaserDowngradeText();
    }
}