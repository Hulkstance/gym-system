using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GymManagement.Infrastructure.Persistence.Converters;

public class ListOfIdsConverter(ConverterMappingHints? mappingHints = null) : ValueConverter<List<Guid>, string>(v => string.Join(',', v),
    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList(),
    mappingHints);

public class ListOfIdsComparer() : ValueComparer<List<Guid>>((t1, t2) => t1!.SequenceEqual(t2!),
    t => t.Select(x => x!.GetHashCode()).Aggregate((x, y) => x ^ y),
    t => t);
