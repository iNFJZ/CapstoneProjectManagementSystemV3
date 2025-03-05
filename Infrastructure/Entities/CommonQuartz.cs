using System.IO;

namespace Infrastructure.Entities
{
    public static class CommonQuartz
    {
        public static void Logs(string message, string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "MyLogs");
            if(!Directory.Exists(path)) 
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, fileName);
            using FileStream stream = new FileStream(path, FileMode.Create);
            using TextWriter textWriter = new StreamWriter(stream);
            textWriter.WriteLine(message);
        }
    }
}
