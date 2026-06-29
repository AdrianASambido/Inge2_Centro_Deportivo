using Microsoft.Data.Sqlite;
using System.Globalization;

var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "CentroDeportivo.UI", "CentroDeportivo.db");
if (!File.Exists(dbPath))
{
    // Fallback: look in workspace root
    dbPath = Path.Combine(Directory.GetCurrentDirectory(), "CentroDeportivo.db");
}

Console.WriteLine($"Usando BD: {dbPath}");
if (!File.Exists(dbPath))
{
    Console.WriteLine("No se encontró el archivo de base de datos. Asegúrate de ejecutar desde la raíz del repo o de haber inicializado la app.");
    return 1;
}

using var conn = new SqliteConnection($"Data Source={dbPath}");
conn.Open();

static long ScalarCount(SqliteConnection c, string sql, params SqliteParameter[] ps)
{
    using var cmd = c.CreateCommand();
    cmd.CommandText = sql;
    if (ps != null) cmd.Parameters.AddRange(ps);
    var res = cmd.ExecuteScalar();
    return res == null || res == DBNull.Value ? 0 : Convert.ToInt64(res);
}

static decimal ScalarDecimal(SqliteConnection c, string sql, params SqliteParameter[] ps)
{
    using var cmd = c.CreateCommand();
    cmd.CommandText = sql;
    if (ps != null) cmd.Parameters.AddRange(ps);
    var res = cmd.ExecuteScalar();
    return res == null || res == DBNull.Value ? 0m : Convert.ToDecimal(res, CultureInfo.InvariantCulture);
}

// Conteos
var turnosFinalizados = ScalarCount(conn, "SELECT COUNT(*) FROM Turnos WHERE Estado = 3;");
var reservasConfirmadasFinalizados = ScalarCount(conn,
    "SELECT COUNT(*) FROM Reservas r JOIN Turnos t ON r.Id_Turno = t.Id WHERE t.Estado = 3 AND r.Estado = 1;");

// Pagos últimos 6 meses
var fechaHasta = DateTime.Now;
var fechaDesde = fechaHasta.AddMonths(-6);
var p1 = new SqliteParameter("@desde", fechaDesde);
var p2 = new SqliteParameter("@hasta", fechaHasta);
// Intentar sumar los pagos robustamente leyendo filas y parseando si es necesario
decimal SumarPagos(SqliteConnection c, DateTime desde, DateTime hasta)
{
    using var cmd = c.CreateCommand();
    // Leer todas las filas y filtrar en C# para evitar comparaciones por tipo en SQLite
    cmd.CommandText = "SELECT Monto, Fecha FROM Pagos;";

    using var rdr = cmd.ExecuteReader();
    decimal total = 0m;
    while (rdr.Read())
    {
        decimal monto = 0m;
        DateTime fecha = DateTime.MinValue;

        // Parse monto
        try
        {
            if (!rdr.IsDBNull(0))
            {
                var val = rdr.GetValue(0);
                if (val is decimal dec) monto = dec;
                else if (val is double d) monto = Convert.ToDecimal(d);
                else if (val is long l) monto = Convert.ToDecimal(l);
                else
                {
                    var s = val.ToString();
                    if (!decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out monto))
                        decimal.TryParse(s, NumberStyles.Any, CultureInfo.CurrentCulture, out monto);
                }
            }
        }
        catch { continue; }

        // Parse fecha
        try
        {
            if (!rdr.IsDBNull(1))
            {
                var val = rdr.GetValue(1);
                if (val is DateTime dt) fecha = dt;
                else
                {
                    var s = val.ToString();
                    if (!DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out fecha))
                        DateTime.TryParse(s, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out fecha);
                }
            }
        }
        catch { }

        if (fecha == DateTime.MinValue)
        {
            // si no se pudo parsear la fecha, incluir por seguridad
            total += monto;
        }
        else
        {
            if (fecha >= desde && fecha <= hasta) total += monto;
        }
    }

    return total;
}

var totalPagosPeriodo = SumarPagos(conn, fechaDesde, fechaHasta);

Console.WriteLine();
Console.WriteLine("Resumen diagnóstico:");
Console.WriteLine($" - Turnos finalizados: {turnosFinalizados}");
Console.WriteLine($" - Reservas confirmadas sobre turnos finalizados: {reservasConfirmadasFinalizados}");
Console.WriteLine($" - Total de pagos (últimos 6 meses): {totalPagosPeriodo:C}");

// Mostrar algunos pagos recientes
Console.WriteLine();
Console.WriteLine("Pagos recientes (5):");
using (var cmd = conn.CreateCommand())
{
    cmd.CommandText = "SELECT Id, Id_Usuario, Id_Reserva, Id_Turno, Monto, Fecha FROM Pagos ORDER BY Fecha DESC LIMIT 5;";
    using var rdr = cmd.ExecuteReader();
    while (rdr.Read())
    {
        var reservaStr = rdr.IsDBNull(2) ? "null" : rdr.GetInt32(2).ToString();
        var turnoStr = rdr.IsDBNull(3) ? "null" : rdr.GetInt32(3).ToString();
        Console.WriteLine($"Id={rdr.GetInt32(0)} Usuario={rdr.GetInt32(1)} Reserva={reservaStr} Turno={turnoStr} Monto={rdr.GetDecimal(4):C} Fecha={rdr.GetDateTime(5)}");
    }
}

// Mostrar algunas reservas finalizadas
Console.WriteLine();
Console.WriteLine("Reservas confirmadas sobre turnos finalizados (5):");
using (var cmd = conn.CreateCommand())
{
    cmd.CommandText = @"SELECT r.Id, r.Id_Usuario, r.Id_Turno, r.PrecioPagado, r.Estado, r.Asistencia
                        FROM Reservas r
                        JOIN Turnos t ON r.Id_Turno = t.Id
                        WHERE t.Estado = 3 AND r.Estado = 1
                        ORDER BY r.FechaAsistencia DESC LIMIT 5;";
    using var rdr = cmd.ExecuteReader();
    while (rdr.Read())
    {
        Console.WriteLine($"Id={rdr.GetInt32(0)} Usuario={rdr.GetInt32(1)} Turno={rdr.GetInt32(2)} Precio={rdr.GetDecimal(3):C} Estado={rdr.GetInt32(4)} Asistencia={rdr.GetInt32(5)}");
    }
}

conn.Close();
return 0;