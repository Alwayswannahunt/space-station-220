using Content.Shared.Mobs;
using Content.Shared.SS220.Rituals.RitualFactory;
using Robust.Shared.Serialization;

namespace Content.Shared.SS220.Rituals;
[Serializable, NetSerializable]
public enum RitualSelectorUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public readonly struct MasterRuneParams(float charge, float maxCharge, float recharge, float efficiency)
{
    public readonly float Charge = charge;
    public readonly float MaxCharge = maxCharge;
    public readonly float Recharge = recharge;
    public readonly float Efficiency = efficiency;

}

[Serializable, NetSerializable]
public sealed class RitualSelectorUpdateState(MasterRuneParams masterParams, List<string> ritualList) : BoundUserInterfaceState
{
    public readonly MasterRuneParams MasterParams = masterParams;
    public readonly List<string> RitualList = ritualList;
}

[Serializable, NetSerializable]
public sealed class RitualPerformMessage(string ritualProtoId) : BoundUserInterfaceMessage
{
    public string RitualProtoId = ritualProtoId;
}
