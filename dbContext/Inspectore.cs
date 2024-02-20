using System;
using System.Collections.Generic;

namespace inspectores_api.dbContext;

public partial class Inspectore
{
    public long Id { get; set; }

    public string? NumeroAfiliado { get; set; }

    public string? NombreCompleto { get; set; }

    public long? FuncionId { get; set; }

    public string? Documento { get; set; }

    public int? Activo { get; set; }

    public string? HashLagajo { get; set; }

    public string? QrBase64 { get; set; }

    public string? UrlImagen { get; set; }

    public string? Tarea { get; set; }

    public string? Oficina { get; set; }

    public virtual Funcione? Funcion { get; set; }
}
