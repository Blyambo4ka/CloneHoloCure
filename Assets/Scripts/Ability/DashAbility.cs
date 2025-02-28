using UnityEngine;
using System.Collections;

public class DashAbility : MonoBehaviour
{
    public GameObject fireTrailPrefab;
    public GameObject fireTrailMaxLevelPrefab;
    public Transform feetstep;
    public float fireDuration = 2f;
    private int fireDamage = 10;
    private int upgradeLevel = 0;
    private const int maxUpgradeLevel = 5;
    public bool isUnlocked = false;
    public float dashSpeed = 10f;

    public Sprite abilityIcon; // Иконка способности

    private void Start()
    {
        StartCoroutine(AutoSpawnFireTrail());
    }


    public void UnlockAbility()
    {
        if (isUnlocked) return;

        isUnlocked = true;

        // Добавляем способность в инвентарь
        InventoryUIManager.Instance.AddAbilityToUI("DashAbility", abilityIcon, upgradeLevel);

        Debug.Log("Dash unlocked!");
    }

    public void UpgradeAbility()
    {
        if (!isUnlocked || upgradeLevel >= maxUpgradeLevel) return;

        upgradeLevel++;
        fireDamage += 1;
        dashSpeed += 2f;

        // Обновляем уровень способности в инвентаре
        InventoryUIManager.Instance.UpdateAbilityLevelInUI("DashAbility", upgradeLevel);

        if (upgradeLevel == maxUpgradeLevel)
        {
            Debug.Log("Dash upgraded to MAX level! Using alternative fire trail!");
        }

        Debug.Log($"Dash upgraded! Level: {upgradeLevel}, Damage: {fireDamage}, Speed: {dashSpeed}");
    }
    private IEnumerator AutoSpawnFireTrail()
    {
        while (true)
        {
            if (isUnlocked && feetstep != null)
            {
                Vector3 spawnPosition = feetstep.position;
                GameObject fireTrail = Instantiate(
                    upgradeLevel >= maxUpgradeLevel ? fireTrailMaxLevelPrefab : fireTrailPrefab,
                    spawnPosition,
                    Quaternion.identity
                );
                fireTrail.GetComponent<FireTrail>().Initialize(fireDamage, fireDuration);
            }
            else if (!isUnlocked)
            {
                yield return new WaitUntil(() => isUnlocked);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }
}
