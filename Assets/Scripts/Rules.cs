using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum GameCondition
{
    IN_PLAY, WIN, LOSS
}

[System.Serializable]
public class GameState
{
    public PlayerData player;
    public List<EnemyData> enemies;
    public List<CollectibleData> collectibles;
    public GameCondition GameOver;
}

public class Rules : MonoBehaviour
{

    public PlayerController PlayerController;
    public CollectibleController CollectibleController;
    public EnemiesController EnemiesController;
    public GameObject gameOverText;

    bool doGameOverOperations = true;

    public WebSocketHandler WebSocketHandler;

    // Update is called once per frame
    void Update()
    {
        GameState state = new GameState();

        state.player = new PlayerData();
        state.player.x = PlayerController.gameObject.transform.position.x;
        state.player.y = PlayerController.gameObject.transform.position.y;
        state.player.z = PlayerController.gameObject.transform.position.z;

        state.enemies = new List<EnemyData> { };

        foreach (Enemy item in EnemiesController.Enemies)
        {
            state.enemies.Add(new EnemyData { x = item.transform.position.x, y = item.transform.position.y, z = item.transform.position.z});   
        }

        state.collectibles = new List<CollectibleData> { };

        foreach (Collectible collectible in CollectibleController.Collectibles)
        {
            if (collectible.dead) continue;
            state.collectibles.Add(new CollectibleData { x = collectible.transform.position.x, y = collectible.transform.position.y, z = collectible.transform.position.z });
        }

        if (CollectibleController.allDead)
        {
            state.GameOver = GameCondition.WIN;
        }
        else if (PlayerController.Dead)
        {
            state.GameOver = GameCondition.LOSS;
        }
        else
        {
            state.GameOver = GameCondition.IN_PLAY;
        }

        WebSocketHandler.SendState(state);

        if (CollectibleController.allDead || PlayerController.Dead)
        {

            if (doGameOverOperations)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene("Scenes/GameOverMenu");
            }
        }
    }
}
