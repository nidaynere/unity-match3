using UnityEngine;

[CreateAssetMenu(fileName = "Match3GameSettings", menuName = "Create Match3 Game Settings", order = 1)]
public class Match3GameSettings : ScriptableObject {
    [Tooltip("X size of the grid")]
    public int GridSizeX;

    [Tooltip("Y size of the grid")]
    public int GridSizeY;

    [Tooltip("Members of the game")]
    public GameMember[] Members;

    [Tooltip("How many horizontal match required?")]
    public ushort RequiredMatch = 3;

    [Tooltip ("How many pool will be generated for per member?")]
    public ushort PoolSize = 100;

    private string[] GetMembersAsString() {
        int length = Members.Length;

        string[] membersAsString = new string[length];
        for (int i = 0; i < length; i++) {
            membersAsString[i] = Members[i].name;
        }

        return membersAsString;
    }

    public bool ValidateMembers (out string [] memberIds) {
        memberIds = GetMembersAsString();

        for (int i = 0, length = memberIds.Length; i < length; i++) {
            for (int e = i-1; e >= 0; e--) {
                if (memberIds[i].Equals(memberIds[e])) {
                    Debug.LogError("Members have conflict. Member prefab names used as unique identifiers. There is a conflict in the given members array. Conflict name => " + memberIds[i]);
                    return false;
                }
            }
        }

        return true;
    }
}
