﻿namespace Tour_Planner_Business;
public static class ReportGenerator
{
    public const string reportsFolder = "./Reports/";
    public const string fileHeader = "[Tour Report] ";
    public const string summarizeReport = "SummarizeReport";
    public static void GenerateTourReport(Tour myTour, byte[] myImage)
    {
        PdfWriter writer = new(reportsFolder + myTour.Id + ".pdf");
        PdfDocument myReport = new(writer);
        Document document = new(myReport);

        //Header
        Paragraph header = new Paragraph(fileHeader + myTour.TourName)
            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20)
            .SetBold();
        document.Add(header);

        // Line separator
        LineSeparator ls = new(new SolidLine());
        document.Add(ls);

        //Tour Data List
        Paragraph TourListHeader = new Paragraph("Tour Data:")
            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
            .SetFontSize(14)
            .SetBold();
        List tourList = new List()
          .SetSymbolIndent(12)
          .SetListSymbol("\u2022")
          .SetFontSize(13)
          .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
        tourList.Add(new ListItem("Name: " + myTour.TourName))
            .Add(new ListItem("Description: " + myTour.TourDescription))
            .Add(new ListItem("Starting Point: " + myTour.StartingPoint))
            .Add(new ListItem("Destination: " + myTour.Destination))
            .Add(new ListItem("Transport Type: " + myTour.TransportType))
            .Add(new ListItem("Tour Distance: " + myTour.TourDistance + " km"))
            .Add(new ListItem("Estimated Time In Min: " + myTour.EstimatedTimeInMin + " min"))
            .Add(new ListItem("Tour Type: " + myTour.TourType));
        document.Add(TourListHeader);
        document.Add(tourList);

        //Tour Logs List
        if (myTour.TourLogs is null)
        {
            goto skip;
        }
        Paragraph logListHeader = new Paragraph("\nTour Logs:")
            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
            .SetFontSize(14)
            .SetBold();
        List logList = new List()
            .SetSymbolIndent(12)
            .SetListSymbol("\u2022")
            .SetFontSize(12)
            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
        foreach (TourLog? item in myTour.TourLogs)
        {
            logList.Add("Date and Time: " + item.TourDateAndTime)
                .Add("Comment: " + item.TourComment)
                .Add("Tour Difficulty [0-4]: " + (int)item.TourDifficulty)
                .Add("Time In Min: " + item.TourTimeInMin + " min")
                .Add("Tour Rating [0-4]: " + (int)item.TourRating + "\n\n");
        }
        document.Add(logListHeader);
        document.Add(logList);

        skip:

        //Image
        Paragraph imageHeader = new Paragraph("Tour Image: ")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(14)
                    .SetBold();
        ImageData test = ImageDataFactory.Create(myImage);
        document.Add(imageHeader);
        document.Add(new Image(test));

        //Close
        document.Close();
    }
    public static void GenerateSummaryReport(IEnumerable<Tour> allTours)
    {
        PdfWriter writer = new(reportsFolder + summarizeReport + ".pdf");
        PdfDocument myReport = new(writer);
        Document document = new(myReport);

        //Header
        Paragraph header = new Paragraph(fileHeader + "Summary")
            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(20)
            .SetBold();
        document.Add(header);

        // Line separator
        LineSeparator ls = new(new SolidLine());
        document.Add(ls);

        //Tour List
        if (allTours is null)
        {
            document.Close();
            return;
        }
        Paragraph tourListHeader = new Paragraph("Tour Data:")
            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
            .SetFontSize(14)
            .SetBold();
        List tourList = new List()
            .SetSymbolIndent(12)
            .SetListSymbol("\u2022")
            .SetFontSize(13)
            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA));
        foreach (Tour? item in allTours)
        {
            tourList.Add(new ListItem("Name: " + item.TourName))
                .Add(new ListItem("Description: " + item.TourDescription))
                .Add(new ListItem("Starting Point: " + item.StartingPoint))
                .Add(new ListItem("Destination: " + item.Destination))
                .Add(new ListItem("Transport Type: " + item.TransportType))
                .Add(new ListItem("Tour Distance: " + item.TourDistance + " km"))
                .Add(new ListItem("Estimated Time In Min: " + item.EstimatedTimeInMin + " min"))
                .Add(new ListItem("Tour Type: " + item.TourType))
                .Add(new ListItem("Average Time: " + ReportCalculations.GetAverageTime(item) + " min"))
                .Add(new ListItem("Average Rating [0-4]: " + ReportCalculations.GetAverageRating(item) + "\n\n"));
        }
        document.Add(tourListHeader);
        document.Add(tourList);
        document.Close();
    }
}