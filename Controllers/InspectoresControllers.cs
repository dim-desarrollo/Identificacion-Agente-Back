using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using inspectores_api.dbContext;
using inspectores_api.DTO;
using inspectores_api.mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCoder;

namespace inspectores_api.Controllers;

[ApiController]
[Route("api/")]
public class InspectoresControllers : ControllerBase
{
    private readonly InspectoresContext db;
    private readonly IConfiguration configuration;
    private readonly ILogger<InspectoresControllers> logger;

    public InspectoresControllers(InspectoresContext db, IConfiguration configuration, ILogger<InspectoresControllers> logger)
    {
        this.db = db;
        this.configuration = configuration;
        this.logger = logger;
    }



    [HttpGet("inspector/")]
    public async Task<ActionResult<InspectoresDTO>> GetInpector(string hashLagajo)
    {   
         
       var inspectore =  await db.Inspectores.Include(f => f.Funcion).FirstOrDefaultAsync(ele => ele.HashLagajo == hashLagajo);

       InspectoresDTO resu;

       if(inspectore != null){

        resu =  Mappers.InspectoreMapInspectorDTO(inspectore);
        return Ok(resu);     
       }  
       return NotFound("No se encontro el Inspector"); 
    }


      [HttpGet("inspectores/list")]
     public async Task<ActionResult<List<InspectoresDTO>>> GetInspectores(int pagina = 1, int sizePagina = 1){

           var listado =  await db.Inspectores
                        .OrderBy(i => i.NumeroAfiliado)    
                        .Skip((pagina -1) * sizePagina)
                        .Include(f => f.Funcion)
                        .Take(sizePagina)
                        .ToListAsync();     

           var DtosList = Mappers.InspectorMapArrayInpectoresDTO(listado);

           return Ok(DtosList);
     }  


    [HttpPost("subi-imagen")]
    public async Task<ActionResult<string>> SubitImagen([FromForm] SubitImagenDTO subitImaenDTO )

    {
        var inspectore = await db.Inspectores.Include(f => f.Funcion).FirstOrDefaultAsync(ele => ele.NumeroAfiliado == subitImaenDTO.Legajo);
    
        if (inspectore != null)
        {
               
            inspectore.UrlImagen = GuardarImagenEnServidor(subitImaenDTO.Imagen,inspectore.Documento);
            await db.SaveChangesAsync();
            return Ok("se guardo con exito");
        }

        return BadRequest("No se encontro el inspector");
    }    


    private string GuardarImagenEnServidor(IFormFile imagen, string documento)
    {
        var currentDirectoryDirtory = Directory.GetCurrentDirectory();
            currentDirectoryDirtory += @"/imagenes";
            
            if (!Directory.Exists(currentDirectoryDirtory))
            {
                Directory.CreateDirectory(currentDirectoryDirtory);
            }

            var namefile = $"{documento}_{imagen.FileName}";

            var saveDbPath = $"/imagenes/{namefile}";

            var fiplePath = Path.Combine(currentDirectoryDirtory, namefile);

            using(var fileStream = new FileStream(fiplePath,FileMode.Create))
            {
                imagen.CopyTo(fileStream);
            }

            return saveDbPath;
    }



    [HttpGet("inspectores/to-list-dos")]
    public async Task<ActionResult<InspectoresDTO>> GetAllInpectores(int pagina = 1, int sizePagina = 1)
    {
        var listado = await db.Inspectores
                      .OrderBy(i => i.NumeroAfiliado)
                      .Skip((pagina - 1) * sizePagina)
                      .Include(f => f.Funcion)
                      .Take(sizePagina)
                      .ToListAsync();

        var DtosList = Mappers.InspectorMapArrayInpectoresDTO(listado);

        foreach (var item in DtosList)
        {
            item.QrBase64 = generabase64(item.HashLagajo);
        }


        return Ok(DtosList);


    }



    private string generabase64(string legajohash){

        var cadenaUrl = $"http://localhost:5212/agentes/${legajohash}";

        var qrGenerartor = new QRCodeGenerator();
        var qrCodeData = qrGenerartor.CreateQrCode(cadenaUrl, QRCodeGenerator.ECCLevel.Q);
        BitmapByteQRCode bitmapByteQRCode = new BitmapByteQRCode(qrCodeData);
        var bitmap = bitmapByteQRCode.GetGraphic(20);

        using var ms = new MemoryStream();
        ms.Write(bitmap);
        byte[] byteImage = ms.ToArray();
        return Convert.ToBase64String(byteImage);

    }

    // [HttpGet("inspecores/add-qr")]
    // public async Task<ActionResult<bool>> PasarTodos(){

            
    //    var todos = await db.Inspectores.ToListAsync();

    //     foreach (var item in todos)
    //     {
    //         item.QrBase64 = generabase64(item.HashLagajo);
    //     }         

    //     await db.SaveChangesAsync();
    //     return Ok(true);
    // }

        // [HttpGet("inspectores/qr/generate")]
    // public ActionResult<object> GenerateQr(string qr){

    //     var qrGenerartor = new QRCodeGenerator();
    //     var qrCodeData = qrGenerartor.CreateQrCode(qr, QRCodeGenerator.ECCLevel.Q);
    //     BitmapByteQRCode bitmapByteQRCode = new BitmapByteQRCode(qrCodeData);
    //     var bitmap = bitmapByteQRCode.GetGraphic(20);

    //     using var ms = new MemoryStream();
    //     ms.Write(bitmap);
    //     byte[] byteImage = ms.ToArray();
    //     return Convert.ToBase64String(byteImage);


    // }


    [HttpGet("prueba-ambientes")]
    public bool prueba(){

        var resu =  configuration["isaias"];   
        logger.LogInformation(resu);
        

        return true;

    }



}