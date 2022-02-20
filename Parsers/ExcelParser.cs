using System;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using NmarketTestTask.Models;

namespace NmarketTestTask.Parsers
{
    public class ExcelParser : IParser
    {
        public IList<House> GetHouses(string path)
        {
            var flatsInEachRow = new List<List<IXLCell>>();
            var zonesWithFlats = new List<IXLRows>();
            var houses = new List<House>();
            var flats = new List<List<Flat>>();
            var rowsWithHouseNumber = new List<int>();
            var workbook = new XLWorkbook(path);
            var sheet = workbook.Worksheets.First();
            var housesCells = sheet.CellsUsed().Where(c => c.GetValue<string>().Contains("Дом")).ToList();

            foreach (var rows in housesCells)//finding rows with house number
                rowsWithHouseNumber.Add(rows.Address.RowNumber);
            rowsWithHouseNumber.Add(sheet.LastRowUsed().RowNumber());//last row with data

            for (int i = 0; i < rowsWithHouseNumber.Count - 1; i++)//fide zones with flats between houses
                zonesWithFlats.Add(sheet.Rows(rowsWithHouseNumber[i], rowsWithHouseNumber[i + 1]));

            foreach (var row in zonesWithFlats)//split each zones with rows into single rows
            {
                var rowsWithFlats = new List<IXLCell>();
                foreach (var CellsWithFlats in row.CellsUsed())//add cells with flat into list
                    if (CellsWithFlats.GetValue<string>().Contains("№"))
                        rowsWithFlats.Add(CellsWithFlats);
                flatsInEachRow.Add(rowsWithFlats);
            }

            for (int i = 0; i < flatsInEachRow.Count; i++)//creating list of the flats
            {
                var tempFlats = new List<Flat>();
                foreach (var flat in flatsInEachRow[i])
                    tempFlats.Add(new Flat() { Number = flat.Value.ToString(), Price = flat.CellBelow().Value.ToString() });
                flats.Add(tempFlats);
            }

            for (int i = 0; i < housesCells.Count; i++)//adding houses to list
                houses.Add(new House() { Name = housesCells[i].Value.ToString(), Flats = flats[i] });

            return houses;
        }
    }
}