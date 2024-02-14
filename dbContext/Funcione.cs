using System;
using System.Collections.Generic;

namespace inspectores_api.dbContext;

public partial class Funcione
{
    public long Id { get; set; }

    public string? NameFuncion { get; set; }

    public virtual ICollection<Inspectore> Inspectores { get; set; } = new List<Inspectore>();
}
