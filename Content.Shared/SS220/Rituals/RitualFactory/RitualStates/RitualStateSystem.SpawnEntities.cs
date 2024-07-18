using Robust.Shared.Map;

namespace Content.Shared.SS220.Rituals.RitualFactory.RitualStates;

public sealed partial class RitualStateSystem : EntitySystem
{

    private EntityUid? SpawnEntity(TileRef targetTile, string entityToSpawn)
    {
        var result = TrySpawnEntity(targetTile, entityToSpawn);
        if (result == null)
            Log.Error($" Unable to spawn entity proto {entityToSpawn} in target {targetTile}");

        return result;
    }

    private EntityUid? TrySpawnEntity(TileRef targetTile, string entityToSpawn)
    {

        if (_prototypeManager.TryIndex(entityToSpawn, out var _) == false)
            return null;
        // What else i can encapsule here to save from errors?
        return ForceSpawnEntity(targetTile, entityToSpawn);
    }

    private EntityUid? ForceSpawnEntity(TileRef targetTile, string entityToSpawn)
    {
        var resultUid = Spawn(entityToSpawn, _turfSystem.GetTileCenter(targetTile));
        //_entityManager.AddComponents(resultUid, Components, true); // force rewriting of comp
        return resultUid;
    }



};
