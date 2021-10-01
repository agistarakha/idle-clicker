using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResourceController : MonoBehaviour
{
    public Text ResourceDescription;
    public Text ResourceUpgradeCost;
    public Text ResourceUnlockCost;
    public Button ResourceButton;
    public Image ResourceImage;
    private ResourceConfig _config;
    private int _index;
    private int _level
    {
        set
        {
            //Menyimpan value yang di set ke _level pada ProgressData
            UserDataManager.Progress.ResourcesLevels[_index] = value;
            UserDataManager.Save();
        }

        get
        {
            // Mengecek apakah index sudah terdapat pada Progress Data

            if (!UserDataManager.HasResources(_index))

            {

                // Jika tidak maka tampilkan level 1

                return 1;

            }

            // Jika iya maka tampilkan berdasarkan Progress Data
            return UserDataManager.Progress.ResourcesLevels[_index];
        }
    }
    public bool isUnlocked { get; private set; }
    public void SetConfig(int index, ResourceConfig config)
    {
        _index = index;
        _config = config;

        ResourceDescription.text = $"{ _config.Name } Lv. { _level }\n+{ GetOutput().ToString("0") }";

        ResourceUnlockCost.text = $"Unlock Cost\n{ _config.UnlockCost }";

        ResourceUpgradeCost.text = $"Upgrade Cost\n{ GetUpgradeCost() }";
        SetUnlocked(_config.UnlockCost == 0 || UserDataManager.HasResources(_index));
    }
    public void UnlockResource()
    {
        double unlockCost = GetUnlockCost();
        if (UserDataManager.Progress.Gold < unlockCost)
        {
            return;
        }
        SetUnlocked(true);
        AchievementController.Instance.UnlockAchievement(AchievementType.UnlockResource, _config.Name);
        GameManager.Instance.ShowNextResource();
    }
    public void SetUnlocked(bool unlocked)

    {

        IsUnlocked = unlocked;

        if (unlocked)
        {
            // Jika resources baru di unlock dan belum ada di Progress Data, maka tambahkan data

            if (!UserDataManager.HasResources(_index))

            {

                UserDataManager.Progress.ResourcesLevels.Add(_level);

                UserDataManager.Save();

            }
        }

        ResourceImage.color = IsUnlocked ? Color.white : Color.grey;

        ResourceUnlockCost.gameObject.SetActive(!unlocked);

        ResourceUpgradeCost.gameObject.SetActive(unlocked);

    }
    public double GetOutput()

    {

        return _config.Output * _level;

    }
    public double GetUpgradeCost()
    {

        return _config.UpgradeCost * _level;

    }
    public double GetUnlockCost()
    {

        return _config.UnlockCost;

    }
    // Start is called before the first frame update
    void Start()
    {
        ResourceButton.onClick.AddListener(() =>
        {
            if (isUnlocked)
            {
                UpgradeLevel();
            }
            else
            {
                UnlockResource();
            }
        });
    }

    public void UpgradeLevel()
    {
        double upgradeCost = GetUpgradeCost();
        if (UserDataManager.Progress.Gold < upgradeCost)
        {
            return;
        }
        GameManager.Instance.AddGold(-upgradeCost);
        _level++;

        ResourceUpgradeCost.text = $"Upgrade Cost\n{ GetUpgradeCost() }";

        ResourceDescription.text = $"{ _config.Name } Lv. { _level }\n+{ GetOutput().ToString("0") }";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsUnlocked
    {
        get
        {
            return isUnlocked;
        }
        set
        {
            isUnlocked = value;
        }
    }

}



