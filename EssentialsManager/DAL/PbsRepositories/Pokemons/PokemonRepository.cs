﻿using DAL.EF;
using DOM.Project.Pokemons;
using Microsoft.EntityFrameworkCore;

namespace DAL.PbsRepositories.Pokemons;

public class PokemonRepository : IPokemonRepository
{
    private readonly ProjectDbContext _context;

    public PokemonRepository(ProjectDbContext context)
    {
        _context = context;
    }

    public void CreatePokemon(Pokemon pokemon)
    {
        _context.Pokemons.Add(pokemon);
        // _context.SaveChanges();
    }

    public void CreatePokemonColor(PokemonColor pokemonColor)
    {
        _context.PokemonColors.Add(pokemonColor);
        // _context.SaveChanges();
    }

    public void CreatePokemonEggGroup(PokemonEggGroup pokemonEggGroup)
    {
        _context.PokemonEggGroups.Add(pokemonEggGroup);
        // _context.SaveChanges();
    }

    public void CreatePokemonEvolution(PokemonEvolution pokemonEvolution)
    {
        _context.PokemonEvolutions.Add(pokemonEvolution);
        // _context.SaveChanges();
    }

    public void CreatePokemonEvolutionMethod(PokemonEvolutionMethod pokemonEvolutionMethod)
    {
        _context.PokemonEvolutionMethods.Add(pokemonEvolutionMethod);
        // _context.SaveChanges();
    }

    public void CreatePokemonFlag(PokemonFlag pokemonFlag)
    {
        _context.PokemonFlags.Add(pokemonFlag);
        // _context.SaveChanges();
    }

    public void CreatePokemonGenderRatio(PokemonGenderRatio pokemonGenderRatio)
    {
        _context.PokemonGenderRatios.Add(pokemonGenderRatio);
        // _context.SaveChanges();
    }

    public void CreatePokemonGrowthRate(PokemonGrowthRate pokemonGrowthRate)
    {
        _context.PokemonGrowthRates.Add(pokemonGrowthRate);
        // _context.SaveChanges();
    }

    public void CreatePokemonHabitat(PokemonHabitat pokemonHabitat)
    {
        _context.PokemonHabitats.Add(pokemonHabitat);
        // _context.SaveChanges();
    }

    public void CreatePokemonShape(PokemonShape pokemonShape)
    {
        _context.PokemonShapes.Add(pokemonShape);
        // _context.SaveChanges();
    }

    public void CreateEvGained(PokemonEvGained pokemonEvGained)
    {
        _context.PokemonEvGained.Add(pokemonEvGained);
        // _context.SaveChanges();
    }

    public IEnumerable<Pokemon> ReadAllPokemonsWithEvolutionAndOffspring()
    {
        return _context.Pokemons
            .Include(p => p.Offspring)
            .Include(p => p.Evolutions)
            .ToList();
    }

    public Pokemon ReadPokemonByInternalName(string internalName)
    {
        return _context.Pokemons.Find(internalName);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}