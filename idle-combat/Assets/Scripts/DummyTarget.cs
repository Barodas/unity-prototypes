using UnityEngine;

public class DummyTarget : MonoBehaviour
{
    public int maxHealth = 100;
    public int curHealth = 100;

    public void TakeDamage(int amount)
    {
        //curHealth -= amount;
        //curHealth = Mathf.Max(curHealth, 0);

        // Add visual feedback later
    }
}