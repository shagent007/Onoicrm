namespace Onoicrm.DataContext.Utils;

public static class FileUtils
{
    public static bool SaveToFileSystem(string fileName, byte[] byteArray)
    {
        try
        {
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception caught in process: {0}", ex);
            return false;
        }
    }
    
    public static byte[] ReadFileBytes(string filePath)
    {
        try
        {
            if (!File.Exists(filePath)) throw new FileNotFoundException($"Файл {filePath} не найден.");
            return File.ReadAllBytes(filePath);
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при чтении файла: {ex.Message}", ex);
        }
    }
    
    public static void DeleteFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            else
            {
                throw new FileNotFoundException($"Файл {filePath} не найден.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка при удалении файла: {ex.Message}", ex);
        }
    }
    
    public static void CreateFoldersIfNotExist(string folderPath)
    {
        try
        {
            if (Directory.Exists(folderPath)) return;
            var folders = folderPath.Split('/');

            var currentPath = folders[0];
            for (var i = 1; i < folders.Length; i++)
            {
                currentPath = Path.Combine(currentPath, folders[i]);
                if (Directory.Exists(currentPath)) continue;
                Directory.CreateDirectory(currentPath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании папок: {ex.Message}");
        }
    }
}