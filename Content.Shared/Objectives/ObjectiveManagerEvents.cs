using Robust.Shared.Serialization;

namespace Content.Shared.Objectives;

/// <summary>
/// Event from client to server, ask for <see cref="AllObjectiveRecords"/> answer.
/// Require admin permission to get response from server
/// </summary>
[ByRefEvent]
[Serializable, NetSerializable]
public record struct RequestAllObjectiveRecords();

/// <summary>
/// Event from server to client, respond for <see cref="RequestAllObjectiveRecords"/>.
/// Checks if client have permission
/// </summary>
/// <param name="Records"></param>
[ByRefEvent]
[Serializable, NetSerializable]
public record struct AllObjectiveRecords(List<ObjectiveRecord> Records);


[Serializable, NetSerializable]
public record struct ObjectiveRecord(NetEntity ObjectiveNet, ObjectiveOptions Options, ObjectiveServerData ServerData);

public enum ObjectiveOptions : byte
{
    AlwaysPossible = 0,
    ChangeTargetEntity = 1 << 0,
    ChangeTargetAmount = 1 << 1,
}

[Serializable, NetSerializable]
public struct ObjectiveServerData()
{
    public NetEntity? TargetUid;
    // check type
    public int? TargetAmount;
}
