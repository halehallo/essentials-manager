using BL.DataTransferObjects;

namespace BL.PbsManagers.Dex;

public interface IDexManager
{
    IEnumerable<DexTypeCountObject> GetAllTypeCounts();
}