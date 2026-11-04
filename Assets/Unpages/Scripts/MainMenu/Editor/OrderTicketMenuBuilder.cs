using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.Events;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

// Rebuilds the MainMenu UI in the "kitchen order ticket" style (Figma: BomberChef - Main Menu Redesign, page V2).
// Existing buttons, sliders and their onClick bindings are reused; only their visuals and layout change.
// Generated decoration is prefixed with "OT_" so the builder can be run again safely.
public static class OrderTicketMenuBuilder
{
    const string ArtFolder = "Assets/Unpages/Materials/GameMaterials/Menu/OrderTicket";
    const string FontFolder = "Assets/Unpages/Font";
    const string Prefix = "OT_";

    static readonly Color Paper = Hex("F4EEE1");
    static readonly Color Ink = Hex("1F1B16");
    static readonly Color Stamp = Hex("E3342F");
    static readonly Color Pen = Hex("2447C9");
    static readonly Color Mustard = Hex("F2B705");
    static readonly Color Ruled = Hex("7FA7D9", 0.7f);

    static TMP_FontAsset rubik, monoBold, mono, caveat;
    static Sprite tornSprite, circleSprite, dashSprite, fadeSprite, steelSprite, boxSprite;

    [MenuItem("Tools/BomberChef/Build Order Ticket Menu")]
    public static void Build()
    {
        var root = GameObject.Find("MainMenuCanvas");
        if (root == null)
        {
            Debug.LogError("Open the MainMenu scene first (MainMenuCanvas not found).");
            return;
        }

        LoadFonts();
        LoadSprites();

        var scaler = root.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        var menuUI = root.GetComponent<MainMenuUI>();
        var title = root.transform.Find("GameNameText");
        if (title) title.gameObject.SetActive(false);

        BuildMainPanel(root.transform.Find("MenuUI"));
        BuildPlayPanel(root.transform.Find("PlayMenu"));
        BuildOptionsPanel(root.transform.Find("OptionMenu"), menuUI);

        menuUI.mainMenuCanvas.SetActive(true);
        menuUI.playMenuCanvas.SetActive(false);
        menuUI.optionsCanvas.SetActive(false);

        EditorSceneManager.MarkSceneDirty(root.scene);
        EditorSceneManager.SaveScene(root.scene);
        Debug.Log("Order ticket main menu built.");
    }

    // ---------------------------------------------------------------- Main

    static void BuildMainPanel(Transform panel)
    {
        ClearGenerated(panel);
        StretchFull(panel);
        var buttons = panel.Find("Buttons");
        StretchFull(buttons);

        var scrim = Img(panel, Prefix + "Scrim", Hex("1F1B16", 0.94f), fadeSprite);
        scrim.transform.SetSiblingIndex(0);
        var srt = scrim.rectTransform;
        srt.anchorMin = new Vector2(0, 0);
        srt.anchorMax = new Vector2(0, 1);
        srt.pivot = new Vector2(0, 0.5f);
        srt.sizeDelta = new Vector2(1250, 0);
        srt.anchoredPosition = Vector2.zero;

        var logo = Node(Prefix + "Logo", panel);
        Place(logo, new Vector2(0, 1), new Vector2(0.5f, 0.5f), new Vector2(120 + 310, -(110 + 140)), new Vector2(620, 280));
        logo.localRotation = Quaternion.Euler(0, 0, 3);
        var outline = Img(logo, "Outline", Paper, boxSprite);
        outline.type = Image.Type.Sliced;
        Stretch(outline.rectTransform, 0, 0, 0, 0);
        TL(Label(logo, "Bomber", "BOMBER", rubik, 104, Paper, TextAlignmentOptions.MidlineLeft).rectTransform, 30, 22, 560, 104);
        var block = Img(logo, "ChefBlock", Stamp);
        TL(block.rectTransform, 30, 140, 560, 118);
        TL(Label(block.transform, "Chef", "CHEF", rubik, 104, Paper, TextAlignmentOptions.MidlineLeft).rectTransform, 18, 4, 380, 110);
        var est = Label(block.transform, "Est", "EST. 2024\nKITCHEN\nOF CHAOS", monoBold, 18, Paper, TextAlignmentOptions.MidlineLeft);
        est.lineSpacing = -10;
        TL(est.rectTransform, 412, 14, 140, 90);

        var rail = Img(panel, Prefix + "Rail", Color.white, steelSprite);
        TL(rail.rectTransform, 90, 476, 1120, 26);
        foreach (var x in new[] { 97f, 1189f })
            TL(Img(panel, Prefix + "Bolt", Hex("4A5158"), circleSprite).rectTransform, x, 482, 14, 14);
        rail.transform.SetSiblingIndex(buttons.GetSiblingIndex());

        var tickets = new[]
        {
            ("PlayButton", "ORDER #01", "PLAY", "house special!", "QTY 1 ......... GO >", "FIRE!", -2.5f, 120f),
            ("OptionsButton", "ORDER #02", "OPTIONS", "season to taste", "SOUND ...... ADJUST", null, 1.5f, 490f),
            ("QuitButton", "ORDER #03", "QUIT", "kitchen closed", "BILL ....... PAID", null, -1f, 860f),
        };
        foreach (var (name, order, label, note, footer, stamp, rotation, x) in tickets)
        {
            var button = buttons.Find(name).GetComponent<Button>();
            var rt = BuildTicket(button, order, "TBL 1", label, note, footer, stamp);
            Place(rt, new Vector2(0, 1), new Vector2(0.5f, 1), new Vector2(x + 170, -486), new Vector2(340, 230));
            rt.localRotation = Quaternion.Euler(0, 0, rotation);
        }

        var footerText = Label(panel, Prefix + "Footer", "*** KITCHEN OPEN  ·  v1.0  ·  UNITY 6 + PHOTON FUSION ***", mono, 16, Hex("F4EEE1", 0.55f), TextAlignmentOptions.BottomLeft);
        Place(footerText.rectTransform, Vector2.zero, Vector2.zero, new Vector2(120, 60), new Vector2(900, 24));
    }

