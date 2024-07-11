//using Content.Server.Someshit;

namespace Content.Server.SS220.Ritual;
public sealed class RitualMasterRuneSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RitualMasterRuneComponent, ComponentInit>(OnComponentInit);
        //SubscribeLocalEvent<FooComponent, InteractUsingEvent>(Handle);

        SubscribeLocalEvent<RitualMasterRuneComponent, MapInitEvent>(OnMapInit);

    }

    private void OnComponentInit(Entity<RitualMasterRuneComponent> ent, ref ComponentInit _)
    {
        if (HasComp<SupportRitualRuneComponent>(ent.Owner) == false) // change to Ensure and add Test for both components
            Log.Error("Inited MasterRuneComponent without SupportRitualRuneComponent"); // hope this will crash debug & tests
    }

    private void OnMapInit(Entity<RitualMasterRuneComponent> ent, ref MapInitEvent args)
    {
        //
    }



}
