using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStats : MonoBehaviour {

    public Slider HealthBar;

    public int MaxHealth;

    private int _currentHealth;
    public int CurrentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = value; }
    }

    public void TakeDamage()
    {
        CurrentHealth--;
        HealthBar.value = Mathf.Clamp01((float)CurrentHealth / (float)MaxHealth);

        if(CurrentHealth <= 0)
        {
            KillBoss();
        }
    }

    private void KillBoss()
    {
        //RUN CUT SCENE
    }
}
