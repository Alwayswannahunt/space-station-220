using Content.Shared.SS220.Rituals;
using Content.Shared.SS220.Rituals.RitualFactory;

namespace Content.Server.SS220.Ritual;
public sealed partial class RitualMasterRuneSystem : EntitySystem
{

    private void InitializeUi()
    {
        SubscribeLocalEvent<RitualMasterRuneComponent, RitualPerformMessage>(OnRitualPerformanceRequest);
    }

    private void OnRitualPerformanceRequest(Entity<RitualMasterRuneComponent> ent, ref RitualPerformMessage msg)
    {
        // тут пишем, а там какоем...
        Log.Info($" Попытка произвести ритуал, полученное ентити - {ent}, полученное сообщение - {msg} | RitualMasterSystem");
    }

}
