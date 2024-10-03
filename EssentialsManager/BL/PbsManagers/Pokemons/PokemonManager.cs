using DAL.PbsRepositories.Abilities;
using DAL.PbsRepositories.Items;
using DAL.PbsRepositories.Moves;
using DAL.PbsRepositories.Pokemons;
using DAL.PbsRepositories.Types;
using DOM.Project.Abilities;
using DOM.Project.Items;
using DOM.Project.Moves;
using DOM.Project.Pokemons;
using DOM.Project.Typings;

namespace BL.PbsManagers.Pokemons;

public class PokemonManager : IPokemonManager
{
    private readonly ITypingRepository _typingRepository;
    private readonly IMoveRepository _moveRepository;
    private readonly IAbilityRepository _abilityRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IPokemonRepository _pokemonRepository;

    public PokemonManager(ITypingRepository typingRepository, IMoveRepository moveRepository,
        IAbilityRepository abilityRepository, IItemRepository itemRepository, IPokemonRepository pokemonRepository)
    {
        _typingRepository = typingRepository;
        _moveRepository = moveRepository;
        _abilityRepository = abilityRepository;
        _itemRepository = itemRepository;
        _pokemonRepository = pokemonRepository;
    }

    public void ReadAllPokemonFromPbs(Dictionary<string, Dictionary<string, string>> blocks)
    {
        WriteAllTypesWithoutLinksToPbs(blocks);
        LinkAllTypesInDatabase();
    }

