

namespace QF.Utilidades
{
    //Cambiamos apublic, agregamos static
    public static class ConexionDB
    {
        //public, static ruta para Android e Ios
        public static string DevolverRuta(string nombreBaseDatos) { 
            string rutaBaseDatos = string.Empty;
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                rutaBaseDatos = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                rutaBaseDatos = Path.Combine(rutaBaseDatos, nombreBaseDatos);
            } else if(DeviceInfo.Platform == DevicePlatform.iOS)
            {
                rutaBaseDatos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                rutaBaseDatos = Path.Combine(rutaBaseDatos,"..","library", nombreBaseDatos);
            }
            return rutaBaseDatos;
        }
    }
}
