using Content.Shared.Coordinates.Helpers;
using Content.Shared.Maps;
using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Shared.SS220.Rituals.RitualFactory.RitualStates;

/// <summary>
/// This system exist as a summUp of all posible results of rituals
/// like spawn smth or change comps or add comps etc
/// </summary>
public sealed partial class RitualStateSystem : EntitySystem
{
    [Dependency] private readonly TurfSystem _turfSystem = default!;
    [Dependency] private readonly IMapManager _mapManager = default!;
    [Dependency] private readonly IComponentFactory _componentFactory = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;

    // here goes all decoratores and staff. Should ask if it okay to place here

    public EntityUid? SpawnEntityResult(TileRef targetTile, string entityToSpawn)
    {
        return SpawnEntity(targetTile, entityToSpawn);
    }

    public EntityUid? SpawnEntityResult(Entity<TransformComponent> targetEntity, string entityToSpawn)
    {
        var targetTile = targetEntity.Comp.Coordinates.SnapToGrid(EntityManager, _mapManager).GetTileRef(EntityManager, _mapManager);
        if (targetTile == null)
        {
            Log.Error($" Cant find a tile referend to an Entity {targetEntity.Owner} to spawn | RitualStateSystem");
            return null;
        }

        return SpawnEntityResult(targetTile.Value, entityToSpawn);
    }

    public EntityUid? SpawnEntityResult(EntityUid targerUid, string entityToSpawn)
    {
        var targetEntity = new Entity<TransformComponent>(targerUid, Transform(targerUid));
        return SpawnEntityResult(targetEntity, entityToSpawn);
    }

}
