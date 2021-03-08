using System;
using DetailingArsenal.Domain.ProductCatalog;
using DetailingArsenal.Domain.Shared;

namespace DetailingArsenal.Persistence.Shared {
    internal static class PadColorUtils {
        public static PadColor? Parse(string? raw) => raw switch {
            "pink" => PadColor.Pink,
            "red" => PadColor.Red,
            "orange" => PadColor.Orange,
            "yellow" => PadColor.Yellow,
            "green" => PadColor.Green,
            "blue" => PadColor.Blue,
            "purple" => PadColor.Purple,
            "brown" => PadColor.Brown,
            "black" => PadColor.Black,
            "white" => PadColor.White,
            "gray" => PadColor.Gray,
            null => null,
            _ => throw new InvalidOperationException()
        };

        public static string Serialize(this PadColor color) => color switch {
            PadColor.Pink => "pink",
            PadColor.Red => "red",
            PadColor.Orange => "orange",
            PadColor.Yellow => "yellow",
            PadColor.Green => "green",
            PadColor.Blue => "blue",
            PadColor.Purple => "purple",
            PadColor.Brown => "brown",
            PadColor.Black => "black",
            PadColor.White => "white",
            PadColor.Gray => "gray",
            _ => throw new InvalidOperationException()
        };
    }
}