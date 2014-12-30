
Förberedelser som behövs för Länsförsäkringar.

1. Logga in och ta fram ditt kontoutdrag så långt som du vill ha med.
2. Kör följande i console (i Chrome):
	document.getElementById("viewAccountListTransactionsForm:transactionsDataTable").innerHTML
3. Kopiera svaret och klistra in det i en textfil som döps till "lf.html".
4. Öppna lf.html i Chrome och kör följande i consolen:
	Array.prototype.splice.call(document.querySelectorAll("tr > td > span"), 0).forEach(function(e) { e.parentNode.removeChild(e); });
5. Stäng consolen och kopiera alltihop.
6. Klistra in i Excel och spara som "Text (tab delimited) (*.txt)".