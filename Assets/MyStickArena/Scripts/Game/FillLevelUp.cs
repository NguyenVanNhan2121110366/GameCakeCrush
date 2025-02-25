using UnityEngine;

public class FillLevelUp : MonoBehaviour
{
    public void TurnOffFill()
    {
        ExperienceBarManager.Instance.FillLevel.SetActive(false);
    }
}
