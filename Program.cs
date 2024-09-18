using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;


bool condicion = false;


List<Archivo> palabras = new List<Archivo>();

string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
string filePath = $"{escritorio}/.json";



while (!condicion)
{
    Console.WriteLine("MENU");
    Console.WriteLine("1 - Crear archivo y agregar datos");
    Console.WriteLine("2 - Listar archivos");
    Console.WriteLine("3 - Abrir archivo");
    Console.WriteLine("4 - Modificar archivo");
    Console.WriteLine("5 - Eliminar archivo");
    Console.WriteLine("6 - Recuperar archivo");
    Console.WriteLine("7 - Salir");

    Console.WriteLine("Ingrese una opcion:");
    string opciones = Console.ReadLine()! ;
    Console.WriteLine("");

    if (opciones == "1")

    {

        Console.WriteLine("Ingrese el nombre del archivo:");
        string nombre = Console.ReadLine()!;
        Console.WriteLine("Ingrese el texto:");
        string texto = Console.ReadLine()!;

        Archivo archivo = new Archivo(nombre, texto, false, "null");
        palabras.Add(archivo); // Agregar a la lista

        // Serializar la lista a JSON
        string jsonString = JsonSerializer.Serialize(palabras);
        File.WriteAllText(filePath, jsonString);

        
       
    }

    if (opciones == "2")
    {
        string jsonFromFile = File.ReadAllText(filePath);
        List <Archivo> archivos = JsonSerializer.Deserialize<List<Archivo>>(jsonFromFile)!;
        


        int enumerar = 1;
        foreach(var arch in archivos){
            if (arch.papelera == false ){
                Console.WriteLine($"{enumerar}. {arch.nombre}");
                Console.WriteLine($"Total caracteres:{arch.totalcarct}");
                Console.WriteLine($"Fecha de creacion:{arch.fechadecreacion.ToString("yyyy-MM-dd")}");
                Console.WriteLine($"Fecha de modificacion:{arch.fechamod.ToString("yyyy-MM-dd")}");

                enumerar ++;
            }
        }

                
    }

    
    if (opciones == "3"){
        string jsonFromFile = File.ReadAllText(filePath);
        List <Archivo> archivos = JsonSerializer.Deserialize<List<Archivo>>(jsonFromFile)!;
        

        Console.WriteLine("ARCHIVOS");
        Console.WriteLine("");
        int enumerar = 1;
        foreach(var arch in archivos){
            if (arch.papelera == false){
                Console.WriteLine($"{enumerar}. {arch.nombre}");
                Console.WriteLine($"Total caracteres:{arch.totalcarct}");
                Console.WriteLine($"Fecha de creacion:{arch.fechadecreacion.ToString("yyyy-MM-dd")}");
                Console.WriteLine($"Fecha de modificacion:{arch.fechamod.ToString("yyyy-MM-dd")}");

                enumerar ++;
            }
            

        }

        Console.WriteLine("----------------------");


        Console.WriteLine("Ingrese un numero de la lista:");
        string num = Console.ReadLine()!;

        int enumr = 1;
        foreach(var arch in archivos){
            if (arch.papelera == false){
                if (num == Convert.ToString(enumr)){
                    Console.WriteLine($"Nombre del archivo: {arch.nombre}");
                    Console.WriteLine($"Total caracteres:{arch.totalcarct}");
                    Console.WriteLine($"Fecha de creacion:{arch.fechadecreacion.ToString("yyyy-MM-dd")}");
                    Console.WriteLine($"Fecha de modificacion:{arch.fechamod.ToString("yyyy-MM-dd")}");
                    Console.WriteLine($"Contenido: {arch.texto}");

                }
            
                enumr ++;
            }
            

        }

    }

    if (opciones == "4")
{
    string jsonFromFile = File.ReadAllText(filePath);
    List<Archivo> archivos = JsonSerializer.Deserialize<List<Archivo>>(jsonFromFile)!;

    Console.WriteLine("ARCHIVOS");
    Console.WriteLine("");
    int enumerar = 1;
    foreach (var arch in archivos)
    {
        if (arch.papelera == false)
        {
            Console.WriteLine($"{enumerar}. {arch.nombre}");
            Console.WriteLine($"Total caracteres: {arch.totalcarct}");
            Console.WriteLine($"Fecha de creación: {arch.fechadecreacion.ToString("yyyy-MM-dd")}");
            Console.WriteLine($"Fecha de modificación: {arch.fechamod.ToString("yyyy-MM-dd")}");

            enumerar++;
        }
    }

    Console.WriteLine("----------------------");
    Console.WriteLine("Ingrese un número de la lista:");
    string num = Console.ReadLine()!;

    Archivo? archivoseleccionado = null;
    int enumr = 1;
    foreach (var arch in archivos)
    {
        if (arch.papelera == false)
        {
            if (num == Convert.ToString(enumr))
            {
                archivoseleccionado = arch;
                break;
            }
            enumr++;
        }
    }

    if (archivoseleccionado != null)
    {
        Console.WriteLine("-----ARCHIVO SELECCIONADO-----");
        Console.WriteLine($"Nombre del archivo: {archivoseleccionado.nombre}");
        Console.WriteLine($"Total caracteres: {archivoseleccionado.totalcarct}");
        Console.WriteLine($"Fecha de creación: {archivoseleccionado.fechadecreacion.ToString("yyyy-MM-dd")}");
        Console.WriteLine($"Fecha de modificación: {archivoseleccionado.fechamod.ToString("yyyy-MM-dd")}");
        Console.WriteLine($"Contenido: {archivoseleccionado.texto}");

        Console.WriteLine(" ");
        Console.WriteLine("Ingrese el nuevo texto para el archivo:");
        string newtext = Console.ReadLine()!;
        Console.WriteLine("");
        Console.WriteLine("Presione la tecla ESCAPE");

        ConsoleKeyInfo tecla = Console.ReadKey(true);
        if (tecla.Key == ConsoleKey.Escape)
        {
            Console.WriteLine("Desea guardar los cambios? (Si / No):");
            string respuesta = Console.ReadLine()!;

            if (respuesta == "Si")
            {
                // Obtener la ruta del escritorio
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                int numArchivo = 1;

                // Eliminar todos los archivos numerados 
                while (true)
                {
                    string nombrearchivo = Path.Combine(desktopPath, $"{archivoseleccionado.nombre}_no{numArchivo}.json");
                    if (File.Exists(nombrearchivo))
                    {
                        File.Delete(nombrearchivo);
                       
                    }
                    else
                    {
                        break;
                    }
                    numArchivo++;
                }

                // Crear el nuevo archivo modificado con los datos originales y el nuevo texto
                Archivo archivomodificado = new Archivo(archivoseleccionado.nombre, newtext, false, "null")
                {
                    fechadecreacion = archivoseleccionado.fechadecreacion, // Copiar la fecha de creación original
                    fechamod = DateTime.Now, // Actualizar la fecha de modificación actual

                };

                // Actualizar la lista de archivos
                archivos.Remove(archivoseleccionado);
                archivos.Add(archivomodificado);

                // Guardar los cambios en el archivo JSON
                string jsonString = JsonSerializer.Serialize(archivos);
                File.WriteAllText(filePath, jsonString);

                Console.WriteLine("Archivo modificado exitosamente.");
            }
        }
    }
}



    if (opciones == "5")
{
    string jsonFromFile = File.ReadAllText(filePath);
    List<Archivo> archivos = JsonSerializer.Deserialize<List<Archivo>>(jsonFromFile)!;

    Console.WriteLine("");
    Console.WriteLine("ARCHIVOS");
    Console.WriteLine("");
    int enumerar = 1;

    foreach (var arch in archivos)
    {
        if (arch.papelera == false)
        {
            Console.WriteLine($"{enumerar}. {arch.nombre}");
            Console.WriteLine($"Total caracteres: {arch.totalcarct}");
            enumerar++;
        }
    }

    Console.WriteLine("----------------------");

    Console.WriteLine("Ingrese un numero de la lista:");
    string num = Console.ReadLine()!;

    int enumr = 1;
    Archivo ?archivoSeleccionado = null ; 

    foreach (var arch in archivos)
    {
        if (arch.papelera == false)
        {
            if (num == Convert.ToString(enumr))
            {
                archivoSeleccionado = arch; // Almacenar el archivo seleccionado
                break;
            }
            enumr++;
        }
    }

    if (archivoSeleccionado != null)
    {
        Console.WriteLine($"Nombre del archivo: {archivoSeleccionado.nombre}");
        Console.WriteLine($"Total caracteres: {archivoSeleccionado.totalcarct}");

        Console.WriteLine("Desea guardar los cambios? (Si / No):");
        string respuesta = Console.ReadLine()!;

        if (respuesta == "Si")
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            int numArchivo = 1;

            // Eliminar todos los archivos numerados
            while (true)
            {
                string nombrearchivo = Path.Combine(desktopPath, $"{archivoSeleccionado.nombre}_no{numArchivo}.json");
                if (File.Exists(nombrearchivo))
                {
                    File.Delete(nombrearchivo);
                    
                }
                else
                {
                    break;
                }
                numArchivo++;
            }

            // Crear el nuevo archivo modificado con los datos originales y el nuevo texto
            Archivo archivomodificado = new Archivo(archivoSeleccionado.nombre, archivoSeleccionado.texto, true, DateTime.Now.ToString("yyyy-MM-dd"))
            {
                fechadecreacion = archivoSeleccionado.fechadecreacion, 
                fechamod = archivoSeleccionado.fechamod, 
                
            };

            // Actualizar la lista de archivos (fuera del foreach)
            archivos.Remove(archivoSeleccionado);
            archivos.Add(archivomodificado);

            // Guardar los cambios en el archivo JSON
            string jsonString = JsonSerializer.Serialize(archivos);
            File.WriteAllText(filePath, jsonString);

            Console.WriteLine("Se elimino");
        }
    }
}


    if (opciones == "6")
    {
    string jsonFromFile = File.ReadAllText(filePath);
    List<Archivo> archivos = JsonSerializer.Deserialize<List<Archivo>>(jsonFromFile)!;

    Console.WriteLine("");
    Console.WriteLine("ARCHIVOS");
    Console.WriteLine("");
    int enumerar = 1;

    foreach (var arch in archivos)
    {
        if (arch.papelera == true)
        {
            Console.WriteLine($"{enumerar}. {arch.nombre}");
            Console.WriteLine($"Total caracteres: {arch.totalcarct}");
            enumerar++;
        }
    }

    Console.WriteLine("----------------------");

    Console.WriteLine("Ingrese un numero de la lista:");
    string num = Console.ReadLine()!;

    int enumr = 1;
    Archivo ?archivoSeleccionado = null ; // Variable para almacenar el archivo a modificar

    foreach (var arch in archivos)
    {
        if (arch.papelera == true)
        {
            if (num == Convert.ToString(enumr))
            {
                archivoSeleccionado = arch; // Almacenar el archivo seleccionado
                break;
            }
            enumr++;
        }
    }

    if (archivoSeleccionado != null)
    {
        Console.WriteLine($"Nombre del archivo: {archivoSeleccionado.nombre}");
        Console.WriteLine($"Total caracteres: {archivoSeleccionado.totalcarct}");

        Console.WriteLine("Desea guardar los cambios? (Si / No):");
        string respuesta = Console.ReadLine()!;

        if (respuesta == "Si")
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            int numArchivo = 1;

            // Eliminar todos los archivos numerados
            while (true)
            {
                string nombrearchivo = Path.Combine(desktopPath, $"{archivoSeleccionado.nombre}_no{numArchivo}.json");
                if (File.Exists(nombrearchivo))
                {
                    File.Delete(nombrearchivo);
                 
                }
                else
                {
                    break;
                }
                numArchivo++;
            }

            
            Archivo archivomodificado = new Archivo(archivoSeleccionado.nombre, archivoSeleccionado.texto, false, "null")
            {
                fechadecreacion = archivoSeleccionado.fechadecreacion, 
                fechamod = archivoSeleccionado.fechamod, 
                
            };

          
            archivos.Remove(archivoSeleccionado);
            archivos.Add(archivomodificado);

            // Guardar los cambios en el archivo JSON
            string jsonString = JsonSerializer.Serialize(archivos);
            File.WriteAllText(filePath, jsonString);

            Console.WriteLine("Se recupero");
        }
    }
}




    



    
    

    if (opciones == "7")
    {
        Console.WriteLine("Saliendo...");
        condicion = true;
    }
}


