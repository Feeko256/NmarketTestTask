using System.Collections.Generic;
using HtmlAgilityPack;
using NmarketTestTask.Models;

namespace NmarketTestTask.Parsers
{
    public class HtmlParser : IParser
    {
        public IList<House> GetHouses(string path)
        {
            var houses = new List<House>();
            var doc = new HtmlDocument();
            var rows = new List<List<HtmlNode>>();
            doc.Load(path);

            foreach (var table in doc.DocumentNode.SelectNodes(".//table"))
            {
                foreach (var row in table.SelectNodes("tbody/tr"))
                {
                    var cells = new List<HtmlNode>();
                    foreach (var cell in row.SelectNodes("td"))
                    {
                        cells.Add(cell);
                    }
                    rows.Add(cells);
                }
            }
            foreach (var cellsGroup in rows)
            {
                string name = null, number = null, square = null, price = null;
                foreach (var cell in cellsGroup)
                {
                    if (cell.HasClass("house"))
                        name = cell.InnerText;
                    else if (cell.HasClass("number"))
                        number = "№" + cell.InnerText;
                    else if (cell.HasClass("square"))
                        square = cell.InnerText;
                    else if (cell.HasClass("price"))
                        price = cell.InnerText;
                }
                houses.Add(new House()
                {
                    Name = name,
                    Description = new HouseDescription()
                    {
                        Number = number,
                        Square = square,
                        Price = price
                    }
                });
            }
            return houses;
        }
    }
}