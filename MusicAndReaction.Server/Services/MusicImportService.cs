using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicAndReaction.Server.Database;

public class MusicImportService
{
    private readonly AppDbContext _db;

    public MusicImportService(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Przeskanuj folder i dodaj do bazy nowe pliki .mp3/.wav z hashem i czasem trwania.
    /// </summary>
    public async Task ImportAsync(string folderPath)
    {
        if (!Directory.Exists(folderPath))
            throw new DirectoryNotFoundException($"Folder nie istnieje: {folderPath}");

        foreach (var file in Directory.EnumerateFiles(folderPath, "*.*", SearchOption.AllDirectories))
        {
            if (!file.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase)
             && !file.EndsWith(".wav", StringComparison.OrdinalIgnoreCase)
             && !file.EndsWith(".flac", StringComparison.OrdinalIgnoreCase))
                continue;

            // 1) Oblicz hash
            var hash = ComputeSha256(file);
            bool exists = await _db.MusicFiles.AnyAsync(m => m.Hash == hash);
            if (exists)
                continue;

            // 2) Przygotuj nowy encję
            var music = new MusicFile
            {
                FileName = Path.GetFileName(file),
                Hash = hash,
            };

            _db.MusicFiles.Add(music);
        }

        await _db.SaveChangesAsync();
    }

    private string ComputeSha256(string filePath)
    {
        using var sha = SHA256.Create();
        using var stream = File.OpenRead(filePath);
        var hash = sha.ComputeHash(stream);
        var sb = new StringBuilder(hash.Length * 2);
        foreach (var b in hash)
            sb.AppendFormat("{0:x2}", b);
        return sb.ToString();
    }
}
