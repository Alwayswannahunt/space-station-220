using Robust.Shared.Map;
using Robust.Shared.Prototypes;

namespace Content.Shared.SS220.Rituals.RitualFactory;

public abstract class BaseRitualResultEvent(EntityUid? targetEntity,
                                            TileRef? targetTile)
                                             : EntityEventArgs
{
    public readonly EntityUid? TargetEntity = targetEntity;
    public readonly TileRef? TargetTile = targetTile;
}

public sealed class SpawnEntitiesRitualResultEvent(EntityUid? targetEntity,
                                              TileRef? targetTile)
                                              : BaseRitualResultEvent(targetEntity, targetTile)
{
    public readonly string? EntityToSpawn;
    public readonly bool SpawnOnlyInClient = false;
}

public sealed class ChangeComponentsRitualResultEvent(EntityUid? targetEntity,
                                              TileRef? targetTile)
                                              : BaseRitualResultEvent(targetEntity, targetTile)
{
    public readonly ComponentRegistry Components = []; // should check if it works... somehow
}

