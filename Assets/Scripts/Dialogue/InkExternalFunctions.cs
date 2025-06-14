using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions
{
    public void Bind(Story story)
    {
        story.BindExternalFunction("StartQuest", (string questId) => StartQuest(questId));
        story.BindExternalFunction("AdvanceQuest", (string questId) => AdvanceQuest(questId));
        story.BindExternalFunction("FinishQuest", (string questId) => FinishQuest(questId));
        story.BindExternalFunction("EndDay", () => EndDay());
    }

    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("StartQuest");
        story.UnbindExternalFunction("AdvanceQuest");
        story.UnbindExternalFunction("FinishQuest");
        story.UnbindExternalFunction("EndDay");
    }

    private void StartQuest(string questId)
    {
        GameEventsManager.questEvents.StartQuest(questId);
    }

    private void AdvanceQuest(string questId)
    {
        GameEventsManager.questEvents.AdvanceQuest(questId);
    }

    private void FinishQuest(string questId)
    {
        GameEventsManager.questEvents.FinishQuest(questId);
    }

    private void EndDay()
    {
        Debug.Log("EndDay() викликано з INK");
        GameEventsManager.dayEvents.EndDay();
    }
}
