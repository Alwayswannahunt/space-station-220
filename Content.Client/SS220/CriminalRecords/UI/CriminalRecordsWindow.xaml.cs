// © SS220, An EULA/CLA with a hosting restriction, full text: https://raw.githubusercontent.com/SerbiaStrong-220/space-station-14/master/CLA.txt
using Content.Client.Stylesheets;
using Content.Client.UserInterface.Controls;
using Content.Shared.SS220.CriminalRecords;
using Content.Shared.StationRecords;
using Content.Shared.StatusIcon;
using Robust.Client.AutoGenerated;
using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Utility;
using System.Linq;
using System.Threading;
using Timer = Robust.Shared.Timing.Timer;

namespace Content.Client.SS220.CriminalRecords.UI;

[GenerateTypedNameReferences]
public sealed partial class CriminalRecordsWindow : FancyWindow
{
    private readonly IEntitySystemManager _sysMan;
    private readonly IPrototypeManager _prototype;
    private readonly IGameTiming _gameTiming;
    private readonly SpriteSystem _sprite;

    private bool _isPopulating = false;
    private bool _creationMode = false;
    private TimeSpan? _lastTimeEdited;

    private bool _securityMode = true;
    public int MaxEntryMessageLength = 200;
    public int EditCooldown = 5;

    private readonly Color _defaultLineColor = Color.FromHex("#808080");
    private readonly StyleBoxFlat _indicatorOverride;

    public Action<(string, ProtoId<CriminalStatusPrototype>?)>? OnCriminalStatusChange;
    public Action<int>? OnCriminalStatusDelete;
    public Action<uint?>? OnKeySelected;
    private readonly CancellationTokenSource _timerCancelTokenSource = new();

    public CriminalRecordsWindow()
    {
        RobustXamlLoader.Load(this);

        _gameTiming = IoCManager.Resolve<IGameTiming>();
        _prototype = IoCManager.Resolve<IPrototypeManager>();
        _sysMan = IoCManager.Resolve<IEntitySystemManager>();
        _sprite = _sysMan.GetEntitySystem<SpriteSystem>();

        RecordListing.OnItemSelected += args =>
        {
            if (_isPopulating)
                return;

            OnKeySelected?.Invoke(args.Metadata.Key);
        };

        RecordListing.OnItemDeselected += _ =>
        {
            if (!_isPopulating)
                OnKeySelected?.Invoke(null);
        };

        ExpandButton.OnPressed += ToggleExpand;
        CreationExpandButton.OnPressed += ToggleExpand;

        ChangeStatusButton.OnPressed += ToggleCreation;
        CancelRecordCreationButton.OnPressed += ToggleCreation;

        SaveRecordCreationButton.OnPressed += _ =>
        {
            var text = Rope.Collapse(MessageInput.TextRope).Trim();
            ProtoId<CriminalStatusPrototype>? statusTypeId = null;
            if (StatusTypeSelector.SelectedMetadata is string cast)
                statusTypeId = new(cast);

            OnCriminalStatusChange?.Invoke((text, statusTypeId));
            _lastTimeEdited = _gameTiming.CurTime;
            ToggleCreation();
        };

        StatusTypeSelector.OnItemSelected += args =>
        {
            StatusTypeSelector.Select(args.Id);
        };

        CancelRecordCreationButton.StyleClasses.Add(StyleBase.ButtonOpenLeft);
        CancelRecordCreationButton.StyleClasses.Add(StyleBase.ButtonCaution);

        RecordSearch.OnTextChanged += OnSearchChanged;
        _indicatorOverride = new StyleBoxFlat()
        {
            BackgroundColor = _defaultLineColor
        };
        StatusColorIndicator.PanelOverride = _indicatorOverride;

        MessageInput.Placeholder = new Rope.Leaf(Loc.GetString("criminal-records-ui-message-placeholder"));
        //MessageInput.OnKeyBindDown += _ => MessageInputChanged(); // Doesn't work 24.09.2023 textedit is broken

        SetupStatusTypeSelector();

        // 24.09.2023 TheArturZh
        // Hack to fix RichTextLabel line wrapping, remove when fixed properly in the engine
        SetSize = Size + new System.Numerics.Vector2(1, 1);

        RecordIdLabel.FontColorOverride = Color.LightGray;

        UpdateCountdown();
        Timer.SpawnRepeating(500, UpdateCountdown, _timerCancelTokenSource.Token);
    }

    // public void MessageInputChanged()
    // {
    //     if (MessageInput.TextLength >= MaxEntryMessageLength)
    //     {
    //         var newtext = Rope.Collapse(MessageInput.TextRope);
    //         newtext = newtext.Substring(0, (int) MathF.Min(MaxEntryMessageLength, newtext.Length));

    //         MessageInput.CursorPosition = new TextEdit.CursorPos(MessageInput.CursorPosition.Index-1, MessageInput.CursorPosition.Bias);
    //         MessageInput.TextRope = new Rope.Leaf(newtext);
    //     }
    // }

    public void SetupStatusTypeSelector()
    {
        var statuses = _prototype.EnumeratePrototypes<CriminalStatusPrototype>().ToList();

        // no status
        StatusTypeSelector.AddItem(Loc.GetString("criminal-records-ui-no-status"));

        foreach (var status in statuses)
        {
            StatusTypeSelector.AddItem(status.Name);
            StatusTypeSelector.SetItemMetadata(StatusTypeSelector.ItemCount - 1, status.ID);
        }
    }

