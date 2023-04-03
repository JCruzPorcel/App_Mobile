using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace ITES_App.Models
{
    public class Alumno
    {
        public string DNI { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
    }

    public class FirebaseHelper
    {
        private const string dataBaseURL = "https://ites-app-default-rtdb.firebaseio.com/";

        public string dataType = "Alumnos";

        FirebaseClient firebase;

        public FirebaseHelper()
        {
            firebase = new FirebaseClient(dataBaseURL);
        }

        public async Task AgregarAlumno(string dni, string password, string email, string nombre)
        {
            try
            {
                // Crea un nuevo objeto Alumno con los datos proporcionados y lo agrega a la base de datos
                await firebase.Child(dataType).Child(dni).PutAsync(new Alumno() { DNI = dni, Password = password, Email = email, Nombre = nombre });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar alumno: {ex.Message}");
            }
        }

        public async Task<Alumno> ObtenerAlumno(string dni)
        {
            var alumno = (await firebase.Child(dataType).OnceAsync<Alumno>()).FirstOrDefault(x => x.Object.DNI == dni)?.Object;
            return alumno;
        }

        public async Task<bool> ActualizarAlumno(string dni, string password, string email, string nombre)
        {
            try
            {
                var alumno = (await firebase.Child(dataType).OnceAsync<Alumno>()).FirstOrDefault(x => x.Object.DNI == dni);
                await firebase.Child(dataType).Child(alumno.Key).PutAsync(new Alumno { DNI = dni, Password = password, Email = email, Nombre = nombre });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarAlumno(string dni)
        {
            try
            {
                var alumno = (await firebase.Child(dataType).OnceAsync<Alumno>()).FirstOrDefault(x => x.Object.DNI == dni);

                string correo = alumno.Object.Email;

                await firebase.Child(dataType).Child(alumno.Key).DeleteAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }        

    }
}