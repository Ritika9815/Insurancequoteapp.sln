public class InsureeController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext();

    // POST: Insuree/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create([Bind(Include = "FirstName,LastName,EmailAddress,Age,CarYear,CarMake,CarModel,SpeedingTickets,DUI,FullCoverage")] Insuree insuree)
    {
        if (ModelState.IsValid)
        {
            insuree.Quote = CalculateQuote(insuree);
            db.Insurees.Add(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(insuree);
    }

    private decimal CalculateQuote(Insuree insuree)
    {
        decimal monthlyTotal = 50; // Base amount

        // Age-based pricing
        if (insuree.Age <= 18)
        {
            monthlyTotal += 100;
        }
        else if (insuree.Age >= 19 && insuree.Age <= 25)
        {
            monthlyTotal += 50;
        }
        else if (insuree.Age >= 26)
        {
            monthlyTotal += 25;
        }

        // Car year-based pricing
        if (insuree.CarYear < 2000)
        {
            monthlyTotal += 25;
        }
        else if (insuree.CarYear > 2015)
        {
            monthlyTotal += 25;
        }

        // Car make and model-based pricing
        if (insuree.CarMake.Equals("Porsche", StringComparison.OrdinalIgnoreCase))
        {
            monthlyTotal += 25;
            if (insuree.CarModel.Equals("911 Carrera", StringComparison.OrdinalIgnoreCase))
            {
                monthlyTotal += 25; // Additional charge for specific model
            }
        }

        // Speeding ticket-based pricing
        monthlyTotal += insuree.SpeedingTickets * 10;

        // DUI-based pricing (25% increase if true)
        if (insuree.DUI)
        {
            monthlyTotal *= 1.25m;
        }

        // Full coverage-based pricing (50% increase if true)
        if (insuree.FullCoverage)
        {
            monthlyTotal *= 1.5m;
        }

        return monthlyTotal;
    }
}