    public void SetSecurityMode(bool mode)
    {
        RecordHistoryPanel.Visible = mode;
        _securityMode = mode;
    }

    private void OnSearchChanged(LineEdit.LineEditEventArgs args)
    {
        RecordListing.Filter = args.Text;
        RecordListing.RebuildList();
    }

    public void ToggleExpand(BaseButton.ButtonEventArgs? _ = null)
    {
        Details.Visible = !Details.Visible;
        var newIcon = Details.Visible
        ? "/Textures/SS220/Interface/Misc/up.png"
        : "/Textures/SS220/Interface/Misc/down.png";
        ExpandButton.IconTexture = newIcon;
        CreationExpandButton.IconTexture = newIcon;
    }

    public void ToggleCreation(BaseButton.ButtonEventArgs? _ = null)
    {
        _creationMode = !_creationMode;
        RecordHistoryPanel.Visible = !_creationMode;
        RecordCreationPanel.Visible = _creationMode;

        if (!_creationMode)
        {
            MessageInput.TextRope = new Rope.Leaf("");
        }
    }

    public void UpdateState(CriminalRecordConsoleState state)
    {
        if (state.RecordListing != null)
            PopulateRecordListing(state.RecordListing, state.SelectedKey);

        if (state.SelectedKey is { } key)
        {
            RecordIdLabel.Text = $"ID: НТ-{key}";
        }

        if (state.SelectedRecord != null)
        {
            CharacterName.Text = state.SelectedRecord.Name;
            Details.LoadRecordDetails(state.SelectedRecord, _securityMode);
            PopulateRecords(state.SelectedRecord);
            PanelRightPlaceholder.Visible = false;
            PanelRight.Visible = true;
        }
        else
        {
            CharacterName.Text = "Не выбрана запись";
            PanelRightPlaceholder.Visible = true;
            PanelRight.Visible = false;
        }
    }

    public void UpdateCountdown()
    {
        bool onCooldown = false;

        TimeSpan? cooldownRemaining = null;
        if (_lastTimeEdited.HasValue)
        {
            cooldownRemaining = _lastTimeEdited.Value + TimeSpan.FromSeconds(EditCooldown) - _gameTiming.CurTime;
            onCooldown = cooldownRemaining > TimeSpan.Zero;
        }

        SaveRecordCreationButton.Disabled = onCooldown;
        ChangeStatusButton.Disabled = onCooldown;

        if (onCooldown)
        {
            var time = (int) MathF.Ceiling((float) cooldownRemaining!.Value.TotalSeconds);
            ChangeStatusButton.Text = $"Сменить статус ({time})";
            SaveRecordCreationButton.Text = $"Сохранить ({time})";
        }
        else
        {
            ChangeStatusButton.Text = "Сменить статус";
            SaveRecordCreationButton.Text = "Сохранить";
        }
    }

    private void PopulateRecordListing(Dictionary<uint, CriminalRecordShort>? listing, uint? selected)
    {
        if (_isPopulating)
            return;

        _isPopulating = true;
        RecordListing.SetItems(listing, selected);
        _isPopulating = false;
    }

    private void PopulateRecords(GeneralStationRecord record)
    {
        CriminalRecordContainer.DisposeAllChildren();

        if (record.CriminalRecords is not { } catalog)
            return;

        var keyList = catalog.Records.Keys.ToList();
        keyList.Sort();

        _indicatorOverride.BackgroundColor = _defaultLineColor;

        for (var i = 0; i < keyList.Count; i++)
        {
            var time = keyList[i];
            CriminalRecord entry = catalog.Records[time];
            var entryDisplay = new CriminalRecordDisplay();
            var isLast = time == catalog.LastRecordTime;

            Texture? iconTexture = null;
            CriminalStatusPrototype? statusProto = null;
            if (entry.RecordType != null &&
            _prototype.TryIndex(entry.RecordType, out statusProto))
            {
                if (statusProto.StatusIcon != null &&
                _prototype.TryIndex<StatusIconPrototype>(statusProto.StatusIcon, out var statusIconProto))
                {
                    iconTexture = _sprite.Frame0(statusIconProto.Icon);
                }

                if (isLast)
                    _indicatorOverride.BackgroundColor = statusProto != null ? statusProto.Color : _defaultLineColor;
            }
            else if (isLast)
            {
                _indicatorOverride.BackgroundColor = _defaultLineColor;
            }

            CriminalRecordContainer.AddChild(entryDisplay);
            entryDisplay.SetPositionFirst();
            entryDisplay.Setup(iconTexture, statusProto, entry.Message, time, isLast, this);

            if (i % 2 == 0)
                ((StyleBoxFlat) entryDisplay.PanelOverride!).BackgroundColor = Color.Transparent;
        }
    }

    public void DeleteRecord(int time)
    {
        OnCriminalStatusDelete?.Invoke(time);
    }

    public override void Close()
    {
        base.Close();
        _timerCancelTokenSource.Cancel();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
            _timerCancelTokenSource.Cancel();
    }
}