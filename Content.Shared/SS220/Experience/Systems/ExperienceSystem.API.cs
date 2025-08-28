// © SS220, An EULA/CLA with a hosting restriction, full text: https://raw.githubusercontent.com/SerbiaStrong-220/space-station-14/master/CLA.txt


using System.Diagnostics.CodeAnalysis;
using Robust.Shared.Prototypes;

namespace Content.Shared.SS220.Experience.Systems;

public sealed partial class ExperienceSystem : EntitySystem
{
    public bool TryGetSkillTreeLevel(Entity<ExperienceComponent?> entity, ProtoId<SkillTreePrototype> skillTree, [NotNullWhen(true)] out int? level)
    {
        return TryGetSkillTreeLevels(entity, skillTree, out level, out _);
    }

    public bool TryGetSkillTreeSubLevel(Entity<ExperienceComponent?> entity, ProtoId<SkillTreePrototype> skillTree, [NotNullWhen(true)] out int? sublevel)
    {
        return TryGetSkillTreeLevels(entity, skillTree, out _, out sublevel);
    }

    public bool TryGetSkillTreeLevels(Entity<ExperienceComponent?> entity, ProtoId<SkillTreePrototype> skillTree, [NotNullWhen(true)] out int? level, [NotNullWhen(true)] out int? sublevel)
    {
        if (!Resolve(entity.Owner, ref entity.Comp) || !entity.Comp.Skills.TryGetValue(skillTree, out var info))
        {
            level = null;
            sublevel = null;
            return false;
        }

        sublevel = info.SkillSublevel;
        level = info.SkillLevel;
        return true;
    }
}