public class Archivo
{
    public string nombre{ get; set; }
    public string texto { get; set; }
    public DateTime fechadecreacion {get; set;}
    public string ruta {get; set;}
    public bool papelera {get; set;}
    public int totalcarct {get; set;}
    public DateTime fechamod {get; set;}
    public string? fechaelim {get; set;}

    public Archivo(string nombre, string texto, bool papelera, string fechaelim)
    {
        this.nombre = nombre;
        this.texto = texto;
        this.fechadecreacion = DateTime.Now.Date;
        this.ruta = $"{nombre}.json";
        this.papelera = papelera;
        this.totalcarct = texto.Length;
        this.fechamod = DateTime.Now.Date;
        this.fechaelim = fechaelim;
        Repartirdatos(texto);

    }






    public void Repartirdatos(string contenido)
{
    
    
    string escritorio = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    int maxCaracteres = 20;  // Número máximo de caracteres por archivo
    int numArchivo = 1;      // Contador para diferenciar los archivos

    for (int i = 0; i < texto?.Length; i += maxCaracteres)
    {
        // Tomar un segmento de 20 caracteres o menos si es el último
        string segmento = texto.Substring(i, Math.Min(maxCaracteres, texto.Length - i));

        // Crea el nombre del archivo con el número
        string nombreArchivo = $"{escritorio}/{nombre}_no{numArchivo}.json";
        

        string nombreArchivoSiguiente = $"{nombre}_no{numArchivo + 1}.json";
        
        // Definir la ruta si hay más texto, la ruta será la del siguiente archivo sino No hay
        string rutasiguiente = i + maxCaracteres < texto.Length ? nombreArchivoSiguiente : "No hay";


        // Crear el contenido del archivo 
        string contenidoArchivo = 
        $"Nombre: {nombre}\n" +
        $"Texto: {segmento}\n" +
        $"Fecha de Creacion: {fechadecreacion.ToString("yyyy-MM-dd")}\n"+
        $"Fecha de Modificacion: {fechamod.ToString("yyyy-MM-dd")}\n" +
        $"Total Caracteres: {totalcarct}\n" +
        $"Papelera: {papelera}\n" +
        $"Ruta siguiente:: {rutasiguiente}\n" +
        $"Fecha de Eliminacion: {fechaelim ?? "No existe"}\n";


        // Crear el archivo
        File.WriteAllText(nombreArchivo, contenidoArchivo);



        // Aumentar el contador para el próximo archivo
        numArchivo++;
    }

}

   

    



     






    



}



