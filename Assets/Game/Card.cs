using UnityEngine;

enum CardType
{
    LightAttack,
    HeavyAttack,
    Shield,
    Dodge,
    LoadHeavy
}

public class Card : MonoBehaviour
{
    CardType type;
    int damages;
    bool isLoaded;

}
