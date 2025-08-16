// Version 0.8.5
// This script allows the creation and management of lesson data assets.

using UnityEngine;

[CreateAssetMenu(fileName = "NewLessonData", menuName = "Lessons/Lesson Data")]
public class LessonData : ScriptableObject
{
    [Header("Core Lesson Data")]
    public int bpm = 120;
    public int beatCount = 4;
    public int subdivisionCount = 4;

    [TextArea]
    public string beatPattern = "1111";

    [TextArea]
    public string dialogueText = "This is an example of 4/4 time.";

    [TextArea]
    public string lessonText = "This is an example of Lesson text.";
}
