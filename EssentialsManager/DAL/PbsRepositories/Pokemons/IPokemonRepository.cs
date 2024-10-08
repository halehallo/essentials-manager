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
    Pokemon ReadPokemonByInternalName(string internalName);
    void SaveChanges();
}