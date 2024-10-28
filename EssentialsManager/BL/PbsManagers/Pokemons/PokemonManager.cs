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

    public void WriteAllPokemonWithoutLinksToPbs(Dictionary<string, Dictionary<string, string>> blocks)
    {
        ICollection<Typing> typingsFromRepo = _typingRepository.ReadAllTypings();
        var typingDictionary = typingsFromRepo.ToDictionary(t => t.InternalName, t => t);
        
        ICollection<PokemonGenderRatio> genderRatiosFromRepo = _pokemonRepository.ReadAllGenderRatios();
        Dictionary<string, PokemonGenderRatio> pokemonGenderRatiosDictionary = 
            genderRatiosFromRepo.ToDictionary(p => p.GenderRatioName, p => p);
        
        ICollection<PokemonGrowthRate> growthRateFromRepo = _pokemonRepository.ReadAllGrowthRates();
        Dictionary<string, PokemonGrowthRate> pokemonGrowthRatesDictionary = 
            growthRateFromRepo.ToDictionary(p => p.GrowthRateName, p => p);
        
        ICollection<PokemonEvGained> evsGainedFromRepo = _pokemonRepository.ReadAllEvsGained();
        Dictionary<string, PokemonEvGained> pokemonEvsGainedDictionary = 
            evsGainedFromRepo.ToDictionary(e => e.EvGainedName, e => e);

        ICollection<PokemonEggGroup> eggGroupsFromRepo = _pokemonRepository.ReadAllEggGroups();
        Dictionary<string, PokemonEggGroup> pokemonEggGroupsDictionary = 
            eggGroupsFromRepo.ToDictionary(g => g.EggGroupName, g => g);

        ICollection<PokemonColor> colorsFromRepo = _pokemonRepository.ReadAllColors();
        Dictionary<string, PokemonColor> pokemonColorsDictionary = 
            colorsFromRepo.ToDictionary(c => c.ColorName, c => c);

        ICollection<PokemonShape> shapesFromRepo = _pokemonRepository.ReadAllShapes();
        Dictionary<string, PokemonShape> pokemonShapesDictionary = 
            shapesFromRepo.ToDictionary(s => s.ShapeName, s => s);

        ICollection<PokemonHabitat> habitatsFromRepo = _pokemonRepository.ReadAllHabitats();
        Dictionary<string, PokemonHabitat> pokemonHabitatsDictionary = 
            habitatsFromRepo.ToDictionary(h => h.HabitatName, h => h);

        ICollection<PokemonFlag> flagsFromRepo = _pokemonRepository.ReadAllFlags();
        Dictionary<string, PokemonFlag> pokemonFlagsDictionary = 
            flagsFromRepo.ToDictionary(f => f.FlagName, f => f);

        ICollection<PokemonEvolutionMethod> evolutionMethodsFromRepo = _pokemonRepository.ReadAllEvolutionMethods();
        Dictionary<string, PokemonEvolutionMethod> pokemonEvolutionMethodsDictionary = 
            evolutionMethodsFromRepo.ToDictionary(em => em.MethodName, em => em);


        foreach (var block in blocks)
        {
            string[] parts = block.Key.Split(',');

            string internalName = parts[0];
            string formNumber = parts.Length > 1 ? parts[1] : "0";
            bool isBaseForm = formNumber == "0";
            
            block.Value.TryGetValue("Name", out string name);

            block.Value.TryGetValue("FormName", out string formName);

            block.Value.TryGetValue("Types", out string typesString);

            ICollection<string> typesStringCollection = typesString != null ? typesString.Split(',').ToList() : [];
            ICollection<Typing> typings = new List<Typing>();

            foreach (string typingString in typesStringCollection)
            {
                if (typingDictionary.TryGetValue(typingString.Trim(), out Typing typing))
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
                hp = baseStatsCollection[0].Trim();
                attack = baseStatsCollection[1].Trim();
                defense = baseStatsCollection[2].Trim();
                speed = baseStatsCollection[3].Trim();
                specialAttack = baseStatsCollection[4].Trim();
                specialDefense = baseStatsCollection[5].Trim();
            }

            block.Value.TryGetValue("GenderRatio", out string genderRatioString);
            genderRatioString ??= (isBaseForm) ? "Female50Percent" : "BaseFormReference";
            genderRatioString = genderRatioString.Trim();
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
            growthRateString ??= (isBaseForm) ? "Medium" : "BaseFormReference";
            growthRateString = growthRateString.Trim();
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
            baseExp ??= (isBaseForm) ? "100" : "-1";

            block.Value.TryGetValue("EVs", out string evsString);
            List<string> evsStrings = evsString != null ? evsString.Split(',').ToList() : [];
            int evsStringsSize = evsStrings.Count;
            ICollection<PokemonEvGained> evsGained = new List<PokemonEvGained>();

            if (evsStringsSize > 0 && evsStringsSize % 2 == 0)
            {
                for (int i = 0; i < evsStringsSize; i += 2)
                {
                    string evName = evsStrings[i].Trim() + "-" + evsStrings[i + 1].Trim();
                    pokemonEvsGainedDictionary.TryGetValue(evName, out PokemonEvGained evGained);
                    if (evGained == null)
                    {
                        Enum.TryParse(evsStrings[i].Trim(), out EvType evType);
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
            catchRate ??= (isBaseForm) ? "255" : "-1";

            block.Value.TryGetValue("Happiness", out string happiness);
            happiness ??= (isBaseForm) ? "70" : "-1";

            block.Value.TryGetValue("Abilities", out string abilitiesString);
            List<string> abilitiesStrings = abilitiesString != null ? abilitiesString.Split(',').ToList() : [];
            ICollection<Ability> abilities = new List<Ability>();

            foreach (string abilityString in abilitiesStrings)
            {
                Ability ability = _abilityRepository.ReadAbilityByAbilityName(abilityString.Trim());
                if (ability != null)
                {
                    abilities.Add(ability);
                }
            }

            block.Value.TryGetValue("HiddenAbilities", out string hiddenAbilitiesString);
            List<string> hiddenAbilitiesStrings =
                hiddenAbilitiesString != null ? hiddenAbilitiesString.Split(',').ToList() : [];
            ICollection<Ability> hiddenAbilities = new List<Ability>();

            foreach (string hiddenAbilityString in hiddenAbilitiesStrings)
            {
                Ability hiddenAbility = _abilityRepository.ReadAbilityByAbilityName(hiddenAbilityString.Trim());
                if (hiddenAbility != null)
                {
                    hiddenAbilities.Add(hiddenAbility);
                }
            }
            
            block.Value.TryGetValue("Moves", out string movesString);
            List<string> movesStrings = movesString != null ? movesString.Split(',').ToList() : [];
            int movesStringsSize = movesStrings.Count;
            ICollection<LearnedMove> learnedMoves = new List<LearnedMove>();

            if (movesStringsSize > 0 && movesStringsSize % 2 == 0)
            {
                for (int i = 0; i < movesStringsSize; i += 2)
                {
                    Move move = _moveRepository.ReadMoveByInternalName(movesStrings[i+1].Trim()); 
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
                Move move = _moveRepository.ReadMoveByInternalName(moveString.Trim());
                tutorMoves.Add(move);
            }
            
            block.Value.TryGetValue("EggMoves", out string eggMovesString);
            List<string> eggMovesStrings = eggMovesString != null ? eggMovesString.Split(',').ToList() : [];
            ICollection<Move> eggMoves = new List<Move>();

            foreach (string moveString in eggMovesStrings)
            {
                Move move = _moveRepository.ReadMoveByInternalName(moveString.Trim());
                eggMoves.Add(move);
            }
            
            block.Value.TryGetValue("EggGroups", out string eggGroupsString);
            eggGroupsString ??= (isBaseForm) ? "Undiscovered" : "BaseFormReference";
            List<string> eggGroupsStrings = eggGroupsString != null ? eggGroupsString.Split(',').ToList() : [];
            ICollection<PokemonEggGroup> eggGroups = new List<PokemonEggGroup>();
            
            foreach (string eggGroupStr in eggGroupsStrings)
            {
                pokemonEggGroupsDictionary.TryGetValue(eggGroupStr.Trim(), out PokemonEggGroup eggGroup);
                if (eggGroup == null)
                {
                    eggGroup = new PokemonEggGroup()
                    {
                        EggGroupName = eggGroupStr.Trim(),
                    };
                    _pokemonRepository.CreatePokemonEggGroup(eggGroup);
                    pokemonEggGroupsDictionary.Add(eggGroupStr.Trim(), eggGroup);
                }
                eggGroups.Add(eggGroup);
            }
            
            block.Value.TryGetValue("HatchSteps", out string hatchSteps);
            hatchSteps ??= (isBaseForm) ? "1" : "-1";
            
            block.Value.TryGetValue("Incense", out string incenseString);
            Item incense = null;
            if (incenseString != null)
            {
                incense = _itemRepository.ReadItemByItemName(incenseString.Trim());
            }
            
            block.Value.TryGetValue("Offspring", out string offspringString);
            List<string> offspringStrings = offspringString != null ? offspringString.Split(',').ToList() : [];
            
            block.Value.TryGetValue("Height", out string height);
            height ??= (isBaseForm) ? "0.1" : "-1";
            
            block.Value.TryGetValue("Weight", out string weight);
            weight ??= (isBaseForm) ? "0.1" : "-1";
            
            block.Value.TryGetValue("Color", out string colorString);
            colorString ??= (isBaseForm) ? "Red" : "BaseFormReference";

            pokemonColorsDictionary.TryGetValue(colorString.Trim(), out PokemonColor color);
            if (color == null)
            {
                color = new PokemonColor()
                {
                    ColorName = colorString.Trim(),
                };
                _pokemonRepository.CreatePokemonColor(color);
                pokemonColorsDictionary.Add(colorString.Trim(), color);
            }
            
            block.Value.TryGetValue("Shape", out string shapeString);
            shapeString ??= (isBaseForm) ? "Head" : "BaseFormReference";

            pokemonShapesDictionary.TryGetValue(shapeString.Trim(), out PokemonShape shape);
            if (shape == null)
            {
                shape = new PokemonShape()
                {
                    ShapeName = shapeString.Trim(),
                };
                _pokemonRepository.CreatePokemonShape(shape);
                pokemonShapesDictionary.Add(shapeString.Trim(), shape);
            }
            
            block.Value.TryGetValue("Habitat", out string habitatString);
            habitatString ??= (isBaseForm) ? "None" : "BaseFormReference";

            pokemonHabitatsDictionary.TryGetValue(habitatString.Trim(), out PokemonHabitat habitat);
            if (habitat == null)
            {
                habitat = new PokemonHabitat()
                {
                    HabitatName = habitatString.Trim(),
                };
                _pokemonRepository.CreatePokemonHabitat(habitat);
                pokemonHabitatsDictionary.Add(habitatString.Trim(), habitat);
            }
            
            block.Value.TryGetValue("Category", out string category);
            category ??= (isBaseForm) ? "???" : "BaseFormReference";
            
            block.Value.TryGetValue("Pokedex", out string pokedex);
            pokedex ??= (isBaseForm) ? "???" : "BaseFormReference";
            
            block.Value.TryGetValue("Generation", out string generation);
            generation ??= (isBaseForm) ? "0" : "-1";
            
            block.Value.TryGetValue("Flags", out string flagsString);
            flagsString ??= (isBaseForm) ? "None" : "BaseFormReference";
            List<string> flagsStrings = flagsString != null ? flagsString.Split(',').ToList() : [];
            List<PokemonFlag> flags = new List<PokemonFlag>();

            foreach (string flagstr in flagsStrings)
            {
                pokemonFlagsDictionary.TryGetValue(flagstr.Trim(), out PokemonFlag flag);
                if (flag == null)
                {
                    flag = new PokemonFlag()
                    {
                        FlagName = flagstr.Trim(),
                    };
                    _pokemonRepository.CreatePokemonFlag(flag);
                    pokemonFlagsDictionary.Add(flagstr.Trim(), flag);
                }
                flags.Add(flag);
            }
            
            

            block.Value.TryGetValue("WildItemCommon", out string wildItemCommonString);
            Item wildItemCommon = null;
            if (wildItemCommonString != null)
            {
                wildItemCommon = _itemRepository.ReadItemByItemName(wildItemCommonString.Trim());
            }
            
            block.Value.TryGetValue("WildItemUncommon", out string wildItemUncommonString);
            Item wildItemUncommon = null;
            if (wildItemUncommonString != null)
            {
                wildItemUncommon = _itemRepository.ReadItemByItemName(wildItemUncommonString.Trim());
            }
            
            block.Value.TryGetValue("WildItemRare", out string wildItemRareString);
            Item wildItemRare = null;
            if (wildItemRareString != null)
            {
                wildItemRare = _itemRepository.ReadItemByItemName(wildItemRareString.Trim());
            }
            
            block.Value.TryGetValue("Evolutions", out string evolutionsString);
            List<string> evolutionsStrings = evolutionsString != null ? evolutionsString.Split(',').ToList() : [];
            int evolutionsStringsSize = evolutionsStrings.Count;
            ICollection<PokemonEvolution> evolutions = new List<PokemonEvolution>();

            if (evolutionsStringsSize > 0 && evolutionsStringsSize % 3 == 0)
            {
                for (int i = 0; i < evolutionsStringsSize; i += 3)
                {
                    pokemonEvolutionMethodsDictionary.TryGetValue(evolutionsStrings[i+1].Trim(), out PokemonEvolutionMethod evolutionMethod);
                    if (evolutionMethod == null)
                    {
                        evolutionMethod = new PokemonEvolutionMethod()
                        {
                            MethodName = evolutionsStrings[i+1].Trim(),
                        };
                        _pokemonRepository.CreatePokemonEvolutionMethod(evolutionMethod);
                        pokemonEvolutionMethodsDictionary.Add(evolutionsStrings[i+1].Trim(), evolutionMethod);
                    }
                    
                    PokemonEvolution evolution = new PokemonEvolution()
                    {
                        PokemonBeforeString = internalName,
                        PokemonAfterString = evolutionsStrings[i].Trim(),
                        PokemonEvolutionMethod = evolutionMethod,
                        Parameter = evolutionsStrings[i+2].Trim()
                    };
                    _pokemonRepository.CreatePokemonEvolution(evolution);
                    evolutions.Add(evolution);
                }
            }
            
            block.Value.TryGetValue("MegaStone", out string megaStoneString);
            Item megaStone = null;
            if (megaStoneString != null)
            {
                megaStone = _itemRepository.ReadItemByItemName(megaStoneString.Trim());
            }
            
            block.Value.TryGetValue("UnmegaForm", out string unmegaForm);
            unmegaForm ??= "-1";
            
            block.Value.TryGetValue("MegaMove", out string megaMoveString);
            Move megaMove = null;
            if (megaMoveString != null)
            {
                megaMove = _moveRepository.ReadMoveByInternalName(megaMoveString.Trim());
            }
            
            block.Value.TryGetValue("MegaMessage", out string megaMessage);
            megaMessage ??= "-1";

            Pokemon pokemon = new Pokemon()
            {
                KeyName = internalName + "_" + formNumber,
                InternalName = internalName,
                Name = name,
                FormName = formName,
                FormNumber = int.Parse(formNumber),
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
                Evolutions = evolutions,
                MegaStone = megaStone,
                UnmegaForm = int.Parse(unmegaForm),
                MegaMove = megaMove,
                MegaMessage = int.Parse(megaMessage),
            };
            
            _pokemonRepository.CreatePokemon(pokemon);

            foreach (LearnedMove move in learnedMoves)
            {
                move.Pokemon = pokemon;
            }
        }
        _pokemonRepository.SaveChanges();
    }

    public void LinkAllPokemonInDatabase()
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