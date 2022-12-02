using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Enum for declaring the quality of the item
/// </summary>
public enum Quality { Rubbish, Common, Uncommon, Rare, Epic, Artefact }

public static class QualityColor
{
    private static Dictionary<Quality, string> colors = new Dictionary<Quality, string>()
    {
        {Quality.Rubbish, "grey" },
        {Quality.Common, "white" },
        {Quality.Uncommon, "green" },
        {Quality.Rare, "blue" },
        {Quality.Epic, "purple" },
        {Quality.Artefact, "#E5CC80" }
    };

    public static Dictionary<Quality, string> Colors { get => colors; }
}
