﻿using BL.DataTransferObjects;
using DOM.Project.Pokemons;
using DOM.Project.Typings;
using DOM.ProjectFolders;

namespace BL;

public interface IProjectManager
{
    public bool ChangeConnectionString(string folderpath);
    public bool ResetConnectionString();
    public void CompilePbsFiles();
    public IEnumerable<Typing> GetAllTypingsWithFullJoins();
    public IEnumerable<Pokemon> GetAllPokemonsWithTypings();
    public int GetAmountOfTypings();
    public void ChangeTypeEffectiveness(ICollection<TypeEffectivenessFieldChange> changes);
    public void ChangePokemonAvailability(ICollection<PokemonAvailabilityChange> changes);
    public string GetProjectFolderPath();
}