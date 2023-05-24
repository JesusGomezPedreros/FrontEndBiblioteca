using FrontEndBiblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Modelos.Modelos;
using Newtonsoft.Json;
using Servicios;
using Servicios.Seguimiento;
using System.Diagnostics;

namespace FrontEndBiblioteca.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly ProyectoSevices _proyectoSevices;
        public readonly MacService _macService;
        public HomeController(ILogger<HomeController> logger, ProyectoSevices proyectoSevices, MacService macService)
        {
            _logger = logger;
            _proyectoSevices = proyectoSevices;
            _macService = macService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            var respuesta= String.Empty;
            var estudiante = String.Empty;
            var profesores = String.Empty;
            
            try
            {
                respuesta = await _proyectoSevices.IniciarSesion(login);
                estudiante = await _proyectoSevices.ConsultaTodosEstudiantesMaterias();
                profesores = await _proyectoSevices.ConsultaTodosProfesoresMaterias();

            }
            catch (Exception)
            {

               
            }
            if (respuesta != "\"El login no existe\"")
            {
                var informacion = JsonConvert.DeserializeObject<List<RespuestaLogin>>((string)JsonConvert.DeserializeObject(respuesta));
                var informacionEstudiantes = JsonConvert.DeserializeObject<List<RespuestaLogin>>(estudiante);
                var informacionProfesores = JsonConvert.DeserializeObject<List<RespuestaLogin>>(profesores);
                var modeloUsuario = new
                {
                    informacionUsuario = informacion,
                    informacionEstudiantes = informacionEstudiantes,
                    informacionProfesores = informacionProfesores
                };
                return View(modeloUsuario);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
            
        }
        public IActionResult RegistrarUsuario()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegistrarUsuario(CrearUsuarios crearUsuarios)
        {
            var registroUsuario = new CrearUsuarios();
            try
            {
                registroUsuario.nombres = crearUsuarios.nombres.ToLower();
                registroUsuario.apellidos = crearUsuarios.apellidos.ToLower();
                registroUsuario.usuarioCreacion = _macService.TraerDireccionMac();
                registroUsuario.tipoIdentificacion = crearUsuarios.tipoIdentificacion.ToLower();
                registroUsuario.numeroIdentificacion = crearUsuarios.numeroIdentificacion;
                registroUsuario.nombreLogin = crearUsuarios.nombreLogin;
                registroUsuario.contrasena = crearUsuarios.contrasena;
                registroUsuario.idRol = crearUsuarios.idRol;
            }
            catch (Exception)
            {
                
            }
            var usuarioCreado = await _proyectoSevices.RegistrarUsuarios(registroUsuario);
            var resultado = JsonConvert.DeserializeObject(usuarioCreado);

            TempData["UsuarioCreado"] = resultado.ToString();

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}