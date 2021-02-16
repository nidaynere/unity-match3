using UnityEngine;
using Match3;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {
#pragma warning disable CS0649
    [SerializeField] private Transform holder;
    [SerializeField] private Renderer gridRenderer;
    [SerializeField] private GamePlayEvents gamePlayEvents;
    [SerializeField] private AnimationSettings animationSettings;
    [SerializeField] private Match3GameSettings gameSettings;
#pragma warning restore CS0649

    private Dictionary<ushort, GameMember> spawneds = new Dictionary<ushort, GameMember>();
    private Pool memberPool;
    private Match3Game currentSession;
    private AnimationQuery animationQuery;

    /// <summary>
    /// Current status of the game. Is playable currently, or waiting for animation?
    /// </summary>
    private bool isGameInteractable;

    private string[] memberIds;

    private void Start() {
        if (!gameSettings.ValidateMembers(out memberIds)) {
            Debug.LogError("Game is not initialized.");
            return;
        }

        memberPool = new Pool(holder, gameSettings.Members, gameSettings.PoolSize);

        gamePlayEvents.StartGame = CreateGame;
        gamePlayEvents.ClearGame = Clear;
        gamePlayEvents.OnGameplayStatusChange += (value) => { isGameInteractable = value; };
    }

    private void CreateGame() {
        if (memberIds == null) {
            return;
        }

        Clear();

        animationQuery = new AnimationQuery(animationSettings);

        currentSession = new Match3Game(
                gameSettings.RequiredMatch,
                gameSettings.GridSizeX,
                gameSettings.GridSizeY,
                memberIds,
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
        //

        gamePlayEvents.OnGameStarted?.Invoke();
        gamePlayEvents.OnScoreUpdate?.Invoke(0);
        gamePlayEvents.OnGameplayStatusChange?.Invoke(true);
    }

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
        var gameBall = memberPool.GetFromPool(Avatar);
        gameBall.gameObject.name = Id.ToString();

        gameBall.transform.localScale = Vector3.one;

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
        StartCoroutine(animationQuery.DoQuery(() => {
            gamePlayEvents.OnGameplayStatusChange?.Invoke(true);
        }));
    }

    private void MemberDestroyed (ushort Id) {
        if (spawneds.ContainsKey(Id)) {
            animationQuery.AddToQuery(new AnimationQuery.DestroyAction(spawneds[Id]));
            spawneds.Remove(Id);
        }
    }

    private void MemberPositionUpdate (ushort Id, int X, int Y) {
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
