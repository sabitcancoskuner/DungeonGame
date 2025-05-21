using UnityEngine;

[CreateAssetMenu(fileName = "RandomWalkParameters", menuName = "RandomWalk/RandomWalkData")]
public class WalkerSO : ScriptableObject
{
    public int numOfWalkers = 3;
    public int walkLength = 10;
    public int iteration = 5;
    public bool startFromRandomTile;
}