    static RectTransform BuildTicket(Button button, string order, string table, string title, string note, string footer, string stamp)
    {
        var rt = (RectTransform)button.transform;
        ClearChildren(rt);
        var hit = button.GetComponent<Image>();
        hit.sprite = null;
        hit.color = new Color(1, 1, 1, 0);
        hit.raycastTarget = true;
        button.transition = Selectable.Transition.None;

        var shadow = Img(rt, "Shadow", new Color(0, 0, 0, 0.35f));
        Stretch(shadow.rectTransform, 0, 0, 0, 0);
        shadow.rectTransform.offsetMin = new Vector2(6, -14 - 14);
        shadow.rectTransform.offsetMax = new Vector2(6, -14);

        var paper = Img(rt, "Paper", Paper);
        Stretch(paper.rectTransform, 0, 0, 0, 0);
        TornEdge(rt, Paper);

        var clip = Img(rt, "Clip", Color.white, steelSprite);
        Place(clip.rectTransform, new Vector2(0.5f, 1), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(110, 18));

        TL(Label(rt, "Order", order, monoBold, 16, Ink, TextAlignmentOptions.MidlineLeft).rectTransform, 28, 22, 200, 24);
        var tableText = Label(rt, "Table", table, mono, 16, Ink, TextAlignmentOptions.MidlineRight);
        Place(tableText.rectTransform, new Vector2(1, 1), new Vector2(1, 1), new Vector2(-28, -22), new Vector2(120, 24));
        Dash(rt, 54, 28, 28, Ink);
        TL(Label(rt, "Title", title, rubik, 44, Ink, TextAlignmentOptions.MidlineLeft).rectTransform, 28, 64, 284, 56);
        TL(Label(rt, "Note", note, caveat, 34, Pen, TextAlignmentOptions.MidlineLeft).rectTransform, 28, 120, 284, 44);
        Dash(rt, 172, 28, 28, Ink);
        TL(Label(rt, "Footer", footer, monoBold, 16, Ink, TextAlignmentOptions.MidlineLeft).rectTransform, 28, 186, 284, 24);

        if (stamp != null)
        {
            var box = Img(rt, "Stamp", Hex("E3342F", 0.9f), boxSprite);
            box.type = Image.Type.Sliced;
            box.pixelsPerUnitMultiplier = 1.5f;
            Place(box.rectTransform, new Vector2(0, 1), new Vector2(0.5f, 0.5f), new Vector2(250, -150), new Vector2(136, 50));
            box.rectTransform.localRotation = Quaternion.Euler(0, 0, 14);
            Stretch(Label(box.transform, "Text", stamp, rubik, 24, Hex("E3342F", 0.9f), TextAlignmentOptions.Center).rectTransform, 0, 0, 0, 0);

            var tape = Img(rt, "Tape", Hex("F2B705", 0.85f));
            Place(tape.rectTransform, new Vector2(0, 1), new Vector2(0.5f, 0.5f), new Vector2(20, 2), new Vector2(90, 30));
            tape.rectTransform.localRotation = Quaternion.Euler(0, 0, 10);
        }

        if (!button.GetComponent<TicketHover>()) button.gameObject.AddComponent<TicketHover>();
        return rt;
    }

    // ---------------------------------------------------------------- Play

