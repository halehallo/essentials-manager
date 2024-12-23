﻿using DOM.Project.Pokemons;

namespace DAL.PbsRepositories.Pokemons;

public interface IPokemonRepository
{
    void CreatePokemon(Pokemon pokemon);
    void CreatePokemonColor(PokemonColor pokemonColor);
    void CreatePokemonEggGroup(PokemonEggGroup pokemonEggGroup);
    void CreatePokemonEvolution(PokemonEvolution pokemonEvolution);
    void CreatePokemonEvolutionMethod(PokemonEvolutionMethod pokemonEvolutionMethod);
    void CreatePokemonFlag(PokemonFlag pokemonFlag);
    void CreatePokemonGenderRatio(PokemonGenderRatio pokemonGenderRatio);
    void CreatePokemonGrowthRate(PokemonGrowthRate pokemonGrowthRate);
    void CreatePokemonHabitat(PokemonHabitat pokemonHabitat);
    void CreatePokemonShape(PokemonShape pokemonShape);
    void CreateEvGained(PokemonEvGained pokemonEvGained);
    IEnumerable<Pokemon> ReadAllPokemonsWithEvolutionAndOffspring();
    IEnumerable<Pokemon> ReadAllPokemonsWithTypings();
    IEnumerable<Pokemon> ReadAllPokemons();
    Pokemon ReadPokemonByInternalName(string internalName);
    Pokemon ReadPokemonByKeyName(string keyName);
    ICollection<PokemonGenderRatio> ReadAllGenderRatios();
    ICollection<PokemonGrowthRate> ReadAllGrowthRates();
    ICollection<PokemonEvGained> ReadAllEvsGained();
    ICollection<PokemonEggGroup> ReadAllEggGroups();
    ICollection<PokemonColor> ReadAllColors();
    ICollection<PokemonShape> ReadAllShapes();
    ICollection<PokemonHabitat> ReadAllHabitats();
    ICollection<PokemonFlag> ReadAllFlags();
    ICollection<PokemonEvolutionMethod> ReadAllEvolutionMethods();
    int ReadAmountOfPokemon();
    void UpdatePokemon(Pokemon pokemon);

    void SaveChanges();
}