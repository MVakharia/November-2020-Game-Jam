using UnityEngine;
using TMPro;

public class UpgradeShop : MonoBehaviour
{
    private static UpgradeShop singleton;

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
    private GameManager gameMgr;
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
    public static UpgradeShop Singleton { get => singleton; private set => singleton = value; }
    public TMP_Text ShopMessageText { get { return shopMessageText; } private set { shopMessageText = value; } }
    public TMP_Text MetalText { get => metalText; private set => metalText = value; }
    public TMP_Text ShieldLevelText { get => shieldLevelText; private set => shieldLevelText = value; }
    public TMP_Text HullLevelText { get => hullLevelText; private set => hullLevelText = value; }
    public TMP_Text LaserLevelText { get => laserLevelText; private set => laserLevelText = value; }
    public TMP_Text ShieldHealthText { get => shieldHealthText; private set => shieldHealthText = value; }
    public TMP_Text HullHealthText { get => hullHealthText; private set => hullHealthText = value; }
    public GameManager GameMgr { get => gameMgr; private set => gameMgr = value; }
    public TMP_Text UpgradeShieldText { get => upgradeShieldText; private set => upgradeShieldText = value; }
    public TMP_Text DowngradeShieldText { get => downgradeShieldText; private set => downgradeShieldText = value; }
    public TMP_Text UpgradeHullText { get => upgradeHullText; private set => upgradeHullText = value; }
    public TMP_Text DowngradeHullText { get => downgradeHullText; private set => downgradeHullText = value; }
    public TMP_Text UpgradeLaserText { get => upgradeLaserText; private set => upgradeLaserText = value; }
    public TMP_Text DowngradeLaserText { get => downgradeLaserText; private set => downgradeLaserText = value; }

    private void Start()
    {
        //GameMgr = GameManager.Singleton.gameObject.GetComponent<GameManager>();
    }

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

