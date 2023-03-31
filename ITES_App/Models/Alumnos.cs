using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Linq;
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
        private readonly string DataType_Alumnos = "Alumnos";

        FirebaseClient firebase;

        public FirebaseHelper()
        {
            firebase = new FirebaseClient("https://ites-app-default-rtdb.firebaseio.com/");
        }

        public async Task AgregarAlumno(string dni, string password, string email, string nombre)
        {
            try
            {
                // Crea una cuenta de autenticación con el correo electrónico y contraseña proporcionados
                var authResult = await FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password);

                // Crea un nuevo objeto Alumno con los datos proporcionados y lo agrega a la base de datos
                await firebase.Child(DataType_Alumnos).Child(dni).PutAsync(new Alumno() { DNI = dni, Password = password, Email = email, Nombre = nombre });
            }
            catch (FirebaseAuthException ex)
            {
                Console.WriteLine($"Error al crear la cuenta de autenticación: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar alumno: {ex.Message}");
            }
        }


        public async Task<Alumno> ObtenerAlumno(string dni)
        {
            var alumno = (await firebase.Child(DataType_Alumnos).OnceAsync<Alumno>()).FirstOrDefault(x => x.Object.DNI == dni)?.Object;
            return alumno;
        }

        public async Task<bool> ActualizarAlumno(string dni, string password, string email, string nombreCompleto)
        {
            try
            {
                var alumno = (await firebase.Child(DataType_Alumnos).OnceAsync<Alumno>()).FirstOrDefault(x => x.Object.DNI == dni);
                await firebase.Child(DataType_Alumnos).Child(alumno.Key).PutAsync(new Alumno { DNI = dni, Password = password, Email = email, Nombre = nombreCompleto });
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
                var alumno = (await firebase.Child(DataType_Alumnos).OnceAsync<Alumno>()).FirstOrDefault(x => x.Object.DNI == dni);

                string correo = alumno.Object.Email;

                // Eliminar el alumno de Firebase
                await firebase.Child(DataType_Alumnos).Child(alumno.Key).DeleteAsync();

                // Eliminar el correo de la cuenta de Firebase Auth
                var user = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(correo);
                
                if (user != null)
                {
                    await FirebaseAuth.DefaultInstance.DeleteUserAsync(user.Uid);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}
