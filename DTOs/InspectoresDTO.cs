
namespace inspectores_api.DTO;

public class InspectoresDTO
{
    
    private string hashLagajo;

    public string? NumeroAfiliado { get; set; }

    public string? NombreCompleto { get; set; }

    public string? Funcion { get; set; }

    public string? Documento { get; set; }

    public int? Activo { get; set; }

     public string? UrlImagen { get; set; }

    public string? QrBase64 {get;set;}

    public string? Tarea { get; set; }

    public string? Oficina { get; set; }


    public string? HashLagajo
    {
         get{ return hashLagajo;}

        set { hashLagajo = value;}    
    }


}