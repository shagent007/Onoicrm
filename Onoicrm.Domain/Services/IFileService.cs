using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.Domain.Entities;

namespace Onoicrm.Domain.Services;

public interface IFileService
{
    public Task<TFileClass> Save<TFileClass>(TFileClass model, IFormFile file) where TFileClass : AttachedFile, new();
    public Task<FileContentResult> Get<TFileClass>(long? id, bool download = false) where TFileClass : AttachedFile, new();
    public TFileClass Update<TFileClass>(TFileClass model, string base64) where TFileClass : AttachedFile, new();
    public Task<TFileClass> Save<TFileClass>(TFileClass model, string base64) where TFileClass : AttachedFile, new();
    public void Delete<TFileClass>(TFileClass model) where TFileClass : AttachedFile, new();
    public Task AddRange<TFileClass>(List<TFileClass> files, Func<TFileClass, IFormFile> converter) where TFileClass : AttachedFile, new();
    public Task DeleteRange<TFileClass>(List<TFileClass> files) where TFileClass : AttachedFile, new();
    public Task UpdateRange<TFileClass>(List<TFileClass> files) where TFileClass : AttachedFile, new();
}