    private void WriteAllTypesWithoutLinksToPbs(Dictionary<string, Dictionary<string, string>> blocks)
    {
        ICollection<Typing> typingsFromRepo = _typingRepository.ReadAllTypings();
        var typingDictionary = typingsFromRepo.ToDictionary(t => t.InternalName, t => t);
        Dictionary<string, PokemonGenderRatio> pokemonGenderRatiosDictionary =
            new Dictionary<string, PokemonGenderRatio>(8);
        Dictionary<string, PokemonGrowthRate> pokemonGrowthRatesDictionary =
            new Dictionary<string, PokemonGrowthRate>(6);
        Dictionary<string, PokemonEvGained> pokemonEvsGainedDictionary = new Dictionary<string, PokemonEvGained>(18);
        Dictionary<string, PokemonEggGroup> pokemonEggGroupsDictionary = new Dictionary<string, PokemonEggGroup>(15);
        Dictionary<string, PokemonColor> pokemonColorsDictionary = new Dictionary<string, PokemonColor>(10);
        Dictionary<string, PokemonShape> pokemonShapesDictionary = new Dictionary<string, PokemonShape>(14);
        Dictionary<string, PokemonHabitat> pokemonHabitatsDictionary = new Dictionary<string, PokemonHabitat>(10);
        Dictionary<string, PokemonFlag> pokemonFlagsDictionary = new Dictionary<string, PokemonFlag>(9);
        Dictionary<string, PokemonEvolutionMethod> pokemonEvolutionMethodsDictionary = new Dictionary<string, PokemonEvolutionMethod>(33);


        foreach (var block in blocks)
        {
            block.Value.TryGetValue("Name", out string name);

            block.Value.TryGetValue("FormName", out string formName);

            block.Value.TryGetValue("Types", out string typesString);

            ICollection<string> typesStringCollection = typesString != null ? typesString.Split(',').ToList() : [];
            ICollection<Typing> typings = new List<Typing>();

            foreach (string typingString in typesStringCollection)
            {
                if (typingDictionary.TryGetValue(typingString, out Typing typing))
                {
                    typings.Add(typing);
                }
            }

            block.Value.TryGetValue("BaseStats", out string baseStatsString);
            List<string> baseStatsCollection = baseStatsString != null ? baseStatsString.Split(',').ToList() : [];
            string hp = "0";
            string attack = "0";
            string defense = "0";
            string speed = "0";
            string specialAttack = "0";
            string specialDefense = "0";

            if (baseStatsCollection.Count > 0)
            {
                hp = baseStatsCollection[0];
                attack = baseStatsCollection[1];
                defense = baseStatsCollection[2];
                speed = baseStatsCollection[3];
                specialAttack = baseStatsCollection[4];
                specialDefense = baseStatsCollection[5];
            }

            block.Value.TryGetValue("GenderRatio", out string genderRatioString);
            genderRatioString ??= "Female50Percent";

            pokemonGenderRatiosDictionary.TryGetValue(genderRatioString, out PokemonGenderRatio genderRatio);
            if (genderRatio == null)
            {
                genderRatio = new PokemonGenderRatio()
                {
                    GenderRatioName = genderRatioString,
                };
                _pokemonRepository.CreatePokemonGenderRatio(genderRatio);
                pokemonGenderRatiosDictionary.Add(genderRatioString, genderRatio);
            }

            block.Value.TryGetValue("GrowthRate", out string growthRateString);
            growthRateString ??= "Medium";

            pokemonGrowthRatesDictionary.TryGetValue(growthRateString, out PokemonGrowthRate growthRate);
            if (growthRate == null)
            {
                growthRate = new PokemonGrowthRate()
                {
                    GrowthRateName = growthRateString,
                };
                _pokemonRepository.CreatePokemonGrowthRate(growthRate);
                pokemonGrowthRatesDictionary.Add(growthRateString, growthRate);
            }

            block.Value.TryGetValue("BaseExp", out string baseExp);

            block.Value.TryGetValue("EVs", out string evsString);
            List<string> evsStrings = evsString != null ? evsString.Split(',').ToList() : [];
            int evsStringsSize = evsStrings.Count;
            ICollection<PokemonEvGained> evsGained = new List<PokemonEvGained>();

            if (evsStringsSize > 0 && evsStringsSize % 2 == 0)
            {
                for (int i = 0; i < evsStringsSize; i += 2)
                {
                    string evName = evsStrings[i] + "-" + evsStrings[i + 1];
                    pokemonEvsGainedDictionary.TryGetValue(evName, out PokemonEvGained evGained);
                    if (evGained == null)
                    {
                        Enum.TryParse(evsStrings[i], out EvType evType);
                        evGained = new PokemonEvGained()
                        {
                            EvGainedName = evName,
                            EvValue = int.Parse(evsStrings[i + 1]),
                            EvType = evType,
                        };
                        _pokemonRepository.CreateEvGained(evGained);
                        pokemonEvsGainedDictionary.Add(evName, evGained);
                    }

                    evsGained.Add(evGained);
                }
            }

            block.Value.TryGetValue("CatchRate", out string catchRate);
            catchRate ??= "255";

            block.Value.TryGetValue("Happiness", out string happiness);
            happiness ??= "70";

            block.Value.TryGetValue("Abilities", out string abilitiesString);
            List<string> abilitiesStrings = abilitiesString != null ? abilitiesString.Split(',').ToList() : [];
            ICollection<Ability> abilities = new List<Ability>();

            foreach (string abilityString in abilitiesStrings)
            {
                Ability ability = _abilityRepository.ReadAbilityByAbilityName(abilityString);
                abilities.Add(ability);
            }

            block.Value.TryGetValue("HiddenAbilities", out string hiddenAbilitiesString);
            List<string> hiddenAbilitiesStrings =
                hiddenAbilitiesString != null ? hiddenAbilitiesString.Split(',').ToList() : [];
            ICollection<Ability> hiddenAbilities = new List<Ability>();

            foreach (string hiddenAbilityString in hiddenAbilitiesStrings)
            {
                Ability hiddenAbility = _abilityRepository.ReadAbilityByAbilityName(hiddenAbilityString);
                hiddenAbilities.Add(hiddenAbility);
            }
            
            block.Value.TryGetValue("Moves", out string movesString);
            List<string> movesStrings = movesString != null ? movesString.Split(',').ToList() : [];
            int movesStringsSize = movesStrings.Count;
            ICollection<LearnedMove> learnedMoves = new List<LearnedMove>();

            if (movesStringsSize > 0 && movesStringsSize % 2 == 0)
            {
                for (int i = 0; i < movesStringsSize; i += 2)
                {
                    Move move = _moveRepository.ReadMoveByInternalName(movesStrings[i+1]); 
                    LearnedMove learnedMove = new LearnedMove()
                    {
                        Move = move,
                        Level = int.Parse(movesStrings[i]),
                    };
                    _moveRepository.CreateLearnedMove(learnedMove);
                    learnedMoves.Add(learnedMove);
                }
            }
            
            block.Value.TryGetValue("TutorMoves", out string tutorMovesString);
            List<string> tutorMovesStrings = tutorMovesString != null ? tutorMovesString.Split(',').ToList() : [];
            ICollection<Move> tutorMoves = new List<Move>();

            foreach (string moveString in tutorMovesStrings)
            {
                Move move = _moveRepository.ReadMoveByInternalName(moveString);
                tutorMoves.Add(move);
            }
            
            block.Value.TryGetValue("EggMoves", out string eggMovesString);
            List<string> eggMovesStrings = eggMovesString != null ? eggMovesString.Split(',').ToList() : [];
            ICollection<Move> eggMoves = new List<Move>();

            foreach (string moveString in eggMovesStrings)
            {
                Move move = _moveRepository.ReadMoveByInternalName(moveString);
                eggMoves.Add(move);
            }
            
            block.Value.TryGetValue("EggGroups", out string eggGroupsString);
            eggGroupsString ??= "Undiscovered";
            List<string> eggGroupsStrings = eggGroupsString != null ? eggGroupsString.Split(',').ToList() : [];
            ICollection<PokemonEggGroup> eggGroups = new List<PokemonEggGroup>();
            
            foreach (string eggGroupStr in eggGroupsStrings)
            {
                pokemonEggGroupsDictionary.TryGetValue(eggGroupStr, out PokemonEggGroup eggGroup);
                if (eggGroup == null)
                {
                    eggGroup = new PokemonEggGroup()
                    {
                        EggGroupName = eggGroupStr,
                    };
                    _pokemonRepository.CreatePokemonEggGroup(eggGroup);
                    pokemonEggGroupsDictionary.Add(eggGroupStr, eggGroup);
                }
                eggGroups.Add(eggGroup);
            }
            
            block.Value.TryGetValue("HatchSteps", out string hatchSteps);
            hatchSteps ??= "1";
            
            block.Value.TryGetValue("Incense", out string incenseString);
            Item incense = null;
            if (incenseString != null)
            {
                incense = _itemRepository.ReadItemByItemName(incenseString);
            }
            
            block.Value.TryGetValue("Offspring", out string offspringString);
            List<string> offspringStrings = offspringString != null ? offspringString.Split(',').ToList() : [];
            
            block.Value.TryGetValue("Height", out string height);
            height ??= "0.1";
            
            block.Value.TryGetValue("Weight", out string weight);
            weight ??= "0.1";
            
            block.Value.TryGetValue("Color", out string colorString);
            colorString ??= "Red";

            pokemonColorsDictionary.TryGetValue(colorString, out PokemonColor color);
            if (color == null)
            {
                color = new PokemonColor()
                {
                    ColorName = colorString,
                };
                _pokemonRepository.CreatePokemonColor(color);
                pokemonColorsDictionary.Add(colorString, color);
            }
            
            block.Value.TryGetValue("Shape", out string shapeString);
            shapeString ??= "Head";

            pokemonShapesDictionary.TryGetValue(shapeString, out PokemonShape shape);
            if (shape == null)
            {
                shape = new PokemonShape()
                {
                    ShapeName = shapeString,
                };
                _pokemonRepository.CreatePokemonShape(shape);
                pokemonShapesDictionary.Add(shapeString, shape);
            }
            
            block.Value.TryGetValue("Habitat", out string habitatString);
            habitatString ??= "None";

            pokemonHabitatsDictionary.TryGetValue(habitatString, out PokemonHabitat habitat);
            if (habitat == null)
            {
                habitat = new PokemonHabitat()
                {
                    HabitatName = habitatString,
                };
                _pokemonRepository.CreatePokemonHabitat(habitat);
                pokemonHabitatsDictionary.Add(habitatString, habitat);
            }
            
            block.Value.TryGetValue("Category", out string category);
            category ??= "???";
            
            block.Value.TryGetValue("Pokedex", out string pokedex);
            pokedex ??= "???";
            
            block.Value.TryGetValue("Generation", out string generation);
            generation ??= "0";
            
            block.Value.TryGetValue("Flags", out string flagsString);
            flagsString ??= "None";
            List<string> flagsStrings = flagsString != null ? flagsString.Split(',').ToList() : [];
            List<PokemonFlag> flags = new List<PokemonFlag>();

            foreach (string flagstr in flagsStrings)
            {
                pokemonFlagsDictionary.TryGetValue(flagstr, out PokemonFlag flag);
                if (flag == null)
                {
                    flag = new PokemonFlag()
                    {
                        FlagName = flagstr,
                    };
                    _pokemonRepository.CreatePokemonFlag(flag);
                    pokemonFlagsDictionary.Add(flagstr, flag);
                }
                flags.Add(flag);
            }
            
            

            block.Value.TryGetValue("WildItemCommon", out string wildItemCommonString);
            Item wildItemCommon = null;
            if (wildItemCommonString != null)
            {
                wildItemCommon = _itemRepository.ReadItemByItemName(wildItemCommonString);
            }
            
            block.Value.TryGetValue("WildItemUncommon", out string wildItemUncommonString);
            Item wildItemUncommon = null;
            if (wildItemUncommonString != null)
            {
                wildItemUncommon = _itemRepository.ReadItemByItemName(wildItemUncommonString);
            }
            
            block.Value.TryGetValue("WildItemRare", out string wildItemRareString);
            Item wildItemRare = null;
            if (wildItemRareString != null)
            {
                wildItemRare = _itemRepository.ReadItemByItemName(wildItemRareString);
            }
            
            block.Value.TryGetValue("Evolutions", out string evolutionsString);
            List<string> evolutionsStrings = evolutionsString != null ? evolutionsString.Split(',').ToList() : [];
            int evolutionsStringsSize = evolutionsStrings.Count;
            ICollection<PokemonEvolution> evolutions = new List<PokemonEvolution>();

            if (evolutionsStringsSize > 0 && evolutionsStringsSize % 3 == 0)
            {
                for (int i = 0; i < evolutionsStringsSize; i += 3)
                {
                    pokemonEvolutionMethodsDictionary.TryGetValue(evolutionsStrings[i+1], out PokemonEvolutionMethod evolutionMethod);
                    if (evolutionMethod == null)
                    {
                        evolutionMethod = new PokemonEvolutionMethod()
                        {
                            MethodName = evolutionsStrings[i+1],
                        };
                        _pokemonRepository.CreatePokemonEvolutionMethod(evolutionMethod);
                        pokemonEvolutionMethodsDictionary.Add(evolutionsStrings[i+1], evolutionMethod);
                    }
                    
                    PokemonEvolution evolution = new PokemonEvolution()
                    {
                        PokemonBeforeString = block.Key,
                        PokemonAfterString = evolutionsStrings[i],
                        PokemonEvolutionMethod = evolutionMethod,
                        Parameter = evolutionsStrings[i+2]
                    };
                    _pokemonRepository.CreatePokemonEvolution(evolution);
                    evolutions.Add(evolution);
                }
            }

            Pokemon pokemon = new Pokemon()
            {
                InternalName = block.Key,
                Name = name,
                FormName = formName,
                Typings = typings,
                Hp = int.Parse(hp),
                Attack = int.Parse(attack),
                Defense = int.Parse(defense),
                Speed = int.Parse(speed),
                SpecialAttack = int.Parse(specialAttack),
                SpecialDefense = int.Parse(specialDefense),
                GenderRatio = genderRatio,
                GrowthRate = growthRate,
                BaseExp = int.Parse(baseExp),
                EvsGained = evsGained,
                CatchRate = int.Parse(catchRate),
                Happiness = int.Parse(happiness),
                Abilities = abilities,
                HiddenAbilities = hiddenAbilities,
                Moves = learnedMoves,
                TutorMoves = tutorMoves,
                EggMoves = eggMoves,
                EggGroups = eggGroups,
                HatchSteps = int.Parse(hatchSteps),
                Incense = incense,
                OffspringStrings = offspringStrings,
                Height = double.Parse(height),
                Weight = double.Parse(weight),
                Color = color,
                Shape = shape,
                Habitat = habitat,
                Category = category,
                Pokedex = pokedex,
                Generation = int.Parse(generation),
                Flags = flags,
                WildItemCommon = wildItemCommon,
                WildItemUncommon = wildItemUncommon,
                WildItemRare = wildItemRare,
                Evolutions = evolutions
            };
            
            _pokemonRepository.CreatePokemon(pokemon);

            foreach (LearnedMove move in learnedMoves)
            {
                move.Pokemon = pokemon;
            }
        }
        _pokemonRepository.SaveChanges();
    }

    private void LinkAllTypesInDatabase()
    {
        IEnumerable<Pokemon> pokemons = _pokemonRepository.ReadAllPokemonsWithEvolutionAndOffspring();
        foreach (var pokemon in pokemons)
        {
            if (pokemon.OffspringStrings != null && pokemon.OffspringStrings.Any())
            {
                foreach (var offspringString in pokemon.OffspringStrings)
                {
                    Pokemon pokemonOffspring = _pokemonRepository.ReadPokemonByInternalName(offspringString);
                    pokemon.Offspring.Add(pokemonOffspring);
                }
            }

            if (pokemon.Evolutions != null && pokemon.Evolutions.Any())
            {
                foreach (var evolution in pokemon.Evolutions)
                {
                    evolution.PokemonBefore = _pokemonRepository.ReadPokemonByInternalName(evolution.PokemonBeforeString);
                    evolution.PokemonAfter = _pokemonRepository.ReadPokemonByInternalName(evolution.PokemonAfterString);
                }
            }
        }
        _pokemonRepository.SaveChanges();
    }
}