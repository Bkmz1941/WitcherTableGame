using Controllers.EventsScene.models;
using JetBrains.Annotations;

namespace Controllers.EventsScene.Dto
{
    public class UpdateDecisionFieldsDto
    {
        public readonly string Variant;
        public readonly bool ShowSelectPlayer;
        public StoryEventDecision Decision = null;
        public bool IsReplaceLocationBtn = false;

        public UpdateDecisionFieldsDto(
            string variant,
            bool showSelectPlayer,
            bool isReplaceLocationBtn = false,
            StoryEventDecision Decision = null
        )
        {
            Variant = variant;
            ShowSelectPlayer = showSelectPlayer;
            IsReplaceLocationBtn = isReplaceLocationBtn;
            Decision = Decision;
        }
    }
}