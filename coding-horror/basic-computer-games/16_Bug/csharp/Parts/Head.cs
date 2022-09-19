using System.Text;
using BugGame.Resources;

namespace BugGame.Parts;

internal class Head : ParentPart
{
    private Feelers _feelers = new();

    public Head()
        : base(Message.HeadAdded, Message.HeadNotNeeded)
    {
    }

    public override bool IsComplete => _feelers.IsComplete;

    protected override bool TryAddCore(IPart part, out Message message)
        => part switch
        {
            Feeler => _feelers.TryAddOne(out message),
            _ => throw new NotSupportedException($"Can't add a {part.Name} to a {Name}.")
        };

    public void AppendTo(StringBuilder builder, char feelerCharacter)
    {
        if (IsPresent)
        {
            _feelers.AppendTo(builder, feelerCharacter);
            builder
                .AppendLine("        HHHHHHH")
                .AppendLine("        H     H")
                .AppendLine("        H O O H")
                .AppendLine("        H     H")
                .AppendLine("        H  V  H")
                .AppendLine("        HHHHHHH");
        }
    }
}
