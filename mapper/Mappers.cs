
using inspectores_api.dbContext;
using inspectores_api.DTO;

namespace inspectores_api.mapper;

class Mappers
{
    

    public static IList<InspectoresDTO> InspectorMapArrayInpectoresDTO(IList<Inspectore> inspectoresModels)
    {

        var resu =inspectoresModels.Select(ele => new InspectoresDTO{
                NumeroAfiliado = ele.NumeroAfiliado,
                Funcion = ele.Funcion?.NameFuncion,
                Documento = ele.Documento,
                Activo = ele.Activo,
                NombreCompleto = ele.NombreCompleto,
                HashLagajo = ele.HashLagajo,
                UrlImagen = ele.UrlImagen 

            }).ToArray();

        return resu;


    } 


    public static InspectoresDTO InspectoreMapInspectorDTO(Inspectore inspectoreModel )
    {

            var resu = new InspectoresDTO {
                NumeroAfiliado = inspectoreModel.NumeroAfiliado,
                Funcion = inspectoreModel.Funcion?.NameFuncion,
                Documento = inspectoreModel.Documento,
                Activo = inspectoreModel.Activo,
                NombreCompleto = inspectoreModel.NombreCompleto,
                UrlImagen = inspectoreModel.UrlImagen,
                HashLagajo = inspectoreModel.HashLagajo
                
            };

           return resu;
    }

}




// public string? NumeroAfiliado { get; set; }

//     public string? NombreCompleto { get; set; }

//     public string? Funcion { get; set; }

//     public string? Documento { get; set; }

//     public int? Activo { get; set; }
