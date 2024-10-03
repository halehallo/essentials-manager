using DAL.PbsRepositories.Moves;
using DAL.PbsRepositories.Types;
using DOM.Project.Moves;
using DOM.Project.Typings;

namespace BL.PbsManagers.Moves;

public class MoveManager : IMoveManager
{
    private readonly IMoveRepository _moveRepository;
    private readonly ITypingRepository _typingRepository;

    public MoveManager(IMoveRepository moveRepository, ITypingRepository typingRepository)
    {
        _moveRepository = moveRepository;
        _typingRepository = typingRepository;
    }

    public void ReadAllMovesFromPbs(Dictionary<string, Dictionary<string, string>> blocks)
    {
        Dictionary<string, MoveCategory> moveCategoriesDictionary = new Dictionary<string, MoveCategory>(3);
        Dictionary<string, MoveTarget> moveTargetsDictionary = new Dictionary<string, MoveTarget>(16);
        Dictionary<string, MoveFunctionCode> moveFunctionCodesDictionary = new Dictionary<string, MoveFunctionCode>(401);
        Dictionary<string, MoveFlag> moveFlagsDictionary = new Dictionary<string, MoveFlag>(14);
        
        ICollection<Typing> typings = _typingRepository.ReadAllTypings();
        var typingDictionary = typings.ToDictionary(t => t.InternalName, t => t);
        
        foreach (var block in blocks)
        {
            block.Value.TryGetValue("Name", out string name);
            
            block.Value.TryGetValue("Type", out string typeString);
            Typing typing = null;
            if (typeString != null)
            {
                typingDictionary.TryGetValue(typeString, out typing);
            }
            
            block.Value.TryGetValue("Category", out string categoryString);
            categoryString ??= "Status";
            
            moveCategoriesDictionary.TryGetValue(categoryString, out MoveCategory moveCategory);
            if (moveCategory == null)
            {
                moveCategory = new MoveCategory()
                {
                    CategoryName = categoryString,
                };
                _moveRepository.CreateCategory(moveCategory);
                moveCategoriesDictionary.Add(categoryString, moveCategory);
            }
            
            
            block.Value.TryGetValue("Power", out string power);
            power ??= "0";
            
            block.Value.TryGetValue("Accuracy", out string accuracy);
            accuracy ??= "100";
            
            block.Value.TryGetValue("TotalPP", out string totalPP);
            totalPP ??= "5";
            
            block.Value.TryGetValue("Target", out string targetString);
            targetString ??= "None";
            
            moveTargetsDictionary.TryGetValue(targetString, out MoveTarget moveTarget);
            if (moveTarget == null)
            {
                moveTarget = new MoveTarget()
                {
                    TargetName = targetString,
                };
                _moveRepository.CreateMoveTarget(moveTarget);
                moveTargetsDictionary.Add(targetString, moveTarget);
            }

            block.Value.TryGetValue("Priority", out string priority);
            priority ??= "0";
            
            block.Value.TryGetValue("FunctionCode", out string functionCodeString);
            functionCodeString ??= "None";
            
            moveFunctionCodesDictionary.TryGetValue(functionCodeString, out MoveFunctionCode functionCode);
            if (functionCode == null)
            {
                functionCode = new MoveFunctionCode()
                {
                    FunctionName = functionCodeString,
                };
                _moveRepository.CreateMoveFunctionCode(functionCode);
                moveFunctionCodesDictionary.Add(functionCodeString, functionCode);
            }
            
            block.Value.TryGetValue("Flags", out string moveFlagsString);
            ICollection<string> flags = moveFlagsString != null ? moveFlagsString.Split(',').ToList() : [];
            ICollection<MoveFlag> moveFlags = new List<MoveFlag>();

            foreach (string flag in flags)
            {
                moveFlagsDictionary.TryGetValue(flag, out MoveFlag moveFlag);
                if (moveFlag == null)
                {
                    moveFlag = new MoveFlag()
                    {
                        FlagName = flag,
                    };
                    _moveRepository.CreateMoveFlag(moveFlag);
                    moveFlagsDictionary.Add(flag, moveFlag);
                }
                moveFlags.Add(moveFlag);
            }
            
            
            block.Value.TryGetValue("EffectChance", out string effectChance);
            effectChance ??= "0";
            
            block.Value.TryGetValue("Description", out string description);
            description ??= "???";

            Move move = new Move()
            {
                InternalName = block.Key,
                Name = name,
                Typing = typing,
                Category = moveCategory,
                Power = int.Parse(power),
                Accuracy = int.Parse(accuracy),
                TotalPP = int.Parse(totalPP),
                Target = moveTarget,
                Priority = int.Parse(priority),
                FunctionCode = functionCode,
                Flags = moveFlags,
                EffectChance = int.Parse(effectChance),
                Description = description,
            };
            
            _moveRepository.CreateMove(move);
            
        }
        _moveRepository.SaveChanges();
    }
}