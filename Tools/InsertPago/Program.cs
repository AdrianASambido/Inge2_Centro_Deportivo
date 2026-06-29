using Microsoft.Data.Sqlite;
using System.Globalization;

var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "CentroDeportivo.UI", "CentroDeportivo.db");
if (!File.Exists(dbPath)) dbPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "CentroDeportivo.UI", "CentroDeportivo.db");
if (!File.Exists(dbPath)) dbPath = Path.Combine(Directory.GetCurrentDirectory(), "CentroDeportivo.UI", "CentroDeportivo.db");
if (!File.Exists(dbPath)) dbPath = Path.Combine(Directory.GetCurrentDirectory(), "CentroDeportivo.db");

Console.WriteLine($"Usando BD: {dbPath}");
if (!File.Exists(dbPath))
{
    Console.WriteLine("No se encontró el archivo de base de datos.");
    return 1;
}

using var conn = new SqliteConnection($"Data Source={dbPath}");
conn.Open();
// Diagnostic: mostrar esquema de la tabla Pagos antes de insertar
using (var s = conn.CreateCommand())
{
    s.CommandText = "SELECT sql FROM sqlite_master WHERE type='table' AND name='Pagos';";
    using var r = s.ExecuteReader();
    if (r.Read())
    {
        Console.WriteLine("Schema Pagos:\n" + r.GetString(0));
    }
}

using (var s2 = conn.CreateCommand())
{
    s2.CommandText = "PRAGMA table_info('Pagos');";
    using var r2 = s2.ExecuteReader();
    Console.WriteLine("Columns in Pagos:");
    while (r2.Read())
    {
        var dflt = r2.IsDBNull(4) ? "NULL" : r2.GetValue(4).ToString();
        Console.WriteLine($" - {r2.GetInt32(0)}: {r2.GetString(1)} (type={r2.GetString(2)}, notnull={r2.GetInt32(3)}, dflt_value={dflt})");
    }
}

// Después de mostrar esquema, continuar con inserción
using var tr = conn.BeginTransaction();
// Buscar una reserva confirmada sobre turno finalizado
using (var cmd = conn.CreateCommand())
{
    cmd.CommandText = @"SELECT r.Id, r.Id_Usuario, r.Id_Turno, r.PrecioPagado
                        FROM Reservas r
                        JOIN Turnos t ON r.Id_Turno = t.Id
                        WHERE t.Estado = 3 AND r.Estado = 1
                        LIMIT 1;";
    using var rdr = cmd.ExecuteReader();
    if (!rdr.Read())
    {
        Console.WriteLine("No se encontró reserva confirmada sobre turno finalizado.");
        return 1;
    }

    var reservaId = rdr.GetInt32(0);
    var usuarioId = rdr.GetInt32(1);
    var turnoId = rdr.GetInt32(2);
    var monto = rdr.GetDecimal(3);

    rdr.Close();

    using var ins = conn.CreateCommand();
    // Insertar valores en ambas columnas Id_Usuario/UsuarioId y Id_Reserva/ReservaId y Id_Turno/TurnoId
    ins.CommandText = "INSERT INTO Pagos (Id_Usuario, UsuarioId, Id_Reserva, ReservaId, Id_Turno, TurnoId, Monto, Fecha) VALUES (@id_u, @u, @id_r, @r, @id_t, @t, @m, @f);";
    ins.Parameters.AddWithValue("@id_u", usuarioId);
    ins.Parameters.AddWithValue("@u", usuarioId);
    ins.Parameters.AddWithValue("@id_r", reservaId == 0 ? (object)DBNull.Value : reservaId);
    ins.Parameters.AddWithValue("@r", reservaId == 0 ? (object)DBNull.Value : reservaId);
    ins.Parameters.AddWithValue("@id_t", turnoId == 0 ? (object)DBNull.Value : turnoId);
    ins.Parameters.AddWithValue("@t", turnoId == 0 ? (object)DBNull.Value : turnoId);
    // La columna Monto en la BD es TEXT; guardar usando la cultura invariante
    ins.Parameters.AddWithValue("@m", monto.ToString(CultureInfo.InvariantCulture));
    ins.Parameters.AddWithValue("@f", DateTime.Now.ToString("o", CultureInfo.InvariantCulture));

    var rows = ins.ExecuteNonQuery();
    tr.Commit();
    Console.WriteLine($"Insertado Pago: Reserva={reservaId}, Usuario={usuarioId}, Turno={turnoId}, Monto={monto:C}");
}

conn.Close();
return 0;