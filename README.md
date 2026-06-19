# Water Quality Monitoring

WPF/WCF projekat za predmet RVA. Tema projekta je sistem za praćenje kvaliteta vode.

## Struktura projekta

* `WaterQuality.Contracts`
  Zajednički DTO modeli, enum `WaterState` i WCF interfejs `IWaterQualityDataService`.

* `WaterQuality.InformationSystem`
  Komponenta 1 - informacioni sistem za rad sa izvorima vode i merenjima kvaliteta vode.

* `WaterQuality.StatisticsApp`
  Komponenta 2 - statistička obrada podataka. Ovaj deo se radi nakon Komponente 1.

## Komponenta 1 - završeno

Implementirano je:

* CRUD za `WaterSource`
* CRUD za `WaterQualityReading`
* validacija svih unosnih polja
* pretraga izvora i merenja po svim svojstvima
* unos decimalnih brojeva sa tačkom ili zarezom
* JSON čuvanje i učitavanje podataka
* logovanje aktivnosti u tekstualni fajl
* undo/redo za dodavanje, izmenu i brisanje
* simulacija stanja merenja kroz `Safe`, `Acceptable`, `Unsafe`, `Contaminated`
* prikaz broja merenja po stanjima u realnom vremenu
* WCF servis za slanje podataka Komponenti 2

## WCF servis

Komponenta 1 hostuje WCF servis na adresi:

```text
net.pipe://localhost/WaterQualityMonitoring/WaterQualityDataService
```

Dostupne metode:

```csharp
List<WaterSourceDto> GetAllSources();

List<WaterQualityReadingDto> GetReadingsBySourceAndYear(Guid sourceId, int year);
```

Da bi Komponenta 2 mogla da preuzima podatke, Komponenta 1 mora biti pokrenuta.

## Runtime fajlovi

Aplikacija automatski pravi runtime fajlove u `bin/Debug` folderu:

* `Data/water-quality-data.json`
* `Logs/activity-log.txt`

Ovi fajlovi se ne commituju.

## Sledeće

Preostaje izrada Komponente 2:

* WCF client
* izbor izvora i godine
* preuzimanje merenja iz Komponente 1
* mapiranje u `Dictionary<string, List<WaterQualityReadingDto>>`
* prikaz preuzetih podataka
* Strategy pattern za statističke metode
* CSV export rezultata
* NClass i sequence dijagram
