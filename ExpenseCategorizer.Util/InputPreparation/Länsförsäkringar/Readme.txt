
Instructions in Swedish as LF is a swedish bank.

Förberedelser som behövs för Länsförsäkringar.

1. Logga in och ta fram ditt kontoutdrag så långt som du vill ha med.
2. Kör följande i console (i Chrome):
	document.getElementById("viewAccountListTransactionsForm:transactionsDataTable").outerHTML
3. Kopiera svaret (undantaget citattecknen) och klistra in det i en textfil som döps till "lf.html" (spara som UTF-8).
4. Öppna lf.html i Chrome och kör följande i consolen:
	document.querySelectorAll("tr > td > span").forEach(function(e) { if(e.className === "") { e.parentNode.removeChild(e); }});
5. Stäng consolen och kopiera alltihop.
6. Klistra in i Excel och spara som "Text (tab delimited) (*.txt)".
7. Öppna filen i notepad och spara om den som utf-8 eftersom Excel inte gör det rätt.
8. Klart! Kört ExpenseCategorizer med filen och kategorimappningen.