    static void BuildPlayPanel(Transform panel)
    {
        var buttons = panel.Find("Buttons");
        Rescue(panel, buttons, "SinglePlayButton", "MultiPlayButton", "GamePlayVideo", "BackPlayButton");
        ClearGenerated(panel);
        StretchFull(panel);
        StretchFull(buttons);

        var dim = Img(panel, Prefix + "Dim", Hex("1F1B16", 0.94f));
        dim.raycastTarget = true;
        Stretch(dim.rectTransform, 0, 0, 0, 0);
        dim.transform.SetSiblingIndex(0);

        const float width = 760, height = 574;
        var receipt = Node(Prefix + "Receipt", panel);
        Place(receipt, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-160, 10), new Vector2(width, height));
        receipt.localRotation = Quaternion.Euler(0, 0, -1);
        var shadow = Img(receipt, "Shadow", new Color(0, 0, 0, 0.45f));
        Stretch(shadow.rectTransform, 0, 0, 0, 0);
        shadow.rectTransform.offsetMin = new Vector2(10, -24 - 18);
        shadow.rectTransform.offsetMax = new Vector2(10, -24 + 18);
        Stretch(Img(receipt, "Paper", Paper).rectTransform, 0, 0, 0, 0);
        var top = TornEdge(receipt, Paper);
        top.rectTransform.anchorMin = new Vector2(0, 1);
        top.rectTransform.anchorMax = new Vector2(1, 1);
        top.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        top.rectTransform.anchoredPosition = new Vector2(0, 9);
        top.rectTransform.localRotation = Quaternion.Euler(0, 0, 180);
        TornEdge(receipt, Paper);

        TL(Label(receipt, "Kitchen", "BOMBER CHEF · KITCHEN #07", monoBold, 18, Ink, TextAlignmentOptions.Midline).rectTransform, 0, 36, width, 26);
        TL(Label(receipt, "Title", "TODAY'S MENU", rubik, 60, Ink, TextAlignmentOptions.Midline).rectTransform, 0, 72, width, 72);
        TL(Label(receipt, "Info", "CHEF: YOU    TABLE: 1    ORDER: NOW", mono, 16, Ink, TextAlignmentOptions.Midline).rectTransform, 0, 152, width, 24);
        Dash(receipt, 196, 56, 56, Ink);

        var single = buttons.Find("SinglePlayButton").GetComponent<Button>();
        BuildMenuRow(single, "01", "SOLO SHIFT", "1 player · learn every recipe", "1P", false);
        single.transform.SetParent(receipt, false);
        TL((RectTransform)single.transform, 56, 214, width - 112, 90);

        var multi = buttons.Find("MultiPlayButton").GetComponent<Button>();
        BuildMenuRow(multi, "02", "KITCHEN BRAWL", "online · 2-4 chefs · last cook standing", "2-4P", true);
        multi.transform.SetParent(receipt, false);
        TL((RectTransform)multi.transform, 56, 314, width - 112, 90);

        Dash(receipt, 424, 56, 56, Ink);
        TL(Label(receipt, "Pick", "PICK A DISH TO START COOKING", monoBold, 18, Ink, TextAlignmentOptions.Midline).rectTransform, 0, 444, width, 26);
        Barcode(receipt, 486, width);

        var video = buttons.Find("GamePlayVideo").GetComponent<Button>();
        BuildPolaroid(video);
        Place((RectTransform)video.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(560, 40), new Vector2(440, 360));
        video.transform.localRotation = Quaternion.Euler(0, 0, 4);

        var back = buttons.Find("BackPlayButton").GetComponent<Button>();
        var backRt = BuildTicket(back, "RETURN", "", "BACK", "to the kitchen", "<<< ...... MENU", null);
        Place(backRt, Vector2.zero, new Vector2(0.5f, 1), new Vector2(96 + 170, 40 + 248), new Vector2(340, 230));
        backRt.localRotation = Quaternion.Euler(0, 0, 3);

