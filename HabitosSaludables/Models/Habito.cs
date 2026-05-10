using SQLite;

namespace HabitosSaludables.Models
{
    [Table("Habitos")]
    public class Habito
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string Nombre { get; set; } = string.Empty;

        public string Categoria { get; set; } = string.Empty;

        /// <summary>Emoji o ícono representativo de la categoría.</summary>
        public string Icono { get; set; } = "🌿";

        /// <summary>Días consecutivos completados.</summary>
        public int RachaActual { get; set; } = 0;

        /// <summary>Mayor racha lograda histórica.</summary>
        public int MejorRacha { get; set; } = 0;

        /// <summary>Fecha del último check (ISO 8601: yyyy-MM-dd).</summary>
        public string? UltimoCheck { get; set; }

        /// <summary>¿Ya se marcó como completado hoy?</summary>
        public bool CompletadoHoy { get; set; } = false;

        /// <summary>Fecha de creación del hábito.</summary>
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        /// <summary>Hábito requiere uso de cámara.</summary>
        public bool UsarCamara { get; set; } = false;

        /// <summary>Ruta local de la foto del hábito.</summary>
        public string? FotoPath { get; set; }

        /// <summary>Latitud capturada al crear el hábito (null si no aplica).</summary>
        public double? Latitud { get; set; }

        /// <summary>Longitud capturada al crear el hábito (null si no aplica).</summary>
        public double? Longitud { get; set; }
    }
}