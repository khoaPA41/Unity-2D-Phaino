using Raziel.Battle;
using UnityEngine;


public enum EnemyInput
{
    Draw,
    Battle,
    Main_II,
    ChooseCard,
    Summon,
    End
}
public class EnemyAIControllerInput : MonoBehaviour
{
    [SerializeField] private DuelInformation _duelInformation;
    public EnemyInput currentInput;








}
