using UnityEngine;

[CreateAssetMenu(fileName = "Match3GameSettings", menuName = "Create Match3 Game Settings", order = 1)]
public class Match3GameSettings : ScriptableObject {
    [Tooltip ("X size of the grid")]
    public int GridSizeX;

    [Tooltip("Y size of the grid")]
    public int GridSizeY;

    [Tooltip("Members of the game")]
    public GameMember[] Members;

    public string[] GetMembersAsString() {
        int length = Members.Length;

        string[] membersAsString = new string[length];
        for (int i = 0; i < length; i++) {
            membersAsString[i] = Members[i].name;
        }

        return membersAsString;
    }
}
