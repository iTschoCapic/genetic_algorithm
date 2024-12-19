using UnityEngine;

enum CardType
{
    LightAttack,
    HeavyAttack,
    Shield,
    Dodge,
    LoadHeavy
}

public class Card : ScriptableObject
{
    CardType type;
    int damages;
    bool isLoaded;

}