        var hint = Label(panel, Prefix + "Hint", "Kitchen Brawl runs on Photon Fusion: host a room or join a friend.", mono, 16, Hex("F4EEE1", 0.6f), TextAlignmentOptions.BottomRight);
        Place(hint.rectTransform, new Vector2(1, 0), new Vector2(1, 0), new Vector2(-96, 60), new Vector2(900, 24));
    }

    static void BuildMenuRow(Button button, string number, string title, string subtitle, string tag, bool chefsPick)
    {
        var rt = (RectTransform)button.transform;
        ClearChildren(rt);
        var bg = button.GetComponent<Image>();
        bg.sprite = null;
        bg.color = Mustard;
        bg.raycastTarget = true;
        button.targetGraphic = bg;
        button.transition = Selectable.Transition.ColorTint;
        var colors = button.colors;
        colors.normalColor = new Color(1, 1, 1, 0);
        colors.highlightedColor = new Color(1, 1, 1, 0.55f);
        colors.pressedColor = new Color(1, 1, 1, 0.85f);
        colors.selectedColor = new Color(1, 1, 1, 0);
        colors.disabledColor = new Color(1, 1, 1, 0);
        colors.colorMultiplier = 1;
        colors.fadeDuration = 0.08f;
        button.colors = colors;

        // A disabled Button component never runs its color transition, so hide the tint graphic ourselves.
        bool available = button.enabled && button.interactable;
        if (!available) bg.color = new Color(1, 1, 1, 0);
        var ink = available ? Ink : Hex("1F1B16", 0.4f);

        if (chefsPick)
            Stretch(Img(rt, "Highlight", Hex("F2B705", 0.35f)).rectTransform, 0, 0, 0, 0);

        Place(Label(rt, "Number", number, monoBold, 22, ink, TextAlignmentOptions.MidlineLeft).rectTransform, new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(16, 0), new Vector2(50, 30));
        Place(Label(rt, "Title", title, rubik, 32, ink, TextAlignmentOptions.MidlineLeft).rectTransform, new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(80, 14), new Vector2(440, 40));
        Place(Label(rt, "Subtitle", subtitle, mono, 16, ink, TextAlignmentOptions.MidlineLeft).rectTransform, new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(80, -24), new Vector2(440, 24));
        Place(Label(rt, "Tag", tag, monoBold, 22, ink, TextAlignmentOptions.MidlineRight).rectTransform, new Vector2(1, 0.5f), new Vector2(1, 0.5f), new Vector2(-16, 0), new Vector2(100, 30));

        if (!available)
        {
            var soldOut = Img(rt, "SoldOut", Hex("E3342F", 0.9f), boxSprite);
            soldOut.type = Image.Type.Sliced;
            soldOut.pixelsPerUnitMultiplier = 1.5f;
            Place(soldOut.rectTransform, new Vector2(1, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-230, 0), new Vector2(210, 52));
            soldOut.rectTransform.localRotation = Quaternion.Euler(0, 0, -8);
            Stretch(Label(soldOut.transform, "Text", "SOLD OUT", rubik, 24, Hex("E3342F", 0.9f), TextAlignmentOptions.Center).rectTransform, 0, 0, 0, 0);
        }

        if (chefsPick)
        {
            var pick = Label(rt, "ChefsPick", "chef's pick!", caveat, 36, Pen, TextAlignmentOptions.Midline);
            Place(pick.rectTransform, new Vector2(1, 1), new Vector2(0.5f, 0.5f), new Vector2(-190, 2), new Vector2(200, 44));
            pick.rectTransform.localRotation = Quaternion.Euler(0, 0, -6);
        }
    }

    static void BuildPolaroid(Button button)
    {
        var rt = (RectTransform)button.transform;
        var keep = rt.Find("GamePlay");
        ClearChildren(rt, keep);
        button.transition = Selectable.Transition.None;
        var frame = button.GetComponent<Image>();
        frame.sprite = null;
        frame.color = Paper;
        var shadow = button.GetComponent<Shadow>();
        if (!shadow) shadow = button.gameObject.AddComponent<Shadow>();
        shadow.effectColor = new Color(0, 0, 0, 0.45f);
        shadow.effectDistance = new Vector2(10, -18);

        if (keep)
        {
            keep.SetSiblingIndex(0);
            TL((RectTransform)keep, 20, 20, 400, 225);
            var raw = keep.GetComponent<RawImage>();
            if (raw) raw.raycastTarget = false;
        }
        TL(Label(rt, "Caption", "watch the recipe!", caveat, 40, Pen, TextAlignmentOptions.Midline).rectTransform, 20, 258, 400, 50);
        TL(Label(rt, "Sub", "GAMEPLAY · 1 MIN", monoBold, 14, Ink, TextAlignmentOptions.Midline).rectTransform, 20, 310, 400, 22);
        var tape = Img(rt, "Tape", Hex("F2B705", 0.8f));
        Place(tape.rectTransform, new Vector2(0.5f, 1), new Vector2(0.5f, 0.5f), new Vector2(0, 0), new Vector2(150, 38));
        tape.rectTransform.localRotation = Quaternion.Euler(0, 0, -6);
    }

    static void Barcode(Transform parent, float y, float width)
    {
        int[] bars = { 3, 1, 2, 1, 3, 2, 1, 1, 3, 1, 2, 3, 1, 2, 1, 1, 3, 2, 1, 3, 1, 2, 2, 1, 3, 1, 1, 2, 3, 1, 2, 1, 3, 1, 2 };
        float total = -3;
        foreach (var b in bars) total += b * 2 + 3;
        var code = Node("Barcode", parent);
        TL(code, (width - total) / 2, y, total, 56);
        float x = 0;
        foreach (var b in bars)
        {
            TL(Img(code, "Bar", Ink).rectTransform, x, 0, b * 2, 56);
            x += b * 2 + 3;
        }
    }

    // ---------------------------------------------------------------- Options

    static void BuildOptionsPanel(Transform panel, MainMenuUI menuUI)
    {
        Rescue(panel, panel, "GeneralSoundSlider", "EffectSlider", "EnvironmentSlider", "MuteButton", "UnMuteButton", "CloseButton");
        ClearGenerated(panel);
        StretchFull(panel);

        var background = panel.Find("Background").GetComponent<Image>();
        foreach (Transform child in background.transform) child.gameObject.SetActive(false);
        background.sprite = null;
        background.color = Hex("1F1B16", 0.85f);
        background.raycastTarget = true;
        Stretch(background.rectTransform, 0, 0, 0, 0);
        background.transform.SetSiblingIndex(0);

        const float width = 860, height = 564;
        var card = Node(Prefix + "RecipeCard", panel);
        Place(card, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(width, height));
        card.localRotation = Quaternion.Euler(0, 0, 1.5f);
        var shadow = Img(card, "Shadow", new Color(0, 0, 0, 0.45f));
        Stretch(shadow.rectTransform, 0, 0, 0, 0);
        shadow.rectTransform.offsetMin = new Vector2(10, -24);
        shadow.rectTransform.offsetMax = new Vector2(10, -24);
        Stretch(Img(card, "Paper", Paper).rectTransform, 0, 0, 0, 0);

        TL(Label(card, "Recipe", "RECIPE:", monoBold, 18, Stamp, TextAlignmentOptions.MidlineLeft).rectTransform, 56, 30, 300, 24);
        TL(Label(card, "Title", "PERFECT SOUND", rubik, 46, Ink, TextAlignmentOptions.MidlineLeft).rectTransform, 56, 56, 640, 58);

        var close = panel.Find("CloseButton").GetComponent<Button>();
        ClearChildren(close.transform);
        close.transform.SetParent(card, false);
        Place((RectTransform)close.transform, new Vector2(1, 1), new Vector2(1, 1), new Vector2(-36, -30), new Vector2(64, 64));
        var closeHit = close.GetComponent<Image>();
        closeHit.sprite = null;
        closeHit.color = new Color(1, 1, 1, 0);
        close.transition = Selectable.Transition.None;
        Stretch(Label(close.transform, "X", "X", rubik, 40, Ink, TextAlignmentOptions.Center).rectTransform, 0, 0, 0, 0);
        if (!close.GetComponent<TicketHover>()) close.gameObject.AddComponent<TicketHover>();

        var rule = Img(card, "RedRule", Stamp);
        TopBar(rule.rectTransform, 126, 3, 0, 0);

        var sliders = new[] { ("GeneralSoundSlider", "MASTER VOLUME"), ("EffectSlider", "EFFECTS"), ("EnvironmentSlider", "ENVIRONMENT") };
        for (int i = 0; i < sliders.Length; i++)
        {
            float rowTop = 134 + i * 76;
            var slider = panel.Find(sliders[i].Item1).GetComponent<Slider>();
            TL(Label(card, sliders[i].Item1 + "Label", sliders[i].Item2, monoBold, 20, Ink, TextAlignmentOptions.MidlineLeft).rectTransform, 56, rowTop + 25, 230, 26);
            StyleSlider(slider);
            slider.transform.SetParent(card, false);
            Place((RectTransform)slider.transform, new Vector2(0, 1), new Vector2(0, 0.5f), new Vector2(300, -(rowTop + 38)), new Vector2(360, 30));
            var value = Label(card, sliders[i].Item1 + "Value", "", caveat, 36, Pen, TextAlignmentOptions.MidlineLeft);
            TL(value.rectTransform, 690, rowTop + 16, 120, 44);
            var valueLabel = slider.GetComponent<SliderValueLabel>();
            if (!valueLabel) valueLabel = slider.gameObject.AddComponent<SliderValueLabel>();
            valueLabel.slider = slider;
            valueLabel.label = value;
            value.text = Mathf.RoundToInt(slider.normalizedValue * 100f) + "%";
            TopBar(Img(card, "Line", Ruled).rectTransform, rowTop + 74, 2, 56, 56);
        }

        const float muteTop = 362;
        BuildCheckRow(panel.Find("MuteButton").GetComponent<Button>(), card, muteTop, false);
        BuildCheckRow(panel.Find("UnMuteButton").GetComponent<Button>(), card, muteTop, true);
        TopBar(Img(card, "Line", Ruled).rectTransform, muteTop + 80, 2, 56, 56);

        TL(Label(card, "Serves", "serves: 1 chef · prep: 0 min", caveat, 32, Pen, TextAlignmentOptions.MidlineLeft).rectTransform, 56, 470, 420, 44);

        var serveRt = Node("ServeButton", card);
        Place(serveRt, new Vector2(1, 1), new Vector2(1, 1), new Vector2(-56, -462), new Vector2(200, 64));
        var serveImg = serveRt.gameObject.AddComponent<Image>();
        serveImg.color = Stamp;
        var serveShadow = serveRt.gameObject.AddComponent<Shadow>();
        serveShadow.effectColor = Ink;
        serveShadow.effectDistance = new Vector2(5, -6);
        var serve = serveRt.gameObject.AddComponent<Button>();
        serve.targetGraphic = serveImg;
        serve.transition = Selectable.Transition.None;
        UnityEventTools.AddPersistentListener(serve.onClick, new UnityAction(menuUI.MainMenuButton));
        Stretch(Label(serveRt, "Text", "SERVE", rubik, 30, Paper, TextAlignmentOptions.Center).rectTransform, 0, 0, 0, 0);
        serveRt.gameObject.AddComponent<TicketHover>();

        foreach (var (x, rotation) in new[] { (120f, -10f), (width - 120f, 8f) })
        {
            var tape = Img(card, "Tape", Hex("F2B705", 0.8f));
            Place(tape.rectTransform, new Vector2(0, 1), new Vector2(0.5f, 0.5f), new Vector2(x, 0), new Vector2(110, 34));
            tape.rectTransform.localRotation = Quaternion.Euler(0, 0, rotation);
        }
    }

    static void StyleSlider(Slider slider)
    {
        var t = slider.transform;
        var background = t.Find("Background").GetComponent<Image>();
        ClearChildren(background.transform);
        background.sprite = null;
        background.color = Ink;
        var brt = background.rectTransform;
        brt.anchorMin = new Vector2(0, 0.5f);
        brt.anchorMax = new Vector2(1, 0.5f);
        brt.pivot = new Vector2(0.5f, 0.5f);
        brt.offsetMin = new Vector2(0, -1);
        brt.offsetMax = new Vector2(0, 1);
        for (int i = 0; i <= 10; i++)
        {
            var tick = Img(background.transform, "Tick", Ink);
            Place(tick.rectTransform, new Vector2(i / 10f, 0.5f), new Vector2(0.5f, 0), Vector2.zero, new Vector2(2, i % 5 == 0 ? 16 : 9));
        }

        var fillArea = (RectTransform)t.Find("Fill Area");
        fillArea.anchorMin = new Vector2(0, 0.5f);
        fillArea.anchorMax = new Vector2(1, 0.5f);
        fillArea.offsetMin = new Vector2(0, -3);
        fillArea.offsetMax = new Vector2(0, 3);
        var fill = fillArea.Find("Fill").GetComponent<Image>();
        fill.sprite = null;
        fill.color = Stamp;
        fill.rectTransform.sizeDelta = Vector2.zero;

        var handleArea = (RectTransform)t.Find("Handle Slide Area");
        handleArea.anchorMin = Vector2.zero;
        handleArea.anchorMax = Vector2.one;
        handleArea.offsetMin = Vector2.zero;
        handleArea.offsetMax = Vector2.zero;
        var handle = handleArea.Find("Handle").GetComponent<Image>();
        ClearChildren(handle.transform);
        handle.sprite = circleSprite;
        handle.color = Stamp;
        handle.rectTransform.sizeDelta = new Vector2(26, -4);
        var inner = Img(handle.transform, "Inner", Paper, circleSprite);
        Place(inner.rectTransform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(14, 14));
    }

    static void BuildCheckRow(Button button, Transform card, float top, bool isChecked)
    {
        ClearChildren(button.transform);
        button.transform.SetParent(card, false);
        TL((RectTransform)button.transform, 56, top + 12, 560, 56);
        var hit = button.GetComponent<Image>();
        hit.sprite = null;
        hit.color = new Color(1, 1, 1, 0);
        button.transition = Selectable.Transition.None;

        var box = Img(button.transform, "Box", Ink, boxSprite);
        box.type = Image.Type.Sliced;
        box.pixelsPerUnitMultiplier = 2f;
        Place(box.rectTransform, new Vector2(0, 0.5f), new Vector2(0, 0.5f), Vector2.zero, new Vector2(32, 32));
        if (isChecked)
            Stretch(Label(box.transform, "Check", "X", caveat, 44, Pen, TextAlignmentOptions.Center).rectTransform, 0, 0, 0, 0);

        Place(Label(button.transform, "Title", "SILENCE THE KITCHEN", monoBold, 20, Ink, TextAlignmentOptions.MidlineLeft).rectTransform,
            new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(52, 12), new Vector2(500, 26));
        var sub = isChecked ? "kitchen is silent · tap to bring the noise back" : "mutes music, effects & ambience";
        Place(Label(button.transform, "Sub", sub, mono, 15, Ink, TextAlignmentOptions.MidlineLeft).rectTransform,
            new Vector2(0, 0.5f), new Vector2(0, 0.5f), new Vector2(52, -14), new Vector2(500, 22));
    }

    // ---------------------------------------------------------------- Assets

    static void LoadFonts()
    {
        rubik = FontAsset("RubikMonoOne-Regular");
        monoBold = FontAsset("SpaceMono-Bold");
        mono = FontAsset("SpaceMono-Regular");
        caveat = FontAsset("Caveat-Variable");
    }

    static TMP_FontAsset FontAsset(string fileName)
    {
        var assetPath = $"{FontFolder}/{fileName} SDF.asset";
        var existing = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(assetPath);
        if (existing) return existing;

        var font = AssetDatabase.LoadAssetAtPath<Font>($"{FontFolder}/{fileName}.ttf");
        var asset = TMP_FontAsset.CreateFontAsset(font, 90, 9, GlyphRenderMode.SDFAA, 1024, 1024, AtlasPopulationMode.Dynamic, true);
        asset.name = fileName + " SDF";
        AssetDatabase.CreateAsset(asset, assetPath);
        asset.atlasTextures[0].name = fileName + " Atlas";
        AssetDatabase.AddObjectToAsset(asset.atlasTextures[0], asset);
        asset.material.name = fileName + " Material";
        AssetDatabase.AddObjectToAsset(asset.material, asset);
        asset.TryAddCharacters(" !\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~·", out _);
        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
        return asset;
    }

    static void LoadSprites()
    {
        if (!AssetDatabase.IsValidFolder(ArtFolder))
            AssetDatabase.CreateFolder(Path.GetDirectoryName(ArtFolder).Replace('\\', '/'), Path.GetFileName(ArtFolder));

        // One torn-paper tooth; tiled along ticket edges. 36px at 200 PPU = 18 UI units.
        tornSprite = MakeSprite("TornEdge", 36, 36, (x, y) =>
        {
            float edge = 24f * Mathf.Abs(x + 0.5f - 18f) / 18f;
            return Mathf.Clamp01(y + 0.5f - edge + 0.5f);
        }, 200, Vector4.zero, TextureWrapMode.Repeat);

        circleSprite = MakeSprite("Circle", 128, 128, (x, y) =>
        {
            float d = Vector2.Distance(new Vector2(x + 0.5f, y + 0.5f), new Vector2(64, 64));
            return Mathf.Clamp01(64f - d + 0.5f);
        }, 100, Vector4.zero, TextureWrapMode.Clamp);

        dashSprite = MakeSprite("Dash", 12, 2, (x, y) => x < 6 ? 1f : 0f, 100, Vector4.zero, TextureWrapMode.Repeat);

        fadeSprite = MakeSprite("FadeRight", 256, 4, (x, y) =>
        {
            float u = x / 255f;
            return u < 0.6f ? Mathf.Lerp(1f, 0.83f, u / 0.6f) : Mathf.Lerp(0.83f, 0f, (u - 0.6f) / 0.4f);
        }, 100, Vector4.zero, TextureWrapMode.Clamp);

        boxSprite = MakeSprite("BoxOutline", 64, 64, (x, y) =>
        {
            int d = Mathf.Min(Mathf.Min(x, 63 - x), Mathf.Min(y, 63 - y));
            return d < 6 ? 1f : 0f;
        }, 100, new Vector4(16, 16, 16, 16), TextureWrapMode.Clamp);

        var steelTop = Hex("EEF1F3");
        var steelMid = Hex("A7B0B8");
        var steelBottom = Hex("5E666E");
        steelSprite = MakeSprite("Steel", 4, 32, (x, y) => 1f, 100, Vector4.zero, TextureWrapMode.Clamp, (x, y) =>
        {
            float v = y / 31f;
            return v > 0.5f ? Color.Lerp(steelMid, steelTop, (v - 0.5f) * 2f) : Color.Lerp(steelBottom, steelMid, v * 2f);
        });
    }

    static Sprite MakeSprite(string name, int width, int height, System.Func<int, int, float> alpha, float pixelsPerUnit, Vector4 border, TextureWrapMode wrap, System.Func<int, int, Color> color = null)
    {
        var path = $"{ArtFolder}/{name}.png";
        var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
        {
            var c = color != null ? color(x, y) : Color.white;
            c.a = alpha(x, y);
            texture.SetPixel(x, y, c);
        }
        File.WriteAllBytes(path, texture.EncodeToPNG());
        Object.DestroyImmediate(texture);
        AssetDatabase.ImportAsset(path);

        var importer = (TextureImporter)AssetImporter.GetAtPath(path);
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.spritePixelsPerUnit = pixelsPerUnit;
        importer.spriteBorder = border;
        importer.wrapMode = wrap;
        importer.alphaIsTransparency = true;
        importer.mipmapEnabled = false;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.SaveAndReimport();
        return AssetDatabase.LoadAssetAtPath<Sprite>(path);
    }

    // ---------------------------------------------------------------- Helpers

    static Image TornEdge(Transform parent, Color color)
    {
        var edge = Img(parent, "TornEdge", color, tornSprite);
        edge.type = Image.Type.Tiled;
        var rt = edge.rectTransform;
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 0);
        rt.pivot = new Vector2(0.5f, 1);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(0, 18);
        return edge;
    }

    static void Dash(Transform parent, float y, float left, float right, Color color)
    {
        var dash = Img(parent, "Dash", color, dashSprite);
        dash.type = Image.Type.Tiled;
        TopBar(dash.rectTransform, y, 2, left, right);
    }

    static void Rescue(Transform searchRoot, Transform destination, params string[] names)
    {
        var found = new List<Transform>();
        foreach (var t in searchRoot.GetComponentsInChildren<Transform>(true))
            if (System.Array.IndexOf(names, t.name) >= 0 && t.parent != destination && t.GetComponent<Selectable>())
                found.Add(t);
        foreach (var t in found) t.SetParent(destination, false);
    }

    static void ClearGenerated(Transform panel)
    {
        for (int i = panel.childCount - 1; i >= 0; i--)
            if (panel.GetChild(i).name.StartsWith(Prefix))
                Object.DestroyImmediate(panel.GetChild(i).gameObject);
    }

    static void ClearChildren(Transform t, Transform keep = null)
    {
        for (int i = t.childCount - 1; i >= 0; i--)
            if (t.GetChild(i) != keep)
                Object.DestroyImmediate(t.GetChild(i).gameObject);
    }

    static RectTransform Node(string name, Transform parent)
    {
        var go = new GameObject(name, typeof(RectTransform));
        go.layer = 5;
        go.transform.SetParent(parent, false);
        return (RectTransform)go.transform;
    }

    static Image Img(Transform parent, string name, Color color, Sprite sprite = null)
    {
        var rt = Node(name, parent);
        rt.gameObject.AddComponent<CanvasRenderer>();
        var image = rt.gameObject.AddComponent<Image>();
        image.sprite = sprite;
        image.color = color;
        image.raycastTarget = false;
        return image;
    }

    static TextMeshProUGUI Label(Transform parent, string name, string text, TMP_FontAsset font, float size, Color color, TextAlignmentOptions alignment)
    {
        var rt = Node(name, parent);
        var label = rt.gameObject.AddComponent<TextMeshProUGUI>();
        label.font = font;
        label.fontSize = size;
        label.color = color;
        label.text = text;
        label.alignment = alignment;
        label.textWrappingMode = TextWrappingModes.NoWrap;
        label.overflowMode = TextOverflowModes.Overflow;
        label.raycastTarget = false;
        return label;
    }

    static RectTransform Place(RectTransform rt, Vector2 anchor, Vector2 pivot, Vector2 position, Vector2 size)
    {
        rt.anchorMin = anchor;
        rt.anchorMax = anchor;
        rt.pivot = pivot;
        rt.sizeDelta = size;
        rt.anchoredPosition3D = new Vector3(position.x, position.y, 0);
        rt.localRotation = Quaternion.identity;
        rt.localScale = Vector3.one;
        return rt;
    }

    static RectTransform TL(RectTransform rt, float x, float y, float width, float height)
    {
        return Place(rt, new Vector2(0, 1), new Vector2(0, 1), new Vector2(x, -y), new Vector2(width, height));
    }

    static void TopBar(RectTransform rt, float y, float height, float left, float right)
    {
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        rt.pivot = new Vector2(0.5f, 1);
        rt.offsetMin = new Vector2(left, -y - height);
        rt.offsetMax = new Vector2(-right, -y);
    }

    static void Stretch(RectTransform rt, float left, float top, float right, float bottom)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.offsetMin = new Vector2(left, bottom);
        rt.offsetMax = new Vector2(-right, -top);
        rt.localRotation = Quaternion.identity;
        rt.localScale = Vector3.one;
    }

    static void StretchFull(Transform t)
    {
        var rt = (RectTransform)t;
        Stretch(rt, 0, 0, 0, 0);
        rt.anchoredPosition3D = new Vector3(rt.anchoredPosition.x, rt.anchoredPosition.y, 0);
    }

    static Color Hex(string hex, float alpha = 1f)
    {
        ColorUtility.TryParseHtmlString("#" + hex, out var c);
        c.a = alpha;
        return c;
    }
}
