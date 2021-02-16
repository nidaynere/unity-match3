using UnityEngine;
using Match3;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

#pragma warning disable CS0649
    [SerializeField] private Transform holder;
    [SerializeField] private Renderer gridRenderer;
    [SerializeField] private int poolSize;
    [SerializeField] private GamePlayEvents gamePlayEvents;
    [SerializeField] private AnimationSettings animationSettings;
    [SerializeField] private Match3GameSettings gameSettings;
    [SerializeField] private Transform gridPointer;
#pragma warning restore CS0649

    private Dictionary<ushort, GameMember> spawneds = new Dictionary<ushort, GameMember>();
    private Pool memberPool;
    private Match3Game currentSession;
    private AnimationQuery animationQuery;

    /// <summary>
    /// Current status of the game. Is playable, or waiting for something?
    /// </summary>
    private bool isGameInteractable;

    private void Start() {
        memberPool = new Pool(holder, gameSettings.Members, poolSize);

        gamePlayEvents.StartGame = CreateGame;
        gamePlayEvents.ClearGame = Clear;
        gamePlayEvents.OnGameplayStatusChange += (value) => { isGameInteractable = value; };
    }

    private void CreateGame() {
        Clear();

        animationQuery = new AnimationQuery(animationSettings);
        currentSession = new Match3Game(
                gameSettings.GridSizeX,
                gameSettings.GridSizeY,
                gameSettings.GetMembersAsString(),
                SpawnBall
            );

        gridRenderer.transform.localPosition = new Vector3((gameSettings.GridSizeX-1) / 2f, -(gameSettings.GridSizeY - 1) / 2f, 0.01f);
        gridRenderer.transform.localScale = new Vector3(gameSettings.GridSizeX, gameSettings.GridSizeY);
        gridRenderer.material.SetVector("_Tiling", gridRenderer.transform.localScale);

        // Register outputs to the game.
        currentSession.GameEvents.OnMemberPositionUpdate += MemberPositionUpdate;
        currentSession.GameEvents.OnReadyForVisualization += ReadyForVisualization;
        currentSession.GameEvents.OnMemberDestroyed += MemberDestroyed;

        // Match3<-->GamePlayEvents
        currentSession.GameEvents.OnGameScoreUpdate += (int value) => { gamePlayEvents.OnScoreUpdate?.Invoke(value); };
        currentSession.GameEvents.OnGameFinished += (int value) => {
            Debug.Log("OnGameFinished()");
            gamePlayEvents.OnGameplayStatusChange?.Invoke(false);
            gamePlayEvents.OnGameOver?.Invoke(value); 
        };
        //

        gamePlayEvents.OnGameStarted?.Invoke();
        gamePlayEvents.OnScoreUpdate?.Invoke(0);
        gamePlayEvents.OnGameplayStatusChange?.Invoke(true);
    }

    /// <summary>
    /// Clears the game.
    /// </summary>
    private void Clear() {
        if (currentSession != null) {
            StopAllCoroutines();
            foreach (var obj in spawneds)
                obj.Value.gameObject.SetActive(false);
            spawneds.Clear();
            currentSession.Dispose();
            currentSession = null;
        }
    }

    private void SpawnBall(ushort Id, string Avatar, int X, int Y) {
        Debug.Log("Spawnball with Id => " + Id);

        var gameBall = memberPool.GetFromPool(Avatar);
        gameBall.gameObject.name = Id.ToString();

        gameBall.SetClickAction(() => {
            if (isGameInteractable) {
                gamePlayEvents.OnGameplayStatusChange?.Invoke(false);
                currentSession.InteractMember(X, Y);
            }
        });

        gameBall.SetPosition(X, Y);
        gameBall.gameObject.SetActive(true);
        spawneds.Add(Id, gameBall);
    }

    private void ReadyForVisualization() {
        Debug.Log("Ready for visualization.");
        StartCoroutine(animationQuery.DoQuery(() => {
            Debug.Log("Visualization completed.");

            gamePlayEvents.OnGameplayStatusChange?.Invoke(true);
            Debug.Log("next round !!");
        }));
    }

    private void MemberDestroyed (ushort Id) {
        Debug.Log("Member destoyed " + Id);
        if (spawneds.ContainsKey(Id)) {
            animationQuery.AddToQuery(new AnimationQuery.DestroyAction(spawneds[Id]));
        }
    }

    private void MemberPositionUpdate (ushort Id, int X, int Y) {
        Debug.Log("Member position update => " + Id + " X=" + X + " Y=" + Y);

        if (spawneds.ContainsKey(Id)) {

            spawneds[Id].SetClickAction(() => {
                if (isGameInteractable) {
                    gamePlayEvents.OnGameplayStatusChange?.Invoke(false);
                    currentSession.InteractMember(X, Y);
                }
            });

            animationQuery.AddToQuery(new AnimationQuery.MovementAction(spawneds[Id], X, Y));
        }
    }
}
