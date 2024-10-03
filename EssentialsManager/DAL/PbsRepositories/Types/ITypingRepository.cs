using DOM.Project.Typings;

namespace DAL.PbsRepositories.Types;

public interface ITypingRepository
{
    void CreateTyping(Typing type);
    Typing ReadTyping(string internalName);
    void UpdateTyping(Typing type);
    ICollection<Typing> ReadAllTypings();
    ICollection<Typing> ReadAllTypingsWithJoin();
    void CreateTypingWeakness(TypingWeakness weakness);
    void CreateTypingWeaknessesBatch(IEnumerable<TypingWeakness> typingWeaknesses);
}