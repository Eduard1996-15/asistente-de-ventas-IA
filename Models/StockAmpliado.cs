namespace asistenteventas.Models
{
    public class StockAmpliado
    {
        public Stock Stock { get; set; }
        public string Color { get; set; }
        public byte[] Imagen { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is StockAmpliado ampliado &&
                   EqualityComparer<Stock>.Default.Equals(Stock, ampliado.Stock);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Stock);
        }
    }
}
