using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Services;
using Onoicrm.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Onoicrm.DataContext.Utils;

namespace Onoicrm.DataContext.Services;

public class FileService : IFileService
{
    private readonly ApplicationDataContext _dataContext;
    private readonly IConfiguration _configuration;

    public FileService(ApplicationDataContext dataContext, IConfiguration configuration)
    {
        _dataContext = dataContext;
        _configuration = configuration;
    }

    public async Task<TFileClass> Save<TFileClass>(TFileClass model, IFormFile file) where TFileClass : AttachedFile, new()
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var data = ms.ToArray();
        model.StorageId = Guid.NewGuid().ToString();
        var basePath = _configuration.GetValue<string>("FileStorage");
        var folderPath = $"{basePath}/{model.ClassName}";
        FileUtils.CreateFoldersIfNotExist(folderPath);
        FileUtils.SaveToFileSystem($"{folderPath}/{model.StorageId}.{model.Extension}", data);
        await _dataContext.Set<TFileClass>().AddAsync(model);
        return model;    
    }

    public async Task<FileContentResult> Get<TFileClass>(long? id, bool download = false) where TFileClass : AttachedFile, new()
    {
        var file = await _dataContext.Set<TFileClass>().FindAsync(id);
        if (file == null) throw new Exception($"Файл по ID {id}");
        var path =  Environment.GetEnvironmentVariable("FileStorage") ?? _configuration.GetValue<string>("FileStorage");
        var data = FileUtils.ReadFileBytes($"{path}/{file.ClassName}/{file.StorageId}.{file.Extension}");
        return download 
            ? new FileContentResult(data, "image/jpg") { FileDownloadName = file.Name }
            : new FileContentResult(data, "image/jpg");
    }

    public async Task<TFileClass> Save<TFileClass>(TFileClass model, string base64) where TFileClass : AttachedFile, new()
    {
        if (!base64.IsBase64String()) throw new Exception("Файл повреждён");
        var data = Convert.FromBase64String(base64);
        model.StorageId = Guid.NewGuid().ToString();
        var basePath = _configuration.GetValue<string>("FileStorage");
        var folderPath = $"{basePath}/{model.ClassName}";
        FileUtils.CreateFoldersIfNotExist(folderPath);
        FileUtils.SaveToFileSystem($"{folderPath}/{model.StorageId}.{model.Extension}", data);
        await _dataContext.Set<TFileClass>().AddAsync(model);
        return model;    
    }

    public void Delete<TFileClass>(TFileClass model) where TFileClass : AttachedFile, new()
    {
        var basePath =  _configuration.GetValue<string>("FileStorage");
        var filePath = $"{basePath}/{model.ClassName}/{model.StorageId}.{model.Extension}";
        FileUtils.DeleteFile(filePath);
        _dataContext.Set<TFileClass>().Remove(model);
    }

    public async Task AddRange<TFileClass>(List<TFileClass> files,Func<TFileClass, IFormFile> converter) where TFileClass : AttachedFile, new()
    {
        if (files.Any())
        {
            foreach (var file in files)
            {
                await Save(file,converter(file));
            }
        }
    }

    public Task DeleteRange<TFileClass>(List<TFileClass> files) where TFileClass : AttachedFile, new()
    {
        throw new NotImplementedException();
    }

    public Task UpdateRange<TFileClass>(List<TFileClass> files) where TFileClass : AttachedFile, new()
    {
        throw new NotImplementedException();
    }

    public TFileClass Update<TFileClass>(TFileClass model, string base64) where TFileClass : AttachedFile, new()
    {
        if (!base64.IsBase64String()) throw new Exception("Файл повреждён");
        var data = Convert.FromBase64String(base64);
        var basePath = _configuration.GetValue<string>("FileStorage");
        var folderPath = $"{basePath}/{model.ClassName}/{model.StorageId}.{model.Extension}";
        FileUtils.SaveToFileSystem(folderPath, data);
        _dataContext.Set<TFileClass>().Update(model);
        return model;    
    }
}