using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public int level;
    public int exp;
    public int maxExp;
    public int attributePoints;

    public int health; 
    public int maxHealth;
    public int mana;
    public int maxMana;
    public int satiety;

    public int strength;
    public int dexterity;
    public int endurance;
    public int intelligence;
    public int wisdom;
    public int psyche;
    public int luck;


    public void LevelUp() {
        // check if you have the required exp to level up first
        if(exp < maxExp) return;

        level++;
        exp = 0;
        maxExp = (int) (maxExp * 1.085f);
        attributePoints += (Random.Range(1,3));
        maxHealth += (int) Random.Range(20,40) + (((endurance * endurance)/100 + (strength + strength)/200));
        health = maxHealth;
        maxMana = Random.Range(15,30) + (((intelligence * intelligence)/100 + (wisdom * wisdom)/200));
        mana = maxMana;
        satiety = 0;
    }

    public float GetStatBonus(int attribute) {
        return (Mathf.Pow(attribute - 75, 3) / 3600) + 90;
    }
}