    public int ShieldCost() { return PlayerSpaceship.Singleton.ShieldLevel * 1000; }
    public string ShieldCostText() { return "Upgrade: " + ShieldCost() + " metal"; }
    public int ShieldRefund() { return (ShieldCost() - 1000) / 2; }
    public string ShieldRefundText() { return "Downgrade: +" + ShieldRefund() + " metal"; }
    public void UpgradeShields()
    {
        if (ShieldCost() <= GameMgr.Metal) 
        { 
            if(PlayerSpaceship.Singleton.ShieldLevel < 10)
            {
                GameMgr.RemoveFromCurrency(ShieldCost()); PlayerSpaceship.Singleton.UpgradeShield();
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
        if(PlayerSpaceship.Singleton.ShieldLevel > 1)
        {
            GameMgr.AddToCurrency(ShieldRefund());
            PlayerSpaceship.Singleton.DowngradeShield();
        }
        else
        {
            PlayMessage("You can't downgrade a Level 1 shield!");
        }
    }

    public void RepairShields ()
    {
        if(ShieldRepairCost() < GameMgr.Metal)
        {
            if(PlayerSpaceship.Singleton.ShieldHealth < PlayerSpaceship.Singleton.ShieldMaximumHealth)
            {
                GameMgr.RemoveFromCurrency(ShieldRepairCost());
                PlayerSpaceship.Singleton.RepairShield();
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

    public int ShieldRepairCost ()
    {
        return 500;
    }

    public int HullCost() { return PlayerSpaceship.Singleton.HullLevel * 1000; }
    public string HullCostText () { return "Upgrade: " + HullCost() + " metal"; }
    public int HullRefund ()
    {
        return (HullCost() - 1000) / 2;
    }
    public string HullRefundText ()
    {
        return "Downgrade: +" + HullRefund() + " metal";
    }
    
    public void UpgradeHull() 
    {
        if (HullCost() <= GameMgr.Metal)
        {
            if (PlayerSpaceship.Singleton.HullLevel < 10)
            {
                GameMgr.RemoveFromCurrency(HullCost()); PlayerSpaceship.Singleton.UpgradeHull();
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
        if(PlayerSpaceship.Singleton.HullLevel > 1)
        {
            GameMgr.AddToCurrency(HullRefund());
            PlayerSpaceship.Singleton.DowngradeHull();
        }    
        else
        {
            PlayMessage("You can't downgrade a Level 1 hull!");
        }
    }

    public void RepairHull ()
    {
        if (HullRepairCost() < GameMgr.Metal)
        {
            if (PlayerSpaceship.Singleton.HullHealth < PlayerSpaceship.Singleton.HullMaximumHealth)
            {
                GameMgr.RemoveFromCurrency(HullRepairCost());
                PlayerSpaceship.Singleton.RepairHull();
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

    public int HullRepairCost ()
    {
        return 500;
    }

    public int LaserCost() { return PlayerSpaceship.Singleton.LaserLevel * 1000; }
    public string LaserCostText() { return "Upgrade: " + LaserCost() + " metal"; }
    public int LaserRefund ()
    {
        return (LaserCost() - 1000) / 2;
    }
    public string LaserRefundText ()
    {
        return "Downgrade: +" + LaserRefund() + " metal";
    }

    public void UpgradeLaser() 
    { 
        if (LaserCost() <= GameMgr.Metal) 
        { 
            if(PlayerSpaceship.Singleton.LaserLevel < 10)
            {
                GameMgr.RemoveFromCurrency(LaserCost()); PlayerSpaceship.Singleton.UpgradeLaser();
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
        if(PlayerSpaceship.Singleton.LaserLevel > 1)
        {
            GameMgr.AddToCurrency(LaserRefund());
            PlayerSpaceship.Singleton.DowngradeLaser();
        }
        else
        {
            PlayMessage("You can't downgrade a Level 1 laser!");
        }
    }

    private void PlayMessage(string message)
    {
        ShopMessageText.text = message;
    }

    private void SetMetalAmountText ()
    {
        if(GameManager.Singleton.CurrentPhase == GamePhase.StartScreen 
            || GameManager.Singleton.CurrentPhase == GamePhase.Loss
            || GameManager.Singleton.CurrentPhase == GamePhase.Boss)
        {
            MetalText.text = "";
            
        }
        else
        {
            MetalText.text = "Metal: " + GameMgr.Metal;
        }
    }

    private void SetShieldLevelText()
    {
        ShieldLevelText.text = "Shield Level: " + PlayerSpaceship.Singleton.ShieldLevel;
    }

    private void SetHullLevelText ()
    {
        HullLevelText.text = "Hull Level: " + PlayerSpaceship.Singleton.HullLevel;
    }

    private void SetLaserLevelText ()
    {
        LaserLevelText.text = "Laser Level: " + PlayerSpaceship.Singleton.LaserLevel;
    }

    private void SetShieldHealthText ()
    {
        ShieldHealthText.text = "Shield Health: \n" + PlayerSpaceship.Singleton.ShieldHealth + "/ " + PlayerSpaceship.Singleton.ShieldMaximumHealth;
    }

    private void SetHullHealthText ()
    {
        HullHealthText.text = "Hull Health: \n" + PlayerSpaceship.Singleton.HullHealth + "/ " + PlayerSpaceship.Singleton.HullMaximumHealth;
    }

    private void SetShieldUpgradeText ()
    {
        UpgradeShieldText.text = ShieldCostText();
    }
    private void SetShieldDowngradeText()
    {
        DowngradeShieldText.text = ShieldRefundText();
    }
    private void SetHullUpgradeText()
    {
        UpgradeHullText.text = HullCostText();
    }
    private void SetHullDowngradeText()
    {
        DowngradeHullText.text = HullRefundText();
    }
    private void SetLaserUpgradeText()
    {
        UpgradeLaserText.text = LaserCostText();
    }
    private void SetLaserDowngradeText()
    {
        DowngradeLaserText.text = LaserRefundText();
    }
}