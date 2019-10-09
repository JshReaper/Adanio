using UnityEngine;

[BoltGlobalBehaviour(BoltNetworkModes.Server, "game")]
public class TutorialServerCallbacks : Bolt.GlobalEventListener
{
    private void Awake()
    {
        PlayerObjectRegistry.CreateServerPlayer();
    }

    public override void Connected(BoltConnection connection)
    {
        PlayerObjectRegistry.CreateClientPlayer(connection);
    }

    public override void SceneLoadLocalDone(string map)
    {
    }

    public override void SceneLoadRemoteDone(BoltConnection connection)
    {
        PlayerObjectRegistry.GetPlayer(connection).Spawn();
    }
}