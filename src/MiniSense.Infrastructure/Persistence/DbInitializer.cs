using MiniSense.Domain.Entities;
using MiniSense.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace MiniSense.Infrastructure.Persistence;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        Console.WriteLine("🚀 [SEED] Iniciando processo de Seed...");

        if (context.Users.Any())
        {
            Console.WriteLine("⚠️ [SEED] Banco já possui usuários. Pulando Seed.");
            return;
        }

        try
        {
            Console.WriteLine("1️⃣ [SEED] Criando Usuários...");
            var admin = new User("Andrew Admin", "andrew@minisense.com");
            var guest = new User("Visitante", "guest@minisense.com");
            
            context.Users.AddRange(admin, guest);
            context.SaveChanges(); 
            Console.WriteLine($"✅ [SEED] Usuários salvos! ID Admin: {admin.Id}");
            
            Console.WriteLine("2️⃣ [SEED] Criando Dispositivos...");
            
            var station = new SensorDevice(admin.Id, "Estação Meteorológica", "Sensor Externo");
            var rack = new SensorDevice(admin.Id, "Rack Servidor", "Monitoramento TI");
            
            context.SensorDevices.AddRange(station, rack);
            context.SaveChanges();
            Console.WriteLine($"✅ [SEED] Dispositivos salvos! Key Station: {station.Key}");
            
            Console.WriteLine("3️⃣ [SEED] Criando Streams (Sem dados ainda)...");
            
            var stTemp = new DataStream(station.Id, "temperatura", (UnitType)1); 
            var stLux = new DataStream(station.Id, "luz", (UnitType)4); 
            
            var stCpu = new DataStream(rack.Id, "cpu_load", (UnitType)5); 

            context.DataStreams.AddRange(stTemp, stLux, stCpu);
            context.SaveChanges();
            
            Console.WriteLine($"✅ [SEED] Streams salvas com IDs gerados! (Temp ID: {stTemp.Id})");
            
            Console.WriteLine("4️⃣ [SEED] Gerando histórico de medições...");

            GenerateHistory(stTemp, 25.0, 5.0, 20); 
            GenerateHistory(stLux, 800.0, 200.0, 20);
            GenerateHistory(stCpu, 40.0, 30.0, 20);

            context.SaveChanges();
            Console.WriteLine("✅ [SEED] Histórico gravado com sucesso!");
            Console.WriteLine("🎉 [SEED] CONCLUÍDO!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ [SEED FATAL ERROR]: {ex.Message}");
            if (ex.InnerException != null) Console.WriteLine($"   Detalhe: {ex.InnerException.Message}");
        }
    }

    private static void GenerateHistory(DataStream stream, double start, double variation, int count)
    {
        var rnd = new Random();
        var now = DateTime.UtcNow;
        for (int i = count; i > 0; i--)
        {
            double val = start + (rnd.NextDouble() * variation) - (variation / 2);
            if (val < 0 && variation > 10) val = 0; 
            
            stream.AddMeasurement(Math.Round(val, 2), now.AddMinutes(-i * 10));
        }
    }
}